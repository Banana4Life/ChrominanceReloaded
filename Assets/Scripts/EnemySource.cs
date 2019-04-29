using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class EnemySource : MonoBehaviour
{
    public GridController grid;
    public EnemyTarget target;
    
    public float spawnFrequency = 1;
    public float waveFrequency = 10;
    public float spawnCd;
    public SpawnConfig[] configs;
    private Wave currentWave;
    
    private GameObjectPool pool;

    public GameObject[] spawnPoints;

    public int difficulty = 1;
    public int wave = 1;
    
    void Awake()
    {
        pool = GetComponent<GameObjectPool>();
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    public Vector2Int GetCell(GameObject spawn)
    {
        return grid.WorldToCell(spawn.transform.position);
    }

    public void SpawnWavePart()
    {
        spawnCd -= Time.deltaTime;
        if (spawnCd < 0)
        {
            spawnCd = spawnFrequency;
            foreach (var spawnPoint in spawnPoints)
            {
                var variant = currentWave.pick(spawnPoint);
                if (variant.HasValue)
                {
                    var obj = pool.Get();
                    obj.GetComponent<Enemy>().Configure(variant.Value, target);
                    obj.transform.position = grid.CellToCellCenter(GetCell(spawnPoint));
                    
                    currentWave.enemies.Add(obj);
                }
            }
        }
    }

    void Update()
    {
        if (currentWave.isDead())
        {
            currentWave = generateWave();
            spawnCd = waveFrequency;
            wave++;
        }

        SpawnWavePart();
    }


    Wave generateWave()
    {
        Wave newWave = new Wave();
        newWave.configs = new Dictionary<GameObject, Queue<SpawnConfig>>();
        newWave.enemies = new List<GameObject>();
        foreach (var spawnPoint in spawnPoints)
        {
            var list = new Queue<SpawnConfig>();
            newWave.configs.Add(spawnPoint, list);
            var randomCnt = UnityEngine.Random.Range(wave + difficulty, 5 * wave * difficulty);
            for (var i = 0; i < randomCnt; i++)
            {
                list.Enqueue(PickRandomVariant());
            }
        }
        return newWave;
    }

    private SpawnConfig PickRandomVariant()
    {
        float[] weights = new float[configs.Length];
        
        for (var i = 0; i < configs.Length; i++)
        {
            weights[i] = configs[i].weight;
        }

        return Util.chooseWeighted(weights, configs);
    }

    public bool CanReachTarget()
    {
        return target && PathFinder.ExistsPathBetween(grid, grid.WorldToCell(transform.position), target.GetCell());
    }
}

[Serializable]
public struct SpawnConfig
{
    public float weight;
    public string kind;
    public Color color;
    public float health;
    public float spinSpeed;
    public float walkSpeed;
}

public struct Wave
{
    public List<GameObject> enemies;

    public Dictionary<GameObject, Queue<SpawnConfig>> configs;

    public Boolean isDead()
    {
        if (configs == null)
        {
            return true;
        }
        if (configs.Count > 0)
        {
            return false;
        }
        foreach (var enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    public SpawnConfig? pick(GameObject spawnPoint)
    {
        Queue<SpawnConfig> queue;
        if (configs.TryGetValue(spawnPoint, out queue))
        {
            var cfg = queue.Dequeue();
            if (queue.Count == 0)
            {
                configs.Remove(spawnPoint);
            }

            return cfg;
        }

        return null;
    }
}
