using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Accessory : Item {

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
	
	
	public Range DefenseRange {
		get;
		set;
	}
	
	//increase player defense
	public float Defense {
		get;
		set;
	}
	
	public Range HealthRange {
		get;
		set;
	}
	
	//increase player health
	public float Health {
		get;
		set;
	}
	
	public Range AttackRange {
		get;
		set;
	}
	
	//increase player attack
	public float Attack {
		get;
		set;
	}
	
	
	public Range ElementResisRange {
		get;
		set;
	}

    public Range ElementAttackRange
    {
        get;
        set;
    }
	
	
	// the elements that the accessory has certain resistence to
	public Dictionary<ElementType, float> ElementResisIndex {
		get;
		set;
	}
	

    public Dictionary<ElementType, float> ElementAttackIndex{
        get;
        set;
    }

   
	
	//how many propeties should this accessory have
	public int AttributeCount {
		get;
		set;
	}

	//unique key is used to identify non stackable items
	public string UniqueKey {
		get;
		private set;
	}

    public Accessory FixedData
    {
        get;
        set;
    }
    

    public Accessory(string type) : base(type)
    {
        ElementResisIndex = new Dictionary<ElementType, float>();
        ElementAttackIndex = new Dictionary<ElementType, float>();
    }


    public Accessory(string type, float remaining, float cost, Range defense_range , Range health_range, Range attack_range, Range element_resis_range, 
        Range element_attack_range, int attribute_count): base(type){
		Remaining = remaining;
		Cost = cost;

		DefenseRange = defense_range;
		HealthRange = health_range;
		AttackRange = attack_range;
		AttributeCount = attribute_count;
		ElementResisRange = element_resis_range;
        ElementAttackRange = element_attack_range;

		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();
        ElementResisIndex = new Dictionary<ElementType, float>();
        ElementAttackIndex = new Dictionary<ElementType, float>();
        GenerateProperties ();
        FixedData = new Accessory(type);
        
    }

	// copy constructor only copy range data and regenerate the accurate one.
	public Accessory(Accessory a): base(a){
		Remaining = a.Remaining;
		Cost = a.Cost;

		DefenseRange = a.DefenseRange;
		HealthRange = a.HealthRange;
		AttackRange = a.AttackRange;
		AttributeCount = a.AttributeCount;
		ElementResisRange = a.ElementResisRange;
        ElementAttackRange = a.ElementAttackRange;

		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();
		ElementResisIndex = new Dictionary<ElementType, float> ();
        ElementAttackIndex = new Dictionary<ElementType, float>();
        FixedData = a.FixedData;
		GenerateProperties ();
        AddStaticValues(FixedData);
	}

	//generate the actual accssory
	public void GenerateProperties(){
		Reset ();
		List<string> property_types = new List<string>{"attack", "defense", "health"
		,"ice_resis","fire_resis","wind_resis","dark_resis","holy_resis",
        "ice_attack","fire_attack","wind_attack","dark_attack","holy_attack"};
		System.Random random = new System.Random ();
		for (int i = 0; i < AttributeCount; i ++) {
			string type = property_types[random.Next(0, property_types.Count)];
			if(type.Equals("attack")){
				Attack += AttackRange.generateValueInRange();
			}
			if(type.Equals("defense")){
				Defense += DefenseRange.generateValueInRange();
			}
			if(type.Equals("health")){
				Health += HealthRange.generateValueInRange();
			}
			if(type.Equals("ice_resis")){
				IncreaseResis(ElementType.Ice);
			}
			if(type.Equals("fire_resis")){
				IncreaseResis(ElementType.Fire);
			}
			if(type.Equals("wind_resis")){
				IncreaseResis(ElementType.Wind);
			}
			if(type.Equals("dark_resis")){
				IncreaseResis(ElementType.Dark);
			}
			if(type.Equals("holy_resis")){
				IncreaseResis(ElementType.Holy);
			}

            if (type.Equals("ice_attack"))
            {
                IncreaseElementAttack(ElementType.Ice);
            }

            if (type.Equals("fire_attack"))
            {
                IncreaseElementAttack(ElementType.Fire);
            }

            if (type.Equals("wind_attack"))
            {
                IncreaseElementAttack(ElementType.Wind);
            }

            if (type.Equals("holy_attack"))
            {
                IncreaseElementAttack(ElementType.Holy);
            }

            if (type.Equals("dark_attack"))
            {
                IncreaseElementAttack(ElementType.Dark);
            }

            property_types.Remove(type);
		}
    
	}

	private void IncreaseResis(ElementType type){
		if(this.ElementResisIndex.ContainsKey(type)){
			this.ElementResisIndex[type] *= this.ElementResisRange.generateValueInRange();
		}else{
			this.ElementResisIndex[type] = this.ElementResisRange.generateValueInRange();
		}
	}


    private void IncreaseElementAttack(ElementType type)
    {
        if (this.ElementAttackIndex.ContainsKey(type))
        {
            this.ElementAttackIndex[type] += this.ElementAttackRange.generateValueInRange();
        }
        else
        {
            this.ElementAttackIndex[type] = this.ElementAttackRange.generateValueInRange();
        }
    }


    private void Reset(){
		Attack = 0;
		Defense = 0;
		Health = 0;
		ElementResisIndex.Clear ();
        ElementAttackIndex.Clear();
	}

	//TODO: Add Defense Skills
	public override Item Clone(){
        Accessory clone = new Accessory(this);
		return clone;
	}

    //gear specific functions
    public void Fix()
    {
        this.Remaining = 1f;
    }


    public void AddStaticValues(Accessory ac)
    {
        Attack += ac.Attack;
        Defense += ac.Defense;
        Health += ac.Health;

        foreach(ElementType key in ac.ElementResisIndex.Keys)
        {
            if (ElementResisIndex.ContainsKey(key))
            {
                ElementResisIndex[key] *= ac.ElementResisIndex[key];
            }
            else
            {
                ElementResisIndex[key] = ac.ElementResisIndex[key];
            }
        }

        foreach (ElementType key in ac.ElementAttackIndex.Keys)
        {
            if (ElementAttackIndex.ContainsKey(key))
            {
                ElementAttackIndex[key] += ac.ElementAttackIndex[key];
            }
            else
            {
                ElementAttackIndex[key] = ac.ElementAttackIndex[key];
            }
        }


    }


    public void TakeSpecialEffect()
    {
        switch (this.Type)
        {
            case ItemType.SKELETON_RING:
                Game.Current.Hero.DamageMultipler *= 2f;
                Game.Current.Hero.SufferingMultiplier *= 1.3f;
                break;

            case ItemType.HOLY_HAT:
                Game.Current.Hero.FoodCdMultiplier *= 0.5f;
                break;
           
            case ItemType.FLOWER_RING:
                Game.Current.Hero.SkillMultiplier *= 1.15f;
                break;

            case ItemType.MASTER_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier *= 0.6f;
                break;

            case ItemType.SPECIAL_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier *= 0.75f;
                break;

            case ItemType.NORMAL_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier *= 0.85f;
                break;

            case ItemType.BLESSING_RING:
                Game.Current.Hero.SkillMultiplier *= 1.5f;
                break;

        }
    }


    public void RemoveSpecialEffect()
    {
        switch (this.Type)
        {
            case ItemType.SKELETON_RING:
                Game.Current.Hero.DamageMultipler /= 2f;
                Game.Current.Hero.SufferingMultiplier /= 1.3f;
                break;


            case ItemType.HOLY_HAT:
                Game.Current.Hero.FoodCdMultiplier /= 0.5f;
                break;

            case ItemType.FLOWER_RING:
                Game.Current.Hero.SkillMultiplier /= 1.15f;
                break;

            case ItemType.MASTER_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier /= 0.6f;
                break;

            case ItemType.SPECIAL_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier /= 0.75f;
                break;

            case ItemType.NORMAL_BLACK_IRON_NECKLACE:
                Game.Current.Hero.CoolDownMultiplier /= 0.85f;
                break;

            case ItemType.BLESSING_RING:
                Game.Current.Hero.SkillMultiplier /= 1.5f;
                break;
        }
    }



}
