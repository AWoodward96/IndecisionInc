﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectiveScript : MonoBehaviour {

    BoxCollider2D myCollider;
    //public bool SimpleTransition; // If true, then we're just going to jump to the next level
    public bool WorldEnd; // I'm not sure what this is for.  //probably obsolete now that scenes are loaded by name (used to indicate the next scene was the menu)
    public string nextScene; //the name of the next scene (with folder path included) since the sceneManager is dumb

    bool miniCutscene = false; // Are we running the mini cutscene? 
    GameObject Player; // So we can move the player

    bool teleportation = false; //has the final animation started?

    public enum AnimationType 
    {
        Stretch,
        Shrink,
        Fade,
        Simple, //simpleAnimation is here now instead of a bool
    };
    public AnimationType aType; //what type of animation is running

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
            //checks if the animation has not started and if the player is close enough for the animation to start
            if(!teleportation && Vector3.Distance(Player.transform.position, this.transform.position) < .7)
            {
                teleportation = true;

                //disables anything in the way
                Player.GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Find("GameplayUI").SetActive(false);
                GameObject.Find("GrappleObject(Clone)").SetActive(false);
                GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerManager>().freeze = true;
            }
            if(teleportation)
            {
                if (Player.GetComponent<SpriteRenderer>().color.a > 0)
                {
                    //stretches the block vertically
                    if (aType == AnimationType.Stretch)
                    {
                        Vector3 newScale = new Vector3(Player.transform.localScale.x, Player.transform.localScale.y + .05f, Player.transform.localScale.z);
                        Player.transform.localScale = newScale;
                    }
                    //shrinks the block both vertically and horizontally
                    if (aType == AnimationType.Shrink)
                    {
                        Vector3 newScale = new Vector3(Player.transform.localScale.x * .99f, Player.transform.localScale.y * .99f, Player.transform.localScale.z);
                        Player.transform.localScale = newScale;
                    }

                    //slowly fades the block to nothing
                    Color col = Player.GetComponent<SpriteRenderer>().color;
                    col.a -= .050f;
                    Player.GetComponent<SpriteRenderer>().color = col;
                }
                //load next scene when the player disappears
                else
                {
                    //GM.loadNextLevel(WorldEnd);
                    StartCoroutine(wait());
                }
            }
            //else
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
        yield return new WaitForSeconds(.5f);
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
