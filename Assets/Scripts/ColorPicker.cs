using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour
{
    protected GridController grid;
    public Camera cam;
    private Vector2Int cellClicked;

    private GameObject floater;
    public GameObject floaterPrefab;

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
                    floater = Instantiate(floaterPrefab, transform);
                    floater.GetComponent<Floater>().color = obj.GetComponent<Floater>().color;
                }
            }
        }
       
        if (floater)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var cellPos = grid.MouseToCell();

                var obj = grid.GetObjectAt(cellPos);
                var floaterComp = floater.GetComponent<Floater>();
                bool noMess = false;
                if (obj)
                {
                    var turret = obj.GetComponent<Turret>();
                    if (turret)
                    {
                        turret.FillTank(floaterComp.color);
                        noMess = true;
                    }
                }

                if (!noMess)
                {
                    for (int i = 0; i < Random.Range(3, 8); i++)
                    {
                        ParticleLauncher.decalEmitter.GetComponent<ParticleDecalPool>().ParticleHit (floaterComp.color.gradient, floater.transform.position);
                    }
                }
                Destroy(floater);
            }
            
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            floater.transform.position = mousePos;
        }
    }
    
    public bool isActive()
    {
        return floater;
    }
}
