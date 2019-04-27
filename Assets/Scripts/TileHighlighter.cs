using System;
using System.Collections;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public GridController grid;
    public Camera Cam;

    private SpriteRenderer spriteRenderer;
    private Texture2D texture;
    private Sprite whiteSprite;
    
    void Start()
    {
        texture = new Texture2D(1, 1);
        texture.SetPixel(1, 1, Color.white);
        whiteSprite = Sprite.Create(texture, new Rect(Vector2.zero, Vector2.one), Vector2.zero);

        transform.position = Cam.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = whiteSprite;
        spriteRenderer.drawMode = SpriteDrawMode.Tiled;
        spriteRenderer.sortingLayerName = "Default";
        spriteRenderer.size = Vector2.one * grid.cellSize;
        spriteRenderer.color = new Color(1, 0, 0, 0.5f);
    }

    void Update()
    {
        var worldPos = grid.WorldToCellCorner(Cam.ScreenToWorldPoint(Input.mousePosition));
        spriteRenderer.size = Vector2.one * grid.cellSize;
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }
}
