using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : MonoBehaviour {

    private bool recording;
    private IList<Vector3> replay;

    void Start()
    {
        recording = false;
        replay = new List<Vector3>();
    }

    void Update()
    {
        if (recording)
            replay.Add(transform.position);
    }

    void OnTriggerEnter (Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (recording)
                return;
            else
                recording = true;
        }
    }


}
