using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour {

    private float startTime;
    private bool recording;
    private IList<Coords> replay;
    private IList<Coords> lastReplay;
    private IList<Coords> bestReplay;
    private bool onReplay;
    private bool onGhost;
    private int currentFrame;
    public GameObject prefab;
    private GameObject ghost;
    private GameObject player;
    
	public void Initialize () {
        player = GameObject.Find("Car");
        recording = true;
        startTime = Time.time;
        onReplay = false;
        onGhost = false;
        replay = new List<Coords>();
        lastReplay = new List<Coords>();
        bestReplay = new List<Coords>();
	}
	
	void Update () {
		// Si l'enregistrement est en cours on stocke les positions dans la liste 'replay'
        if (recording)
        {
            float time = Time.time;
            GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = ((int)((time - startTime) / 60)) + "min" + ((int)((time - startTime) % 60)) + "sec";
            Coords c = new Coords();
            c.Position = player.transform.position;
            c.Rotation = player.transform.eulerAngles;
            replay.Add(c);
        }

        if (onGhost && currentFrame < bestReplay.Count)
        {
            Coords tmp = bestReplay[currentFrame];
            ghost.transform.position = tmp.Position;
            ghost.transform.eulerAngles = tmp.Rotation;
            currentFrame++;
        }
        else if (onReplay && currentFrame < lastReplay.Count)
        {
            Coords tmp = lastReplay[currentFrame];
            ghost.transform.position = tmp.Position;
            ghost.transform.eulerAngles = tmp.Rotation;
            currentFrame++;
            if (currentFrame == lastReplay.Count - 1)
            {
                Replay();
                GameObject.Find("GameManager").GetComponent<GameManager>().inGameMenu.SetActive(true);
            }
        }
	}

    public void StopRecording()
    {
        recording = false;
        lastReplay = CloneCoords(replay);
        if (bestReplay.Count == 0 || lastReplay.Count < bestReplay.Count)
        {
            float time = Time.time;
            GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = ((int)((time - startTime) / 60)) + "min" + ((int)((time - startTime) % 60)) + "sec - New record !";
            bestReplay = CloneCoords(lastReplay);
        }
    }

    public void Prepare(bool reading)
    {
        currentFrame = 0;
        if(ghost == null) ghost = Instantiate(prefab);
        onReplay = reading;
        onGhost = !reading;
        recording = !reading;
    }

    public void Replay()
    {
        Prepare(true);
    }

    public void Ghost()
    {
        replay.Clear();
        player.transform.position = bestReplay[0].Position;
        player.transform.eulerAngles = bestReplay[0].Rotation;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        if (ghost != null)
        {
            GameObject.Find("ReplayStockcarCamera").GetComponent<UnityEngine.Camera>().enabled = false;
            GameObject.Find("ReplayStockcarCamera").GetComponent<AudioListener>().enabled = false;
        }
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Tour = 1;
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Started = false;
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().TriggerStart();
        StartCoroutine(CountsGhost());
    }

    IEnumerator CountsGhost()
    {
        recording = false;
        onReplay = false;
        if (ghost != null)
        {
            ghost.transform.position = bestReplay[0].Position;
            ghost.transform.eulerAngles = bestReplay[0].Rotation;
        }
        Rigidbody car = GameObject.Find("Car").GetComponent<Rigidbody>();
        car.constraints = RigidbodyConstraints.FreezeAll;
        GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = "0min0sec";
        UnityEngine.UI.Text counts = GameObject.Find("Counts").GetComponent<UnityEngine.UI.Text>();
        yield return new WaitForSeconds(.25f);
        for (int i = 3; i > 0; i--)
        {
            counts.text = i + "";
            yield return new WaitForSeconds(1.0f);
        }
        counts.text = "START !";
        car.constraints = RigidbodyConstraints.None;
        startTime = Time.time;
        Prepare(false);
        yield return new WaitForSeconds(2.0f);
        counts.text = "";
    }

    private IList<Coords> CloneCoords(IList<Coords> from)
    {
        IList<Coords> res = new List<Coords>();
        foreach (Coords current in from)
        {
            Coords temp = new Coords();
            temp.Position = current.Position;
            temp.Rotation = current.Rotation;
            res.Add(temp);
        }
        return res;
    }

}
