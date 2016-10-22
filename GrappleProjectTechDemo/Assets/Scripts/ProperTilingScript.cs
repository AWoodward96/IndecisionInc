using UnityEngine;
using System.Collections;

public class ProperTilingScript : MonoBehaviour {

    // A symple script that modifies the textures tile values based on the current scale
    SpriteRenderer myRenderer;
    public float modifier;

    Material myMat;
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        if(myRenderer)
            myMat = myRenderer.material;

        if (modifier == 0)
            modifier = 1;
    }

	// Update is called once per frame
	void Update () {
	    if(myMat)
        {
            myMat.mainTextureScale = transform.parent.lossyScale / modifier;
        }
	}
}
