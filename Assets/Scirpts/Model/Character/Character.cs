using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character{
	public string Name {
		get;
		protected set;
	}

	public string Description {
		get;
		set;
	}

	public float Attack {
		get;
		set;
	}

	public float Defense {
		get;
		set;
	}

	private float current_health;
	public float CurrentHealth {
		get{
			return current_health;
		}
		set{
			if(value > HealthUpperLimit){
				current_health = HealthUpperLimit;
			}
			else{
				current_health = value;
			}
		}
	}

	public float HealthUpperLimit {
		get;
		set;
	}


}
