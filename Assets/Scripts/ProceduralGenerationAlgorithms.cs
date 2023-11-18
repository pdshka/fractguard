using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPosition);
        Vector2Int previousPosition = startPosition;
        for (int i = 0; i < walkLenght; i++)
        {
            Vector2Int nextPosition = previousPosition + DirectionHex.GetRandomCardinalDirection(previousPosition);
            Debug.Log(nextPosition);
            path.Add(nextPosition);
            previousPosition = nextPosition;
        }
        return path;
    }
}

public static class DirectionHex
{
    public static List<Vector2Int> cardinalDirectionsEvenY = new List<Vector2Int>
    {
        new Vector2Int(1, 0), // up
        new Vector2Int(0, 1), // up-right
        new Vector2Int(-1, 1), // down-right
        new Vector2Int(-1, 0), // down
        new Vector2Int(-1, -1), // down-left
        new Vector2Int(0, -1) // up-left
    };

    public static List<Vector2Int> cardinalDirectionsOddY = new List<Vector2Int>
    {
        new Vector2Int(1, 0), // up
        new Vector2Int(1, 1), // up-right
        new Vector2Int(0, 1), // down-right
        new Vector2Int(-1, 0), // down
        new Vector2Int(0, -1), // down-left
        new Vector2Int(1, -1) // up-left
    };

    public static Vector2Int GetRandomCardinalDirection(Vector2Int previousPosition)
    {
        if (previousPosition.y % 2 == 0)
            return GetRandomCardinalDirectionEvenY();
        else
            return GetRandomCardinalDirectionOddY();
    }

    private static Vector2Int GetRandomCardinalDirectionEvenY()
    {
        return cardinalDirectionsEvenY[Random.Range(0, cardinalDirectionsEvenY.Count)];
    }

    private static Vector2Int GetRandomCardinalDirectionOddY()
    {
        return cardinalDirectionsOddY[Random.Range(0, cardinalDirectionsOddY.Count)];
    }
}