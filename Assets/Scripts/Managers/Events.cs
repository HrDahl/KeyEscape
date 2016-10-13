
using UnityEngine;

public class TakeDamageEvent : GameEvent
{
    public string message { get; private set; }

    public TakeDamageEvent(string message)
    {
        this.message = message;
    }
}
    