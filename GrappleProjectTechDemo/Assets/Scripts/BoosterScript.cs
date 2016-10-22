using UnityEngine;
using System.Collections;

public class BoosterScript : MonoBehaviour {

    PlayerRBController otherScript;
    BoxCollider2D myCollider;

    // Use this for initialization
    void Start ()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;

        GameObject G = GameObject.FindGameObjectWithTag("Player");
        if (G)
        {
            otherScript = G.GetComponent<PlayerRBController>();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            otherScript.boostJetpack = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            otherScript.boostJetpack = false;
        }
    }
}
