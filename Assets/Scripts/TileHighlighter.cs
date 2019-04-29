using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GridController grid;
    public Color buildInCell;
    public Color removeInCell;
    
    private SpriteRenderer spriteRenderer;
    private Vector2Int currentCell;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCell = grid.MouseToCell();
        spriteRenderer.size = Vector2.one * grid.cellSize;
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
        if (grid.HasObjectAt(currentCell))
        {
            spriteRenderer.color = removeInCell;
        }
        else
        {
            spriteRenderer.color = buildInCell;
        }
        spriteRenderer.size = Vector2.one * grid.cellSize;
        transform.position = grid.CellToCellCorner(currentCell);
    }
}
