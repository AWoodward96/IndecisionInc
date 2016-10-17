using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(DistanceJoint2D))]
public class GrappleProjectile : MonoBehaviour {


    public int speed;
    public float maxDistance;
    public Vector3 Move; // the direction that the projectile moves
    public bool fired; // Has this been fired
    public bool Hooked; // Are we currently hanging
    bool SpeedReel;
    Vector3 dirToHook;

    GameObject playerObject;
    BoxCollider2D myCollider;
    LayerMask GroundMask;
    LineRenderer myLineRenderer;
    DistanceJoint2D myDistanceJoint;

    Vector2 CursorWorldPosition;

    // Use this for initialization
    void Start () {

        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
        GroundMask = LayerMask.GetMask("Ground");
        myLineRenderer = GetComponent<LineRenderer>();
        myDistanceJoint = GetComponent<DistanceJoint2D>();


        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
            myDistanceJoint.connectedBody = playerObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if(playerObject)
        {

            handleCursor();
            handleInput();


            // State: The projectile is fired and has yet to hit anything
            if (fired && !Hooked)
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
            if ((!fired) || (!fired && Hooked))
            {
                transform.position = playerObject.transform.position;
            }


            myDistanceJoint.enabled = Hooked;
            myLineRenderer.enabled = Hooked;
            if (Hooked)
            {
                myLineRenderer.SetPosition(0, transform.position);
                myLineRenderer.SetPosition(1, playerObject.transform.position);

            }

        }
        else
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if(playerObject)
                myDistanceJoint.connectedBody = playerObject.GetComponent<Rigidbody2D>();
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


            myDistanceJoint.distance = ((Vector2)playerObject.transform.position - hit.point).magnitude;
            
            myDistanceJoint.connectedAnchor = new Vector2(0,0);
            // Move to that point
            transform.position = hit.point;

        }
        else
        {

        }
    }
    void handleInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if(Hooked)
            {
                Hooked = false;
                resetHook();
            }else
            {
                Hooked = false;
                FireHook(CursorWorldPosition - (Vector2)playerObject.transform.position);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Hooked = false;
            resetHook();
        }

        //calc normal vector to hook from player
        dirToHook = Vector3.Normalize((Vector2)this.transform.position - (Vector2)playerObject.transform.position);

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if ((Input.GetKey(KeyCode.E) || scrollWheel > 0f) && Hooked && !SpeedReel) // Go in
        {
            myDistanceJoint.connectedBody.AddForce(dirToHook * .8f);
            myDistanceJoint.distance -= .2f;
        }

        if ((Input.GetKey(KeyCode.Q) || scrollWheel < 0f) && Hooked && !SpeedReel)
        {
            myDistanceJoint.connectedBody.AddForce(dirToHook * -.8f);
            myDistanceJoint.distance += .2f;
        }

        if (Input.GetKey(KeyCode.Space) && Hooked && !SpeedReel)
        {
            SpeedReel = true;
        }

        //reel in super fast
        if (SpeedReel)
        {
            if (myDistanceJoint.distance < 1.8)
            {
                myDistanceJoint.connectedBody.AddForce(dirToHook * (myDistanceJoint.distance - 1) * 4);
                myDistanceJoint.distance -= (myDistanceJoint.distance - 1);
                SpeedReel = false;
            }
            else
            {
                myDistanceJoint.connectedBody.AddForce(dirToHook * 3.2f);
                myDistanceJoint.distance -= .8f;
            }
        }
    }

    void handleCursor()
    {
        Vector2 pos = Input.mousePosition;
        pos = (Vector2)Camera.main.ScreenToWorldPoint(pos);
        CursorWorldPosition = pos;
    }


    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Unhookable" && fired)
        {
            Hooked = false;
            resetHook();
        }
    }
}
