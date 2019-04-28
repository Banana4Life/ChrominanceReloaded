using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public EnemyTarget target;
    public float speed = 5;

    private List<Vector2Int> currentPath;
    private int pathIndex = 0;
    private Vector3 currentTargetCell;

    private void OnDisable()
    {
        target = null;
        currentPath = null;
    }

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
                    currentTargetCell = target.grid.CellToCellCenter(currentPath[pathIndex]);
                }
            }

            if (currentPath != null)
            {
                PathFinder.DebugRenderPath(target.grid, currentPath);
                var distance = currentTargetCell - transform.position;
                transform.position += Time.deltaTime * speed * distance.normalized;
                if (distance.magnitude < target.grid.cellSize / 8f)
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

    public Vector3 GetVelocity()
    {
        return (currentTargetCell - transform.position).normalized * speed;
    }

    public Vector3 GetPosAt(int node)
    {
        var requestedNode = pathIndex + node;
        if (currentPath != null)
        {
            if (requestedNode < currentPath.Count)
            {
                return target.grid.CellToCellCenter(currentPath[requestedNode]);
            }
            return target.grid.CellToCellCenter(currentPath[currentPath.Count - 1]);
        }
        return transform.position;
    }

    private void OnDrawGizmos()
    {
        if (currentTargetCell != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentTargetCell, 0.25f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetPosAt(0), 0.25f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(GetPosAt(1), 0.15f);
        }
    }
}
