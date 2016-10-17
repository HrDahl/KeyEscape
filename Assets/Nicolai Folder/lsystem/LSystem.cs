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

    GameObject treeSide;
    Mesh mesh;

    List<Vector3> normals = new List<Vector3>();

	void Start(){
        treeSide = (GameObject)GameObject.FindGameObjectWithTag("Tree");

        if (treeSide == null) {
            str = new List<LSElement>();
            ExpandRules();

            mesh = Interpret();
            mesh.RecalculateNormals();
            mesh.uv = new Vector2[mesh.vertexCount];
            mesh.RecalculateBounds();
            mesh.Optimize();

            GetComponent<MeshFilter>().mesh = mesh;
        } else {
            GetComponent<MeshFilter>().mesh = mesh;
        }
	}

    void Update() {
       /* if (count % 10 == 0) {
            ExpandRules();

            mesh = Interpret();
            mesh.RecalculateNormals();
            mesh.uv = new Vector2[mesh.vertexCount];

            GetComponent<MeshFilter>().mesh = mesh;
        }

        count++; */
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

            if (w0 < 0.01f) {
                Vector3 side1 = p2 - p0;
                Vector3 side2 = p1 - p0;
                Vector3 perp = Vector3.Cross(side1, side2);
                var perpLength = perp.magnitude;
                perp /= perpLength;

                Vector3 randomPoint1 = (1 - Mathf.Sqrt(Random.Range(0.0f, 1.0f))) * p0 + (Mathf.Sqrt(Random.Range(0.0f, 1.0f)) * (1 - Random.Range(0.0f, 1.0f))) * p1;

                Debug.Log("Point: " + p3);
                Vector3 normal = p1 + (p2 - p1) + Random.Range(0.0f, 1.0f) * (p3 - p1);
                Debug.Log(perp);
                normals.Add(perp);
            }

			for (int j = 0; j < 6; j++){
				indices.Add(indices.Count);
			}


		}
        //Branch done
        Debug.Log("New Branch:");
	}

	public Mesh Interpret(){
		Turtle turtle = new Turtle(w0);
        Debug.Log(str.Count);
		foreach (var elem in str){
			switch (elem.symbol){
			case LSElement.LSSymbol.DRAW:
                    //Debug.Log("DRAW : " + turtle.Peek().M);
                    AddCone(turtle.Peek().M, elem.data[0], turtle.GetWidth(), turtle.GetWidth() * elem.data[1]);
				turtle.Move(elem.data[0]);
				break;
			case LSElement.LSSymbol.TURN:
                    //Debug.Log("TURN : " + turtle.Peek().M);
                    turtle.Turn(elem.data[0]);
				break;
			case LSElement.LSSymbol.ROLL:
				turtle.Roll(elem.data[0]);
				break;
                case LSElement.LSSymbol.LEFT_BRACKET:
                    //Debug.Log("PUSH (NEW BRANCH) : " + turtle.Peek().M);
				turtle.Push();
				break;
			case LSElement.LSSymbol.RIGHT_BRACKET:
                    //Debug.Log("POP (GO BACK BRANCH) : " + turtle.Peek().M);
                    turtle.Pop();
				break;
			case LSElement.LSSymbol.WIDTH:
                    //Debug.Log("WIDTH : " + turtle.Peek().M);
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
