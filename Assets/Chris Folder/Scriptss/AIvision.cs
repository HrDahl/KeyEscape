using UnityEngine;
using System.Collections;

public class AIvision : MonoBehaviour
{

	public float fieldOfView = 30.0f;
	public Vector3 lastSighting; 
	public float range = 10f;


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);

			if (angle < fieldOfView * 0.5) {
				//Debug.Log ("Sighted!");
				RaycastHit hit;

				if (Physics.Raycast (transform.position , direction.normalized, out hit, range)) {
					Debug.Log (hit);
					if (hit.collider.gameObject.tag == "Player") {
						Debug.Log ("Sighted!");
						lastSighting = hit.transform.position;
						EventManager.Instance.TriggerEvent (new PlayerSpottedEvent (lastSighting));
					}

				}
			
			}

		}
	}
}
