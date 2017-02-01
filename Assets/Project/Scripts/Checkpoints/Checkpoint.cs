using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : AbstractCheckpoint
{
    public override void CheckpointSuccess(GameObject player)
    {
        position = player.transform.position;
        rotation = player.transform.eulerAngles;
    }

    public override bool IsStart()
    {
        return false;
    }
}