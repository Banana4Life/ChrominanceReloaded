using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Sprite turretHead;
    public GameObject head;
    public GameObject projectile;

    public GameObject projectileContainer;

    public float shootCooldown = 20;

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
        var headRenderer = head.GetComponentInChildren<SpriteRenderer>();
        headRenderer.sprite = turretHead;
        head.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        headAngle += Time.deltaTime * 50;

        lastShot -= Time.deltaTime * 50;
        if (lastShot <= 0)
        {
            lastShot = shootCooldown;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, headAngle), projectileContainer.transform);
    }

}
