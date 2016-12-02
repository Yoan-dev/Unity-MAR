using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
	
	// UI
	public UnityEngine.UI.Slider lightSlider;
	public UnityEngine.UI.Toggle shadows;
	public UnityEngine.UI.Button resetTransform;
	public UnityEngine.UI.Toggle[] rotations;
	public UnityEngine.UI.Dropdown type;

	// Objects
	public Light light;
	public GameObject[] objects;
	public GameObject current;

	// Use this for initialization
	void Start () {
		lightSlider.onValueChanged.AddListener(delegate{UpdateLight();});
		shadows.onValueChanged.AddListener(delegate{UpdateShadows();});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void UpdateLight() {
		light.intensity = lightSlider.value;
	}

	private void UpdateShadows() {
		for (int i = 0; i < 4; i++)
			(objects [i].GetComponent<Entity> () as Entity).ShadowsOnOff (shadows.isOn);
	}
}
