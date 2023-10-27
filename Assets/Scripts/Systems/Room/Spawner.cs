using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnCondition
{
    OnLevelLoaded,

    OnAllEnemiesDefeated
}

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnPool;
    [SerializeField] private SpawnCondition _spawnCondition;

    private void Start()
    {
        if (TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            renderer.enabled = false;
    }

    private void OnEnable()
    {
        switch (_spawnCondition)
        {
            case SpawnCondition.OnLevelLoaded:
                SubscribeToLevelLoadedEvents();
                break;
            case SpawnCondition.OnAllEnemiesDefeated:
                SubscribeToAllEnemiesDefeatedEvents();
                break;
        }        
    }

    private void OnDisable()
    {
        switch (_spawnCondition)
        {
            case SpawnCondition.OnLevelLoaded:
                UnsubscribeFromLevelLoadedEvents();
                break;
            case SpawnCondition.OnAllEnemiesDefeated:
                UnsubscribeFromAllEnemiesDefeatedEvents();
                break;
        }
    }

    private void SubscribeToLevelLoadedEvents()
    {
        FloorManager.EnableFloor += SpawnRandomObjectFromList;
    }

    private void UnsubscribeFromLevelLoadedEvents()
    {
        FloorManager.EnableFloor -= SpawnRandomObjectFromList;
    }

    private void SubscribeToAllEnemiesDefeatedEvents()
    {
        FloorManager.AllCreaturesDefeated += SpawnRandomObjectFromList;
    }

    private void UnsubscribeFromAllEnemiesDefeatedEvents()
    {
        FloorManager.AllCreaturesDefeated += SpawnRandomObjectFromList;
    }

    private void SpawnRandomObjectFromList()
    {
        int spawnPoolIndex = Random.Range(0, _spawnPool.Count);
        Instantiate(_spawnPool[spawnPoolIndex], transform.position, Quaternion.identity);
    }

    private void SpawnSpecificGameObject(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
