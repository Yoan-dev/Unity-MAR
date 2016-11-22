using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

	private bool x;
	private bool y;
	private bool z;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.X)) x = !x;
		if (Input.GetKeyUp(KeyCode.Y)) y = !y;
		if (Input.GetKeyUp(KeyCode.Z)) z = !z;
		transform.Rotate(new Vector3((x)? 1 : 0, (y)? 1 : 0, (z)? 1 : 0));
	}
}
