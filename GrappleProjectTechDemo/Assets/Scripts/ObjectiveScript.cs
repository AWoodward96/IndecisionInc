using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectiveScript : MonoBehaviour {

    BoxCollider2D myCollider;
    public bool SimpleTransition; // If true, then we're just going to jump to the next level
    public bool WorldEnd; // I'm not sure what this is for.

    bool miniCutscene = false; // Are we running the mini cutscene? 
    GameObject Player; // So we can move the player


    public GameManager GM;

	// Use this for initialization
	void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;

        GameObject G = GameObject.FindGameObjectWithTag("GameManager");
        if(G)
        {
            GM = G.GetComponent<GameManager>();
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(miniCutscene)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, this.transform.position, Vector3.Distance(Player.transform.position, this.transform.position) * Time.deltaTime);
        }
	}

    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Player")
        {
 
            if (!GM)
            {
                GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }

            if (SimpleTransition)
                GM.loadNextLevel(WorldEnd);
            else
            {
                miniCutscene = true;
                Player = Col.transform.gameObject;
                Player.GetComponent<PlayerRBController>().AcceptInput = false;
                Player.GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(wait());
            }

        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        GM.loadNextLevel(WorldEnd);
    }
 
}
