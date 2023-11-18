using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    private int iterations = 10;
    [SerializeField]
    private int walkLength = 10;
    [SerializeField]
    private bool startRandomlyEachIteration = true;

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;
    
    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> newFloorPositions = RunRandomWalk();
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(newFloorPositions);
        WallGenerator.CreateWalls(newFloorPositions, tilemapVisualizer);
    }

    private HashSet<Vector2Int> RunRandomWalk()
    {
        Vector2Int currentPosition = startPosition;
        HashSet<Vector2Int> newFloorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            HashSet<Vector2Int> path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkLength);
            newFloorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
            {
                currentPosition = newFloorPositions.ElementAt(Random.Range(0, newFloorPositions.Count));
            }
        }
        return newFloorPositions;
    }
}
