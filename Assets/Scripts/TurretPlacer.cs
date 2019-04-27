﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public class TurretPlacer : MonoBehaviour
{
    public Camera cam;
    public GameObject towerPrefab;

    private GridController grid;
    private bool placeUponRelease;
    
    void Start()
    {
        grid = GetComponent<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placeUponRelease)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var cellPos = grid.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
                SpawnTower(cellPos, towerPrefab);
                placeUponRelease = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                placeUponRelease = true;
            }
        }
    }
    
    public GameObject SpawnTower(Vector2Int gridPos, GameObject prefab)
    {
        var tower = Instantiate(prefab);
        tower.name = prefab.GetComponent<Turret>().getVariant().name;
        tower.transform.position = grid.CellToCellCorner(gridPos) + new Vector3(0.5f, 0.5f, 0);
        return tower;
    }
}
