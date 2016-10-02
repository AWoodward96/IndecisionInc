using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Object))]
public class PlayerObject : MonoBehaviour {


    Object myObject;

    // for jumping
    bool canJump = false;

    public float jumpVel;
    public float moveVel;


    //		gravity = -(2 * jumpHeight) / Mathf.Pow (((timeToJumpApex > 0)?timeToJumpApex:1),2);
	//	jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

	// Use this for initialization
	void Start () {
        myObject = GetComponent<Object>(); // Capital O-Object is my class


	}
	
	// Update is called once per frame
	void Update () {


        type1Input();
	}


    void type1Input()
    {

        // Type 1, meaning move with WD and jump with space bar
        // This isn't 100% what we want but I just want to test collision and movement

        // Change the different modifiers based on if you're grounded or not
        if (myObject.isGrounded)
        {
            myObject.frictionModifier = .8f; // On the ground there's more friction, meaning a smaller multiplier
            canJump = true;
        }
        else
        {

            myObject.frictionModifier = .95f; // In the air there's less friction, meaning a larget multiplier
            canJump = false;
        }

        // Input
        if(Input.GetKey(KeyCode.A)) // Move Left
        {
            myObject.ApplyForce(new Vector2(-moveVel, 0));
        }
        if (Input.GetKey(KeyCode.D)) // Move Right
        {
            myObject.ApplyForce(new Vector2(moveVel, 0));
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
     
            myObject.ApplyForce(new Vector2(0, jumpVel));
        }/**/


    }



}
