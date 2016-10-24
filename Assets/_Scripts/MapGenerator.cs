using UnityEngine;
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

	public GameObject meleeAI;
	public GameObject shooterAI;
	public GameObject sniperAI;
	public GameObject cameraStand;
	public GameObject player;
	public GameObject room;
        public GameObject tutorialRoom;


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

				case "E":
					GameObject meleeEnemy = (GameObject)Instantiate (meleeAI, new Vector3 (x, 0.5f, -y), Quaternion.identity);
					meleeEnemy.transform.parent = containerList [0].transform;
					break;

                case "Q":
                     Instantiate (tutorialRoom, new Vector3 (x + 1.5f, 0.8f, -y), Quaternion.identity);
                     break;

				case "R":
					GameObject shooterEnemy = (GameObject)Instantiate (shooterAI, new Vector3 (x, 0.5f, -y), Quaternion.identity);
					shooterEnemy.transform.parent = containerList [0].transform;
					break;
				case "U":
					GameObject sniperEnemy = (GameObject)Instantiate (sniperAI, new Vector3 (x, 0.5f, -y), Quaternion.identity);
					sniperEnemy.transform.parent = containerList [0].transform;
					break;
				case "Y":
					GameObject cameraGo = (GameObject)Instantiate (cameraStand, new Vector3 (x, 0.5f, -y), Quaternion.identity);
					cameraGo.transform.parent = containerList [0].transform;
					break;
				case "H":
					GameObject hWall = (GameObject)Instantiate (obstacleList [0], new Vector3 (x - 0.1f, 1f, -y - 0.5f), obstacleList [0].transform.rotation);
					hWall.transform.parent = containerList [2].transform;
					break;

				case "V":
					GameObject vWall = (GameObject)Instantiate (obstacleList [1], new Vector3 (x - 0.5f, 1f, -y), obstacleList [1].transform.rotation);
					vWall.transform.parent = containerList [2].transform;
					break;

				case "B":
					GameObject box = (GameObject)Instantiate (obstacleList [5], new Vector3 (x, 0.5f, -y), Quaternion.identity);
					box.transform.parent = containerList [2].transform;
					break;

				case "C":
					GameObject barrel = (GameObject)Instantiate (obstacleList [3], new Vector3 (x + Random.Range (-0.2f, 0.2f), 0.5f, -y + Random.Range (-0.2f, 0.2f)), Quaternion.identity);
					barrel.transform.parent = containerList [2].transform;
					break;

				case "G":
					GameObject bridge = (GameObject)Instantiate (obstacleList [4], new Vector3 (x, 0.5f, -y - 0.5f), Quaternion.identity);
					bridge.transform.parent = containerList [2].transform;
					break;

				case "P":
					Instantiate (player, new Vector3 (x, 0.5f, -y), Quaternion.identity);
					break;

				case "K":
					GameObject cWall = (GameObject)Instantiate (obstacleList [2], new Vector3 (x - 0.1f, 1f, -y - 0.5f), Quaternion.identity);
					cWall.transform.parent = containerList [2].transform;
					break;
                    
				case "W":
					GameObject watchTower = (GameObject)Instantiate (environmentList [0], new Vector3 (x, 3.7f, -y), Quaternion.identity);
					watchTower.transform.parent = containerList [3].transform;
					break;

				case "1":
					GameObject greenKey = (GameObject)Instantiate (keyList [0], new Vector3 (x, 1f, -y), Quaternion.identity);
					greenKey.transform.parent = containerList [4].transform;
					break;                       

				case "2":
					GameObject blueKey = (GameObject)Instantiate (keyList [1], new Vector3 (x, 1f, -y), Quaternion.identity);
					blueKey.transform.parent = containerList [4].transform;
					break;       

				case "3":
					GameObject redKey = (GameObject)Instantiate (keyList [2], new Vector3 (x, 1f, -y), Quaternion.identity);
					redKey.transform.parent = containerList [4].transform;
					break;    

				case "4":
					GameObject rainbowKey = (GameObject)Instantiate (keyList [3], new Vector3 (x, 1f, -y), Quaternion.identity);
					rainbowKey.transform.parent = containerList [4].transform;
					break;       

				case "9":
					GameObject greenGate = (GameObject)Instantiate (gateList [0], new Vector3 (x + 0.4f, 1f, -y - 0.5f), Quaternion.identity);
					greenGate.transform.parent = containerList [5].transform;
					break;                           

				case "8":
					GameObject blueGate = (GameObject)Instantiate (gateList [1], new Vector3 (x + 0.4f, 1f, -y - 0.5f), Quaternion.identity);
					blueGate.transform.parent = containerList [5].transform;
					break;  
                    
				case "7":
					GameObject redGate = (GameObject)Instantiate (gateList [2], new Vector3 (x + 0.4f, 1f, -y - 0.5f), Quaternion.identity);
					redGate.transform.parent = containerList [5].transform;
					break;  
                    
				case "6":
					GameObject rainbowGate = (GameObject)Instantiate (gateList [3], new Vector3 (x + 0.4f, 1f, -y - 0.5f), Quaternion.identity);
					rainbowGate.transform.parent = containerList [5].transform;
					break;  
				case "Z":
					Instantiate (room, new Vector3 (x,0.5f, -y), Quaternion.identity);
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

		TextAsset data = Resources.Load ("Full_Level_Room2") as TextAsset;

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
