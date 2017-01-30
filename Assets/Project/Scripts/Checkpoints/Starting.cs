using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : AbstractCheckpoint
{
    private bool started = false;
    private int tour = 1;

    public override void CheckpointSuccess()
    {
        if (!started) started = true;
        else tour++;
        GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Turn " + tour;
    }

    public override bool IsStart()
    {
        return true;
    }
}
