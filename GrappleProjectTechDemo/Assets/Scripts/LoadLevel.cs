using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

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
}
