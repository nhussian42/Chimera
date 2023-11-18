using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] protected bool spreadsOnSpawn;
    [SerializeField] protected bool flingsOnSpawn;
    [SerializeField] [Range(0, 1000)] protected float flingSpeed;

    private void Start()
    {

        Vector3 forceDirection = PointOnXZCircle(transform.position, 1f, Random.Range(0,360)).normalized * 20f + Vector3.up * 100f;
        GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }
    
}
