using UnityEngine;
using System.Collections;

public class FollowCam_Test : MonoBehaviour {

	public GameObject player;
	private Vector3 dist;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.Slerp (this.transform.position, player.transform.position - player.transform.forward*3 +  player.transform.up*2, 0.05f);
		//this.transform.rotation = Quaternion.Slerp (this.transform.rotation, player.transform.rotation, 0.015f);
		this.transform.LookAt (player.transform.position);
	}
}
