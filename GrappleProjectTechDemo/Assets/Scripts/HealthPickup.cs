using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class HealthPickup : MonoBehaviour
{

    BoxCollider2D myCollider;
    public bool isPickedUp;
    // Use this for initialization
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
        isPickedUp = false;


    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Player")
        {
            isPickedUp = true;
            Col.gameObject.GetComponent<PlayerRBController>().refillFuel();
        }
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        if (Col.tag == "Player")
        {
            GameObject.Destroy(gameObject);
        }
    }
}
