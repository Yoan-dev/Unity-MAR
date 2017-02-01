using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCamerasManager : MonoBehaviour {

    private IList<IReplayCamera> cameras;
    private IReplayCamera current;
    private bool activated = false;
    private GameObject car;

    #region Accessors;

    public bool Activated
    {
        get
        {
            return activated;
        }

        set
        {
            activated = value;
        }
    }

    #endregion Accessors;

    void Start ()
    {
        car = GameObject.Find("Car");
    }
	
	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.R)) Activate();
        if (!activated) return;
        if (current == null && GameObject.Find("ReplayStockcarCamera") != null)
        {
            GameObject.Find("ReplayStockcarCamera").GetComponent<UnityEngine.Camera>().enabled = true;
            GameObject.Find("ReplayStockcarCamera").GetComponent<AudioListener>().enabled = true;
        }
	}

    public void Initialize()
    {
        cameras = new List<IReplayCamera>();
        foreach (IReplayCamera camera in GetComponentsInChildren<IReplayCamera>())
        {
            camera.SetManager(this);
            cameras.Add(camera);
        }
    }

    public void Activate()
    {
        car.SetActive(false);
        activated = true;
    }

    public void Desactivate()
    {
        car.SetActive(true);
        activated = false;
        current = null;
    }

    public void ChangeActiveCamera(IReplayCamera camera)
    {
        GameObject.Find("ReplayStockcarCamera").GetComponent<UnityEngine.Camera>().enabled = false;
        GameObject.Find("ReplayStockcarCamera").GetComponent<AudioListener>().enabled = false;
        if (current == null) current = camera;
        else if (camera == current) return;
        else
        {
            current.SetActive(false);
            current = camera;
        }
    }

    public void Abandon()
    {
        current.SetActive(false);
        current = null;
    }
}
