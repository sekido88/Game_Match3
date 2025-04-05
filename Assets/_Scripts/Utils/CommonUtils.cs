using UnityEngine;
using System.Collections.Generic;
using System;
public static class CommonUtils
{
    public static Dictionary<Direction, Vector3> directionToVector3 = new Dictionary<Direction, Vector3>() {
        {Direction.Up, Vector3.up},
        {Direction.Right , Vector3.right},
        {Direction.Down , Vector3.down},
        {Direction.Left , Vector3.left}
    };
    public static Dictionary<Direction, Vector2Int> directionToVector2Int = new Dictionary<Direction, Vector2Int>() {
        {Direction.Up, Vector2Int.up},
        {Direction.Right , Vector2Int.right},
        {Direction.Down , Vector2Int.down},
        {Direction.Left , Vector2Int.left}
    };
    public static Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
        }
        return Direction.Up;
    }
    public static Dictionary<Direction, float> directionToRotation = new Dictionary<Direction, float>() {
        {Direction.Up, 0},
        {Direction.Right, 270f},
        {Direction.Down, 180f},
        {Direction.Left, 90f},
    };
    public static Dictionary<Direction, int> directionToRotate = new Dictionary<Direction, int>() {
        {Direction.Up, 0},
        {Direction.Right, 1},
        {Direction.Down, 2},
        {Direction.Left, 3},
    };
    public static Direction GetRotateDirection(Direction currentDirection,int rotate) {
        Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
        int nextIndex = (directionToRotate[currentDirection] + rotate) % 4;
        
        return directions[nextIndex];
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }
    
}