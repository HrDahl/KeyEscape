using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float maxHealth = 3f;
	public float currentHealth; 
	AudioSource aud;

	void OnEnable() {
		aud = GetComponent<AudioSource> ();
		currentHealth = maxHealth;
		EventManager.Instance.StartListening<TakeDamageEvent>(TakeDamage);
	}

	void OnDestroy() {
		EventManager.Instance.StopListening <TakeDamageEvent>(TakeDamage);
	}
		
	private void TakeDamage(TakeDamageEvent e) {

		currentHealth -= e.damage;

		if (currentHealth <= 0) {
			Debug.Log ("Ded");
			EventManager.Instance.TriggerEvent (new DeadEvent());
			aud.Play ();
		}

	}
}
