using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Main menu UI
    public UnityEngine.UI.Button start;
    public UnityEngine.UI.Button turnMinus;
    public UnityEngine.UI.Text turns;
    public UnityEngine.UI.Button turnPlus;
    public UnityEngine.UI.Button generate;
    public UnityEngine.UI.Button quit;

    // inGameMenu UI
    public GameObject inGameMenu;
    public UnityEngine.UI.Button replay;
    public UnityEngine.UI.Button ghost;
    public UnityEngine.UI.Button mainMenu;

    private int nbTurns = 2;
    private bool started = false;

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
        // main menu initialization
        start.onClick.AddListener(() => StartGame());
        generate.onClick.AddListener(() => Generate());
        turnMinus.onClick.AddListener(() => ChangeTurn(-1));
        turnPlus.onClick.AddListener(() => ChangeTurn(1));
        turns.text = nbTurns+"";
        quit.onClick.AddListener(() => Application.Quit());
        start.interactable = false;

        // inGameMenu initialization
        replay.onClick.AddListener(() => Replay());
        ghost.onClick.AddListener(() => Ghost());
        mainMenu.onClick.AddListener(() => RebootScene());
        replay.interactable = false;
        ghost.interactable = false;
    }

    void Update()
    {
        // Call ingame menu
        if (Input.GetKeyUp(KeyCode.Escape) && started) inGameMenu.SetActive(!inGameMenu.activeSelf);

        // HUD Show/Hide controls
        if (started && Input.GetKeyUp(KeyCode.K))
        {
            UnityEngine.UI.Text keys = GameObject.Find("Keys").GetComponent<UnityEngine.UI.Text>();
            keys.enabled = !keys.enabled;
            GameObject.Find("KeysTitle").GetComponent<UnityEngine.UI.Text>().text = (keys.enabled) ?
                "Press K to hide controls" :
                "Press K to show controls";
        }
    }

    // Turns amount between 1 and 5
    private void ChangeTurn(int inc)
    {
        if (started) return;
        nbTurns += inc;
        turns.text = nbTurns + "";
        if (nbTurns == 1) turnMinus.interactable = false;
        if (nbTurns == 5) turnPlus.interactable = false;
        if (inc == 1) turnMinus.interactable = true;
        if (inc == -1) turnPlus.interactable = true;
    }

    // Generate a new map
    private void Generate()
    {
        GameObject.Find("Info").GetComponent<UnityEngine.UI.Text>().text = "";
        GameObject.Find("GenerationManager").GetComponent<GenerationManager>().Generate();
        GameObject.Find("TerrainCamera").transform.position = new Vector3(128, 256, 128);
        start.interactable = true;

    }

    // Start a race
    public void StartGame()
    {
        if (started) return;
        started = true;
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().Initialize();
        GameObject.Find("ReplayCamerasManager").GetComponent<ReplayCamerasManager>().Initialize();
        GameObject.Find("TerrainCamera").GetComponent<Camera>().Switch();
        StartCoroutine(Counts());
    }

    // Race finished
    public void EndGame()
    {
        Debug.Log("Race finished");
        GameObject.Find("Turn").GetComponent<UnityEngine.UI.Text>().text = "Race finished !";
        GameObject.Find("RecordManager").GetComponent<RecordManager>().StopRecording();
        replay.interactable = true;
        ghost.interactable = true;
        inGameMenu.SetActive(true);
    }

    // Launch a replay
    private void Replay()
    {
        GameObject.Find("RecordManager").GetComponent<RecordManager>().Replay();
        GameObject.Find("ReplayCamerasManager").GetComponent<ReplayCamerasManager>().Activate();
        GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "";
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().Deviated1 = false;
        inGameMenu.SetActive(false);
    } 

    // Fight against the best time
    private void Ghost()
    {
        GameObject.Find("ReplayCamerasManager").GetComponent<ReplayCamerasManager>().Desactivate();
        GameObject.Find("Alert").GetComponent<UnityEngine.UI.Text>().text = "";
        GameObject.Find("RecordManager").GetComponent<RecordManager>().Ghost();
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().Deviated1 = false;
        inGameMenu.SetActive(false);
    }

    // Reload the scene (return to main menu)
    private void RebootScene()
    {
        SceneManager.LoadScene("Game");
    }

    IEnumerator Counts()
    {
        GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = "0min0sec";
        UnityEngine.UI.Text counts = GameObject.Find("Counts").GetComponent<UnityEngine.UI.Text>();
        yield return new WaitForSeconds(.25f);
        for (int i = 3; i > 0; i--)
        {
            counts.text = i + "";
            yield return new WaitForSeconds(1.0f);
        }
        counts.text = "START !";
        GameObject.Find("RecordManager").GetComponent<RecordManager>().Initialize();
        Rigidbody car = GameObject.Find("Car").GetComponent<Rigidbody>();
        car.constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(2.0f);
        counts.text = "";
    }
}
