using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathTarget : MonoBehaviour
{
    public GridController grid;
    
    public Vector2Int GetCell()
    {
        return grid.WorldToCell(transform.position);
    }

    public List<Vector2Int> FindPathFrom(Vector3 world)
    {
        return PathFinder.FindPath(grid, grid.WorldToCell(world), GetCell());
    }

    public abstract void Reached(PathFollower follower);
}

public class PathFollower : MonoBehaviour
{
    public PathTarget target;
    public float speed = 5;

    public bool debug;

    private List<Vector2Int> currentPath;
    private float pathCalculatedAt;
    private int pathIndex;
    private Vector3 currentTargetCell;

    private void OnDisable()
    {
        target = null;
        currentPath = null;
    }

    public void UpdatePath(List<Vector2Int> path, float calculatedAt)
    {
        if (path != null)
        {
            currentPath = path;
            pathIndex = 0;
            pathCalculatedAt = calculatedAt;
            currentTargetCell = target.grid.CellToCellCenter(currentPath[pathIndex]);
        }
    }

    void Update()
    {
        if (target)
        {
            UpdatePath();
            UpdatePosition();
        }
    }

    private void UpdatePath()
    {
        if (currentPath == null)
        {
            UpdatePath(target.FindPathFrom(transform.position), Time.time);
        }
    }

    private void UpdatePosition()
    {
        if (currentPath != null)
        {
            if (debug)
            {
                PathFinder.DebugRenderPath(target.grid, currentPath);
            }
            var distance = currentTargetCell - transform.position;
            transform.position += Time.deltaTime * speed * distance.normalized;
            
            if (distance.magnitude < target.grid.cellSize / 8f)
            {
                if (target.grid.HasChangedSince(pathCalculatedAt))
                {
                    // grid changed, recalculate
                    currentPath = null;
                    return;
                }
                pathIndex++;
                if (pathIndex >= currentPath.Count)
                {
                    target.Reached(this);
                    currentPath = null;
                }
                else
                {
                    currentTargetCell = target.grid.CellToCellCenter(currentPath[pathIndex]);
                }
            }
        }
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
            if (currentPath.Count > 0)
            {
                return target.grid.CellToCellCenter(currentPath[currentPath.Count - 1]);
            }
        }
        return transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentTargetCell, 0.25f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetPosAt(0), 0.25f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GetPosAt(1), 0.15f);
    }
}
