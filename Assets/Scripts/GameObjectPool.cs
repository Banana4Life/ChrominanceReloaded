using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public bool preallocate = true;
    public GameObject prefab;
    public int initialSize = 30;
    public int growBy = 30;

    private List<GameObject> objects;
    
    private void Awake()
    {
        objects = new List<GameObject>(initialSize);
        if (preallocate && prefab)
        {
            GrowPool(initialSize);
        }
    }

    public GameObject Get()
    {
        if (!prefab)
        {
            return null;
        }
        int i;
        for (i = 0; i < objects.Count; i++)
        {
            var pooled = objects[i];
            if (!pooled.activeInHierarchy)
            {
                pooled.SetActive(true);
                return pooled;
            }
        }

        GrowPool(growBy);
        return objects[i];
    }

    private void GrowPool(int by)
    {
        for (var i = 0; i < by; ++i)
        {
            var newObject = Instantiate(prefab, gameObject.transform, true);
            newObject.SetActive(false);
            objects.Add(newObject);
        }
    }
}
