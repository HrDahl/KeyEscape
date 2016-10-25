using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour {

    public Image[] keyList; 
    public Sprite[] keyMaterials;
	public Material rainbow;
	private GameObject player;

    public Sprite keyPlaceholder;

    void OnEnable() {
        EventManager.Instance.StartListening<PickUpKey>(KeyPickUp);
        EventManager.Instance.StartListening<StartTimer>(RestartGame);
    }

    void OnDestroy() {
        EventManager.Instance.StopListening<PickUpKey>(KeyPickUp);
        EventManager.Instance.StopListening<StartTimer>(RestartGame);
    }

    private void RestartGame(StartTimer e) {
        foreach (var key in keyList) {
            key.GetComponent<Image>().sprite = keyPlaceholder;
        }
    }

    private void KeyPickUp(PickUpKey e) {
		player = GameObject.FindGameObjectWithTag("Ethan");
        switch (e.key.name) {
            case "Green Key":
                keyList[0].GetComponent<Image>().sprite = keyMaterials[0];
				player.GetComponent<Renderer> ().material.color = Color.green;
                break;
            case "Blue Key":
                keyList[1].GetComponent<Image>().sprite = keyMaterials[1];
				player.GetComponent<Renderer> ().material.color = Color.blue;
                break;
            case "Red Key":
                keyList[2].GetComponent<Image>().sprite = keyMaterials[2];
				player.GetComponent<Renderer> ().material.color = Color.red;
                break;
            case "Rainbow Key":
                keyList[3].GetComponent<Image>().sprite = keyMaterials[3];
				player.GetComponent<Renderer> ().material = rainbow;
                break;
            default:
                break;
        }
    }
}
