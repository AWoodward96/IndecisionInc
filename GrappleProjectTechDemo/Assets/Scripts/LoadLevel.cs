using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

    public string id;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnMouseDown()
    {
        Debug.Log("Mouse Is Down");
        Application.LoadLevel(id);
    }
}
