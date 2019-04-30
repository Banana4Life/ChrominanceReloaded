using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBaseController : MonoBehaviour
{
    public GridController grid;
    public PlayerBaseController playerBase;
    
    public float redAmount = 1000;
    public float greenAmount = 1000;
    public float blueAmount = 1000;

    public Vector2Int gridLocation = Vector2Int.zero;
    public float maxAmount = 1000;

    private void Update()
    {
        transform.position = grid.CellToCellCenter(gridLocation);


        if (redAmount < 0 || greenAmount < 0 || blueAmount < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
        float colorBonus = 10f;
        switch (color)
        {
            case ColorType.Red:
                redAmount += baseAmount * colorBonus;
                greenAmount += baseAmount;
                blueAmount += baseAmount;
                break;
            case ColorType.Green:
                redAmount += baseAmount;
                greenAmount += baseAmount * colorBonus;
                blueAmount += baseAmount;
                break;
            case ColorType.Blue:
                redAmount += baseAmount;
                greenAmount += baseAmount;
                blueAmount += baseAmount * colorBonus;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(color), color, null);
        }

        redAmount = Mathf.Min(redAmount, maxAmount);
        greenAmount = Mathf.Min(greenAmount, maxAmount);
        blueAmount = Mathf.Min(blueAmount, maxAmount);
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
