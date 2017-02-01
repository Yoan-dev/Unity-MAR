using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public GameObject otherCamera;
	
	void Update () {
		/*if (Input.GetKeyUp(KeyCode.C))
        {
            Switch();
        }*/
	}

    public void Switch()
    {
        otherCamera.SetActive(true);
        gameObject.SetActive(false);
    }
}
