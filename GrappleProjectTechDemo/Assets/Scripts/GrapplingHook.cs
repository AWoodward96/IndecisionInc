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

        GameObject gObject = (GameObject)Instantiate(GrapplePrefab, transform.position, Quaternion.identity);
        GrappleObject = gObject.GetComponent<GrappleProjectile>();
        GrappleObject.playerGrappleScript = this;
        GrappleObject.playerObject = this.gameObject;

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
        //if (Input.GetMouseButtonDown(0))
        //{

        //    // Raycast check towards the position of the mouse to see if it'll hit anything
        //    Vector2 DistanceVector = CursorWorldPosition - (Vector2)transform.position;
        //    Ray2D r = new Ray2D(transform.position, DistanceVector);

        //    RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, DistanceVector.magnitude, GroundMask);
        //    // Cast it
        //    if (hit)
        //    {
        //        // We got a hook
        //        hitPosition = hit.point; // This wil set the sprite of the hook to the location
        //        Hooked = true;

        //        // Now hook it (aaaaaa) just hook it (aaaaaaa) just lose it (aaaaaaaa) it's super fucking late (aaaaaaaaa)
        //        if (!hit.rigidbody)
        //        {
        //            Rigidbody2D body = hit.transform.gameObject.AddComponent<Rigidbody2D>();
        //            body.isKinematic = true;
        //            body.gravityScale = 0;
        //        }

        //        currentHit = hit.rigidbody;

        //        // Attach a joint
        //        Vector2 point = hit.point - new Vector2(hit.transform.position.x, hit.transform.position.y);
        //        point.x = point.x / hit.transform.localScale.x;
        //        point.y = point.y / hit.transform.localScale.y;

        //        ParentJoint.distance = hit.distance;
        //        ParentJoint.connectedBody = hit.rigidbody;
        //        ParentJoint.connectedAnchor = point;
        //    }
        //    else
        //    {
        //        Hooked = false;
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            GrappleCall();
        }

            if (Input.GetKey(KeyCode.E) && Hooked) // Go in
        {
            ParentJoint.distance -= .2f;
        }

        if (Input.GetKey(KeyCode.Q) && Hooked)
        {
            ParentJoint.distance += .2f;
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

    public void GrappleResponse(Vector2 hit)
    {
        //Vector2 point = hit.point - new Vector2(hit.transform.position.x, hit.transform.position.y);
        //point.x = point.x / hit.transform.localScale.x;
        //point.y = point.y / hit.transform.localScale.y;
    }

}
