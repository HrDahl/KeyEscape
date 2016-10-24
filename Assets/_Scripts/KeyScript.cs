using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	void Update () {
        transform.Rotate(new Vector3(0f, 30f, 0f) * Time.deltaTime * 3);
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag ("Player")) {
            
            EventManager.Instance.TriggerEvent(new PickUpKey(gameObject));

            gameObject.SetActive(false);
        }

    }
}
