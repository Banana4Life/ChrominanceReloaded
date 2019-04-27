using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer : Placer
{
    public GameObject wallPrefab;

    public WallPlacer() : base(1)
    {
    }

    public override GameObject Spawn(Vector2Int gridPos)
    {
        var wall = Instantiate(wallPrefab);
        wall.transform.position = grid.CellToCellCenter(gridPos);
        return wall;
    }
}
