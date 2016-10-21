using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GateController : MonoBehaviour {

	bool canOpen = false;
	GameObject container;

	void OnEnable() {
		EventManager.Instance.StartListening<OpenGate>(OpenGate);
	}

	void OnDestroy() {
		EventManager.Instance.StopListening<OpenGate>(OpenGate);
	}

	void Start() {
		container = (GameObject)GameObject.FindGameObjectWithTag ("OpenGate");
	}

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag ("Player")) {

            List<GameObject> keysObtained = other.gameObject.GetComponent<PlayerController>().keysObtained;

            foreach (var key in keysObtained) {
                if (key.tag == gameObject.tag) {
					SetupButton (key);
					canOpen = true;
                    break;
                }
            }
        }
    }

	private void SetupButton(GameObject key) {
		int i = 0;
		foreach (Transform child in container.transform) {
			if (i > 0) {
				switch (gameObject.tag) {
				case "GreenPass":
					child.GetComponent<Image>().sprite = container.GetComponent<GateManager>().gateImage[0];
					break;
				case "BluePass":
					child.GetComponent<Image>().sprite = container.GetComponent<GateManager>().gateImage[1];
					break;
				case "RedPass":
					child.GetComponent<Image>().sprite = container.GetComponent<GateManager>().gateImage[2];
					break;
				case "RainbowPass":
					child.GetComponent<Image>().sprite = container.GetComponent<GateManager>().gateImage[3];
					break;
				default:
					break;
				}
			}
			child.gameObject.SetActive (true);
			i++;
		}
	}

	public void OpenGate(GameEvent e) {
		if (canOpen) {
			canOpen = false;
			foreach (Transform child in container.transform) {
				child.gameObject.SetActive (false);
			}			
			StartCoroutine (beginOpen());
		}
	}

	IEnumerator beginOpen() {
		Vector3 targetPosition = new Vector3 (transform.parent.position.x, transform.parent.position.y + 3f, transform.parent.position.z);

		while(Vector3.Distance(transform.parent.position, targetPosition) > 0.01f) {
			transform.parent.position = Vector3.MoveTowards(transform.parent.position, targetPosition, Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
}
