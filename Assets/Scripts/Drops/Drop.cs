using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Drop Spread")]
    [SerializeField] protected bool spreadsOnSpawn = true;
    [SerializeField] protected float spreadDistance = 2f;
    [SerializeField] protected bool flingsOnSpawn = true;
    [SerializeField][Range(0, 1000)] protected float flingSpeed;
    [SerializeField][Range(0, 1000)] protected float upwardForce;

    [SerializeField] PickupIndicator pickupIndicator;

    private void OnEnable()
    {
        DebugControls.DestroyAllDrops += DestroyDrop;
    }

    private void OnDisable()
    {
        DebugControls.DestroyAllDrops -= DestroyDrop;
    }

    private void Start()
    {
        DetermineDropSpawn();
    }

    private Vector3 PointOnXZCircle(Vector3 center, float radius, float angle)
    {
        float a = angle * Mathf.PI / 180f;
        return center + new Vector3(Mathf.Sin(a), 0, Mathf.Cos(a)) * radius;
    }

    private void DetermineDropSpawn()
    {
        Vector3 spreadXZ = PointOnXZCircle(transform.position, spreadDistance, Random.Range(0, 360)).normalized;

        if (spreadsOnSpawn && !flingsOnSpawn)
            transform.position = spreadXZ;
        else if (!spreadsOnSpawn && flingsOnSpawn)
            GetComponent<Rigidbody>().AddForce(Vector3.up * upwardForce * flingSpeed, ForceMode.Impulse);
        else if (spreadsOnSpawn && flingsOnSpawn)
        {
            Vector3 spreadVector = spreadXZ * flingSpeed + Vector3.up * upwardForce;
            GetComponent<Rigidbody>().AddForce(spreadVector, ForceMode.Impulse);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController pc))
            pc.AddToDrops(this);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController pc))
            pc.RemoveFromDrops(this);
    }

    public void DestroyDrop()
    {
        Destroy(gameObject);
    }

    public void EnablePickupIndicator()
    {
        if (pickupIndicator != null)
            pickupIndicator.gameObject.SetActive(true);
    }

    public void DisablePickupIndicator()
    {
        if (pickupIndicator != null)
            pickupIndicator.gameObject.SetActive(false);
    }
}
