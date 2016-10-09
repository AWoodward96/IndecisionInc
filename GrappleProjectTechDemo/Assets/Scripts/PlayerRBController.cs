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

    public ParticleSystem emitterUp;
    public ParticleSystem emitterDown;
    public ParticleSystem emitterLeft;
    public ParticleSystem emitterRight;

    // Just in case we need it
    public GameObject GrapplingHookPrefab;
    GrappleProjectile grapplingHook;

    [HideInInspector]
    public GameManager GM;


    // Use this for initialization
    void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody2D>();
        /*
        emitterUp    = GameObject.Find("up_facing_jet").GetComponent<ParticleSystem>();
        emitterDown  = GameObject.Find("down_facing_jet").GetComponent<ParticleSystem>();
        emitterLeft  = GameObject.Find("left_facing_jet").GetComponent<ParticleSystem>();
        emitterRight = GameObject.Find("right_facing_jet").GetComponent<ParticleSystem>();*/
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
                emitterDown.enableEmission = true;
                myRigidbody.AddForce(new Vector2(0, jumpForce));
                jetpackCooldown -= 30;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                emitterRight.enableEmission = true;
                myRigidbody.AddForce(new Vector2(-jumpForce, 0));
                jetpackCooldown -= 30;
            }


            if (Input.GetKeyDown(KeyCode.S))
            {
                emitterUp.enableEmission = true;
                myRigidbody.AddForce(new Vector2(0, -jumpForce));
                jetpackCooldown -= 30;
            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                emitterLeft.enableEmission = true;
                myRigidbody.AddForce(new Vector2(jumpForce, 0));
                jetpackCooldown -= 30;
            }

            if (!Input.anyKey)
            {
                emitterLeft.enableEmission = false;
                emitterUp.enableEmission = false;
                emitterRight.enableEmission = false;
                emitterDown.enableEmission = false;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Application.LoadLevel("LevelSelector");
        }
    }


    public void KillPlayer()
    {
        grapplingHook.resetHook();
        if (GM)
            GM.resetPlayer();
        gameObject.SetActive(false); // This will shut everything down for a bit
    }

}
