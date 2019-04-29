using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class EnemySource : MonoBehaviour
{
    public GridController grid;
    public EnemyTarget target;
    
    public float spawnFrequency = 1;
    public float waveFrequency = 10;
    public float spawnCooldown;
    public SpawnConfig[] configs;
    private Wave currentWave;
    
    private GameObjectPool pool;

    public GameObject[] spawnPoints;

    public int difficulty = 1;
    
    void Awake()
    {
        pool = GetComponent<GameObjectPool>();
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    private void Start()
    {
        currentWave = PrepareWave(1);
    }

    void Update()
    {
        SpawnWavePart();
    }

    public Wave GetCurrentWave()
    {
        return currentWave;
    }

    public void SpawnWavePart()
    {
        spawnCooldown -= Time.deltaTime;
        if (spawnCooldown < 0)
        {
            spawnCooldown = spawnFrequency;
            SpawnBatch();
        }
    }

    private void SpawnBatch()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var variant = currentWave.Pick(spawnPoint);
            if (variant.HasValue)
            {
                var obj = pool.Get();
                obj.GetComponent<Enemy>().Configure(variant.Value, target, currentWave);
                obj.transform.position = grid.CellToCellCenter(grid.WorldToCell(spawnPoint.transform.position));
                    
                currentWave.AddEnemy(obj);
            }
        }
    }


    private Wave PrepareWave(int waveNumber)
    {
        var waveConfigs = new Dictionary<GameObject, Queue<SpawnConfig>>();
        var enemies = new HashSet<GameObject>();
        foreach (var spawnPoint in spawnPoints)
        {
            var list = new Queue<SpawnConfig>();
            waveConfigs.Add(spawnPoint, list);
            var randomCnt = UnityEngine.Random.Range(waveNumber + difficulty, 5 * waveNumber * difficulty);
            for (var i = 0; i < randomCnt; i++)
            {
                list.Enqueue(PickRandomVariant());
            }
        }

        spawnCooldown = waveFrequency;
        return new Wave(waveNumber, this, enemies, waveConfigs);
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

    public void WaveComplete(Wave completeWave)
    {
        currentWave = PrepareWave(completeWave.number + 1);
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

public class Wave
{
    public readonly int number;
    private readonly EnemySource source;
    private readonly HashSet<GameObject> enemies;
    private readonly Dictionary<GameObject, Queue<SpawnConfig>> configs;
    private readonly int size;
    private int killed;

    public Wave(int number, EnemySource source, HashSet<GameObject> enemies, Dictionary<GameObject, Queue<SpawnConfig>> configs)
    {
        this.number = number;
        this.source = source;
        this.enemies = enemies;
        this.configs = configs;
        size = configs.Select(e => e.Value.Count).Sum();
    }

    public int EnemiesLeft()
    {
        return size - killed;
    }

    public bool IsDead()
    {
        return killed >= size;
    }

    public SpawnConfig? Pick(GameObject spawnPoint)
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

    public void AddEnemy(GameObject e)
    {
        enemies.Add(e);
    }

    public void EnemyDied(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject);
        killed++;
        if (IsDead())
        {
            source.WaveComplete(this);
        }
    }
}
