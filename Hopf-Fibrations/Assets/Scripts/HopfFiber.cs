using UnityEngine;

// Parameterization of the Hopf Fiber, adapted from https://github.com/nilesjohnson/hopf_fibration/blob/master/fib_param.pyx

public class HopfFiber {
	float a, b, c;
	float alpha, beta;

	public HopfFiber(Vector3 basePoint) {
		a = basePoint.x;
		b = basePoint.y;
		c = basePoint.z;

		alpha = Mathf.Sqrt(0.5f * (1 + c));
		beta = Mathf.Sqrt(0.5f * (1 - c));
	}

	public Vector3 GetPoint(float phi) {
		float theta = Mathf.Atan2(-a, b) - phi;

		float w =      alpha * Mathf.Cos(theta);
		float x = -1 * beta  * Mathf.Cos(phi);
		float y =      alpha * Mathf.Sin(theta);
		float z = -1 * beta  * Mathf.Sin(phi);

		float ONE_OVER_PI = 1 / Mathf.PI;
		float rr = Mathf.Acos(w) / ONE_OVER_PI / Mathf.Sqrt(1 - w * w);
		return new Vector3(x, y, z) * rr;
	}
}
