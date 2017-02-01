using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	// UI
	public UnityEngine.UI.Slider lightSlider;
	public UnityEngine.UI.Toggle shadows;
	public UnityEngine.UI.Toggle rotations;

	// Objects
	public new Light light;
	public GameObject[] objects;

	void Start () {
		lightSlider.onValueChanged.AddListener(delegate{UpdateLight();});
		shadows.onValueChanged.AddListener(delegate{UpdateShadows();});
		rotations.onValueChanged.AddListener(delegate{UpdateRotations();});
	}

	private void UpdateLight() {
		light.intensity = lightSlider.value;
	}

	private void UpdateShadows() {
		for (int i = 0; i < objects.Length; i++)
			(objects [i].GetComponent<Entity> () as Entity).ShadowsOnOff (shadows.isOn);
	}

	private void UpdateRotations() {
		for (int i = 0; i < objects.Length; i++)
			(objects [i].GetComponent<Entity> () as Entity).RotationOnOff (rotations.isOn);
	}
}
