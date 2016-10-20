using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour {

    public Image[] keyList; 
    public Sprite[] keyMaterials;

    void OnEnable() {
        EventManager.Instance.StartListening<PickUpKey>(KeyPickUp);
    }

    void OnDestroy() {
        EventManager.Instance.StopListening<PickUpKey>(KeyPickUp);
    }

    private void KeyPickUp(PickUpKey e) {
        Debug.Log(e.key.name);
        switch (e.key.name) {
            case "Green Key(Clone)":
                keyList[0].GetComponent<Image>().sprite = keyMaterials[0];
                break;
            case "Blue Key(Clone)":
                keyList[1].GetComponent<Image>().sprite = keyMaterials[1];
                break;
            case "Red Key(Clone)":
                keyList[2].GetComponent<Image>().sprite = keyMaterials[2];
                break;
            case "Rainbow Key(Clone)":
                keyList[3].GetComponent<Image>().sprite = keyMaterials[3];
                break;
            default:
                break;
        }

    }
}
