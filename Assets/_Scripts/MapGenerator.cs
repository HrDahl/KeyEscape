using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	public GameObject[] treeList;
    public GameObject[] obstacleList;
    public GameObject[] containerList;
    public GameObject[] environmentList;

	public GameObject enemy;
	public GameObject player;
	public GameObject pickup;

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
                        tree.transform.parent = containerList[1].transform;

					break;

				case "E":
					GameObject childEnemy = (GameObject) Instantiate(enemy, new Vector3(x, 0.5f, -y), Quaternion.identity);
                        childEnemy.transform.parent = containerList[0].transform;
					break;


				case "H":
                        GameObject hWall = (GameObject)Instantiate(obstacleList[0], new Vector3(x - 0.1f, 1f, -y  - 0.5f), obstacleList[0].transform.rotation);
                        hWall.transform.parent = containerList[2].transform;
                        break;

				case "V":
                        GameObject vWall = (GameObject)Instantiate(obstacleList[1], new Vector3(x - 0.5f, 1f, -y), obstacleList[1].transform.rotation);
                        vWall.transform.parent = containerList[2].transform;
                        break;

				case "B":
                        GameObject box = (GameObject)Instantiate(obstacleList[5], new Vector3(x , 0.5f, -y), Quaternion.identity);
                        box.transform.parent = containerList[2].transform;
                        break;

				case "C":
                        GameObject barrel = (GameObject)Instantiate(obstacleList[3], new Vector3(x + Random.Range (-0.2f, 0.2f), 0.5f, -y + Random.Range (-0.2f, 0.2f)), Quaternion.identity);
                        barrel.transform.parent = containerList[2].transform;
                        break;

				case "G":
                        GameObject bridge = (GameObject)Instantiate(obstacleList[4], new Vector3(x, 0.5f, -y - 0.5f), Quaternion.identity);
                        bridge.transform.parent = containerList[2].transform;
                        break;

				case "P":
					Instantiate(player, new Vector3(x, 0.5f, -y), Quaternion.identity);
					break;

				case "K":
                        GameObject cWall = (GameObject)Instantiate(obstacleList[2], new Vector3(x - 0.1f, 1f, -y - 0.5f), Quaternion.identity);
                        cWall.transform.parent = containerList[2].transform;
                        break;

				case "O":
					Instantiate(pickup, new Vector3(x - 0.1f, 1f, -y - 0.5f), Quaternion.identity);
					break;

				case "W":
                        GameObject watchTower = (GameObject)Instantiate(environmentList[0], new Vector3(x , 3.7f, -y), Quaternion.identity);
                        watchTower.transform.parent = containerList[3].transform;
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

        GameObject ground = (GameObject) Instantiate(environmentList[1], new Vector3(xPos, 0, -zPos), Quaternion.identity);

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
        ground.transform.parent = containerList[3].transform;
	}

	private string[] ReadTextFile(){

        TextAsset data = Resources.Load ("Full_Level") as TextAsset;

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
			g = 0f;
		}
		return g; 
	}

}
