using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_CharacterRotationController : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray, out hit)) {
        this.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }
}
