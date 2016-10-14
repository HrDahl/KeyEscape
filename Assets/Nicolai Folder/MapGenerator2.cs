using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator2 : MonoBehaviour {

	public GameObject tree;
	public GameObject enemy;
	public GameObject player;
	public GameObject ground;

	void OnEnable ()
	{
		EventManager.Instance.StartListening<InstantiateGame> (SpawnObjects);
	}

	void OnDisable ()
	{
		EventManager.Instance.StopListening<InstantiateGame> (SpawnObjects);
	}

	private void SpawnObjects(GameEvent e){

		string[] lvlFile = ReadTextFile ();

		instantiateGround (lvlFile);

		for (int y = 0; y < lvlFile.Length; y++) {
			
			for (int x = 0; x < lvlFile[y].Length; x++) {
				
				switch (lvlFile[y][x].ToString()){

				case "T":
					Instantiate(tree, new Vector3(x, 0f, -y), tree.transform.rotation);
					break;

				case "E":
					Instantiate(enemy, new Vector3(x, 0.5f, -y), Quaternion.identity);
					break;

				case "P":
					Instantiate(player, new Vector3(x, 0.5f, -y), Quaternion.identity);
					break;

				default:
					break;
				}
			}
		}
	}

	public void instantiateGround(string[] lvlFile) {
		int colCount = lvlFile.Length;
		int rowCount = lvlFile[colCount-1].Length;

		float xPos = (rowCount / 2f) - 0.5f;
		float zPos = (colCount / 2f) - 0.5f;

		ground = (GameObject) Instantiate(ground, new Vector3(xPos, 0, -zPos), Quaternion.identity);

		ground.transform.localScale = new Vector3(rowCount * 0.1f, 1f, colCount * 0.1f);
	}

	private string[] ReadTextFile(){

		TextAsset data = Resources.Load ("Level2") as TextAsset;

		string[] content = data.text.Split('\n');

		return  content;

	}
}
