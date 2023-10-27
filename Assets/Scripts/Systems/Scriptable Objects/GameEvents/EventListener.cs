using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventListener : ScriptableObject
{
    [SerializeField][Tooltip("When do I need to calculate this value/do this thing?")] private List<GameEvent> subscribedEvents = new List<GameEvent>();
    bool isEnabled = false;
    protected bool activated { get; private set; }

    public bool IsEnabled { get { return isEnabled; } }

    public void Enable()
    {
        isEnabled = true;
        activated = false;
        foreach(GameEvent gameEvent in subscribedEvents)
        {
            gameEvent.Subscribe(this);
        }
    }

    public void Disable()
    {
        isEnabled = false;
        foreach (GameEvent gameEvent in subscribedEvents)
        {
            gameEvent.Unsubscribe(this);
        }
    }

    public abstract void Activate();

    public virtual void SetActivatedTrue()
    {
        activated = true;
    }

    public virtual void ResetEvent()
    {
        activated = false;
        Debug.Log("event reset");
    }

    
}
