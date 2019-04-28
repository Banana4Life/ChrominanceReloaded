using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public float health = 100;
    public float waypointDistance = 3;
    public EnemyTarget target;

    private List<Vector2Int> currentPath;
    private int pathIndex = 0;
    private Vector3 currentTargetCell;
    
    void Update()
    {
        if (target)
        {
            if (currentPath == null)
            {
                currentPath = target.FindPathFrom(transform.position);
                if (currentPath != null)
                {
                    pathIndex = 0;
                    currentTargetCell = target.grid.CellToCellCenter(currentPath[pathIndex++]);
                }
            }

            if (currentPath != null)
            {
                PathFinder.DebugRenderPath(target.grid, currentPath);
                // TODO i am cheap movment code kill me
                var distance = currentTargetCell - transform.position;
                transform.position += Time.deltaTime * speed * distance.normalized;
                if (distance.sqrMagnitude < waypointDistance * waypointDistance)
                {
                    pathIndex++;
                    if (pathIndex >= currentPath.Count)
                    {
                        currentPath = null;
                    }
                    else
                    {
                        currentTargetCell = target.grid.CellToCellCenter(currentPath[pathIndex]);
                    }
                }
            }
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

    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 getPosAt(int node)
    {
        // TODO predicted movement?
        var velocity = transform.rotation * Vector3.up * speed;
        return transform.position + velocity * node;
    }
}
