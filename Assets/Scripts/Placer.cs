using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridController))]
public abstract class Placer : MonoBehaviour
{
    public readonly int mouseButton;
    
    protected GridController grid;
    private bool placeUponRelease;
    private Vector2Int cellClicked;

    public GameObject container;

    protected Placer(int mouseButton)
    {
        this.mouseButton = mouseButton;
    }

    private void Awake()
    {
        grid = GetComponent<GridController>();
    }

    private Vector2Int GetCellPos()
    {
        return grid.MouseToCell();
    }

    // Update is called once per frame
    void Update()
    {
        if (placeUponRelease)
        {
            if (Input.GetMouseButtonUp(mouseButton))    
            {
                var cellPos = GetCellPos();
                if (cellPos == cellClicked)
                {
                    var obj = Spawn(cellPos);
                    grid.SetObjectAt(cellPos, obj);
                    obj.transform.parent = container.transform;
                }

                placeUponRelease = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(mouseButton) && !EventSystem.current.IsPointerOverGameObject())
            {
                var cellPos = GetCellPos();
                if (!grid.HasObjectAt(cellPos))
                {
                    cellClicked = GetCellPos();
                    placeUponRelease = true;
                }
            }
        }
    }

    public abstract GameObject Spawn(Vector2Int cell);
}