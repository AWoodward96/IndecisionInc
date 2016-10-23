using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerManager : MonoBehaviour {
    public Text levelTimer;
    public Text lifeTimer;
    public Text checkpointTimer;
    private float levelTime;
    private float lifeTime;
    private float checkpointTime;

	// Use this for initialization
	void Start () {
        levelTime = 0;
        lifeTime = 0;
        checkpointTime = 0;

    }
	
	// Update is called once per frame
	void Update () {

        levelTime      += Time.deltaTime;
        lifeTime       += Time.deltaTime;
        checkpointTime += Time.deltaTime;

        levelTimer.text = "Total: "  + levelTime.ToString("n2");
        lifeTimer.text = "Current: " + lifeTime.ToString("n2");
        checkpointTimer.text = "Checkpoint: " + checkpointTime.ToString("n2");
    }

    public void resetLife ()
    {
        lifeTime = 0;
    }

    public void resetCheckpoint ()
    {
        checkpointTime = 0;
    }
}
