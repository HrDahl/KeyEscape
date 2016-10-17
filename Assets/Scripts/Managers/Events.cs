
using UnityEngine;

public class TakeDamageEvent : GameEvent
{
    public string message { get; private set; }

    public TakeDamageEvent(string message)
    {
        this.message = message;
    }
}

public class InstantiateGame : GameEvent {

	public InstantiateGame () {}

}

public class PlayerSpottedEvent : GameEvent {

	public Vector3 lastSeenPosition { get; private set; }

	public PlayerSpottedEvent(Vector3 lastSeenPosition) {
		this.lastSeenPosition = lastSeenPosition;
	}

}
    