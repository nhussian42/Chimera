using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventListener : ScriptableObject
{
    [SerializeField][Tooltip("When do I need to calculate this value/do this thing?")] private List<GameEvent> subscribedEvents = new List<GameEvent>();
    bool isEnabled = false;

    public bool IsEnabled { get { return isEnabled; } }

    public void Enable()
    {
        isEnabled = true;
        foreach(GameEvent gameEvent in subscribedEvents)
        {
            gameEvent.Subscribe(this);
        }
    }

    public abstract void Activate();
}
