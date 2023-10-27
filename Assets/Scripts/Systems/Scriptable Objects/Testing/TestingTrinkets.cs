using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingTrinkets : MonoBehaviour
{
    [SerializeField] List<Trinket> testObjects;
    [SerializeField] private int amount;

    [SerializeField] List<GameEvent> testEvents;

    [SerializeField] UnityEvent onTest;

    private void Start()
    {
        foreach (Trinket trinket in testObjects) { trinket.Disable(); }
        foreach (GameEvent gameEvent in testEvents) { gameEvent.UnsubscribeAll(); }
        foreach (Trinket trinket in testObjects) { trinket.Enable(); }
        foreach (Trinket trinket in testObjects) { trinket.Add(amount); }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Input E called");
            onTest.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Trinket trinket in testObjects) { trinket.ResetTrinket(); }
        }
    }

}
