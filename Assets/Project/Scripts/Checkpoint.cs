using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : AbstractCheckpoint
{
    public override void CheckpointSuccess()
    {
        // do nothing
    }

    public override bool IsStart()
    {
        return false;
    }
}