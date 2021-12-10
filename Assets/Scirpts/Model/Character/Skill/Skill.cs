using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Skill: BaseModel {

	//generics fields for different skills
	public float Arg1 {
		get;
		set;
	}
	
	public float Arg2 {
		get;
		set;
	}
	
	public Skill(string type): base(type){
	}

	public Skill(string type, float arg1):this(type){
		this.Arg1 = arg1;
	}

	public Skill(string type, float arg1, float arg2):this(type, arg1){
		this.Arg1 = arg1;
		this.Arg2 = arg2;
	}

	public Skill(Skill s): base(s){
		Arg1 = s.Arg1;
		Arg2 = s.Arg2;
	}

	abstract public Skill Clone ();
}
