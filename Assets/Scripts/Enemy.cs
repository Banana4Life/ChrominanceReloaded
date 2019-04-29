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
        spriteRenderer.sprite = sprites.GetSprite(config.kind);
        spriteRenderer.color = config.color;
        health = config.health;
        spinner.speed = config.spinSpeed;
        pathFollower.speed = config.walkSpeed;
        pathFollower.target = target;
    }
}
