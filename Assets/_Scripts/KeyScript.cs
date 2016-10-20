using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	void Update () {
        transform.Rotate(new Vector3(0f, 0f, 30) * Time.deltaTime * 3);
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag ("Player")) {
            Debug.Log("Hit: " + other.gameObject.name);

            EventManager.Instance.TriggerEvent(new PickUpPrefab(gameObject));

            gameObject.SetActive(false);
        }

    }
}
