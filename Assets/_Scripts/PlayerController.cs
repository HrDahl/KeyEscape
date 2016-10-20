using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float maxHealth = 3f;
	public float currentHealth; 
    public List<GameObject> keysObtained;

	void OnEnable() {
		EventManager.Instance.StartListening<TakeDamageEvent>(TakeDamage);
        EventManager.Instance.StartListening<PickUpKey>(KeyPickUp);
	}

	void OnDestroy() {
        EventManager.Instance.StopListening <TakeDamageEvent>(TakeDamage);
        EventManager.Instance.StopListening<PickUpKey>(KeyPickUp);
	}
	
    void Start() {
        currentHealth = maxHealth;
    }

	private void TakeDamage(TakeDamageEvent e) {

		currentHealth -= e.damage;

		if (currentHealth <= 0) {
			Debug.Log ("Ded");
		}
	}

    private void KeyPickUp(PickUpKey e) {
        keysObtained.Add(e.key);
    }
}
