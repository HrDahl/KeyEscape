using UnityEngine;
using System.Collections;

public class AlertSound : MonoBehaviour {

	AudioSource audio;
	bool played = false;

	// Use this for initialization
	void Start () {
		
		audio = GetComponent<AudioSource>(); 
	}

	void OnEnable() {
		EventManager.Instance.StartListening<PlayerSpottedEvent> (PlayerSpotted);
	}

	void OnDestroy() {
		EventManager.Instance.StopListening<PlayerSpottedEvent> (PlayerSpotted);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerSpotted(PlayerSpottedEvent e) 
	{
		Debug.Log ("hello");
		if (!played) {
			audio.Play ();
			played = true;
		} 
	}
}
