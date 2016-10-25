using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	private GameObject player;
	private GameObject room2RespawnPosition;
	private GameObject room3RespawnPosition;
	private GameObject room4RespawnPosition;
	private int numberOfOpenedGates = 0;

	void OnEnable() {
		EventManager.Instance.StartListening<DeadEvent>(Respawn);
		EventManager.Instance.StartListening<CompletedLevel>(GatesOpened);
	}

	void OnDestroy() {
		EventManager.Instance.StopListening<DeadEvent>(Respawn);
		EventManager.Instance.StopListening<CompletedLevel>(GatesOpened);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		room2RespawnPosition = GameObject.FindGameObjectWithTag("RespawnRoom2");
		room3RespawnPosition = GameObject.FindGameObjectWithTag("RespawnRoom3");
		room4RespawnPosition = GameObject.FindGameObjectWithTag("RespawnRoom4");
	}

	private void Respawn(DeadEvent e){
		if (numberOfOpenedGates == 2) {
			player.transform.position = room2RespawnPosition.transform.position;
		}
		if (numberOfOpenedGates == 4) {
			player.transform.position = room3RespawnPosition.transform.position;
		}
		if (numberOfOpenedGates == 6) {
			player.transform.position = room4RespawnPosition.transform.position;
		}
	}

	private void GatesOpened(CompletedLevel e){
		numberOfOpenedGates++;
		Debug.Log (numberOfOpenedGates);
	}
}
