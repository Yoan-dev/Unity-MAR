using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public UnityEngine.UI.Button start;
    public UnityEngine.UI.Button turnMinus;
    public UnityEngine.UI.Text turns;
    public UnityEngine.UI.Button turnPlus;
    public UnityEngine.UI.Button generate;

    private int nbTurns = 2;

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

    void Start()
    {
        start.onClick.AddListener(() => StartGame());
        generate.onClick.AddListener(() => Generate());
        turnMinus.onClick.AddListener(() => ChangeTurn(-1));
        turnPlus.onClick.AddListener(() => ChangeTurn(1));
        turns.text = nbTurns+"";
        start.interactable = false;
    }

    void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.Space))
            StartGame();*/
        if (Input.GetKeyUp(KeyCode.R))
            Replay();
    }

    private void ChangeTurn(int inc)
    {
        nbTurns += inc;
        turns.text = nbTurns + "";
        if (nbTurns == 1) turnMinus.interactable = false;
        if (nbTurns == 5) turnPlus.interactable = false;
        if (inc == 1) turnMinus.interactable = true;
        if (inc == -1) turnPlus.interactable = true;
    }

    private void Generate()
    {
        GameObject.Find("GenerationManager").GetComponent<GenerationManager>().Generate();
        GameObject.Find("TerrainCamera").transform.position = new Vector3(128, 256, 128);
        start.interactable = true;

    }

    // public car sera appelé par l'UI du menu
    public void StartGame()
    {
        GameObject.Find("RecordManager").GetComponent<RecordManager>().Initialize();
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().Initialize();
        GameObject.Find("ReplayCamerasManager").GetComponent<ReplayCamerasManager>().Initialize();
        GameObject.Find("TerrainCamera").GetComponent<Camera>().Switch();
        Rigidbody car = GameObject.Find("Car").GetComponent<Rigidbody>();
        car.constraints = RigidbodyConstraints.None;
    }

    public void EndGame()
    {
        Debug.Log("Race finished");
        GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Race finished !";
        GameObject.Find("RecordManager").GetComponent<RecordManager>().StopRecording();
    }

   private void Replay()
    {
        GameObject.Find("RecordManager").GetComponent<RecordManager>().Replay();
        GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "";
    } 
}
