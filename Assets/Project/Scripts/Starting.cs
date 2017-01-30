using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : AbstractCheckpoint
{
    private int tour = 0;

    public override void CheckpointSuccess()
    {
        tour++;
        Debug.Log("Tour: " + tour);
    }

    public override bool IsStart()
    {
        return true;
    }
}
