using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public GameObject tree;
	public GameObject enemy;
	public GameObject player;

	// Use this for initialization
	void Start () {
		SpawnObjects ();
	}

	public string[] ReadTextFile(){
		
		TextAsset data = Resources.Load ("Level") as TextAsset;

		string content = data.text;

		return  content.Split ('\n');

	}

	private void SpawnObjects(){
		
		for (int y = 0; y < ReadTextFile().Length; y++) {
			
			for (int x = 0; x < ReadTextFile()[y].Length; x++) {
				
				switch (ReadTextFile()[y][x].ToString()){

				case "0":
					break;

				case "T":
					Instantiate(tree, new Vector3(x + 0.5f, 0, -y - 0.5f), Quaternion.identity);
					break;

				case "E":
					Instantiate(enemy, new Vector3(x + 0.5f, 0, -y - 0.5f), Quaternion.identity);
					break;

				case "P":
					Instantiate(player, new Vector3(x + 0.5f, 0, -y - 0.5f), Quaternion.identity);
					break;
				}
			}
		}
	}
}
