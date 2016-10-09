using UnityEngine;
using System.Collections;

public class DeathMaker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            // Kill the player
            PlayerRBController s = col.gameObject.GetComponent<PlayerRBController>();
            s.KillPlayer();
        }
    }
}
