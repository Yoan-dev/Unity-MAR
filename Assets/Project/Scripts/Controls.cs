using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public GameObject guide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Z)) GetComponent<Rigidbody>().AddForce((guide.transform.position - transform.position).normalized * 20.0f);
        if (Input.GetKey(KeyCode.S)) GetComponent<Rigidbody>().AddForce((transform.position - guide.transform.position).normalized * 10.0f);
        if (Input.GetKey(KeyCode.Q)) GetComponent<Rigidbody>().AddTorque(new Vector3(0, -10.0f, 0));
        if (Input.GetKey(KeyCode.D)) GetComponent<Rigidbody>().AddTorque(new Vector3(0, 10.0f, 0));
    }
}
