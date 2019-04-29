using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : PathTarget
{
    public PlayerBaseController playerBase;

    private void Awake()
    {
        playerBase = FindObjectOfType<PlayerBaseController>();
    }

    public override void Reached(PathFollower follower)
    {
        var enemy = follower.GetComponent<Enemy>();
        if (enemy)
        {
            playerBase.EnemyReached(enemy);
        }
    }
}
