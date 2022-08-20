using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LissajousGenerator : MonoBehaviour {
	Mesh mesh;

	MeshFilter meshFilter;

	void Awake() {
		meshFilter = GetComponent<MeshFilter>();
		mesh = new Mesh();
	}

    Vector3 LissajousPoint(LissajousCurve Curve, float T) {
        float X = Curve.A_Amp * Mathf.Sin(T * Curve.A_Freq);
        float Y = Curve.B_Amp * Mathf.Sin(T * Curve.B_Freq + Curve.B_Phase);
        float Z = Curve.C_Amp * Mathf.Sin(T * Curve.C_Freq + Curve.C_Phase);
        return new Vector3(X, Y, Z);
    }

	/** Resolution determines the number of vertices in the fiber meshes' cross section */
	public void GenerateMesh(LissajousCurve Curve, float Thickness, int NumSamples, int Resolution) {
		float TubeRadius = Thickness / 2f;

		int NumVerts = NumSamples * Resolution;

		Vector3[] Vertices = new Vector3[NumVerts];
		Vector2[] UVs = new Vector2[NumVerts];
		int[] Triangles = new int[NumVerts * 6];
		int TriangleIndex = 0;

		float phi_Increment = MathHelper.TWO_PI / NumSamples;
		float theta_Increment = MathHelper.TWO_PI / Resolution;

		Vector3[] PointSamples = new Vector3[NumSamples];
		for (int Sample = 0; Sample < NumSamples; Sample++) {
			float phi = Sample * phi_Increment; // [0, 2pi] in with NumSamples many samples
			PointSamples[Sample] = LissajousPoint(Curve, phi);
		}

		for (int Sample = 0; Sample < NumSamples; Sample++) {
			int BaseVertexIndex = Sample * Resolution;

			int PreviousSampleIndex = Sample - 1;
			if (PreviousSampleIndex < 0) PreviousSampleIndex += NumSamples;

			Vector3 PreviousPoint = PointSamples[PreviousSampleIndex];
			Vector3 BasePoint = PointSamples[Sample];
			Vector3 NextPoint = PointSamples[(Sample + 1) % NumSamples];

			Vector3 LocalForward = Vector3.Normalize(((BasePoint - PreviousPoint) + (NextPoint - BasePoint)) * 0.5f);
			Vector3 LocalRight = Vector3.Cross(Vector3.up, LocalForward);
			Vector3 LocalUp = Vector3.Cross(LocalForward, LocalRight);

			for (int i = 0; i < Resolution; i++) {
				float theta = i * theta_Increment; // [0, 360)

				// P0, P2, P3, P1 form a clockwise quad from this ring to the next ring
				int P0_Index = BaseVertexIndex + i;
				int P1_Index = BaseVertexIndex + (i + 1) % Resolution;
				int P2_Index = (P0_Index + Resolution) % NumVerts;
				int P3_Index = (P1_Index + Resolution) % NumVerts;

				Vector3 LocalPointUnscaled = LocalRight * Mathf.Cos(theta) + LocalUp * Mathf.Sin(theta);
				Vertices[P0_Index] = PointSamples[Sample] + LocalPointUnscaled * TubeRadius;
                UVs[P0_Index] = new Vector2(Sample / (NumSamples - 1f), 0f);

				Triangles[TriangleIndex++] = P0_Index;
				Triangles[TriangleIndex++] = P2_Index;
				Triangles[TriangleIndex++] = P1_Index;

				Triangles[TriangleIndex++] = P1_Index;
				Triangles[TriangleIndex++] = P2_Index;
				Triangles[TriangleIndex++] = P3_Index;
			}
		}

		mesh.Clear();
		mesh.vertices = Vertices;
		mesh.uv = UVs;
		mesh.triangles = Triangles;
		mesh.RecalculateNormals();
		meshFilter.mesh = mesh;
	}
}

