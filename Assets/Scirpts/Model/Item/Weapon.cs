using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Weapon : Item {
	//unique key is used to identify non stackable items
	public string UniqueKey {
		get;
		private set;
	}
	
	//0% - 100%
	public float Remaining {
		get;
		set;
	}
	
	//different weapoon used up in different speed
	//counted as per battle cost
	public float Cost {
		get;
		set;
	}
	
	public float Attack {
		get;
		set;
	}
	
	public Range AttackRange {
		get;
		set;
	}
	
	public float CoolDown {
		get;
		set;
	}
	
	
	//the movement speed of the attack ring
	public float RingSpeed {
		get;
		set;
	}
	
	public ElementType Element {
		get;
		set;
	}
	
	
	//Note don't add Skills directly, use addSkill
	public List<WeaponSkill> WeaponSkills {
		get;
		private set;
	}
	
	public Dictionary<string, WeaponSkill> WeaponSkillIndex {
		get;
		private set;
	}

	public Weapon(string type, float remaining, float cost, Range attack_range, float cd, float ring_speed, ElementType element_type): base(type){
		Remaining = remaining;
		Cost = cost;
		AttackRange = attack_range;
		CoolDown = cd;
		RingSpeed = ring_speed;
		Element = element_type;

		WeaponSkills = new List<WeaponSkill> ();
		WeaponSkillIndex = new Dictionary<string, WeaponSkill> ();

		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();

		GenerateProperties ();
	}

	public Weapon(Weapon w): base (w){
		Remaining = w.Remaining;
		Cost = w.Cost;

		AttackRange = w.AttackRange;
		CoolDown = w.CoolDown;
		RingSpeed = w.RingSpeed;
		Element = w.Element;

		WeaponSkills = new List<WeaponSkill> ();
		for (int i = 0; i < w.WeaponSkills.Count; i ++) {
			WeaponSkills.Add(w.WeaponSkills[i].Clone() as WeaponSkill);
		}
		WeaponSkillIndex = new Dictionary<string, WeaponSkill> ();
		foreach (string key in w.WeaponSkillIndex.Keys) {
			WeaponSkillIndex.Add (key, w.WeaponSkillIndex[key]);
		}
		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();

		GenerateProperties ();
	}

	public void GenerateProperties(){
		Reset ();
		
		Attack = AttackRange.generateValueInRange() * CoolDown;
	}

	private void Reset(){
		Attack = 0;
	}

	public void AddSkill(WeaponSkill skill){
		this.WeaponSkills.Add (skill);
		this.WeaponSkillIndex.Add (skill.Type, skill);
	}

	public override Item Clone(){
		return new Weapon(this);
	}

    //gear specific functions
    public void Fix()
    {
        this.Remaining = 1f;
    }

    

}
