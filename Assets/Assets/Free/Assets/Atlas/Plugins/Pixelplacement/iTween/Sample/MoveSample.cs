using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "y", 2,"easeType", "easeInOutExpo", 
		                                       "delay", .1));
	}
}

