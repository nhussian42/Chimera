using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Tools/PublicClassesSO", order = 1)]
public class PublicClassesSO : ScriptableObject
{
    private PlayerController playerController;

    public void AddPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void UpdatePlayerStats()
    {
        playerController.UpdateStats();
    }

    public void UpdateBaseStats()
    {
        playerController.UpdateBaseStats();
    }
}
