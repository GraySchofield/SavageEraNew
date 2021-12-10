using UnityEngine;
using System.Collections;

public class BuildingDetailDialogController : MonoBehaviour {
	public void CloseDialog(){
		Destroy (transform.parent.gameObject); // close the dialog
	}
}
