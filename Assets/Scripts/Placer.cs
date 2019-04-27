using System;
using UnityEngine;

[RequireComponent(typeof(GridController))]
public abstract class Placer : MonoBehaviour
{
    public Camera cam;
    public readonly int mouseButton;
    
    protected GridController grid;
    private bool placeUponRelease;

    protected Placer(int mouseButton)
    {
        this.mouseButton = mouseButton;
    }

    private void Awake()
    {
        grid = GetComponent<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placeUponRelease)
        {
            if (Input.GetMouseButtonUp(mouseButton))
            {
                var cellPos = grid.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
                var obj = Spawn(cellPos);
                grid.SetObjectAt(cellPos, obj);
                placeUponRelease = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                placeUponRelease = true;
            }
        }
    }

    public abstract GameObject Spawn(Vector2Int cell);
}