using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_CameraController : MonoBehaviour
{
    public Transform playerPos;
    
    void Update()
    {
        transform.position = new Vector3(playerPos.position.x - 10, playerPos.position.y + 9, playerPos.position.z - 10);
    }
}
