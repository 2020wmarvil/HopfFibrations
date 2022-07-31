using System.Collections.Generic;
using UnityEngine;

public class TwoSphere : MonoBehaviour {
	[SerializeField] GameObject TwoSphereDotTemplate;

	// TODO: pooling
	List<GameObject> TwoSphereDots = new List<GameObject>();

	public void SpawnDots(List<Vector3> PointsOnS2) {
		ClearPoints();

		float SphereRadius = transform.localScale.x / 2f;
		Vector3 SphereRadiusVector = new Vector3(SphereRadius, SphereRadius, SphereRadius);

		foreach (Vector3 point in PointsOnS2) {
			GameObject go = Instantiate(TwoSphereDotTemplate, transform);
			go.transform.position += point * SphereRadius;
			go.SetActive(true);

			// Set color from position
			go.GetComponent<Renderer>().material.color = MathHelper.GetColorFromPointOnUnitSphere((point + SphereRadiusVector) / 2f);

			TwoSphereDots.Add(go);
		}
	}

	void ClearPoints() {
		foreach (GameObject go in TwoSphereDots) Destroy(go);
		TwoSphereDots.Clear();
	}
}
