using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour {

	void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {

        }
    }
}
