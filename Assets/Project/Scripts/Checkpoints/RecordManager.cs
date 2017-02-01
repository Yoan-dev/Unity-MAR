using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour {

    private float startTime;
    private bool recording;
    private IList<Coords> replay;
    private bool onReplay;
    private int currentFrame;
    public GameObject prefab;
    private GameObject ghost;
    private GameObject player;
    
	public void Initialize () {
        player = GameObject.Find("Car");
        recording = true;
        startTime = Time.time;
        replay = new List<Coords>();
	}
	
	void Update () {
        if (recording)
        {
            float time = Time.time;
            GameObject.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = ((int)((time - startTime) / 60)) + "min" + ((int)((time - startTime) % 60)) + "sec";
            //Debug.Log((int)((time - startTime) / 60) + "min" + (int)((time - startTime) % 60) + "sec");
            Coords c = new Coords();
            c.Position = player.transform.position;
            c.Rotation = player.transform.eulerAngles;
            replay.Add(c);
        }

        if (onReplay)
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
    }

    public void Replay()
    {
        onReplay = true;
        currentFrame = 0;
        ghost = Instantiate(prefab);
        //ghost.GetComponentInChildren<Camera>().gameObject.SetActive(false);
    }

}
