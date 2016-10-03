using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class Object : MonoBehaviour {

    const float GRAVITY = -4.9f; // aprox 50% of normal gravity
    //public float GRAVITY = -4;
    const float VELOCITYCLAMP = 100;
    LayerMask Ground;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public float mass;
    public bool isEffectedByGravity;

    public float frictionModifier;

    CharacterController myCharacterController;

    [HideInInspector]
    public bool isGrounded;



    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
        Ground = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        checkGrounded();


        if (isEffectedByGravity && !isGrounded)
            ApplyForce(new Vector2(0, GRAVITY));

        Velocity += Acceleration;
        Velocity = Vector3.ClampMagnitude(Velocity, VELOCITYCLAMP);

        myCharacterController.Move(Velocity * Time.deltaTime);


        Acceleration = Vector2.zero;
        Velocity *= frictionModifier;
    }


    public void ApplyForce(Vector2 _force)
    {
        Acceleration += _force / mass;
    }

    void checkGrounded()
    {
        // Determines if this object is on the ground or not
        Vector2 basePosition = transform.position;
        basePosition.y -= myCharacterController.height/2;
        Ray r = new Ray(basePosition, Vector2.down);

        // Raycast check
        isGrounded = Physics.Raycast(r, myCharacterController.skinWidth, Ground);
    }
}
