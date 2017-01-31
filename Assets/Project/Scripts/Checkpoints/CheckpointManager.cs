using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private IDictionary<ICheckpoint, int> checkpoints;
    private ICheckpoint current;

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
        if (
            current == null || 
            checkpoints[checkpoint] > checkpoints[current] ||
            checkpoints[current] == checkpoints.Count - 1 && checkpoint.IsStart())
        {
            current = checkpoint;
            return true;
        }
        else return false;
    }
}
