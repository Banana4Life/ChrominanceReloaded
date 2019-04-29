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
    public int tankSize = 100;
    
    private Dictionary<String, Sprite> sprites = new Dictionary<String, Sprite>();

    public Sprite getBaseSprite()
    {
        init();
        return sprites["base"];
    }

    public Sprite getHeadSprite(int tankState)
    {
        init();
        String state;
        var pCent = 100 * tankState / tankSize;
        if (pCent > 66)
        {
            state = "full";
        }
        else if (pCent > 33)
        {
            state = "half_full";
        }
        else if (pCent > 0)
        {
            state = "half_empty";
        }
        else
        {
            state = "empty";
        }

        return sprites[state];
}
    
    // Start is called before the first frame update
    private void init()
    {
        if (sprites.Count == 0)
        {
            sprites.Add("base", spriteAtlas.GetSprite("base"));
            sprites.Add("full", spriteAtlas.GetSprite("full"));
            sprites.Add("empty", spriteAtlas.GetSprite("empty"));
            sprites.Add("half_full", spriteAtlas.GetSprite("half_full"));
            sprites.Add("half_empty", spriteAtlas.GetSprite("half_empty"));    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
