﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private int nbTurns = 3;

    #region Accessors;

    public int NbTurns
    {
        get
        {
            return nbTurns;
        }

        set
        {
            nbTurns = value;
        }
    }

    #endregion Accessors;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            StartGame();
    }

    // public car sera appelé par l'UI du menu
    public void StartGame()
    {
        GameObject.Find("GenerationManager").GetComponent<GenerationManager>().Generate();
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().Initialize();
    }

    public void EndGame()
    {
        Debug.Log("Race finished");
        // do stuff
    }
}