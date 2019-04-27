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

    public GameObject projectileContainer;
    
    private float offsetLR = -1f;

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
        var offset = Quaternion.Euler(0, 0, headAngle) * Vector3.left * turretVariant.offset * offsetLR;
        offsetLR *= -1;
        Instantiate(turretVariant.projectile, transform.position + offset, Quaternion.Euler(0, 0, headAngle), projectileContainer.transform);
    }

}

[Serializable]
public class TurretVariant
{
    public String name;
    public Sprite turretHead;
    public float shootCooldown;
    public float damage;
    public GameObject projectile;

    public float offset = 0;
}
