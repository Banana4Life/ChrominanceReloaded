using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class EnemySource : MonoBehaviour
{
    public GridController grid;
    public EnemyTarget target;
    public float initialSpawnFrequency;
    
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
        obj.GetComponent<Enemy>().target = target;
        obj.transform.position = grid.CellToCellCenter(GetCell());

        return obj;
    }
}
