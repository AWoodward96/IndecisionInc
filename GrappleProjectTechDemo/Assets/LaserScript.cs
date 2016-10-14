using UnityEngine;
using System.Collections;


[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{

    public int maxLength;
    public Vector2 Direction;
    public bool state;
    
    public float toggleTime;

    LayerMask GroundMask;
    LayerMask PlayerMask;
    LineRenderer myLineRenderer;
    bool running;

    // Use this for initialization
    void Start()
    {
        GroundMask = LayerMask.GetMask("Ground");
        PlayerMask = LayerMask.GetMask("Player");
        myLineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!running)
        {
            running = true;
            StartCoroutine(waitTime());
        }

        myLineRenderer.enabled = state;
        if(state)
        {
            Ray2D r = new Ray2D(transform.position, Direction);
            myLineRenderer.SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, maxLength, GroundMask);
            RaycastHit2D hit2 = Physics2D.Raycast(r.origin, r.direction, maxLength, PlayerMask);
            if (hit)
            {
                Debug.Log(hit.point);
                myLineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                myLineRenderer.SetPosition(1, (Vector2)transform.position + (Direction).normalized * maxLength);
            }


            if(hit2)
            {
                hit2.rigidbody.GetComponent<PlayerRBController>().KillPlayer();
            }
        }
    }

    IEnumerator waitTime()
    {
        if (toggleTime == 0)
        {
            Debug.Log("Your wait time on " + gameObject + " is currently 0 or null. This isn't optimal.");
        }
        else
        {
            yield return new WaitForSeconds(toggleTime);
        }


        state = !state;
        running = false;
    }
}
