using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    RaycastHit hit;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                GameObject obj = hit.collider.gameObject;
                Debug.Log("Clicked: " + obj);
            }
        }
    }
}
