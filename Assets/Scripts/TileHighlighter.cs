using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GridController grid;
    public Color validCellColor;
    public Color invalidCellColor;
    
    private EnemySource[] sources;
    private SpriteRenderer spriteRenderer;
    private Vector2Int currentCell;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCell = grid.MouseToCell();
        spriteRenderer.size = Vector2.one * grid.cellSize;
        spriteRenderer.color = invalidCellColor;
        sources = FindObjectsOfType<EnemySource>();
        UpdateHighlight();
    }

    void Update()
    {
        var newCell = grid.MouseToCell();
        if (currentCell != newCell)
        {
            currentCell = newCell;
            UpdateHighlight();
        }
    }

    private void UpdateHighlight()
    {
        if (sources.Any(s => !s.CanReachTarget()))
        {
            spriteRenderer.color = invalidCellColor;
        }
        else
        {
            spriteRenderer.color = validCellColor;
        }
        spriteRenderer.size = Vector2.one * grid.cellSize;
        transform.position = grid.CellToCellCorner(currentCell);
    }
}
