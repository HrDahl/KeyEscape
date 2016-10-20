
using UnityEngine;

public class TakeDamageEvent : GameEvent
{
    public float damage { get; private set; }

    public TakeDamageEvent(float damage)
    {
        this.damage = damage;
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

public class AlarmEvent : GameEvent {

	public Vector3 AlarmAtPosition { get; private set; }

	public AlarmEvent(Vector3 AlarmAtPosition) {
		this.AlarmAtPosition = AlarmAtPosition;
	}

}
 
public class DeadEvent : GameEvent {

	public DeadEvent() {
	}

}

public class PickUpPrefab : GameEvent {
    public GameObject key;

    public PickUpPrefab (GameObject key) {
        this.key = key;
    }
}