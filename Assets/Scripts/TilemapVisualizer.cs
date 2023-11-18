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
    private GameObject wallObject;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public void CreateWalls(IEnumerable<Vector2Int> wallPositions)
    {
        foreach (var position in wallPositions)
        {
            GameObject newWall = GameObject.Instantiate(wallObject);
            newWall.transform.position = floorTilemap.CellToWorld((Vector3Int)position);
            newWall.transform.SetParent(this.gameObject.transform);
        }
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
}
