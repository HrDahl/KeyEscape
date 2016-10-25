using UnityEngine;
using System.Collections;

public class AlarmOnEnter : MonoBehaviour
{

	void OnTriggerStay (Collider other)
	{ 
		if (other.gameObject.tag == "Player") {
				EventManager.Instance.TriggerEvent (new AlarmEvent (other.transform.position));
		}
	}
}
