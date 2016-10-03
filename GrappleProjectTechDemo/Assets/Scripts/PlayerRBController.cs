using UnityEngine;
using System.Collections;

public class PlayerRBController : MonoBehaviour {

    public float gravity;
    Rigidbody2D myRigidbody;
    public float jumpForce;
	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
       
	}
	
	// Update is called once per frame
	void Update () {
        Physics.gravity = new Vector3(0, gravity, 0);
        handleInput(); 
	}


    void handleInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            myRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            myRigidbody.AddForce(new Vector2(-jumpForce, 0));
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            myRigidbody.AddForce(new Vector2(0,-jumpForce));
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            myRigidbody.AddForce(new Vector2(jumpForce, 0));
        }
    }
}
