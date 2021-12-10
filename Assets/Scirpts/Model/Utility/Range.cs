using UnityEngine;

[System.Serializable]
public class Range{
	public float Min {
		get;
		set;
	}
	
	public float Max {
		get;
		set;
	}
	
	
	public Range(float min, float max){
		this.Min = min;
		this.Max = max;
	}
	
	public float generateValueInRange(){
		return (Random.Range(Min, Max));
	}
	
}