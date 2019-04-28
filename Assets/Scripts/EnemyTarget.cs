using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    public GridController grid;

    public Vector2Int GetCell()
    {
        return grid.WorldToCell(transform.position);
    }
}
