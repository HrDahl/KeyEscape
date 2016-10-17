using UnityEngine;
using System.Collections;

public class addMesh : MonoBehaviour {


    GameObject tree;

	// Use this for initialization
	void Start () {
        tree = (GameObject)GameObject.FindGameObjectWithTag("Tree");
        GetComponent<MeshFilter>().mesh = tree.GetComponent<MeshFilter>().mesh;
	}
	

}
