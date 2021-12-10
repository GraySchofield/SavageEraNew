using UnityEngine;
using System.Collections;
[System.Serializable]
public class Damage {
	public float DamageAmount {
		get;
		set;
	}

	public ElementType EType {
		get;
		set;
	}

	public bool isAOE {
		get;
		set;
	}

	public Damage(ElementType type, float amount){
		this.EType = type;
		this.DamageAmount = amount;
		this.isAOE = false;
	}

	public Damage(ElementType type, float amount, bool is_aoe){
		this.EType = type;
		this.DamageAmount = amount;
		this.isAOE = is_aoe;
	}


	public Damage Clone(){
		Damage clone = new Damage (this.EType, this.DamageAmount, this.isAOE);
		return clone;
	}

    public Damage CloneWithSkillMultiplier()
    {
        Damage clone = new Damage(this.EType, this.DamageAmount, this.isAOE);
        clone.DamageAmount *= Game.Current.Hero.SkillMultiplier;
        return clone;
    }
    

}
