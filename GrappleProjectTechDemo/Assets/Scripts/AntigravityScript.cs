using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class AntigravityScript : MonoBehaviour {

    BoxCollider2D myCollider;

    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
}
