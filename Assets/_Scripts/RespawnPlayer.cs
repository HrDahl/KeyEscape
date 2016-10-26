using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

	private GameObject player;
	private GameObject room2RespawnPosition;
	private GameObject room3RespawnPosition;
	private GameObject room4RespawnPosition;
	public GameManager gm; 
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
		gm = GetComponent<GameManager> ();
	}

	void Update () {
		if (gm.enabled == false) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				gm.enabled = true;
			}
		}
	}

	private void Respawn(DeadEvent e){
		if (numberOfOpenedGates == 2) {
			player.transform.position = room2RespawnPosition.transform.position;
			if (player.GetComponent<PlayerController> ().keysObtained.Count > 1) {
				player.GetComponentsInChildren<Transform>()[1].GetComponent<Renderer> ().material.color = Color.green;
				player.GetComponent<PlayerController> ().keysObtained [1].SetActive (true);
				player.GetComponent<PlayerController> ().keysObtained.RemoveAt(1);
				EventManager.Instance.TriggerEvent (new RemoveUI(3));
			}
		}
		if (numberOfOpenedGates == 4) {
			player.transform.position = room3RespawnPosition.transform.position;
			if (player.GetComponent<PlayerController> ().keysObtained.Count > 2) {
				player.GetComponentsInChildren<Transform>()[1].GetComponent<Renderer> ().material.color = Color.blue;
				player.GetComponent<PlayerController> ().keysObtained [2].SetActive (true);
				player.GetComponent<PlayerController> ().keysObtained.RemoveAt(2);
				EventManager.Instance.TriggerEvent (new RemoveUI(2));
			}
		}
		if (numberOfOpenedGates == 6) {
			player.transform.position = room4RespawnPosition.transform.position;
			if (player.GetComponent<PlayerController> ().keysObtained.Count > 3) {
				player.GetComponentsInChildren<Transform>()[1].GetComponent<Renderer> ().material.color = Color.red;
				player.GetComponent<PlayerController> ().keysObtained [3].SetActive (true);
				player.GetComponent<PlayerController> ().keysObtained.RemoveAt(3);
				EventManager.Instance.TriggerEvent (new RemoveUI(1));
			}
		}
	}

	private void GatesOpened(CompletedLevel e){
		numberOfOpenedGates++;

	}
}
