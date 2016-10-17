using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public Canvas startCanvas;

    void Start()
    {
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("WorldScreen"))
        {
            gObj.SetActive(false);
        }
        startCanvas.gameObject.SetActive(true);
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

    public void nextWorldPage(GameObject newCanvas)
    {
        foreach (GameObject gObj in GameObject.FindGameObjectsWithTag("WorldScreen"))
        {
            gObj.SetActive(false);
        }
        newCanvas.SetActive(true);
    }
    public void pastWorldPage(int index)
    {
        // This is a lot less safe, but it's also a lot more convinient
        GameObject.Find("World " + (index - 1)).SetActive(true);
        GameObject.Find("World " + index).SetActive(false);
    }
}
