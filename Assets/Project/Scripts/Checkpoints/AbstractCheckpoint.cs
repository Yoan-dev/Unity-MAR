using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCheckpoint : MonoBehaviour, ICheckpoint
{
    protected CheckpointManager manager;
    protected Vector3 position;
    protected Vector3 rotation;
    
    public abstract bool IsStart();

    public abstract void CheckpointSuccess(GameObject player);

    public void SetManager(CheckpointManager manager)
    {
        this.manager = manager;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (manager == null) return;
        //Debug.Log("Trigger " + name);
        if (manager.TriggerCheckpoint(GetComponent<ICheckpoint>()))
        {
            //Debug.Log("Success");
            CheckpointSuccess(collider.gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetRotation()
    {
        return rotation;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
