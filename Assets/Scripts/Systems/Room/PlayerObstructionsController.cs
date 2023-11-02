using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstructionsController : MonoBehaviour
{
    private PlayerController player;
    private Material mat;

    Vector4 playerPosProperty;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        //playerPosProperty = mat.GetVector()
    }

    private void Start()
    {
        player = PlayerController.Instance;
    }

    private void LateUpdate()
    {
        mat.SetVector("_PlayerWorldPos", player.transform.position);
    }
}
