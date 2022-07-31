// Adaptation of BaseList from https://github.com/nilesjohnson/hopf_fibration/blob/master/hopf_animate.sage

using System.Collections.Generic;
using UnityEngine;

/** Start and end points should be points on S2 */
public class PointGenerator {
	public static List<Vector3> RandomPoints(int NumPoints) {
		List<Vector3> Points = new List<Vector3>();

		for (int i=0; i<NumPoints; i++) {
			Points.Add(Random.onUnitSphere);
		}

		return Points;
	}

	public static List<Vector3> Circle(int NumPoints) {
		List<Vector3> Points = new List<Vector3>();

		for (int i=0; i<NumPoints; i++) {
			float theta = i * MathHelper.TWO_PI / NumPoints;
			Points.Add(new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta)));
		}

		return Points;
	}

	public static List<Vector3> GreatCircle(Vector3 StartPoint, Vector3 EndPoint, int NumPoints) {
		List<Vector3> Points = new List<Vector3>();

		float a = Vector3.Dot(StartPoint, EndPoint);
		Vector3 w = EndPoint - a * StartPoint;

		Vector3 QPrime = Vector3.Normalize(w);

		float alpha = MathHelper.TWO_PI;

		for (int i=0; i<NumPoints; i++) {
			float alpha_i = alpha * i / NumPoints;
			Vector3 Point = StartPoint * Mathf.Cos(alpha_i) + QPrime * Mathf.Sin(alpha_i);
			Points.Add(Point);

			Debug.Log(i + " - " + Point);
		}

		return Points;
	}
}
