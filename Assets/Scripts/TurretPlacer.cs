using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public class TurretPlacer : MonoBehaviour
{
    public Camera cam;
    public GameObject turretPrefab;
    public GameObject wallPrefab;
    public int variant = 0;

    private GridController grid;
    private bool placeUponRelease;
    private bool placeWallUponRelease;
    
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
                SpawnTower(cellPos, turretPrefab);
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
        if (placeWallUponRelease)
        {
            if (Input.GetMouseButtonUp(1))
            {
                var cellPos = grid.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
                SpawnWall(cellPos, wallPrefab);
                placeWallUponRelease = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                placeWallUponRelease = true;
            }
        }
    }
    
    public GameObject SpawnTower(Vector2Int gridPos, GameObject prefab)
    {
        var tower = Instantiate(prefab);
        tower.transform.position = grid.CellToCellCenter(gridPos);
        tower.GetComponent<Turret>().variant = variant;
        return tower;
    } 
    
    public GameObject SpawnWall(Vector2Int gridPos, GameObject prefab)
    {
        var wall = Instantiate(prefab);
        wall.transform.position = grid.CellToCellCorner(gridPos) + new Vector3(0.5f, 0.5f, 0);
        return wall;
    }
}
