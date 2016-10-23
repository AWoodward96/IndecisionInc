using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckpointScript : MonoBehaviour {

    bool isActive;
    BoxCollider2D myCollider;
    Vector3 respawnLoc;

    public GameManager GM;

	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;

        respawnLoc = GetComponent<Transform>().position;

        isActive = false;

        GameObject G = GameObject.FindGameObjectWithTag("GameManager");
        if (G)
        {
            GM = G.GetComponent<GameManager>();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Player" && !isActive)
        {
            Col.gameObject.GetComponent<PlayerRBController>().jetpackCooldown = 100;
            Debug.Log("CHECKPOINT!!!");
            SetActive();
            GameObject.Find("UI").GetComponent<TimerManager>().resetCheckpoint();
        }
    }

    void SetActive() {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject cp in checkpoints)
        {
            cp.GetComponent<CheckpointScript>().isActive = false;
        }
        isActive = true;
    }

    void Disable()
    {
        isActive = false;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public Vector3 RespawnLocation()
    {
        return respawnLoc;
    }
}


