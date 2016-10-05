using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider2D))]
public class GrappleProjectile : MonoBehaviour {


    public int speed;
    public Vector3 Move; // the direction that the projectile moves
    Rigidbody Attached; // The object that the grappling hook has attached to
    bool fired; // Has this been fired
    public GameObject playerObject;
    public GrapplingHook playerGrappleScript;
    BoxCollider2D myCollider;

    // Use this for initialization
    void Start () {

        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if(fired)
        {
            transform.Translate(Move * Time.deltaTime * speed);
        }
	}


    public void FireHook(Vector3 dir)
    {
        fired = true;
        transform.position = playerObject.transform.position;

        Move = dir;
        Move.Normalize();
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Hit");
        if(col.gameObject.tag == "Ground")
        {
            Debug.Log("Registered Collision");
            if (!col.rigidbody)
            {
                Rigidbody2D body = col.transform.gameObject.AddComponent<Rigidbody2D>();
                body.isKinematic = true;
                body.gravityScale = 0;
            }
            fired = false;

            // We've hit something
            //playerGrappleScript.GrappleResponse(col.contacts[0].point, )
        }
    }
}
