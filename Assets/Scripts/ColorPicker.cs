using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour
{
    protected GridController grid;
    public Camera cam;
    private Vector2Int cellClicked;

    public GameObject floater;

    private void Awake()
    {
        grid = GetComponent<GridController>();
        foreach (var fl in GetComponentsInChildren<Floater>())
        {
            var cell = grid.WorldToCell(fl.gameObject.transform.position);
            grid.SetObjectAt(cell, fl.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cellPos = grid.MouseToCell();

            var obj = grid.GetObjectAt(cellPos);
            if (obj)
            {
                
                if (obj.GetComponent<Floater>())
                {
                    floater = Instantiate(obj, transform);
                }
            }
        }
       
        if (floater)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var cellPos = grid.MouseToCell();

                var obj = grid.GetObjectAt(cellPos);
                if (obj)
                {
                    var turret = obj.GetComponent<Turret>();
                    if (turret)
                    {
                        turret.FillTank(floater.GetComponent<Floater>().color);
                    }
                }
                Destroy(floater);
            }
            
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            floater.transform.position = mousePos;
        }
    }
}
