﻿using System;
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
    public float spawnCooldown;
    public SpawnConfig[] configs;
    private Wave currentWave;
    
    private GameObjectPool pool;

    private GameObject[] spawnPoints;
    private readonly Dictionary<GameObject, PrecalculatedPath> precalculatedPaths = new Dictionary<GameObject, PrecalculatedPath>();

    public int difficulty = 1;
    
    void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        pool = GetComponent<GameObjectPool>();

        var topTightCell = grid.GetTopRightCornerCell();
        for (var i = 0; i < spawnPoints.Length; i++)
        {
            var spawnerCell = grid.WorldToCell(spawnPoints[i].transform.position);
            spawnerCell.x = topTightCell.x;
            grid.CellToCellCenter(spawnerCell);
            spawnPoints[i].SetActive(false);
        }
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

    private PrecalculatedPath GetPathToTarget(GameObject from)
    {
        PrecalculatedPath precalced;
        if (!precalculatedPaths.TryGetValue(@from, out precalced))
        {
            precalced = new PrecalculatedPath
            {
                path = target.FindPathFrom(@from.transform.position),
                calculatedAt = Time.time
            };
            precalculatedPaths.Add(from, precalced);
        }

        if (grid.HasChangedSince(precalced.calculatedAt))
        {
            precalculatedPaths.Remove(from);
            return GetPathToTarget(from);
        }

        return precalced;
    }

    private void SpawnBatch()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var variant = currentWave.Pick(spawnPoint);
            if (variant.HasValue)
            {
                var path = GetPathToTarget(spawnPoint);
                var obj = pool.Get();
                obj.GetComponent<Enemy>().Configure(variant.Value, target, currentWave, path);
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
            var enemyCount = UnityEngine.Random.Range(waveNumber + difficulty, 5 + waveNumber * difficulty);
            var list = new Queue<SpawnConfig>(enemyCount);
            waveConfigs.Add(spawnPoint, list);
            for (var i = 0; i < enemyCount; i++)
            {
                list.Enqueue(PickRandomVariant());
            }
        }

        spawnCooldown = waveFrequency;
        return new Wave(waveNumber,  this, enemies, waveConfigs);
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

public struct PrecalculatedPath
{
    public List<Vector2Int> path;
    public float calculatedAt;
}

[Serializable]
public struct SpawnConfig
{
    public float weight;
    public string kind;
    public ColorType color;
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

    public Wave(int number, EnemySource source, HashSet<GameObject> enemies, Dictionary<GameObject, Queue<SpawnConfig>> configs)
    {
        this.number = number;
        this.source = source;
        this.enemies = enemies;
        this.configs = configs;
    }

    public ISet<GameObject> GetLivingEnemies()
    {
        return enemies;
    }

    public bool IsDead()
    {
        return enemies.Count == 0 && configs.Count == 0;
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
        if (IsDead())
        {
            source.WaveComplete(this);
        }
    }
}
