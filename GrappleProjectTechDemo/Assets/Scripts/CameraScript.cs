using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Transform Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);
	}
}
