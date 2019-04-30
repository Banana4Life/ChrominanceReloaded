using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    public float predictedDamage = 100;
    public SpriteAtlas sprites;
    public ColorType color;

    [Header("Audio")]
    public AudioSource hitSound;
    public AudioSource killSound;
    
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
            killSound.Play();
            Die();
        }
        else
        {
            hitSound.Play();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    public void Configure(SpawnConfig config, EnemyTarget target, Wave wave, PrecalculatedPath path)
    {
        // just for reference
        currentConfig = config;
        currentWave = wave;
        color = config.color;
        
        spriteRenderer.sprite = sprites.GetSprite(currentConfig.kind);
        switch (currentConfig.color)
        {
            case ColorType.Red:
                spriteRenderer.color = Color.red;
                break;
            case ColorType.Green:
                spriteRenderer.color = Color.green;
                break;
            case ColorType.Blue:
                spriteRenderer.color = Color.blue;
                break;
        }
        
        health = currentConfig.health;
        
        spinner.speed = currentConfig.spinSpeed;
        
        pathFollower.speed = currentConfig.walkSpeed;
        pathFollower.target = target;
        pathFollower.UpdatePath(path.path, path.calculatedAt);
    }

    public static int colorForType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Green:
                return 0;
            case ColorType.Red:
                return 1;
            case ColorType.Blue:
                return 2;
        }

        return -1;
    }
}

public enum ColorType
{
    Red, Green, Blue
}
