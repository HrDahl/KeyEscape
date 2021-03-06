﻿using UnityEngine;
using System.Collections;

public class AIvision : MonoBehaviour
{

	public float fieldOfView = 120.0f;
	public float range = 12f;
	Transform parent; 
	public bool inSight = false;
	public bool chasing = false;
	GameObject player;
	AIns.FSM.AI ai; 

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		parent = transform.parent;
		ai = parent.GetComponent<AIns.FSM.AI> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (chasing) {
			RaycastHit hit;
			Vector3 direction = player.transform.position - parent.transform.position;
			if (Physics.Raycast (parent.transform.position, direction.normalized, out hit, range)) {
				if (hit.collider.gameObject.tag == "Player") {
					chasing = true;
					ai.playerSpotted = true;
				} else {
					chasing = false;
				}
			}
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Player") {

			Vector3 direction = other.transform.position - parent.transform.position;
			float angle = Vector3.Angle (direction, parent.transform.forward);

			if (angle < fieldOfView * 0.5) {
				RaycastHit hit;

				if (Physics.Raycast (parent.transform.position , direction.normalized, out hit, range)) {
					if (hit.collider.gameObject.tag == "Player") {
						inSight = true;
						chasing = true;
						ai.playerSpotted = true;;
					} else {
						inSight = false;
					}

				}
			
			}

		}
	}
}
