using UnityEngine;
using System.Collections;

[System.Serializable]
public class AttackRange  {
	//min is inclusive
	//max is exlusive
	public float Min {
		get;
		private set;
	}

	public float Max {
		get;
		private set;
	}

	public AttackRange(float min, float max){
		this.Min = min;
		this.Max = max;
	}

	public bool IsInRange(float value){
		if (value >= Min && value < Max) {
			return true;
		} else {
			return false;
		}
	}
}
