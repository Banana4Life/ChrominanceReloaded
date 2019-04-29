using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public class TurretPlacer : Placer
{
    public GameObject turretPrefab;
    public int variant = 0;

    public TurretPlacer() : base(0)
    {
    }

    public override GameObject Spawn(Vector2Int gridPos)
    {
        var tower = Instantiate(turretPrefab);
        tower.transform.position = grid.CellToCellCenter(gridPos);
        var turret = tower.GetComponent<Turret>();
        turret.variant = variant;
        turret.tankState = 0;
        return tower;
    }
}
