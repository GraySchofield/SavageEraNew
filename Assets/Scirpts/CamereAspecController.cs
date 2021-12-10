using UnityEngine;
using System.Collections;

public class CamereAspecController : MonoBehaviour {
	public float width = 9f;
	public float height = 16f;

	// Use this for initialization
	void Start () {
		float targetAspect = width / height;

		float windowAspect = (float)Screen.width / (float)Screen.height;

		float scaleHeight = windowAspect / targetAspect; // the viewport height should be scaled by this

		Camera cam = Camera.main;

		if (scaleHeight < 1.0f) {
			Rect rect = cam.rect;
			rect.width = 1.0f;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (1f - scaleHeight) / 2.0f;

			cam.rect = rect;
		} else {
			float scaleWidth = 1f / scaleHeight;
			Rect rect = cam.rect;

			rect.width = scaleWidth;
			rect.height = 1f;
			rect.x = (1f - scaleWidth) / 2f;
			rect.y = 0;
	
			cam.rect = rect;
		}

	}
	

}
