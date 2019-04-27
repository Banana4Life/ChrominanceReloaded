using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileController : MonoBehaviour
{
    private Tilemap tilemap;
    private Tile hovered;
    public Camera Cam;
    
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        var mousePos = Input.mousePosition;
        var worldPos = Cam.ScreenToWorldPoint(mousePos);
        var tilemapPos = tilemap.WorldToCell(worldPos);
        var tile = tilemap.GetTile<Tile>(tilemapPos);
        if (tile != hovered)
        {
            if (hovered)
            {
                tile.color = Color.white;
            }

            if (!tile)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                tilemap.SetTile(tilemapPos, tile);
            }

            hovered = tile;
            tile.color = Color.red;
        }
    }
}
