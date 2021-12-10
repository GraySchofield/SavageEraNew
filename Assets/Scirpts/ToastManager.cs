using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour {
	public Transform toastTextPrefab;
	public Transform toastMapTextPrefab;


	public  void ToastMessage(string msg){
		Transform text_clone = Instantiate (toastTextPrefab) as Transform;
		text_clone.SetParent (transform);
		RectTransform rf = text_clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3(1f,1f,1f);
		//rf.offsetMax = new Vector2 (0f, 0f);
		//rf.offsetMin = new Vector2 (0f, 0f);
		rf.anchoredPosition3D = new Vector3 (0f, 0f, 0f);
		text_clone.GetComponent<Text> ().text = msg;

		Destroy (text_clone.gameObject, 1.2f); //time delay is a little longer than the animation
	}



	public  void ToastMessageInMap(string msg){
		Transform text_clone = Instantiate (toastMapTextPrefab) as Transform;
		text_clone.SetParent (transform);
		RectTransform rf = text_clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3(1f,1f,1f);
		rf.anchoredPosition3D = new Vector3 (0f, 0f, 0f);
		text_clone.GetComponent<Text> ().text = msg;
		
		Destroy (text_clone.gameObject, 1.2f); //time delay is a little longer than the animation
	}

}
