using UnityEngine;

public class MathHelper
{
	public static Color GetColorFromPointOnUnitSphere(Vector3 Point, float Alpha=1f) {
		return new Color(Point.x, Point.y, Point.z, Alpha);
	}
}
