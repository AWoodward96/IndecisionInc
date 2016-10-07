using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRBController : MonoBehaviour
{

    public float gravity;
    Rigidbody2D myRigidbody;
    public float jumpForce;
    public Slider jetpackSlider;
    private int jetpackCooldown;

    // Just in case we need it
    public GameObject GrapplingHookPrefab;
    GrappleProjectile grapplingHook;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        jetpackCooldown = 100;

        GameObject grapple = GameObject.FindGameObjectWithTag("GrapplingHook");
        if(grapple)
        {
            grapplingHook = grapple.GetComponent<GrappleProjectile>();
        }
        else
        {
            grapple = (GameObject)Instantiate(GrapplingHookPrefab, transform.position, Quaternion.identity);
            grapplingHook = grapple.GetComponent<GrappleProjectile>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = new Vector3(0, gravity, 0);
        handleInput();
    }

    void FixedUpdate()
    {
        if (jetpackCooldown < 100)
        {
            jetpackCooldown += 1;
        }

        if(jetpackSlider)
        {
            jetpackSlider.value = jetpackCooldown;

            if (jetpackCooldown < 30)
            {
                jetpackSlider.image.color = Color.red;
            }
            else
            {
                jetpackSlider.image.color = Color.cyan;
            }
        }

    }


    void handleInput()
    {
        if (jetpackCooldown >= 30)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                myRigidbody.AddForce(new Vector2(0, jumpForce));
                jetpackCooldown -= 30;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                myRigidbody.AddForce(new Vector2(-jumpForce, 0));
                jetpackCooldown -= 30;
            }


            if (Input.GetKeyDown(KeyCode.S))
            {
                myRigidbody.AddForce(new Vector2(0, -jumpForce));
                jetpackCooldown -= 30;
            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                myRigidbody.AddForce(new Vector2(jumpForce, 0));
                jetpackCooldown -= 30;
            }
        }
    }



}
