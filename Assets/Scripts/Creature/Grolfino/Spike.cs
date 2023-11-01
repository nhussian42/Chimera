using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float spikeSpeed = 0.1f;
    void Start()
    {
        StartCoroutine(SpikeRiseFall(spikeSpeed));
    }

    private IEnumerator SpikeRiseFall(float duration)
    {
        float timer = duration;
        float posY;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            posY = Mathf.Lerp(1, -1, timer / duration);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            yield return null;
        }
        timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            posY = Mathf.Lerp(-1, 1, timer / duration);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
