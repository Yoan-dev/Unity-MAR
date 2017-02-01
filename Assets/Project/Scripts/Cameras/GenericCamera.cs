using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCamera : AbstractReplayCamera {
    
	void Start ()
    {
        range = 30;
    }

    protected override void UpdateCamera()
    {
        transform.LookAt(car.transform);
    }
}
