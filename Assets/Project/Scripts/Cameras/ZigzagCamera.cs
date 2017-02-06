using UnityEngine;

// replay camera placed above the zigzag sections
public class ZigzagCamera : AbstractReplayCamera {

    void Start ()
    {
        range = 75;
        transform.eulerAngles = new Vector3(90, 0, 0);
	}

    protected override void UpdateCamera()
    {
        // do nothing
    }
}
