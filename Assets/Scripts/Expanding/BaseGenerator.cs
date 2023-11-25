using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseGenerator : MonoBehaviour
{
    public HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
    public Dictionary<Vector2Int, GameObject> wallPositions = new Dictionary<Vector2Int, GameObject>();
    private HashSet<Vector2Int> castlePositions;

    private Vector2Int startPosition;

    [SerializeField]
    private int iterations = 5;
    [SerializeField]
    private int walkLength = 5;
    [SerializeField]
    private bool startRandomlyEachIteration = true;

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;

    [SerializeField]
    private bool symmetry = true;
    [SerializeField]
    private bool symmetryFourSides = false;

    private void Start()
    {
        castlePositions = tilemapVisualizer.GetFloorTilePositions();
        wallPositions = tilemapVisualizer.CreateWalls(tilemapVisualizer.GetWallPositions());
    }

    public Vector2Int GetRandomStartPosition()
    {
        while(true)
        {
            Vector2Int startPosition = wallPositions.Keys.ElementAt(Random.Range(0, wallPositions.Count));
            List<Vector2Int> directionList = startPosition.y % 2 == 0 ? DirectionHex.cardinalDirectionsEvenY : DirectionHex.cardinalDirectionsOddY;
            foreach (var direction in directionList)
            {
                var neighborPosition = startPosition + direction;
                if (!floorPositions.Contains(neighborPosition) && !wallPositions.Keys.Contains(neighborPosition) && !castlePositions.Contains(neighborPosition))
                {
                    return neighborPosition;
                }
            }
        }
    }
    
    public void RunProceduralGeneration()
    {
        startPosition = GetRandomStartPosition();
        HashSet<Vector2Int> newFloorPositions = RunRandomWalk();
        //tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(newFloorPositions);
        floorPositions.UnionWith(newFloorPositions);

        HashSet<Vector2Int> newWallPositions = FindWallsHex(newFloorPositions, wallPositions.Keys.ToHashSet());
        wallPositions.Union(tilemapVisualizer.CreateWalls(newWallPositions));
    }

    private HashSet<Vector2Int> RunRandomWalk()
    {
        //Debug.Log("Start Position: " + startPosition);
        Vector2Int currentPosition = startPosition;
        HashSet<Vector2Int> newFloorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            foreach (var position in path)
            {
                if(!floorPositions.Contains(position) && !wallPositions.Keys.Contains(position) && !castlePositions.Contains(position))
                {
                    newFloorPositions.Add(position);
                    if (symmetry)
                    {
                        newFloorPositions.Add(tilemapVisualizer.GetSymmetricPosition(position));
                        if (symmetryFourSides)
                        {
                            newFloorPositions.Add(tilemapVisualizer.GetSymmetricPositionHorizontal(position));
                            newFloorPositions.Add(tilemapVisualizer.GetSymmetricPositionVertical(position));
                        }
                    }
                }
            }
            if (startRandomlyEachIteration)
            {
                var index = Random.Range(0, newFloorPositions.Count);
                //Debug.Log("index = " + index);
                currentPosition = newFloorPositions.ElementAt(index);
            }
        }
        return newFloorPositions;
    }

    private static HashSet<Vector2Int> FindWallsHex(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallPositions)
    {
        HashSet<Vector2Int> newWallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            List<Vector2Int> directionList = position.y % 2 == 0 ? DirectionHex.cardinalDirectionsEvenY : DirectionHex.cardinalDirectionsOddY;
            foreach (var direction in directionList)
            {
                var neighborPosition = position + direction;
                if (!floorPositions.Contains(neighborPosition) && !wallPositions.Contains(neighborPosition))
                {
                    newWallPositions.Add(neighborPosition);
                    //Debug.Log("Wall in " + neighborPosition);
                }
            }
        }
        return newWallPositions;
    }

    public GameObject CreateBuilding(GameObject building, Vector2Int position)
    {
        return tilemapVisualizer.CreateBuilding(building, position);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return tilemapVisualizer.WorldToCell(position);
    }

    public Vector3 CellToWorld(Vector3Int position)
    {
        return tilemapVisualizer.CellToWorld(position);
    }
}
