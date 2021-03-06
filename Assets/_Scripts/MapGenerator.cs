﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{

	public GameObject[] treeList;
	public GameObject[] obstacleList;
	public GameObject[] containerList;
	public GameObject[] environmentList;
	public GameObject[] keyList;
	public GameObject[] gateList;

    public GameObject[] roomList;

	public GameObject cameraStand;
	public GameObject player;

	public float steepFactor = 1f;
	int colCount = 0;
	int rowCount = 0;

	void OnEnable ()
	{
		EventManager.Instance.StartListening<InstantiateGame> (SpawnObjects);
	}

	void OnDisable ()
	{
		EventManager.Instance.StopListening<InstantiateGame> (SpawnObjects);
	}

	void Awake ()
	{
		treeList = GameObject.FindGameObjectsWithTag ("Tree");
	}

	private void SpawnObjects (GameEvent e)
	{

		string[] lvlFile = ReadTextFile ();

		instantiateGround (lvlFile);

		for (int y = 0; y < lvlFile.Length; y++) {

			for (int x = 0; x < lvlFile [y].Length; x++) {

				switch (lvlFile [y] [x].ToString ()) {
                    
				case "T":

					int index = Random.Range (0, treeList.Length);
					GameObject chosenTree = treeList [index];

					GameObject tree = (GameObject)Instantiate (chosenTree);
					Destroy (tree.GetComponent<LSystem> ());

					tree.transform.position = new Vector3 (x, 0f, -y);
					tree.transform.rotation = Quaternion.Euler (new Vector3 (tree.transform.rotation.x - 90f, tree.transform.rotation.y, tree.transform.rotation.z));
					tree.transform.parent = containerList [1].transform;

					break;

				case "B":
					GameObject box = (GameObject)Instantiate (obstacleList [5], new Vector3 (x, 0.5f, -y), Quaternion.identity);
					box.transform.parent = containerList [2].transform;
					break;

				case "C":
					GameObject barrel = (GameObject)Instantiate (obstacleList [3], new Vector3 (x + Random.Range (-0.2f, 0.2f), 0.5f, -y + Random.Range (-0.2f, 0.2f)), Quaternion.identity);
					barrel.transform.parent = containerList [2].transform;
					break;

				case "P":
                        Instantiate (player, new Vector3 (x, 0.5f, -y), Quaternion.Euler(new Vector3(0,90,0)));
					break;
                    
				case "W":
					GameObject watchTower = (GameObject)Instantiate (environmentList [0], new Vector3 (x, 3.7f, -y), Quaternion.identity);
					watchTower.transform.parent = containerList [2].transform;
					break;
               
                    case "1":
                        Instantiate(roomList[0], new Vector3(x + 1.5f, 0.8f, -y), Quaternion.identity);

                    break;

				case "2":
                    Instantiate (roomList[1], new Vector3 (x,0.5f, -y), Quaternion.Euler(0, 270, 0));
					break;

                case "3": 
                    Instantiate (roomList[2], new Vector3 (x, 0.4f, -y), Quaternion.Euler(0, 0, 0));
                    break;

                case "4": 
                    Instantiate (roomList[3], new Vector3 (x, 0.4f, -y), Quaternion.Euler(0, 180, 0));
                    break;

				default:
					break;
				}
			}
		}
	}

	public void instantiateGround (string[] lvlFile)
	{
		colCount = lvlFile.Length;
		rowCount = lvlFile [colCount - 1].Length;

		float xPos = (rowCount / 2f) - 0.5f;
		float zPos = (colCount / 2f) - 0.5f;

		int vertexIndex = 0;

		GameObject ground = (GameObject)Instantiate (environmentList [1], new Vector3 (xPos, 0, -zPos), Quaternion.identity);

		Mesh mesh = new Mesh ();
		mesh.name = "Procedural Grid";

		Vector3[] vertices = new Vector3[(rowCount + 1) * (colCount + 1)];

		Vector2[] uv = new Vector2[vertices.Length];


		for (int y = 0; y < colCount + 1; y++) {
			for (int x = 0; x < rowCount + 1; x++) {
				vertices [vertexIndex] = new Vector3 ((x - rowCount / 2), Gauss (x, y) + Random.Range (0f, 0.3f), y - colCount / 2);
				uv [vertexIndex] = new Vector2 ((float)(x - rowCount / 2) / rowCount, (float)(y - colCount / 2) / colCount);
				vertexIndex++;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[rowCount * colCount * 6];
		for (int ti = 0, vi = 0, y = 0; y < colCount; y++, vi++) {
			for (int x = 0; x < rowCount; x++, ti += 6, vi++) {
				triangles [ti] = vi;
				triangles [ti + 3] = triangles [ti + 2] = vi + 1;
				triangles [ti + 4] = triangles [ti + 1] = vi + rowCount + 1;
				triangles [ti + 5] = vi + rowCount + 2;
				mesh.triangles = triangles;
				mesh.RecalculateNormals ();
			}
		}

		ground.GetComponent<MeshFilter> ().mesh = mesh;
		ground.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (rowCount / 5, colCount / 5);
		ground.GetComponent<MeshCollider> ().sharedMesh = mesh;
		ground.transform.parent = containerList [3].transform;
	}

	private string[] ReadTextFile ()
	{

		TextAsset data = Resources.Load ("Full_Level") as TextAsset;

		string[] content = data.text.Split ('\n');

		return  content;

	}

	private float Gauss (int x, int y)
	{
		float gx;
		float gy;
		float g;

		int sideDivider = 10;
		int startSideLength = rowCount / sideDivider >= 1 ? (rowCount / sideDivider) : 1;

		int endXSideLength = colCount - startSideLength;
		int endYSideLength = rowCount - startSideLength;

		float height = 10f;
		float steepness = 2f * steepFactor;

		if (x < startSideLength && y < startSideLength) {
			gx = height * Mathf.Exp ((-1) * (Mathf.Pow ((x), 2f) / (Mathf.Pow (steepness, 2f))));
			gy = height * Mathf.Exp ((-1) * (Mathf.Pow ((y), 2f) / (Mathf.Pow (steepness, 2f))));
			g = (gx + gy) / 2;

		} else if (x < startSideLength && y >= startSideLength && y <= endXSideLength) {
			g = (height / 2f) * Mathf.Exp ((-1) * (Mathf.Pow ((x), 2f) / (Mathf.Pow (steepness, 2f))));

		} else if (x < startSideLength && y > endXSideLength) {
			gx = height * Mathf.Exp ((-1) * (Mathf.Pow ((x), 2f) / (Mathf.Pow (steepness, 2f))));
			gy = height * Mathf.Exp ((-1) * (Mathf.Pow ((y - colCount), 2f) / (Mathf.Pow (steepness, 2f))));
			g = (gx + gy) / 2;

		} else if (x >= startSideLength && x <= endYSideLength && y < startSideLength) {
			g = (height / 2f) * Mathf.Exp ((-1) * (Mathf.Pow ((y), 2f) / (Mathf.Pow (steepness, 2f))));

		} else if (x >= startSideLength && x <= endYSideLength && y > endXSideLength) {
			g = (height / 2f) * Mathf.Exp ((-1) * (Mathf.Pow ((y - colCount), 2f) / (Mathf.Pow (steepness, 2f))));

		} else if (x > endYSideLength && y < startSideLength) {
			gx = height * Mathf.Exp ((-1) * (Mathf.Pow ((x - rowCount), 2f) / (Mathf.Pow (steepness, 2f))));
			gy = height * Mathf.Exp ((-1) * (Mathf.Pow ((y), 2f) / (Mathf.Pow (steepness, 2f))));
			g = (gx + gy) / 2;

		} else if (x > endYSideLength && y >= startSideLength && y <= endXSideLength) {
			g = (height / 2f) * Mathf.Exp ((-1) * (Mathf.Pow ((x - rowCount), 2f) / (Mathf.Pow (steepness, 2f))));

		} else if (x > endYSideLength && y > endXSideLength) {
			gx = height * Mathf.Exp ((-1) * (Mathf.Pow ((x - rowCount), 2f) / (Mathf.Pow (steepness, 2f))));
			gy = height * Mathf.Exp ((-1) * (Mathf.Pow ((y - colCount), 2f) / (Mathf.Pow (steepness, 2f))));
			g = (gx + gy) / 2;

		} else {
			g = 0f;
		}

		return g; 
	}

}
