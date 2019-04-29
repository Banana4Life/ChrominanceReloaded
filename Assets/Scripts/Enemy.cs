using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public SpriteAtlas sprites;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Configure(SpawnConfig config)
    {
        switch (config.kind)
        {
            case 0:
                spriteRenderer.sprite = sprites.GetSprite("dreieck");
                break;
            case 1:
                spriteRenderer.sprite = sprites.GetSprite("kreis");
                break;
            case 2:
                spriteRenderer.sprite = sprites.GetSprite("quadrat");
                break;
        }
        spriteRenderer.color = config.color;
        health = config.health;
    }
}
