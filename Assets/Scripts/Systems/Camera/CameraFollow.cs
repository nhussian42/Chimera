using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 startOffest;

    private void Start()
    {
        startOffest = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = _target.position + startOffest;
    }

}
