// Adapted from https://github.com/nilesjohnson/hopf_fibration/blob/master/hopf_animate.sage

using System.Collections.Generic;
using UnityEngine;

public class HopfVisualization : MonoBehaviour {
	[SerializeField] float Thickness = 1f;
	[SerializeField] int NumSamples = 5;
	[SerializeField] int Resolution = 5;
	[SerializeField] int NumPoints = 10;

	[SerializeField] TwoSphere twoSphere;
	[SerializeField] List<Vector3> PointsOnS2 = new List<Vector3>();

	void Start() {
		UpdateVisualization();
	}

	void Update() {
		//if (Input.GetKeyDown(KeyCode.Space)) {
			UpdateVisualization();
		//}
	}

	void UpdateVisualization() { 
		PointsOnS2 = PointGenerator.Circle(NumPoints);
		twoSphere.SpawnDots(PointsOnS2);

		AddFibers(PointsOnS2);
	}

	// TODO: manager class + pooling
	[SerializeField] GameObject FiberTemplate;
	List<FiberGenerator> Fibers = new List<FiberGenerator>();

	void AddFibers(List<Vector3> PointsOnS2) {
		ClearFibers();

		foreach (Vector3 point in PointsOnS2) {
			FiberGenerator Fiber = Instantiate(FiberTemplate, transform).GetComponent<FiberGenerator>();
			Fiber.GenerateMesh(point, Thickness, NumSamples, Resolution);

			Fibers.Add(Fiber);
		}
	}

	void ClearFibers() {
		foreach (FiberGenerator Fiber in Fibers) Destroy(Fiber.gameObject);
		Fibers.Clear();
	}
}
