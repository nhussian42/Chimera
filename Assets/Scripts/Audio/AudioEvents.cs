using UnityEngine;
using FMODUnity;

public class AudioEvents : Singleton<AudioEvents>
{
    [field: Header("Music Events")]
    [field: SerializeField] public EventReference OnMainMenuStarted { get; private set; }
    [field: SerializeField] public EventReference OnGameplayStarted { get; private set; }
    [field: SerializeField] public EventReference OnCombatStarted { get; private set; }
    [field: SerializeField] public EventReference OnBoss1Started { get; private set; }
    [field: SerializeField] public EventReference OnShopEntered { get; private set; }

    [field: Header("SFX Events")]

    [field: Header("UI SFX Events")]
    [field: SerializeField] public EventReference OnGameStart { get; private set; }
    [field: SerializeField] public EventReference OnGameQuit { get; private set; }
    [field: SerializeField] public EventReference OnMenuButtonSelected { get; private set; }
    [field: SerializeField] public EventReference OnMenuButtonClicked { get; private set; }

    [field: Header("Player SFX Events")]
    [field: SerializeField] public EventReference OnPlayerWalk { get; private set; }
    [field: SerializeField] public EventReference OnPlayerBaseAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerLightAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerHeavyAttack { get; private set; }

    [field: Header("Enemy SFX Events")]
    [field: Header("Hedgehog")]
    [field: SerializeField] public EventReference OnHedgehogSpawn { get; private set; }
    [field: SerializeField] public EventReference OnHedgehogAttack { get; private set; }
    [field: SerializeField] public EventReference OnHedgehogDeath { get; private set; }
    
    [field: Header("Wolf")]
    [field: SerializeField] public EventReference OnWolfSpawn { get; private set; }
    [field: SerializeField] public EventReference OnWolfAttack { get; private set; }
    [field: SerializeField] public EventReference OnWolfDeath { get; private set; }

    [field: Header("Croc")]
    [field: SerializeField] public EventReference OnCrocSpawn { get; private set; }
    [field: SerializeField] public EventReference OnCrocAttack { get; private set; }
    [field: SerializeField] public EventReference OnCrocDeath { get; private set; }

    [field: Header("Rhino")]
    [field: SerializeField] public EventReference OnRhinoSpawn { get; private set; }
    [field: SerializeField] public EventReference OnRhinoAttack { get; private set; }
    [field: SerializeField] public EventReference OnRhinoDeath { get; private set; }

    [field: Header("Boss1")]
    [field: SerializeField] public EventReference OnBoss1Spawn { get; private set; }
    [field: SerializeField] public EventReference OnBoss1Attack { get; private set; }
    [field: SerializeField] public EventReference OnBoss1Death { get; private set; }

    [field: Header("Room SFX Events")]
    [field: SerializeField] public EventReference OnRoomCleared { get; private set; }

    [field: Header("Environment SFX Events")]
    [field: SerializeField] public EventReference OnPotBroken { get; private set; }

    [field: Header("Ambience Events")]
    [field: SerializeField] public EventReference OnWaterfallApproached { get; private set; }




    private void OnEnable()
    {
        MainMenu.StartPressed += () => AudioManager.PlaySound2D(OnGameStart);
    }

    private void OnDisable()
    {
        MainMenu.StartPressed -= () => AudioManager.PlaySound2D(OnGameStart);
    }
}
