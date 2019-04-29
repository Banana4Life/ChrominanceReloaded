using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public SpriteAtlas sprites;
    
    // for viewing only
    public SpawnConfig currentConfig;
    
    private SpriteRenderer spriteRenderer;
    private SpinnyThing spinner;
    private PathFollower pathFollower;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spinner = GetComponentInChildren<SpinnyThing>();
        pathFollower = GetComponent<PathFollower>();
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Configure(SpawnConfig config, EnemyTarget target)
    {
        currentConfig = config;
        
        spriteRenderer.sprite = sprites.GetSprite(currentConfig.kind);
        spriteRenderer.color = currentConfig.color;
        
        health = currentConfig.health;
        
        spinner.speed = currentConfig.spinSpeed;
        
        pathFollower.speed = currentConfig.walkSpeed;
        pathFollower.target = target;
    }
}
