using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	public GameObject audioGO;
	AudioSource audio;

	void Start() {
		audio = audioGO.GetComponent<AudioSource> ();

	}

	void Update () {
        transform.Rotate(new Vector3(0f, 30f, 0f) * Time.deltaTime * 3);
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag ("Player")) {
			audio.Play ();
            EventManager.Instance.TriggerEvent(new PickUpKey(gameObject));

            gameObject.SetActive(false);
        }

    }
}
