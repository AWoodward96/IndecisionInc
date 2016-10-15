using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Transform Target;
    public float val;

    float minimumSize;
    float maximumSize;
    Rigidbody2D TargetRGB;

	// Use this for initialization
	void Start () {
        TargetRGB = Target.GetComponent<Rigidbody2D>();
        minimumSize = Camera.main.orthographicSize;
        maximumSize = minimumSize + 5;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z);

        if (TargetRGB)
            handleZoom();
	}


    void handleZoom()
    {
        float velocityMag = TargetRGB.velocity.magnitude;
        velocityMag /= val;

        float curSize = minimumSize + velocityMag;

        // Ensure it's between the min and the max
        curSize = Mathf.Clamp(curSize, minimumSize, maximumSize);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, curSize, Time.deltaTime);
    }
}
