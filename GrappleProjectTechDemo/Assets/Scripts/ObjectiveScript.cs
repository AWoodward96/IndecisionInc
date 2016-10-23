using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectiveScript : MonoBehaviour {

    BoxCollider2D myCollider;
    //public bool SimpleTransition; // If true, then we're just going to jump to the next level
    public bool WorldEnd; // I'm not sure what this is for.
    public string nextScene;

    bool miniCutscene = false; // Are we running the mini cutscene? 
    GameObject Player; // So we can move the player

    bool teleportation = false;

    public enum AnimationType
    {
        Stretch,
        Shrink,
        Fade,
        Simple,
    };
    public AnimationType aType;

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
            if(!teleportation && Vector3.Distance(Player.transform.position, this.transform.position) < .7)
            {
                teleportation = true;

                Player.GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Find("GamplayUI").SetActive(false);
                GameObject.Find("GrappleObject(Clone)").SetActive(false);
            }
            if(teleportation)
            {
                if (Player.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    if (aType == AnimationType.Stretch)
                    {
                        Vector3 newScale = new Vector3(Player.transform.localScale.x, Player.transform.localScale.y + .05f, Player.transform.localScale.z);
                        Player.transform.localScale = newScale;
                    }
                    if (aType == AnimationType.Shrink)
                    {
                        Vector3 newScale = new Vector3(Player.transform.localScale.x * .99f, Player.transform.localScale.y * .99f, Player.transform.localScale.z);
                        Player.transform.localScale = newScale;
                    }

                    Color col = Player.GetComponent<SpriteRenderer>().color;
                    col.a -= .015f;
                    Player.GetComponent<SpriteRenderer>().color = col;
                }
                else
                {
                    //GM.loadNextLevel(WorldEnd);
                    StartCoroutine(wait());
                }
            }
            else
            {
                Player.transform.position = Vector3.Lerp(Player.transform.position, this.transform.position, Vector3.Distance(Player.transform.position, this.transform.position) * Time.deltaTime);
                Player.transform.rotation = Quaternion.identity;
            }
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

            if (aType == AnimationType.Simple)
            {
                if (nextScene == "")
                {
                    GM.loadNextLevel(WorldEnd);
                }
                else
                {
                    GM.loadSpecificScene(nextScene);
                }
            }
            else
            {
                miniCutscene = true;
                Player = Col.transform.gameObject;
                Player.GetComponent<PlayerRBController>().AcceptInput = false;
                Player.GetComponent<Rigidbody2D>().isKinematic = true;
                //StartCoroutine(wait());
            }

        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        if (nextScene == "")
        {
            GM.loadNextLevel(WorldEnd);
        }
        else
        {
            GM.loadSpecificScene(nextScene);
        }
    }
 
}
