using UnityEngine;

[System.Serializable]
public struct LissajousCurve {
    [Range(1, 4)]
    public float A_Amp;
    [Range(1, 4)]
    public float B_Amp;
    [Range(1, 4)]
    public float C_Amp;

    [Range(1, 10)]
    public int A_Freq;
    [Range(1, 10)]
    public int B_Freq;
    [Range(1, 10)]
    public int C_Freq;

    [Range(0, Mathf.PI * 2)]
    public float B_Phase;
    [Range(0, Mathf.PI * 2)]
    public float C_Phase;
}

public class LissajousCurves : MonoBehaviour {
	[SerializeField] float Thickness = 1f;
	[SerializeField] int NumSamples = 5;
	[SerializeField] int Resolution = 5;
    [SerializeField] LissajousCurve Curve;
	[SerializeField] GameObject CurveTemplate;

	LissajousGenerator Lissajous;

    void Start() {
		Lissajous = Instantiate(CurveTemplate, transform).GetComponent<LissajousGenerator>();
    }

	void Update() {
        Lissajous.GenerateMesh(Curve, Thickness, NumSamples, Resolution);
	}
}
