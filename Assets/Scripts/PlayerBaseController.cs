using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseController : MonoBehaviour
{
    public GridController grid;
    public PlayerBaseController playerBase;
    
    public float redAmount = 1000;
    public float greenAmount = 1000;
    public float blueAmount = 1000;

    public Vector2Int gridLocation = Vector2Int.zero;

    private void Update()
    {
        transform.position = grid.CellToCellCenter(gridLocation);
    }

    public void EnemyReached(Enemy enemy)
    {
        switch (enemy.color)
        {
            case ColorType.Red:
                redAmount -= enemy.health;
                break;
            case ColorType.Green:
                greenAmount -= enemy.health;
                break;
            case ColorType.Blue:
                blueAmount -= enemy.health;
                break;
        }

        enemy.Die();
    }
}
