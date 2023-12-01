using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DebugControls : MonoBehaviour
{
    public static Action<int> DamageAllCreatures;
    public static Action SpawnDebugCreature;
    public static Action SpawnRandomCreature;
    public static Action toggleTrinketBuffMenu;
    public static Action TestTimerStart;
    public static Action TestTimerStop;
    public static Action DestroyAllDrops;
    public static Action SpawnRandomLimb;


    #if DEVELOPMENT_BUILD || UNITY_EDITOR
    private void Update()
    {
        // A = PlayerInputActions Move

        // B
        if (Input.GetKeyDown(KeyCode.B))
            TestTimerStop?.Invoke();
        // C
        if (Input.GetKeyDown(KeyCode.C))
            TestTimerStart?.Invoke();
        // D = PlayerInputActions Move

        // E = PlayerInputActions Pickup Item
        

        // F
        // G
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnRandomCreature?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerController.Instance.DistributeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
            SpawnDebugCreature?.Invoke();

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            toggleTrinketBuffMenu?.Invoke();
        }
        // R

        // S = PlayerInputActions Move

        // T
        // U
        // V

        // W = PlayerInputActions Move

        // X
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnRandomLimb?.Invoke();
        }
        // Z

        // Alpha 1 = PlayerInputActions UI Swap Menu Switch Limb
        // Alpha 2 = PlayerInputActions UI Swap Menu Switch Limb
        // Alpha 3
        // Alpha 4
        // Alpha 5
        // Alpha 6
        // Alpha 7
        // Alpha 8
        // Alpha 9

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyAllDrops.Invoke();
        }

        // -
        // =
        // [
        // ]
        // \
        // ;
        // '
        // ,
        // .
        
    }
    #else
    #endif
}


