using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControls : MonoBehaviour
{
    public static Action<int> DamageAllCreatures;

    private void Update()
    {
        // A = PlayerInputActions Move

        // B
        // C

        // D = PlayerInputActions Move

        // E = AMON STUFF // TRINKET TIMER

        // F
        // G
        // H
        // I
        // J

        // K = PlayerController Scrap Limb Debug

        if (Input.GetKeyDown(KeyCode.L))
            PlayerController.Instance.UpdateCoreHealth(+10);

        if (Input.GetKeyDown(KeyCode.M))
            PlayerController.Instance.AddBones(100);

        // N = PlayerInputActions Swap Button

        if (Input.GetKeyDown(KeyCode.O))
            PlayerController.Instance.UpdateCoreHealth(-10);

        if (Input.GetKeyDown(KeyCode.P))
            DamageAllCreatures?.Invoke(10);

        // Q
        // R

        // S = PlayerInputActions Move

        // T
        // U
        // V

        // W = PlayerInputActions Move
        
        // X
        // Y
        // Z
    }
}
