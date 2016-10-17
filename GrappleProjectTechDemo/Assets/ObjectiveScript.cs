using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectiveScript : MonoBehaviour {

    BoxCollider2D myCollider;
    public bool Activatable;

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
	
        //obsolete
        /*if(Activatable && Input.GetKeyDown(KeyCode.W))
        {
            if(!GM)
            {
                GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }

            GM.loadNextLevel();
        }*/
	}

    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Player")
        {
            Activatable = true;
            if (!GM)
            {
                GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }

            GM.loadNextLevel();
        }
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        if(Col.tag == "Player")
        {
            Activatable = false;
        }
    }
}
