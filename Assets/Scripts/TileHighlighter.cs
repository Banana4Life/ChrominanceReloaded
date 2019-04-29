using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GridController grid;
    public Color buildInCell;
    public Color removeInCell;
    
    private SpriteRenderer r;
    private Vector2Int currentCell;
    private float cellSwitchedAt;
    private bool canBuild;
    
    void Awake()
    {
        r = GetComponentInChildren<SpriteRenderer>();
        r.size = Vector2.one * grid.cellSize;
        
        currentCell = grid.MouseToCell();
        UpdateHighlight();
    }

    void Update()
    {
        r.size = Vector2.one * grid.cellSize;
        var newCell = grid.MouseToCell();
        if (currentCell != newCell || grid.HasChangedSince(cellSwitchedAt))
        {
            currentCell = newCell;
            cellSwitchedAt = Time.time;
            UpdateHighlight();
            r.color = canBuild ? buildInCell :removeInCell;
        }
    }

    private void UpdateHighlight()
    {
        canBuild = grid.HasObjectAt(currentCell);
        transform.position = grid.CellToCellCorner(currentCell);
    }
}
