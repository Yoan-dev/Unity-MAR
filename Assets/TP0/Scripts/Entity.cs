using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public GameObject sphere;

	private bool rotate;

	void Update () {
		if (rotate) transform.Rotate(new Vector3(-0.75f, 0, 0));
	}

	public void ShadowsOnOff (bool value) {
		ShadowOnOff (gameObject, value);
		ShadowOnOff (sphere, value);
	}

	private void ShadowOnOff (GameObject obj, bool value) {
		obj.GetComponent<MeshRenderer> ().receiveShadows = value;

	}

	public void RotationOnOff (bool value) {
		rotate = value;
	}
}
