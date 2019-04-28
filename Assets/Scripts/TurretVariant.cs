using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TurretVariant : MonoBehaviour
{
    
    public String displayName;

    public SpriteAtlas spriteAtlas;

    public float shootCooldown;
    public float damage;
    public GameObject projectile;
    public float speed = 2;
    public float offset = 0;
    public float maxRotationPerTick = 300;
    public Vector3 launcherOffset = new Vector3(0, 0.72f, 0);
    
    private Dictionary<Color, Sprite> generatedSprites = new Dictionary<Color, Sprite>();

    private static Sprite generateSprite(Sprite origSprite, Color color)
    {
        Texture2D orig = origSprite.texture;
        Texture2D texture = new Texture2D(orig.width, orig.height);
        for (int y = 0; y < orig.height; y++)
        {
            for (int x = 0; x < orig.width; x++)
            {
                var col = orig.GetPixel(x,y);
                if (col == Color.magenta)
                {
                    col = color;
                }
                texture.SetPixel(x, y, col);
            }
        }
        texture.Apply();
        return Sprite.Create(texture, origSprite.rect, new Vector2(0.5f, 0.5f));
    }

    public Sprite getBaseSprite()
    {
        return spriteAtlas.GetSprite("base");
    }

    public Sprite getHeadSprite(ColorVariant variant)
    {
        // TODO return based on state
        return spriteAtlas.GetSprite("half_empty");
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
