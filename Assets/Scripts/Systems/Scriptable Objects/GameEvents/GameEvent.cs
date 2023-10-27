using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Scriptable Objects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    private List<EventListener> listeners = new List<EventListener>();

    public void Subscribe(EventListener newListener) { listeners.Add(newListener); }

    public void Unsubscribe(EventListener newListener) { listeners.Remove(newListener); }

    public void UnsubscribeAll()
    {
        listeners.Clear();
    }

    public void Invoke()
    {
        foreach(EventListener listener in listeners)
        {
            listener.Activate();
        }
    }
}
