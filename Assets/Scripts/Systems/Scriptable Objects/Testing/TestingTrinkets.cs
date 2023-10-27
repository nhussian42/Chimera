using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingTrinkets : MonoBehaviour
{
    public List<EventListener> testObjects;

    public List<GameEvent> testEvents;

    public UnityEvent onTest;

    private void Start()
    {
        foreach (EventListener listener in testObjects) { listener.Disable(); }
        foreach (GameEvent gameEvent in testEvents) { gameEvent.UnsubscribeAll(); }
        foreach (EventListener listener in testObjects) { listener.Enable(); }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            onTest.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            foreach (EventListener listener in testObjects) { listener.ResetEvent(); }
        }
    }

}
