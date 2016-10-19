using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	private ParticleSystem pSystem;

	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(15, 30, 45) * Time.deltaTime);
	}


	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			gameObject.SetActive (false);
			pSystem.Play();
		}

	}

}
