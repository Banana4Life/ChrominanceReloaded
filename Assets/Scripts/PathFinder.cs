using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public static KeyValuePair<List<Vector2Int>, int> ShortestPath(GridController g, Vector2Int source, Vector2Int target)
    {
        var nodeQueue = new List<Vector2Int>();
        var known = new HashSet<Vector2Int>();
        var distances = new Dictionary<Vector2Int, int>();
        var paths = new Dictionary<Vector2Int, List<Vector2Int>>();

        nodeQueue.Add(source);
        distances[source] = 0;
        paths[source] = new List<Vector2Int> {source};

        while (nodeQueue.Count > 0)
        {
            // get the node closest to the target
            var current = nodeQueue.OrderBy(n => (n - target).sqrMagnitude).First();
            if (current == target)
            {
                break;
            }
            
            nodeQueue.Remove(current);
            known.Add(current);

            var path = paths[current];
            var distance = distances[current];
    
            var next = g.GetFreeNeighborsOf(current).Where(n => !known.Contains(n));
            foreach (var node in next)
            {
                var newDistance = distance + 1;
                var newPath = path.ToList();
                newPath.Add(node);
                if (!distances.ContainsKey(node) || newDistance < distances[node])
                {
                    distances[node] = newDistance;
                    paths[node] = newPath;
                }
                nodeQueue.Add(node);
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
