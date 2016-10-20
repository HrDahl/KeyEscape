using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateController : MonoBehaviour {

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag ("Player")) {

            List<GameObject> keysObtained = other.gameObject.GetComponent<PlayerController>().keysObtained;



            foreach (var key in keysObtained) {
                Debug.Log(key.tag);
                Debug.Log(gameObject.tag);
                if (key.tag == gameObject.tag) {
                    transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 3f, transform.parent.position.z);
                    break;
                }
            }
        }

    }
}
