using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private IDictionary<ICheckpoint, int> checkpoints;
    private ICheckpoint current;
    private bool deviated = false;

    void Update()
    {
        GameObject car = GameObject.Find("Car");
        Debug.Log(car.transform.eulerAngles.z);
        if (deviated && Input.GetKeyDown(KeyCode.X))
        {
            car.transform.position = current.GetPosition();
            car.transform.eulerAngles = current.GetRotation();
            car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            deviated = false;
            GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "";
        }
        else if (
            current != null &&
            !deviated &&
            Vector3.Distance(
                car.transform.position,
                current.GetGameObject().transform.position) > 60)
        {
            Deviated();
        }
        else if (car.transform.eulerAngles.z > 170 && car.transform.eulerAngles.z < 190)
            Deviated();
    }

    public void Initialize()
    {
        checkpoints = new Dictionary<ICheckpoint, int>();
        int i = 0;
        foreach (ICheckpoint checkpoint in GetComponentsInChildren<ICheckpoint>())
        {
            checkpoint.SetManager(this);
            checkpoints.Add(checkpoint, i);
            i++;
        }
    }

    public bool TriggerCheckpoint (ICheckpoint checkpoint)
    {
        if (current == checkpoint || deviated) return false;
        else if (
            current == null && checkpoint.IsStart() || current != null &&
            (checkpoints[checkpoint] == checkpoints[current] + 1 ||
            checkpoints[current] == checkpoints.Count - 1 && checkpoint.IsStart()))
        {
            current = checkpoint;
            return true;
        }
        else
        {
            Deviated();
            return false;
        }
    }

    public void Deviated()
    {
        GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "Trajectory deviated\r\nPress X to teleport back";
        deviated = true;
    }
}
