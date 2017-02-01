using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starting : AbstractCheckpoint
{
    private bool started = false;
    private int tour = 1;

    public override void CheckpointSuccess(GameObject player)
    {
        position = player.transform.position;
        rotation = player.transform.eulerAngles;
        if (!started) started = true;
        else tour++;
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (tour > gameManager.NbTurns)
        {
            GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Race finished !";
            gameManager.EndGame();
        }
        else GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Turn " + tour;
    }

    public override bool IsStart()
    {
        return true;
    }
}
