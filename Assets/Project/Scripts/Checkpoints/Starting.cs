using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : AbstractCheckpoint
{
    private bool started = false;
    private int tour = 1;

    #region Accessors;

    public int Tour
    {
        get
        {
            return tour;
        }

        set
        {
            tour = value;
        }
    }

    public bool Started
    {
        get
        {
            return started;
        }

        set
        {
            started = value;
        }
    }

    #endregion Accessors;

    public override void CheckpointSuccess(GameObject player)
    {
        position = player.transform.position;
        rotation = player.transform.eulerAngles;
        if (!started) started = true;
        else tour++;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (tour > gameManager.NbTurns)
        {
            gameManager.EndGame();
        }
        else GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Turn " + tour + "/" + GameObject.Find("GameManager").GetComponent<GameManager>().NbTurns;
    }

    public override bool IsStart()
    {
        return true;
    }
}
