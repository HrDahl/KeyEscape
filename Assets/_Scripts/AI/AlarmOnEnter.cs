using UnityEngine;
using System.Collections;

public class AlarmOnEnter : MonoBehaviour
{

	Transform parent;

	// Use this for initialization
	void Start ()
	{
		parent = transform.parent;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	void OnTriggerStay (Collider other)
	{ 
		if (other.gameObject.tag == "Player") {
				EventManager.Instance.TriggerEvent (new AlarmEvent (other.transform.position));
		}
	}
}
