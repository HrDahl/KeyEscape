using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LSystem : MonoBehaviour {
	List<LSElement> str;
	int length0 = 1;
	public float w0 = 20;
	public float alpha1 = 30;
	public float alpha2 = -30;
	public float phi1 = 137;
	public float phi2 = 137;
	public float r1 = 0.8f;
	public float r2 = 0.8f;
	public float q = 0.5f;
	public float e = 0.5f;
	public float smin = 0;
	public int iter = 8;
    public GameObject mappleLeaf;

	List<Vector3> vertices = new List<Vector3>();
	List<int> indices = new List<int>();
	int count = 0;
    Mesh mesh;

    public float multiplierLeafs;

    public bool calculateMesh = false;

    void Awake(){
        if (calculateMesh) {
            calculateMesh = false;
            str = new List<LSElement>();
            ExpandRules();

            mesh = Interpret();
            mesh.RecalculateNormals();
            mesh.uv = new Vector2[mesh.vertexCount];

            mesh.RecalculateBounds();

            GetComponent<MeshFilter>().mesh = mesh; 

            transform.position = new Vector3(0f, -20f, 0f);
            mesh.name = gameObject.name;
            StartCoroutine(waitDisable());
        }
	}

    IEnumerator waitDisable() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

	void ExpandRules() {
		str.Clear();
		str.Add(new LSElement(LSElement.LSSymbol.A, length0, w0));

		Rule r = new Rule(alpha1, alpha2, phi1, phi2, r1, r2, q, e, smin);

		for (int i = 0; i < iter; i++){
			List<LSElement> outList = new List<LSElement>();

			foreach (var s in str){
				r.Apply(s, outList);
			}

			str = outList;
		}
	}

	void AddCone(Matrix4x4 m, float l, float w0, float w1){
        int N = 5;
        if (w0 < 0.01f) {
            N = 3;
        }

		for (int i = 0; i <= N; i++){
			
			float alpha = 2.0f*Mathf.PI*i/(float)N;
			Vector3 p0 = m.MultiplyPoint(new Vector3(w0 * Mathf.Cos(alpha), w0 * Mathf.Sin(alpha), 0));
			Vector3 p1 = m.MultiplyPoint(new Vector3(w1 * Mathf.Cos(alpha), w1 * Mathf.Sin(alpha), l));

			alpha = 2.0f*Mathf.PI*(i+1)/(float)N;
			Vector3 p2 = m.MultiplyPoint(new Vector3(w0 * Mathf.Cos(alpha), w0 * Mathf.Sin(alpha), 0));
			Vector3 p3 = m.MultiplyPoint(new Vector3(w1 * Mathf.Cos(alpha), w1 * Mathf.Sin(alpha), l));

			vertices.Add(p0);
			vertices.Add(p2);
			vertices.Add(p1);

			vertices.Add(p1);
			vertices.Add(p2);
			vertices.Add(p3);

            if (w0 < 0.04f) {

               interpolationLeaf(p0,p1,p2,w0);

            }

			for (int j = 0; j < 6; j++){
				indices.Add(indices.Count);
			}


		}
	}

    private void interpolationLeaf(Vector3 p0, Vector3 p1, Vector3 p2, float width) {
        List<Vector3> lerpVals = new List<Vector3>();
        Vector3 normal = Vector3.Cross(p1 - p0, p1 - p2).normalized;
        float ratioLeafs = 0;

        if (width > 0.03f) {
            ratioLeafs = 1f * multiplierLeafs;
        } else if (width > 0.02f) {
            ratioLeafs = 0.7f * multiplierLeafs;
        } else {
            ratioLeafs = 0.4f * multiplierLeafs;
        }

        for (float i = 0; i <= 0.2; i+=ratioLeafs) {
            lerpVals.Add(Vector3.Lerp(p0,p1,i));
        }

        foreach (var lerpVal in lerpVals) {
            GameObject leaf = (GameObject)Instantiate(mappleLeaf, new Vector3(lerpVal.x, lerpVal.y, lerpVal.z), Quaternion.identity, gameObject.transform);
            leaf.transform.up = normal;
        }
    }

	public Mesh Interpret(){
		Turtle turtle = new Turtle(w0);
		foreach (var elem in str){
			switch (elem.symbol){
			case LSElement.LSSymbol.DRAW:
                    AddCone(turtle.Peek().M, elem.data[0], turtle.GetWidth(), turtle.GetWidth() * elem.data[1]);
				turtle.Move(elem.data[0]);
				break;
			case LSElement.LSSymbol.TURN:
                    turtle.Turn(elem.data[0]);
				break;
			case LSElement.LSSymbol.ROLL:
				turtle.Roll(elem.data[0]);
				break;
                case LSElement.LSSymbol.LEFT_BRACKET:
				turtle.Push();
				break;
			case LSElement.LSSymbol.RIGHT_BRACKET:
                    turtle.Pop();
				break;
			case LSElement.LSSymbol.WIDTH:
                    turtle.SetWidth(elem.data[0]);
				break;
            }
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = indices.ToArray();
		vertices.Clear();
		indices.Clear();
		return mesh;
	}
}
