using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public float cellSize = 1f;

    private float CoordToCell(float coord)
    {
        return coord / cellSize;
    }

    public Vector3 WorldToCellCorner(Vector3 world)
    {
        var x = Mathf.Floor(CoordToCell(world.x)) * cellSize;
        var y = Mathf.Floor(CoordToCell(world.y)) * cellSize;
        
        return new Vector3(x, y, world.z);
    }

    public Vector3 WorldToCellCenter(Vector3 world)
    {
        var halfCell = cellSize / 2f;
        var x = Mathf.Floor(CoordToCell(world.x)) * cellSize + halfCell;
        var y = Mathf.Floor(CoordToCell(world.y)) * cellSize + halfCell;
        
        return new Vector3(x, y, world.z);
    }

    public Vector2Int WorldToCell(Vector3 world)
    {
        return new Vector2Int((int) CoordToCell(world.x), (int) CoordToCell(world.y));
    }
}
