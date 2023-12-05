using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    public Creature bossCreature;
    public bool bossSpawned;

    public void SpawnBoss()
    {
        bossCreature = Instantiate(bossCreature, new Vector3(transform.position.x, transform.position.y, transform.position.z + 3), Quaternion.identity);
        _numCreaturesAlive++;
    }

    private void Update()
    {
        if (bossSpawned && !BossHealthBarUI.Instance.entered)
        {
            BossHealthBarUI.Instance.UpdateHealthBar(bossCreature.CurrentHealth, 250);
        }
        
    }
}
