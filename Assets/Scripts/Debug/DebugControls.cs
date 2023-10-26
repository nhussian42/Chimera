using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControls : MonoBehaviour
{
    public static Action<int> DamageCreatures;

    private void Update()
    {
        // A = WALK
        // B
        // C
        // D = WALK
        // E = AMON STUFF // TRINKET TIMER
        // F
        // G
        // H
        // I
        // J
        // K = PLAYERCONTROLLER SCRAP LIMB
        
        if (Input.GetKeyDown(KeyCode.L))
            PlayerController.Instance.UpdateCoreHealth(+10);
        if (Input.GetKeyDown(KeyCode.M))
            PlayerController.Instance.AddBones(100);
        // N = PLAYERCONTROLLER SWAP BUTTON
        if (Input.GetKeyDown(KeyCode.O))
            PlayerController.Instance.UpdateCoreHealth(-10);
        if (Input.GetKeyDown(KeyCode.P))
            DamageCreatures?.Invoke(10);
        // Q
        // R
        // S = WALK
        // T
        // U
        // V
        // W = WALK
        // X
        // Y
        // Z
    }
}
