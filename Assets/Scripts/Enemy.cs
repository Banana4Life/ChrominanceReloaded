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
    private Wave currentWave;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spinner = GetComponentInChildren<SpinnyThing>();
        pathFollower = GetComponent<PathFollower>();
    }

    private void OnDisable()
    {
        currentWave?.EnemyDied(this);
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Configure(SpawnConfig config, EnemyTarget target, Wave wave)
    {
        // just for reference
        currentConfig = config;
        currentWave = wave;
        
        spriteRenderer.sprite = sprites.GetSprite(currentConfig.kind);
        spriteRenderer.color = currentConfig.color;
        
        health = currentConfig.health;
        
        spinner.speed = currentConfig.spinSpeed;
        
        pathFollower.speed = currentConfig.walkSpeed;
        pathFollower.target = target;
    }
}
