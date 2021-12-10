using UnityEngine;
using System.Collections;

public class CameraFollowMe: MonoBehaviour {
	public Transform target;
	Camera cam;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

	public bool isDragging = false; //whether the camera is being dragged by the user

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	//	cam.orthographicSize = (Screen.height / 100f) / 3f;

		if (target != null && !isDragging) {
            Vector3 newPos = new Vector3(target.position.x, target.position.y, target.position.z);
			transform.position = Vector3.Lerp(transform.position, newPos, 0.1f) + new Vector3(0,0,-10f);
		}


        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
            Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y), -10);


	}

	public float speed = 0.05F;
	void Update() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			isDragging = true;
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
		}
	}
}
