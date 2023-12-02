using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    public Creature bossCreature;

    public void SpawnBoss()
    {
        Instantiate(bossCreature, new Vector3(transform.position.x, transform.position.y, transform.position.z + 3), Quaternion.identity);
        _numCreaturesAlive++;
    }

}
