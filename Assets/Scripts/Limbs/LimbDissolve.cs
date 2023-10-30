using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDissolve : MonoBehaviour
{
    [SerializeField] float top;
    [SerializeField] float bottom;

    private void Start()
    {
        InvokeRepeating(nameof(DissolveDown), 0, 3f);
    }

    private void DissolveDown()
    {

    }

    // private IEnumerator DissolveDownwards()
    // {
    //     float value = top;
    //     while (value )
    // }
}
