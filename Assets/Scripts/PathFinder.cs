using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;

public class PathFinder
{
    public const uint MaxIterations = 1000;
    public const int TowerCost = 10000;
    public const int EmptyCost = 1;

    private static float EstimateCost(Vector2Int from, Vector2Int to)
    {
        return (to - from).sqrMagnitude;
    }

    public static int GetCost(GridController g, Vector2Int from, Vector2Int to)
    {
        if (g.HasObjectAt(to) || g.HasObjectAt(from))
        {
            return TowerCost;
        }
        return EmptyCost;
    }
    
    public static KeyValuePair<List<Vector2Int>, int> ShortestPath(GridController g, Vector2Int source, Vector2Int target)
    {
        var nodeQueue = new SimplePriorityQueue<Vector2Int, float>();
        var known = new HashSet<Vector2Int>();
        var distances = new Dictionary<Vector2Int, int>();
        var paths = new Dictionary<Vector2Int, List<Vector2Int>>();

        nodeQueue.Enqueue(source, EstimateCost(source, target));
        distances[source] = 0;
        paths[source] = new List<Vector2Int> {source};

        uint i = 0;
        while (nodeQueue.Count > 0 && i < MaxIterations)
        {
            i++;
            // get the node closest to the target
            var current = nodeQueue.Dequeue();
            if (current == target)
            {
                break;
            }
            
            known.Add(current);

            var path = paths[current];
            var distance = distances[current];
    
            var next = g.GetNeighborsOf(current).Where(n => !known.Contains(n));
            foreach (var node in next)
            {
                var newDistance = distance + GetCost(g, current, node);
                var newPath = path.ToList();
                newPath.Add(node);
                if (!distances.ContainsKey(node) || newDistance < distances[node])
                {
                    distances[node] = newDistance;
                    paths[node] = newPath;
                }
                nodeQueue.Enqueue(node, EstimateCost(node, target));
            }
        }

        if (paths.ContainsKey(target))
        {
            return new KeyValuePair<List<Vector2Int>, int>(paths[target], distances[target]);
        }
        return new KeyValuePair<List<Vector2Int>, int>();
    }

    public static List<Vector2Int> FindPath(GridController g, Vector2Int source, Vector2Int target)
    {
        var shortestPath = ShortestPath(g, source, target);
        return shortestPath.Key;
    }

    public static int FindDistance(GridController g, Vector2Int source, Vector2Int target)
    {
        var shortestPath = ShortestPath(g, source, target);
        if (shortestPath.Key == null)
        {
            return -1;
        }
        return shortestPath.Value;
    }

    public static bool ExistsPathBetween(GridController g, Vector2Int source, Vector2Int target)
    {
        return FindPath(g, source, target) != null;
    }

    public static void DebugRenderPath(GridController grid, List<Vector2Int> path)
    {
        var e = path.GetEnumerator();
        if (e.MoveNext())
        {
            var lastPos = grid.CellToCellCenter(e.Current);

            while (e.MoveNext())
            {
                var next = grid.CellToCellCenter(e.Current);
                Debug.DrawLine(lastPos, next, Color.red);
                lastPos = next;
            }
        }
        e.Dispose();
    }
}
