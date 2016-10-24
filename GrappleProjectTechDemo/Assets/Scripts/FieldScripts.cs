using UnityEngine;
using System.Collections;

public class FieldScripts : MonoBehaviour {

    public enum FieldType
    {
        Antigravity,
        Booster,
        Inhibitor,
    };
    public FieldType fType;

    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fType == FieldType.Antigravity)
            {
                other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
            if (fType == FieldType.Booster)
            {
                other.GetComponent<PlayerRBController>().boostJetpack = true;
            }
            if (fType == FieldType.Inhibitor)
            {
                other.GetComponent<PlayerRBController>().lockJetpack = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (fType == FieldType.Antigravity)
            {
                other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
            if (fType == FieldType.Booster)
            {
                other.GetComponent<PlayerRBController>().boostJetpack = false;
            }
            if (fType == FieldType.Inhibitor)
            {
                other.GetComponent<PlayerRBController>().lockJetpack = false;
            }
        }
    }
}
