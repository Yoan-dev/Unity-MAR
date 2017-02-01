using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour {

    private float startTime;
    private bool recording;
    private IList<Coords> replay;
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
        bestReplay = new List<Coords>();
	}
	
	void Update () {
        if (recording)
        {
            float time = Time.time;
            GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = ((int)((time - startTime) / 60)) + "min" + ((int)((time - startTime) % 60)) + "sec";
            Coords c = new Coords();
            c.Position = player.transform.position;
            c.Rotation = player.transform.eulerAngles;
            replay.Add(c);
        }

        if (onGhost && currentFrame <= bestReplay.Count)
        {
            Coords tmp = bestReplay[currentFrame];
            ghost.transform.position = tmp.Position;
            ghost.transform.eulerAngles = tmp.Rotation;
            currentFrame++;
        }
        else if (onReplay && currentFrame <= replay.Count)
        {
            Coords tmp = replay[currentFrame];
            ghost.transform.position = tmp.Position;
            ghost.transform.eulerAngles = tmp.Rotation;
            currentFrame++;
        }
	}

    public void StopRecording()
    {
        recording = false;
        if (bestReplay.Count == 0 || replay.Count > bestReplay.Count)
        {
            bestReplay = replay;
        }
    }

    public void Prepare(bool reading)
    {
        StopRecording();
        currentFrame = 0;
        if(ghost == null) ghost = Instantiate(prefab);
        onReplay = reading;
        onGhost = !reading;
    }

    public void Replay()
    {
        Prepare(true);
    }

    public void Ghost()
    {
        Prepare(false);
        player.transform.position = replay[0].Position;
        player.transform.eulerAngles = replay[0].Rotation;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        startTime = Time.time;
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Tour = 1;
        GameObject.Find("Start(Clone)").GetComponent<Starting>().Started = false;
        GameObject.Find("CheckpointsManager").GetComponent<CheckpointManager>().TriggerStart();
    }

}
