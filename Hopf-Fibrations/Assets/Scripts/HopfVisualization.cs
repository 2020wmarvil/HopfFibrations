// Adapted from https://github.com/nilesjohnson/hopf_fibration/blob/master/hopf_animate.sage

using System.Collections.Generic;
using UnityEngine;

public class HopfVisualization : MonoBehaviour {
	[SerializeField] TwoSphere twoSphere;

	[SerializeField] List<Vector3> PointsOnS2 = new List<Vector3>();

	void Update() {
		DrawFrame(PointsOnS2);
	}

	void DrawFrame(List<Vector3> PointsOnS2) {
		// TODO: initialize fibration system?

		twoSphere.SpawnDots(PointsOnS2);

		// TODO: AddFibers(PointsOnS2)
	}
}
