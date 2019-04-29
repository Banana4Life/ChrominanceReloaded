using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseController : MonoBehaviour
{
    public GridController grid;
    public PlayerBaseController playerBase;
    
    public int redHealth = 1000;
    public int greenHealth = 1000;
    public int blueHealth = 1000;

    public Vector2Int gridLocation = Vector2Int.zero;

    private void Update()
    {
        transform.position = grid.CellToCellCenter(gridLocation);
    }

    public void EnemyReached(Enemy enemy)
    {
        enemy.Die();
    }
}
