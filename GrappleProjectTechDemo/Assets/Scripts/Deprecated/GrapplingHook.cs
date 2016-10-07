using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class GrapplingHook : MonoBehaviour
{


    public GameObject HookSprite;
    SpriteRenderer hookSpriteRenderer;
    DistanceJoint2D ParentJoint;

    Vector2 CursorWorldPosition = Vector2.zero;
    public Vector2 hitPosition = Vector2.zero;
    bool Hooked = false;
    bool SpeedReel = false;
    LayerMask GroundMask;

    public GameObject GrapplePrefab;
    public GrappleProjectile GrappleObject;

    Rigidbody2D currentHit;
    Rigidbody2D myRigidbody;

    // Use this for initialization
    void Start()
    {

        GroundMask = LayerMask.GetMask("Ground");
        if (HookSprite)
        {
            hookSpriteRenderer = HookSprite.GetComponent<SpriteRenderer>();
            hookSpriteRenderer.enabled = false;
        }
        else
            Debug.Log("You have not assigned a hook sprite object to a hook controller");

        //GameObject gObject = (GameObject)Instantiate(GrapplePrefab, transform.position, Quaternion.identity);
        //GrappleObject = gObject.GetComponent<GrappleProjectile>();
        //GrappleObject.playerGrappleScript = this;
        //GrappleObject.playerObject = this.gameObject;

        myRigidbody = GetComponent<Rigidbody2D>();
        ParentJoint = gameObject.GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hookSpriteRenderer.enabled = Hooked;
        hookSpriteRenderer.transform.position = hitPosition;

        ParentJoint.enabled = Hooked;

        handleCursor();
        handleInput();
    }


    void handleCursor()
    {
        Vector2 pos = Input.mousePosition;
        pos = (Vector2)Camera.main.ScreenToWorldPoint(pos);
        CursorWorldPosition = pos;
    }

    // Handles mouse clicking for grappling hooks
    void handleInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Hooked = false;
            GrappleCall();
        }

        if(Input.GetMouseButtonDown(1) && GrappleObject)
        {
            Hooked = false;
            GrappleObject.resetHook();    
        }


        if (Input.GetKey(KeyCode.E) && Hooked && !SpeedReel) // Go in
        {
            ParentJoint.distance -= .2f;
        }

        if (Input.GetKey(KeyCode.Q) && Hooked && !SpeedReel)
        {
            ParentJoint.distance += .2f;
        }

        if (Input.GetKey(KeyCode.Space) && Hooked && !SpeedReel)
        {
            SpeedReel = true;
        }

        //reel in super fast
        if (SpeedReel)
        {
            if (ParentJoint.distance < 1.8)
            {
                ParentJoint.distance -= (ParentJoint.distance - 1);
                SpeedReel = false;
            }
            else
            {
                ParentJoint.distance -= .8f;
            }
        }
    }

    void GrappleCall()
    {
        Vector2 MoveVector = CursorWorldPosition - (Vector2)transform.position;

        if(GrappleObject)
        {
            GrappleObject.FireHook(MoveVector);
        }
    }

    public void GrappleResponse(Rigidbody2D rigidbody, float distance, Vector2 attachedPoint)
    {
        Debug.Log("Recieved Response");
        Hooked = true;
        ParentJoint.distance = distance;
        ParentJoint.connectedBody = rigidbody;
        ParentJoint.connectedAnchor = attachedPoint;
    }

}
