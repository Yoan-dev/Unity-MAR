using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractReplayCamera : MonoBehaviour, IReplayCamera {

    protected ReplayCamerasManager manager;
    protected GameObject car;
    protected float range;
	
	void Update ()
    {
        if (!manager.Activated) return;
        if (car == null) car = GameObject.Find("Replay(Clone)");
        if (car != null && Vector3.Distance(gameObject.transform.position, car.transform.position) < range)
        {
            //Debug.Log(Vector3.Distance(gameObject.transform.position, car.transform.position) + " < " + range);
            SetActive(true);
            manager.ChangeActiveCamera(this);
        }
        if (GetComponent<UnityEngine.Camera>().enabled)
        {
            UpdateCamera();
            if (Vector3.Distance(gameObject.transform.position, car.transform.position) > range)
                manager.Abandon();
        }
    }

    public void SetManager(ReplayCamerasManager manager)
    {
        this.manager = manager;
    }

    public void SetActive(bool active)
    {
        //Debug.Log(name + ": " + active);
        GetComponent<UnityEngine.Camera>().enabled = active;
        GetComponent<AudioListener>().enabled = active;
    }

    protected abstract void UpdateCamera();
}
