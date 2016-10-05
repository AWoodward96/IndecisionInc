using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider2D))]
public class GrappleProjectile : MonoBehaviour {


    public int speed;
    public float maxDistance;
    public Vector3 Move; // the direction that the projectile moves
    Rigidbody Attached; // The object that the grappling hook has attached to
    public bool fired; // Has this been fired
    public bool Hooked;
    public GameObject playerObject;
    public GrapplingHook playerGrappleScript;
    BoxCollider2D myCollider;
    LayerMask GroundMask;
    LineRenderer myLineRenderer;

    // Use this for initialization
    void Start () {

        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
        GroundMask = LayerMask.GetMask("Ground");
        myLineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        // State: The projectile is fired and has yet to hit anything
        if(fired && !Hooked)
        {
            // Move
            transform.Translate(Move * Time.deltaTime * speed);
            handleCasting(); // Raycast to check if it hit

            // Make sure it doesn't go too far
            if (Vector2.Distance(transform.position, playerObject.transform.position) > maxDistance && !Hooked)
            {
                fired = false;
            }

        }

        // State: The projectile hasn't been fired
        if((!fired) || (!fired && Hooked))
        {
            transform.position = playerObject.transform.position;
        }

        

        myLineRenderer.enabled = Hooked;
        if (Hooked)
        {
            myLineRenderer.SetPosition(0, transform.position);
            myLineRenderer.SetPosition(1, playerObject.transform.position);
           
        }

	}


    public void FireHook(Vector3 dir)
    {
        fired = true;
        Hooked = false;

        transform.position = playerObject.transform.position;

        Move = dir;
        Move.Normalize();
    }

    public void resetHook()
    {
        Hooked = false;
        fired = false;
    }

    void handleCasting()
    {

        Ray r = new Ray(transform.position, Move);
        RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, speed * Time.deltaTime, GroundMask);

        if (hit)
        {
            Debug.Log("Hit something");
            Vector2 hitPosition = hit.point; // This wil set the sprite of the hook to the location
            Hooked = true; 

            // We hit something  add  a rigidbody to it
            if (!hit.rigidbody)
            {
                Rigidbody2D body = hit.transform.gameObject.AddComponent<Rigidbody2D>();
                body.isKinematic = true;
                body.gravityScale = 0;
            }



            Rigidbody2D currentHit = hit.rigidbody;


            // Get the real world point position
            Vector2 point = hit.point - new Vector2(hit.transform.position.x, hit.transform.position.y);
            point.x = point.x / hit.transform.localScale.x;
            point.y = point.y / hit.transform.localScale.y;

            // Move to that point
            transform.position = hit.point;

            playerGrappleScript.GrappleResponse(currentHit, (playerObject.transform.position - transform.position).magnitude, point);
        }
        else
        {

        }
    }

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    Debug.Log("Hit");
    //    if(col.gameObject.tag == "Ground")
    //    {
    //        Debug.Log("Registered Collision");
    //        if (!col.rigidbody)
    //        {
    //            Rigidbody2D body = col.transform.gameObject.AddComponent<Rigidbody2D>();
    //            body.isKinematic = true;
    //            body.gravityScale = 0;
    //        }
    //        fired = false;

    //        // We've hit something
    //        //playerGrappleScript.GrappleResponse(col.contacts[0].point, )
    //    }
    //}
}
