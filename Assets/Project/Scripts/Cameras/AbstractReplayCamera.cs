using UnityEngine;

// camera template used during the replay
public abstract class AbstractReplayCamera : MonoBehaviour, IReplayCamera {

    protected ReplayCamerasManager manager;
    protected GameObject car;
    protected float range;
	
	void Update ()
    {
        if (manager == null || !manager.Activated) return;
        if (car == null) car = GameObject.Find("Replay(Clone)");

        // active this camera if the car is close enough
        if (car != null && Vector3.Distance(gameObject.transform.position, car.transform.position) < range)
        {
            SetActive(true);
            manager.ChangeActiveCamera(this);
        }

        // abandon the car if he is out of range
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
        GetComponent<UnityEngine.Camera>().enabled = active;
        GetComponent<AudioListener>().enabled = active;
    }

    // depends of the camera type
    protected abstract void UpdateCamera();
}
