using UnityEngine;

public class MathHelper
{
	public static float TWO_PI = 2f * Mathf.PI;
	public static float ONE_OVER_PI = 1f / Mathf.PI;

	public static Color GetColorFromPointOnUnitSphere(Vector3 Point, float Alpha=1f) {
		return new Color(Point.x, Point.y, Point.z, Alpha);
	}
}
