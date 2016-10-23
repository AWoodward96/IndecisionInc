using UnityEngine;
using System.Collections;


[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{

    public int maxLength;
    public Vector2 Direction;
    public bool state;
    //public Color prefireColor;
    //public Color fireColor;
    public float toggleTime;

    LayerMask GroundMask;
    LayerMask PlayerMask;
    LineRenderer myLineRenderer;
    bool running;
    bool prefire;
    float lerpedVal;

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
            StartCoroutine(waitTimeMinusOne());
        }

        myLineRenderer.enabled = (state || prefire);
        if(state)
        {

            myLineRenderer.SetWidth(.6f, .6f);
            //myLineRenderer.SetColors(fireColor, fireColor);


            Ray2D r = new Ray2D(transform.position, Direction);
            myLineRenderer.SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, maxLength, GroundMask | PlayerMask);
            if (hit)
            {
                Debug.Log(hit.point);
                myLineRenderer.SetPosition(1, hit.point);
                if(hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<PlayerRBController>().KillPlayer();
                }
            }
            else
            {
                myLineRenderer.SetPosition(1, (Vector2)transform.position + (Direction).normalized * maxLength);

            }

        }

        if(prefire)
        {
            
            lerpedVal = Mathf.Lerp(lerpedVal, .1f, .1f);
            myLineRenderer.SetWidth(lerpedVal, lerpedVal);
            //myLineRenderer.SetColors(prefireColor, prefireColor);

            Ray2D r = new Ray2D(transform.position, Direction);
            myLineRenderer.SetPosition(0, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, maxLength, GroundMask);
            if (hit)
            {
                Debug.Log(hit.point);
                myLineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                myLineRenderer.SetPosition(1, (Vector2)transform.position + (Direction).normalized * maxLength);
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
        Debug.Log("Normal");

        state = !state;
        running = false;
        prefire = false;
    }

    IEnumerator waitTimeMinusOne()
    {
        float num = toggleTime - 1;
        if (num < 0)
            yield break;

        if (state)
            yield break;

        yield return new WaitForSeconds(num);
        Debug.Log("Minus 1");
        lerpedVal = .0001f;
        prefire = true;
    }
}
