﻿using UnityEngine;
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

        int vertexIndex = 0;

		ground = (GameObject) Instantiate(ground, new Vector3(xPos, 0, -zPos), Quaternion.identity);

		Mesh mesh = new Mesh();
		mesh.name = "Procedural Grid";

		Vector3[] vertices = new Vector3[(rowCount + 1) * (colCount + 1)];

		Vector2[] uv = new Vector2[vertices.Length];

		for (int y = 0; y < colCount + 1; y++) {
			for (int x = 0; x < rowCount + 1; x++) {
				vertices [vertexIndex] = new Vector3 ((x - rowCount/2), Random.Range(0f, 0.3f), y - colCount/2);
				uv[vertexIndex] = new Vector2((float) (x - rowCount/2) / rowCount, (float) (y - colCount/2) / colCount);
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
				mesh.RecalculateNormals();
			}
        }

        ground.GetComponent<MeshFilter> ().mesh = mesh;
		ground.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (rowCount/5, colCount/5);
        ground.GetComponent<MeshCollider> ().sharedMesh = mesh;
	}

	private string[] ReadTextFile(){

		TextAsset data = Resources.Load ("Level2") as TextAsset;

		string[] content = data.text.Split('\n');

		return  content;

	}

}
