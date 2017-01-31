using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCheckpoint : MonoBehaviour, ICheckpoint
{
    protected CheckpointManager manager;

    public abstract bool IsStart();

    public abstract void CheckpointSuccess();

    private void CheckpointFailed()
    {
        Debug.Log("Wrong way");
        Deviated();
    }

    private void Deviated()
    {
        GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "Trajectory deviated\r\nPress X to teleport back";
    }

    public void SetManager(CheckpointManager manager)
    {
        this.manager = manager;
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger " + name);
        if (manager.TriggerCheckpoint(GetComponent<ICheckpoint>()))
        {
            Debug.Log("Success");
            CheckpointSuccess();
        }
        else CheckpointFailed();
    }
}
