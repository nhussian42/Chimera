using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDrop : MonoBehaviour
{
    [SerializeField] private float value;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Vector3 forceDirection = PointOnXZCircle(transform.position, 1f, Random.Range(0, 360)).normalized * 20f + Vector3.up * 100f;
        GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.Instance.AddBones(value);
            Destroy(gameObject);
            
        }
    }

}
