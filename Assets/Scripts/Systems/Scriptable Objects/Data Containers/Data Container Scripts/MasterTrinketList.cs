using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/Player Data/MasterTrinketList", order = 1)]
public class MasterTrinketList : ScriptableObject
{
    [SerializeField][Tooltip("A list of all trinkets in the game (ADD NEW TRINKETS HERE)")]
    List<Trinket> masterList; //(trinkets are NEVER added/removed from this list at runtime)

    [SerializeField][Tooltip("A list of all trinkets that the player has picked up (DO NOT ADD TRINKETS TO THIS LIST)")]
    public List<Trinket> playerInventory; // get; private set; later for OOP purposes - Amon

    [SerializeField][Tooltip("A list of trinkets that are in the current run (DO NOT ADD TRINKETS TO THIS LIST)")]
    List<Trinket> gameInventory; // (initialized at start, trinkets that are OneTime trinkets are removed from here at runtime)

    [SerializeField][Tooltip("The list of 3 unique, random trinkets that the current trinket bag contains (DO NOT ADD TRINKETS TO THIS LIST)")]
    List<Trinket> bagInventory;

    [SerializeField]
    [Tooltip("Placeholder for storing Relic Information (DO NOT ADD TRINKETS TO THIS LIST)")]
    public Trinket Relic;

    List<Trinket> rarityInventory; // this list is used to store all trinkets of a specified rarity so the manager can pick from them

    // Call before starting a new session (a new run)
    public void FullReset()
    {
        // Clearing runtime lists and disabling what trinkets were there, disabling trinkets in master list
        foreach (Trinket trinket in playerInventory) { trinket.Disable(); }
        playerInventory.Clear();
        foreach(Trinket trinket in gameInventory) { trinket.Disable(); }
        gameInventory.Clear();
        foreach (Trinket trinket in masterList) { trinket.Disable(); }
        foreach(Trinket trinket in bagInventory) { trinket.Disable(); }
        bagInventory.Clear();
        Relic = null;

        // Game inventory copies master list, this serves as the pool from which trinkets will be pulled from in-game
        foreach (Trinket trinket in masterList) { gameInventory.Add(trinket); }
    }

    // Call before any GameEvent is invoked
    public void InitializeTrinkets()
    {
        foreach(Trinket trinket in playerInventory)
        {
            trinket.ResetTrinket();
        }
    }

    // Call when player opens a trinket bag to get a unique, random trinket from the game inventory
    public Trinket GetRandomTrinket()
    {
        Trinket randomTrinket = gameInventory[Random.Range(0, gameInventory.Count)];
        bagInventory.Add(randomTrinket);
        gameInventory.Remove(randomTrinket);

        return randomTrinket;
    }

    // Call when player has selected a trinket from the trinket bag
    public void Pickup(Trinket trinket, int amount)
    {
        // See if the player already has this trinket
        bool hasTrinket = false;
        
        foreach(Trinket playersTrinket in playerInventory)
        {
            if(playersTrinket.TrinketName == trinket.TrinketName)
            {
                // If player already has trinket, just add how many they picked up to that instance
                hasTrinket = true;
                playersTrinket.Add(amount);
                Debug.Log(playersTrinket.TrinketName + " " + playersTrinket.amount);
            }
        }

        if (hasTrinket == false) // If player does not have trinket, add to their inventory, enable it, and add how many they picked up
        {
            playerInventory.Add(trinket);
            trinket.Enable();
            trinket.Add(amount);
        }

        switch (trinket.TrinketType)
        {
            case TrinketType.OneTime: // if the trinket the player pickup up can only be picked up once, keep it removed from game inventory
                bagInventory.Remove(trinket);
                break;
            case TrinketType.Stackable: // if the trinket the player picked up is stackable, add it back to the game's inventory
                bagInventory.Remove(trinket);
                gameInventory.Add(trinket);
                break;
        }
        ResetTrinketBag();
    }

    // Clears trinket bag for next pickup
    private void ResetTrinketBag()
    {
        for(int i = 0; i < bagInventory.Count; i++)
        {
            gameInventory.Add(bagInventory[i]);
            bagInventory.Remove(bagInventory[i]);
            
        }
        if(bagInventory != null)
        {
            gameInventory.Add(bagInventory[0]);
            bagInventory.Remove(bagInventory[0]);
        }
    }

    public void AddRelic(Trinket trinket)
    {
        Relic = trinket;
        playerInventory.Add(trinket);
        trinket.Enable();
        trinket.Add(1);
    }





}
