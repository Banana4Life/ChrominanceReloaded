using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public GridController grid;

    public Vector2Int GetCell()
    {
        return grid.WorldToCell(transform.position);
    }

    public List<Vector2Int> FindPathFrom(Vector3 world)
    {
        return PathFinder.FindPath(grid, grid.WorldToCell(world), GetCell());
    }
}
