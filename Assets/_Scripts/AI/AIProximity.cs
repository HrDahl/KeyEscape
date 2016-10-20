using UnityEngine;
using System.Collections;

public class AIProximity : MonoBehaviour
{

	Transform parent;
	AIns.FSM.AI ai; 

	// Use this for initialization
	void Start ()
	{
		parent = transform.parent;
		ai = parent.GetComponent<AIns.FSM.AI> ();
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


			RaycastHit hit;

			if (Physics.Raycast (parent.transform.position, direction.normalized, out hit, 5)) {
				if (hit.collider.gameObject.tag == "Player") {

					ai.playerSpotted = true;;
				} 

			}



		}
	}
}
