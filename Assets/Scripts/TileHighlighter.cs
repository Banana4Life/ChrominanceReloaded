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

    void Awake()
    {
        r = GetComponentInChildren<SpriteRenderer>();
        r.drawMode = SpriteDrawMode.Tiled;
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
        }

        UpdateHover();
    }

    private void UpdateHover()
    {
        var gridObj = grid.GetObjectAt(currentCell);
        if (gridObj)
        {
            var floaterComp = gridObj.GetComponent<Floater>();
            if (floaterComp)
            {
                floaterComp.hovered = 0.05f;
            }
        }
    }

    private void UpdateHighlight()
    {
        transform.position = grid.CellToCellCorner(currentCell);
        var colorPicker = FindObjectOfType<ColorPicker>();
        if (colorPicker.isActive())
        {
            r.enabled = false;
        }
        else
        {
            r.enabled = true;
            var gridObj = grid.GetObjectAt(currentCell);
            if (gridObj)
            {
                var floaterComp = gridObj.GetComponent<Floater>();
                if (!floaterComp)
                {
                    r.color = removeInCell;
                }
            }
            else
            {
                r.color = buildInCell;
            }
        }
    }
}