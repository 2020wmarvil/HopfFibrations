using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FiberGenerator : MonoBehaviour {
	Mesh mesh;

	MeshFilter meshFilter;

	void Awake() {
		meshFilter = GetComponent<MeshFilter>();
		mesh = new Mesh();
	}

	/** Resolution determines the number of vertices in the fiber meshes' cross section */
	public void GenerateMesh(Vector3 Point, float Thickness, int NumSamples, int Resolution) {
		HopfFiber Fiber = new HopfFiber(Point);
		float TubeRadius = Thickness / 2f;

		int NumVerts = NumSamples * Resolution;

		Vector3[] Vertices = new Vector3[NumVerts];
		int[] Triangles = new int[NumVerts * 6];
		int TriangleIndex = 0;

		float phi_Increment = MathHelper.TWO_PI / NumSamples;
		float theta_Increment = MathHelper.TWO_PI / Resolution;

		Vector3[] PointSamples = new Vector3[NumSamples];
		for (int Sample = 0; Sample < NumSamples; Sample++) {
			float phi = Sample * phi_Increment; // [0, 2pi] in with NumSamples many samples
			PointSamples[Sample] = Fiber.GetPoint(phi);
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
		mesh.triangles = Triangles;
		mesh.RecalculateNormals();
		meshFilter.mesh = mesh;

		// Set color from position
		GetComponent<Renderer>().material.color = MathHelper.GetColorFromPointOnUnitSphere((Point + Vector3.one) / 2f);
	}
}
