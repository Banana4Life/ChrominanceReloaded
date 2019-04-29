using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GameObjectPool))]
public class EnemySource : MonoBehaviour
{
    public GridController grid;
    public EnemyTarget target;
    public float initialSpawnFrequency;
    public SpawnConfig[] configs;
    
    private GameObjectPool pool;

    void Awake()
    {
        pool = GetComponent<GameObjectPool>();
        SetFrequency(initialSpawnFrequency);
    }

    public Vector2Int GetCell()
    {
        return grid.WorldToCell(transform.position);
    }

    public void SetFrequency(float f)
    {
        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), f, f);
    }

    public GameObject SpawnEnemy()
    {
        var obj = pool.Get();
        obj.GetComponent<Enemy>().Configure(PickRandomVariant());
        obj.GetComponent<PathFollower>().target = target;
        obj.transform.position = grid.CellToCellCenter(GetCell());

        return obj;
    }

    private SpawnConfig PickRandomVariant()
    {
        float[] weights = new float[configs.Length];
        
        for (var i = 0; i < configs.Length; i++)
        {
            weights[i] = configs[i].weight;
        }

        return Util.chooseWeighted(weights, configs, UnityEngine.Random.value);
    }
}

[Serializable]
public struct SpawnConfig
{
    public float weight;
    public uint kind;
    public Color color;
    public float health;
}