using System.Collections.Generic;
using UnityEngine;

// manage the checkpoints
public class CheckpointManager : MonoBehaviour {

    private IDictionary<ICheckpoint, int> checkpoints;
    private ICheckpoint start;
    private ICheckpoint current;
    private bool deviated = false;

    #region Accessors;

    public bool Deviated1
    {
        get
        {
            return deviated;
        }

        set
        {
            deviated = value;
        }
    }

    #endregion Accessors;

    void Update()
    {
        GameObject car = GameObject.Find("Car");

        // if the player goes the wrong way / is too far from the last checkpoint
        // he can teleport back by pressing X
        if (deviated && Input.GetKeyDown(KeyCode.X))
        {
            car.transform.position = current.GetPosition();
            car.transform.eulerAngles = current.GetRotation();
            car.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            deviated = false;
            GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "";
        }

        // else we verify if he is too far
        else if (
            car != null &&
            current != null &&
            !deviated &&
            Vector3.Distance(
                car.transform.position,
                current.GetGameObject().transform.position) > 80)
        {
            Deviated();
        }

        // else we verify if the car is upside down (count as deviated and allow teleporting back)
        else if (car != null && car.transform.eulerAngles.z > 170 && car.transform.eulerAngles.z < 190)
            Deviated();
    }

    public void Initialize()
    {
        checkpoints = new Dictionary<ICheckpoint, int>();
        int i = 0;
        foreach (ICheckpoint checkpoint in GetComponentsInChildren<ICheckpoint>())
        {
            if (checkpoint.IsStart())
            {
                start = checkpoint;
            }
            checkpoint.SetManager(this);
            checkpoints.Add(checkpoint, i);
            i++;
        }
        TriggerStart();
    }

    public void TriggerStart()
    {
        // trigger the starting checkpoint
        // used at the start of a race
        start.CheckpointSuccess(GameObject.Find("Car"));
        current = start;
    }

    // verify if a checkpoint is valid when triggered
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
