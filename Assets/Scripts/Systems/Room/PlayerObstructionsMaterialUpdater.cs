using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstructionsMaterialUpdater : MonoBehaviour
{
    private PlayerController player;
    private Material mat;

    Vector4 playerPosProperty;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        player = PlayerController.Instance;
    }

    private void LateUpdate()
    {
        if (player != null)
            mat.SetVector("_PlayerWorldPos", player.transform.position);
    }
}
