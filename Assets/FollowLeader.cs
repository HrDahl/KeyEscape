using UnityEngine;
using System.Collections;

public class FollowLeader : MonoBehaviour {

	NavMeshAgent agent;
	public GameObject leader;
	public int index;


	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (index == 0) {
			agent.SetDestination (leader.transform.position + Vector3.left+ Vector3.left);
		} else if (index == 1) {
			agent.SetDestination (leader.transform.position + Vector3.right + Vector3.right);
		} else if (index == 2) {
			agent.SetDestination (leader.transform.position + Vector3.forward+ Vector3.forward);
		} else if (index == 3) {
			agent.SetDestination (leader.transform.position + Vector3.back+ Vector3.back);
		}
	}
}
