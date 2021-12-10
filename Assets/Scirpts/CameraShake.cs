using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Camera mainCamera;
	float shakeAmount = 0;

	void Awake(){
		if (mainCamera == null) {
			mainCamera = Camera.main;
		}
	}


	//Shake() will be the function that you will call anywhere you want to shake and stop the camera
	public void Shake(float amt, float length){
		shakeAmount = amt;
		InvokeRepeating ("DoShake", 0, 0.01f); //call function after 0 seconds for every 0.01seconds
		Invoke ("StopShake", length); //call function after "length" seconds
        
	}


	void DoShake(){
		if (shakeAmount > 0) {
			Vector3 camPos = mainCamera.transform.position;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x += offsetX;
			camPos.y += offsetY;
			mainCamera.transform.position = camPos;
		}
	}

	void StopShake(){
		CancelInvoke("DoShake");
		mainCamera.transform.localPosition = Vector3.zero;  //reset the position relative to the parent object
	}
}
