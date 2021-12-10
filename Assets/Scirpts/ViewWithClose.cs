using UnityEngine;
using System.Collections;

public class ViewWithClose : MonoBehaviour {

	public void CloseSelf(){
		Destroy (gameObject);
	}
}
