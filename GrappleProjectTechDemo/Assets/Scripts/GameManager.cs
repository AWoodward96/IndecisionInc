using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Vector3 PlayerStartPos;
    //public int nextSceneIndex;

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

        if(PlayerObject)
        {
            PlayerScript = PlayerObject.GetComponent<PlayerRBController>();
            PlayerScript.GM = this;
            PlayerStartPos = PlayerObject.transform.position;
        }
        else
        {
            PlayerStartPos = Vector3.zero;
        }


    }


    // Just  in case you desperitely need to reload the scene (can't think of a reason, but hey we have it)
    public void reloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // For transitioning to the next level
    public void loadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        { 
            loadSpecificScene("LevelSelector");
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // For loading specific scenes
    public void loadSpecificScene(int index)
    {
        if (index < SceneManager.sceneCount)
        SceneManager.LoadScene(index);
    }

    public void loadSpecificScene(string sceneName)
    {
        // This is a lot less safe, but it's also a lot more convinient
        SceneManager.LoadScene(sceneName);
    }

    public void resetPlayer(GrappleProjectile proj)
    {
        // The players died and needs to be sent back to the starting position
        StartCoroutine(resetCoroutine(proj));
    }

    IEnumerator resetCoroutine(GrappleProjectile grapplinghook)
    {
        yield return new WaitForSeconds(1);
        grapplinghook.gameObject.SetActive(true);
        PlayerObject.SetActive(true);
        PlayerObject.transform.position = PlayerStartPos;
    }
}
