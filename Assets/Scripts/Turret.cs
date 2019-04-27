using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Turret : MonoBehaviour
{

    public TurretVariant[] variants = new TurretVariant[4];
    public int variant = 0;

    private TurretVariant turretVariant;
    
    public GameObject head;
    public GameObject projectile;

    public GameObject projectileContainer;

    private float lastShot;

    [Range(0.0f,360.0f)]
    public float headAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        turretVariant = variants[variant];
        
        var headRenderer = head.GetComponentInChildren<SpriteRenderer>();
        headRenderer.sprite = turretVariant.turretHead;
        head.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        headAngle += Time.deltaTime * 50;

        lastShot -= Time.deltaTime * 50;
        if (lastShot <= 0)
        {
            lastShot = turretVariant.shootCooldown;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, headAngle), projectileContainer.transform);
    }

}

[Serializable]
public class TurretVariant
{
    public String name;
    public Sprite turretHead;
    public float shootCooldown;
    public float damage;

}
