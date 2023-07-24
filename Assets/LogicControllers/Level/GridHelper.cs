using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper
{
    public static int gridSize = 5;

    static public Vector2 ClosestGridPoint(Vector2 checkPosition)
    {
        float closestX = Mathf.RoundToInt(checkPosition.x / gridSize) * gridSize;
        float closestY = Mathf.RoundToInt(checkPosition.y / gridSize) * gridSize;
        return new Vector2(closestX, closestY);
    }
}
