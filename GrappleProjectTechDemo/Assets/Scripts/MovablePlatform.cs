using UnityEngine;
using System.Collections;

public class MovablePlatform : MonoBehaviour {
    public float speed;
    private float currentRange;
    public int range;
    public bool isAttached;
    //Rigidbody2D myRigidbody;
    Transform myTransform;
    GameObject hook;
    // Use this for initialization
    void Start () {
        myTransform = GetComponent<Transform>();
        isAttached = false;
        currentRange = 0;
    }
	
	// Update is called once per frame
	void Update () {
        
       
        float velocity = speed * Time.deltaTime;
        currentRange += velocity;

        if (Mathf.Abs(currentRange) > range) speed = -speed;
        myTransform.position = new Vector3(myTransform.position.x + velocity, myTransform.position.y, myTransform.position.z);

        if (isAttached)
        {
            hook = GameObject.FindGameObjectWithTag("GrapplingHook");
            GrappleProjectile hookScript = hook.GetComponent<GrappleProjectile>();
            if (hookScript.Hooked)
            {
                hookScript.updateHookOnMove(myTransform);
            }
        }
    }
}
