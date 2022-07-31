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

		// TODO: real resolution?
		int TempResolution = 2;

		Vector3[] Vertices = new Vector3[NumSamples * TempResolution];
		int[] Triangles = new int[NumSamples * TempResolution * 3];
		int TriangleIndex = 0;

		Vector3 FirstPoint = Fiber.GetPoint(0f);

		Vertices[0] = FirstPoint + Vector3.up * TubeRadius;
		Vertices[1] = FirstPoint + Vector3.down * TubeRadius;

		for (int Sample = 1; Sample < NumSamples; Sample++) {
			float phi = MathHelper.TWO_PI / (NumSamples - 1); // [0, 2pi] in with NumSamples many samples
			Vector3 BasePoint = Fiber.GetPoint(phi);

			Vector3 P2 = BasePoint + Vector3.up * TubeRadius;
			Vector3 P3 = BasePoint + Vector3.down * TubeRadius;

			int BaseVertexIndex = Sample * TempResolution;

			int P0_Index = BaseVertexIndex - TempResolution;
			int P1_Index = BaseVertexIndex - TempResolution + 1;
			int P2_Index = BaseVertexIndex + 0;
			int P3_Index = BaseVertexIndex + 1;

			Vertices[P2_Index] = P2;
			Vertices[P3_Index] = P3;

			// TODO: connect to two points around next point (modulus trick)?

			Triangles[TriangleIndex++] = P0_Index;
			Triangles[TriangleIndex++] = P2_Index;
			Triangles[TriangleIndex++] = P1_Index;

			Triangles[TriangleIndex++] = P0_Index;
			Triangles[TriangleIndex++] = P3_Index;
			Triangles[TriangleIndex++] = P1_Index;
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
