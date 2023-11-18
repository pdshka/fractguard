using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> wallPositions = FindWallsHex(floorPositions);
        tilemapVisualizer.CreateWalls(wallPositions);
    }

    private static HashSet<Vector2Int> FindWallsHex(HashSet<Vector2Int> floorPositions)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            List<Vector2Int> directionList = position.y % 2 == 0 ? DirectionHex.cardinalDirectionsEvenY : DirectionHex.cardinalDirectionsOddY;
            foreach (var direction in directionList)
            {
                var neighborPosition = position + direction;
                if (!floorPositions.Contains(neighborPosition))
                {
                    wallPositions.Add(neighborPosition);
                    //Debug.Log("Wall in " + neighborPosition);
                }
            }
        }
        return wallPositions;
    }
}
