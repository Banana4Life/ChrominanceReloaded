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

    public void AwardKill(ColorType color)
    {
        float baseAmount = 1f;
        switch (color)
        {
            case ColorType.Red:
                redAmount += baseAmount * 20;
                greenAmount += baseAmount;
                blueAmount += baseAmount;
                break;
            case ColorType.Green:
                redAmount += baseAmount;
                greenAmount += baseAmount * 20;
                blueAmount += baseAmount;
                break;
            case ColorType.Blue:
                redAmount += baseAmount;
                greenAmount += baseAmount;
                blueAmount += baseAmount * 20;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }

        redAmount = Mathf.Max(redAmount, 1000);
        greenAmount = Mathf.Max(greenAmount, 1000);
        blueAmount = Mathf.Max(blueAmount, 1000);
    }

    public bool Take(ColorVariant color)
    {
        float amount = 100;
        switch (color.colorId)
        {
            case 0:
                if (amount > greenAmount)
                {
                    return false;
                } 
                greenAmount -= amount;
                return true;
            case 1:
                if (amount > redAmount)
                {
                    return false;
                } 
                redAmount -= amount;
                return true;
            case 2:
                if (amount > blueAmount)
                {
                    return false;
                } 
                blueAmount -= amount;
                return true;
        }

        return false;
    }
}
