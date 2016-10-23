using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

static class Constants
{
    public const int EMITTER_UP     = 0;
    public const int EMITTER_DOWN   = 1;
    public const int EMITTER_LEFT   = 2;
    public const int EMITTER_RIGHT  = 3;
}

public class AnimationDuration
{
    private float duration;
    private float currentTime;
    public bool active;

    public AnimationDuration(float lengthOfAnimation)
    {
        duration = lengthOfAnimation;
        currentTime = 0;
        active = true;
    }

    public void update(float dt)
    {
        currentTime += dt;
        if (currentTime >= duration) active = false;
    }
    public void reset()
    {
        currentTime = 0;
        active = true;
    }
}

public class PlayerRBController : MonoBehaviour
{

    public float gravity;
    Rigidbody2D myRigidbody;
    public float jumpForce;
    public Slider jetpackSlider;
    public float jetpackCooldown;
    public int jetpackFuel;
    public Image sliderColor;

    //public Dictionary<string, ParticleSystem> emitters;
    public ParticleSystem[] emitters;
    private AnimationDuration[] durations = new AnimationDuration[4];
    public float jetpackDuration;
    public bool permaJetpack;

    public bool lockJetpack;
    public bool boostJetpack;

    // Just in case we need it
    public GameObject GrapplingHookPrefab;
    GrappleProjectile grapplingHook;

    [HideInInspector]
    public GameManager GM;
    public bool AcceptInput = true;


    // Use this for initialization
    void Start()
    {
        
        Debug.Log(jetpackSlider.GetComponent("Image"));

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

        lockJetpack = false;
        boostJetpack = false;
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = new Vector3(0, gravity, 0);

        grapplingHook.AcceptInput = AcceptInput;
        if(AcceptInput)
            handleInput();

        for (int i = 0; i < durations.Length; i++)
        {
            if (durations[i] != null) durations[i].update(Time.deltaTime);
        }

        // Also sync the GUI to the players position
        if(jetpackSlider)
        {
            jetpackSlider.transform.parent.transform.position = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (jetpackCooldown < jetpackFuel)
        {
            jetpackCooldown += .4f;
        }

        if(jetpackSlider)
        {
            jetpackSlider.value = jetpackCooldown;
            
            if (jetpackCooldown < 30)
            {
                sliderColor.color = Color.red;
            }
            else
            {
                sliderColor.color = Color.cyan;
            }
        }

    }


    void handleInput()
    {
            if (Input.GetKeyDown(KeyCode.W) && jetpackCooldown >= 30 && !lockJetpack)
            {
                setJetpackState(Constants.EMITTER_DOWN);
                if(!boostJetpack)
                {
                    myRigidbody.AddForce(new Vector2(0, jumpForce));
                }
                else
                {
                    myRigidbody.AddForce(new Vector2(0, jumpForce * 2));
                }             
                if (!permaJetpack)
                {
                    jetpackCooldown -= 30;
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && jetpackCooldown >= 30 && !lockJetpack)
            {
                setJetpackState(Constants.EMITTER_RIGHT);
                if (!boostJetpack)
                {
                    myRigidbody.AddForce(new Vector2(-jumpForce, 0));
                }
                else
                {
                    myRigidbody.AddForce(new Vector2(-jumpForce * 2, 0));
                }
                if (!permaJetpack)
                {
                    jetpackCooldown -= 30;
                }
            }


            if (Input.GetKeyDown(KeyCode.S) && jetpackCooldown >= 30 && !lockJetpack)
            {
                setJetpackState(Constants.EMITTER_UP);
                if (!boostJetpack)
                {
                    myRigidbody.AddForce(new Vector2(0, -jumpForce));
                }
                else
                {
                    myRigidbody.AddForce(new Vector2(0, -jumpForce * 2));
                }
                if (!permaJetpack)
                {
                    jetpackCooldown -= 30;
                }
            }


            if (Input.GetKeyDown(KeyCode.D) && jetpackCooldown >= 30 && !lockJetpack)
            {
                setJetpackState(Constants.EMITTER_LEFT);
                if (!boostJetpack)
                {
                    myRigidbody.AddForce(new Vector2(jumpForce, 0));
                }
                else
                {
                    myRigidbody.AddForce(new Vector2(jumpForce * 2, 0));
                }
                if (!permaJetpack)
                {
                    jetpackCooldown -= 30;
                }
            }

            if (!Input.anyKey)
            {
               
            }

            for (int i = 0; i < durations.Length; i++)
            {
                if (durations[i] != null && !durations[i].active) emitters[i].enableEmission = false;
            }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Application.LoadLevel("LevelSelector");
        }
    }


    public void KillPlayer()
    {
        grapplingHook.Hooked = false;
        jetpackCooldown = 100;
        grapplingHook.resetHook();

        if (GM)
            GM.resetPlayer(grapplingHook);
        else
        {
            GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            GM.resetPlayer(grapplingHook);
        }

        grapplingHook.gameObject.SetActive(false);
        gameObject.SetActive(false); // This will shut everything down for a bit
    }


    private void setJetpackState(int emitterIndex)
    {
        emitters[emitterIndex].enableEmission = true;

        if (durations[emitterIndex]!= null)
        {
            durations[emitterIndex].reset();
        }
        else
        {
            durations[emitterIndex] = new AnimationDuration(jetpackDuration);
        }
    }

    public void refillFuel()
    {
        jetpackCooldown = jetpackFuel;
    }

}
