using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO i am cheap movment code kill me
        var newPos = transform.position + transform.rotation * Vector3.up * Time.deltaTime * speed;
        if ((newPos - transform.parent.position).sqrMagnitude > 20)
        {
            transform.Rotate(Vector3.forward, 90f); 
        }
        else
        {
            transform.position = newPos;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var proj = other.gameObject.GetComponent<Projectile>();
        if (proj)
        {
            health -= proj.variant.damage;
            if (health < 0)
            {
                Destroy(gameObject);
            }

            proj.Die();
        }

    }

    public Vector3 getPosAt(int node)
    {
        // TODO predicted movement?
        var velocity = transform.rotation * Vector3.up * speed;
        return transform.position + velocity * node;
    }
}
