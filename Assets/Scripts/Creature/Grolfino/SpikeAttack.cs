using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] private GameObject spike;
    [SerializeField] private int numberOfSpikes;
    [SerializeField] private float timeBetweenSpikes;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            StartCoroutine(StartSpikeAttack(numberOfSpikes, timeBetweenSpikes));
        }
    }

    private IEnumerator StartSpikeAttack(int numberOfSpikes, float timeBetweenSpikes)
    {
        for (int i = 0; i < numberOfSpikes; i++)
        {
            GameObject s = Instantiate(spike, Vector3.forward * i, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpikes);      
        }
        yield return null;
    }
}
