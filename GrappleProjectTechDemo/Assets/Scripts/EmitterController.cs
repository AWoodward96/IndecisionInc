﻿using UnityEngine;
using System.Collections;

public class EmitterController : MonoBehaviour {

    Quaternion rotation;

	// Use this for initialization
	void Start () {
        rotation = transform.rotation;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
