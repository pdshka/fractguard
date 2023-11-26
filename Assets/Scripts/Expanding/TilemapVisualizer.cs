using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private Tilemap wallTilemap;
    [SerializeField]
    private GameObject wallObject;
    [SerializeField]
    private GameObject wallsContainer;
    [SerializeField]
    private GameObject buildingsContainer;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public Dictionary<Vector2Int, GameObject> CreateWalls(IEnumerable<Vector2Int> wallPositions)
    {
        Dictionary<Vector2Int, GameObject> walls = new Dictionary<Vector2Int, GameObject>();
        foreach (var position in wallPositions)
        {
            GameObject newWall = GameObject.Instantiate(wallObject);
            newWall.transform.position = floorTilemap.CellToWorld((Vector3Int)position);
            if (wallsContainer != null)
                newWall.transform.SetParent(wallsContainer.transform);
            walls.Add(position, newWall);
        }
        return walls;
    }

    public GameObject CreateBuilding(GameObject building, Vector2Int position)
    {
        GameObject newBuilding = Instantiate(building);
        newBuilding.transform.position = floorTilemap.CellToWorld((Vector3Int)position);
        if (buildingsContainer != null)
            newBuilding.transform.SetParent(buildingsContainer.transform);
        return newBuilding;
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(position, tilemap, tile);
        }
    }

    private void PaintSingleTile(Vector2Int position, Tilemap tilemap, TileBase tile)
    {
        //var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile((Vector3Int)position, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }

    private HashSet<Vector2Int> GetTilePositions(Tilemap tilemap)
    {
        HashSet<Vector2Int> tilePositions = new HashSet<Vector2Int>();

        BoundsInt bounds = tilemap.cellBounds;
        //Debug.Log(bounds);
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    var tilePosition = new Vector2Int(x, y) + (Vector2Int)bounds.position;
                    tilePositions.Add(tilePosition);
                    //Debug.Log(tilePosition +  " tile:" + tile.name);
                }
            }
        }
        return tilePositions;
    }

    public HashSet<Vector2Int> GetFloorTilePositions()
    {
        return GetTilePositions(floorTilemap);
    }

    public HashSet<Vector2Int> GetWallPositions()
    {
        return GetTilePositions(wallTilemap);
    }

    public Vector2Int GetSymmetricPosition(Vector2Int position)
    {
        var worldPosition = floorTilemap.CellToWorld((Vector3Int)position);
        worldPosition *= -1;
        return (Vector2Int)floorTilemap.WorldToCell(worldPosition);
    }

    public Vector2Int GetSymmetricPositionHorizontal(Vector2Int position)
    {
        var worldPosition = floorTilemap.CellToWorld((Vector3Int)position);
        worldPosition *= new Vector2(-1, 1);
        return (Vector2Int)floorTilemap.WorldToCell(worldPosition);
    }

    public Vector2Int GetSymmetricPositionVertical(Vector2Int position)
    {
        var worldPosition = floorTilemap.CellToWorld((Vector3Int)position);
        worldPosition *= new Vector2(1, -1);
        return (Vector2Int)floorTilemap.WorldToCell(worldPosition);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return floorTilemap.WorldToCell(position);
    }

    public Vector3 CellToWorld(Vector3Int position)
    {
        return floorTilemap.CellToWorld(position);
    }
}
