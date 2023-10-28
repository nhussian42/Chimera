using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    public Creature bossCreature;    

    public void SpawnBoss(FloorSO floorInfo, Transform parent)
    {
        Instantiate(floorInfo.boss, Vector3.zero, Quaternion.identity);
        _numCreaturesAlive++;
    }

}
