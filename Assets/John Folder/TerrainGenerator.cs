using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour {


	int mapWidth = 10;
	int mapHeight = 10;

	Vector3[] vertices;
	int[] triangleVertices;

	int vertexIndex = 0;

	private Mesh mesh;

	private void Awake () {
		Generate();
	}

	private void Generate () {

		mesh = new Mesh();
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(mapWidth + 1) * (mapHeight + 1)];

		Vector2[] uv = new Vector2[vertices.Length];

		for (int y = 0; y < mapHeight + 1; y++) {
			for (int x = 0; x < mapWidth + 1; x++) {
				vertices [vertexIndex] = new Vector3 ((x - mapWidth/2), Random.Range(0f, 0.2f), y - mapHeight/2);
				uv[vertexIndex] = new Vector2((float) (x - mapWidth/2) / mapWidth, (float) (y - mapHeight/2) / mapHeight);
				vertexIndex++;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = uv;

		int[] triangles = new int[mapWidth * mapHeight * 6];
		for (int ti = 0, vi = 0, y = 0; y < mapHeight; y++, vi++) {
			for (int x = 0; x < mapWidth; x++, ti += 6, vi++) {
				triangles [ti] = vi;
				triangles [ti + 3] = triangles [ti + 2] = vi + 1;
				triangles [ti + 4] = triangles [ti + 1] = vi + mapWidth + 1;
				triangles [ti + 5] = vi + mapWidth + 2;
				mesh.triangles = triangles;
				mesh.RecalculateNormals();
			}
		}

	}

}
