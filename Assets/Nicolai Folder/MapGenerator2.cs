using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator2 : MonoBehaviour {

	public GameObject[] treeList;
	public GameObject enemy;
	public GameObject player;
	public GameObject ground;
	public GameObject parentTrees;
	public GameObject horizontalWall;
	public GameObject verticalWall;
	public GameObject box;
	public GameObject barrel;
	public GameObject bridge;
	public GameObject corner;
	public GameObject pickup;
	public GameObject watchtower;
	public GameObject parentEnemies;

	void OnEnable ()
	{
		EventManager.Instance.StartListening<InstantiateGame> (SpawnObjects);
	}

	void OnDisable ()
	{
		EventManager.Instance.StopListening<InstantiateGame> (SpawnObjects);
	}

	void Awake() {
		treeList = GameObject.FindGameObjectsWithTag("Tree");
	}

	public void SpawnObjects(GameEvent e){

		string[] lvlFile = ReadTextFile ();

		instantiateGround (lvlFile);

		for (int y = 0; y < lvlFile.Length; y++) {

			for (int x = 0; x < lvlFile[y].Length; x++) {

				switch (lvlFile[y][x].ToString()){



				case "T":

					int index = Random.Range(0, treeList.Length);
					GameObject chosenTree = treeList[index];

					GameObject tree = (GameObject)Instantiate(chosenTree);
					Destroy(tree.GetComponent<LSystem>());

					tree.transform.position = new Vector3(x, 0f, -y);
					tree.transform.rotation = Quaternion.Euler(new Vector3(tree.transform.rotation.x - 90f, tree.transform.rotation.y, tree.transform.rotation.z));
					tree.transform.parent = parentTrees.transform;

					break;

				case "E":
					GameObject childEnemy = (GameObject) Instantiate(enemy, new Vector3(x, 0.5f, -y), Quaternion.identity);
					childEnemy.transform.parent = parentEnemies.transform;
					break;


				case "H":
					Instantiate(horizontalWall, new Vector3(x - 0.1f, 1f, -y  - 0.5f), horizontalWall.transform.rotation);
					break;

				case "V":
					Instantiate(verticalWall, new Vector3(x - 0.5f, 1f, -y), verticalWall.transform.rotation);
					break;

				case "B":
					Instantiate(box, new Vector3(x , 0.5f, -y), Quaternion.identity);
					break;

				case "C":
					Instantiate(barrel, new Vector3(x + Random.Range (-0.2f, 0.2f), 0.5f, -y + Random.Range (-0.2f, 0.2f)), Quaternion.identity);
					break;

				case "G":
					Instantiate(bridge, new Vector3(x, 0.5f, -y - 0.5f), Quaternion.identity);
					break;

				case "P":
					Instantiate(player, new Vector3(x, 0.5f, -y), Quaternion.identity);
					break;

				case "K":
					Instantiate(corner, new Vector3(x - 0.1f, 1f, -y - 0.5f), Quaternion.identity);
					break;

				case "O":
					Instantiate(pickup, new Vector3(x - 0.1f, 1f, -y - 0.5f), Quaternion.identity);
					break;

				case "W":
					Instantiate(watchtower, new Vector3(x , 3.7f, -y), Quaternion.identity);
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
				//				vertices [vertexIndex] = new Vector3 ((x - rowCount / 2), Random.Range (0f, 0.3f), y - colCount / 2);
				vertices [vertexIndex] = new Vector3 ((x - rowCount / 2), Gauss(x, y) + Random.Range (0f, 0.3f), y - colCount / 2);
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

	private float Gauss(int x, int y){
		float gx;
		float gy;
		float g;

		if(x < 10 && y < 10){
			gx = 10f * Mathf.Exp((-1)*(Mathf.Pow((x - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			gy = 10f * Mathf.Exp((-1)*(Mathf.Pow((y - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			g = (gx + gy) / 2;
		}
		else if (x < 10 && y >= 10 && y <= 30){
			g = 5f * Mathf.Exp((-1)*(Mathf.Pow((x - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
		}
		else if(x < 10 && y > 30){
			gx = 10f * Mathf.Exp((-1)*(Mathf.Pow((x - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			gy = 10f * Mathf.Exp((-1)*(Mathf.Pow((y - 40f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			g = (gx + gy) / 2;
		}
		else if (x >= 10 && x <= 90 && y < 10) {
			g = 5f * Mathf.Exp((-1)*(Mathf.Pow((y - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
		} 
		else if (x >= 10 && x <= 90 && y > 30) {
			g = 5f * Mathf.Exp((-1)*(Mathf.Pow((y - 40f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
		} 
		else if(x > 90 && y < 10){
			gx = 10f * Mathf.Exp((-1)*(Mathf.Pow((x - 100f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			gy = 10f * Mathf.Exp((-1)*(Mathf.Pow((y - 0f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			g = (gx + gy) / 2;
		}
		else if (x > 90 && y >= 10 && y <= 30){
			g = 5f * Mathf.Exp((-1)*(Mathf.Pow((x - 100f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
		}
		else if(x > 90 && y > 30){
			gx = 10f * Mathf.Exp((-1)*(Mathf.Pow((x - 100f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			gy = 10f * Mathf.Exp((-1)*(Mathf.Pow((y - 40f), 2f)/(Mathf.Pow(2f * 1f, 2f))));
			g = (gx + gy) / 2;
		}

		else {
			//g = Random.Range (0f, 0.3f);
			g = 0f;
		}
		Debug.Log (g);
		return g; 
	}

}
