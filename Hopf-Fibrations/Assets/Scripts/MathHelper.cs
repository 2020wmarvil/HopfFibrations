using UnityEngine;

public class MathHelper
{
	public static float TWO_PI = 2f * Mathf.PI;
	public static float ONE_OVER_PI = 1f / Mathf.PI;

	public static Color GetColorFromPointOnUnitSphere(Vector3 Point, float Alpha=1f) {
		return new Color(Point.x, Point.y, Point.z, Alpha);
	}

	public static Vector3 SphericalToCylindrical(float r, float az, float po) {
		float one = r * Mathf.Cos(az) * Mathf.Sin(po);
		float two = r * Mathf.Sin(az) * Mathf.Sin(po);
		float three = r * Mathf.Cos(po);

		return Vector3.Normalize(new Vector3(one, two, three));
	}
}
