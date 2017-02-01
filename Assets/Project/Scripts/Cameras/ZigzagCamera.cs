using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagCamera : AbstractReplayCamera {

    void Start ()
    {
        range = 80;
        transform.eulerAngles = new Vector3(90, 0, 0);
	}

    protected override void UpdateCamera()
    {
        // do nothing
    }
}
