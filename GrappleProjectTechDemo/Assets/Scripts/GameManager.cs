using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Vector3 PlayerStartPos;
    public int nextSceneIndex;

   
    GameObject PlayerObject;
    PlayerRBController PlayerScript;

	// Since this object won't be destroyed (See the Object.DontDestroyOnLoad) this will only be called once
	void Start () {
        Object.DontDestroyOnLoad(this);
        GameObject otherGameManager = GameObject.FindGameObjectWithTag("GameManager");
        // Ensure there's only one instance of the game manager
        if(otherGameManager != this.gameObject)
        {
            Destroy(this.gameObject);
        }


        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	}

    // This is called every time unity loads a level
    // You don't need to call this from anywhere, Unity does it for us
    void OnLevelWasLoaded(int level)
    {
        Initialize();
    }


    void Initialize()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<PlayerRBController>();
        PlayerScript.GM = this;

        PlayerStartPos = PlayerObject.transform.position;
    }


    public void reloadLevel()
    {

    }

    public void resetPlayer()
    {
        // The players died and needs to be sent back to the starting position
        StartCoroutine(resetCoroutine());
    }

    IEnumerator resetCoroutine()
    {
        yield return new WaitForSeconds(1);
        PlayerObject.SetActive(true);
        PlayerObject.transform.position = PlayerStartPos;
    }
}
