using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public GameObject sphere;

	private bool rotate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.R)) rotate = !rotate;
		if (rotate) transform.Rotate(new Vector3(-0.75f, 0, 0));
	}

	public void ShadowsOnOff (bool value) {
		ShadowOnOff (gameObject, value);
		ShadowOnOff (sphere, value);
	}

	private void ShadowOnOff (GameObject obj, bool value) {
		gameObject.GetComponent<MeshRenderer> ().receiveShadows = value;
			
	}
}
