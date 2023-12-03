using UnityEngine;
using FMODUnity;

public class AudioEvents : Singleton<AudioEvents>
{
    [field: Header("Music Events")]
    [field: SerializeField] public EventReference OnMainMenuStarted { get; private set; }
    [field: SerializeField] public EventReference OnGameplayStarted { get; private set; }
    [field: SerializeField] public EventReference OnCombatStarted { get; private set; }
    [field: SerializeField] public EventReference OnDungeonBossStarted { get; private set; }
    [field: SerializeField] public EventReference OnShopEntered { get; private set; }

    [field: Header("SFX Events")]

    [field: Header("UI SFX Events")]
    [field: SerializeField] public EventReference OnGameStart { get; private set; }
    [field: SerializeField] public EventReference OnGameQuit { get; private set; }
    [field: SerializeField] public EventReference OnMenuButtonSelected { get; private set; }
    [field: SerializeField] public EventReference OnMenuButtonHovered { get; private set; }
    [field: SerializeField] public EventReference OnPurchaseItems { get; private set; }
    [field: SerializeField] public EventReference OnScrapLimb { get; private set; }

    [field: Header("Player SFX Events")]

    [field: Header("Player Attacks")]
    [field: SerializeField] public EventReference OnPlayerBaseAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerWolfAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerCrocAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerRhinoAttack { get; private set; }
    [field: SerializeField] public EventReference OnPlayerGeckoAttack { get; private set; }

    [field: Header("Player Movement")]
    [field: SerializeField] public EventReference OnPlayerWalk { get; private set; }
    [field: SerializeField] public EventReference OnPlayerBaseDash { get; private set; }
    [field: SerializeField] public EventReference OnPlayerPounce { get; private set; }
    [field: SerializeField] public EventReference OnPlayerBurrow { get; private set; }
    
    [field: Header("Player Status")]
    [field: SerializeField] public EventReference OnPlayerStun { get; private set; }

    [field: Header("Impact SFX Events")]
    [field: SerializeField] public EventReference OnPlayerDamagedSmall { get; private set; }
    [field: SerializeField] public EventReference OnPlayerDamagedLarge { get; private set; }
    [field: SerializeField] public EventReference OnPlayerLimbLost { get; private set; }
    [field: SerializeField] public EventReference OnPlayerDeath { get; private set; }
    [field: SerializeField] public EventReference OnPlayerHitConnectedWeak { get; private set; } // might need individual limbs to have their own connected sounds
    [field: SerializeField] public EventReference OnPlayerHitConnectedStrong { get; private set; }

    [field: Header("Creature SFX Events")]

    [field: Header("All Creatures")]
    [field: SerializeField] public EventReference OnCreatureDamaged { get; private set; } // Placeholder for all creatures take damage events

    [field: Header("Minor Enemies")]

    [field: Header("Hedgehog")]
    [field: SerializeField] public EventReference OnHedgehogAttack { get; private set; }
    [field: SerializeField] public EventReference OnHedgehogDamaged { get; private set; }
    [field: SerializeField] public EventReference OnHedgehogDeath { get; private set; }

    [field: Header("Tortoise")]
    [field: SerializeField] public EventReference OnTortoiseAttack { get; private set; }
    [field: SerializeField] public EventReference OnTortoiseDamaged { get; private set; }
    [field: SerializeField] public EventReference OnTortoiseDeath { get; private set; }

    [field: Header("Major Enemies")]

    [field: Header("Wolf")]
    [field: SerializeField] public EventReference OnWolfAttack { get; private set; }
    [field: SerializeField] public EventReference OnWolfDamaged { get; private set; }
    [field: SerializeField] public EventReference OnWolfDeath { get; private set; }

    [field: Header("Croc")]
    [field: SerializeField] public EventReference OnCrocAttack { get; private set; }
    [field: SerializeField] public EventReference OnCrocBurrow { get; private set; }
    [field: SerializeField] public EventReference OnCrocResurface { get; private set; }
    [field: SerializeField] public EventReference OnCrocDamaged { get; private set; }
    [field: SerializeField] public EventReference OnCrocDeath { get; private set; }
    
    [field: Header("Rhino")]
    [field: SerializeField] public EventReference OnRhinoSlam { get; private set; }
    [field: SerializeField] public EventReference OnRhinoCharge { get; private set; }
    [field: SerializeField] public EventReference OnRhinoDamaged { get; private set; }
    [field: SerializeField] public EventReference OnRhinoDeath { get; private set; }

    [field: Header("Gecko")]
    [field: SerializeField] public EventReference OnGeckoAttack { get; private set; }
    [field: SerializeField] public EventReference OnGeckoDash { get; private set; }
    [field: SerializeField] public EventReference OnGeckoDamaged { get; private set; }
    [field: SerializeField] public EventReference OnGeckoDeath { get; private set; }

    [field: Header("Bosses")]
    [field: Header("Centipede")]
    [field: SerializeField] public EventReference OnCentipedeSpawn { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeBurrow { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeSwipeAttack { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeScreamAttack { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeStompScreamAttack { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeSpreadAttack { get; private set; }
    [field: SerializeField] public EventReference OnCentipedeDefeated { get; private set; }

    [field: Header("Room SFX Events")]
    [field: SerializeField] public EventReference OnRoomCleared { get; private set; }
    [field: SerializeField] public EventReference OnDoorOpened { get; private set; }
    [field: SerializeField] public EventReference OnBossDoorOpened { get; private set; }
    [field: SerializeField] public EventReference OnPotBroken { get; private set; }
    [field: SerializeField] public EventReference OnSpikeTrapActivated { get; private set; }
    [field: SerializeField] public EventReference OnSpikeTrapRetracted { get; private set; }
}
