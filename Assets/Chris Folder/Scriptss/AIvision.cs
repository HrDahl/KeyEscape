using UnityEngine;
using System.Collections;

public class AIvision : MonoBehaviour
{

	public float fieldOfView = 30.0f;
	public Vector3 lastSighting; 
	public float range = 10f;
	public Transform parent; 
	public bool inSight = false;

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

			Vector3 direction = other.transform.position - parent.transform.position;
			float angle = Vector3.Angle (direction, parent.transform.forward);

			if (angle < fieldOfView * 0.5) {
				RaycastHit hit;

				if (Physics.Raycast (parent.transform.position , direction.normalized, out hit, range)) {
					Debug.Log (hit);
					if (hit.collider.gameObject.tag == "Player") {
						inSight = true;
						Debug.Log ("Sighted!");
						lastSighting = hit.transform.position;
						EventManager.Instance.TriggerEvent (new PlayerSpottedEvent (lastSighting));
					} else {
						inSight = false;
					}

				}
			
			}

		}
	}
}
