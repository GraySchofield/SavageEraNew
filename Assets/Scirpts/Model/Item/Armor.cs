using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Armor : Item {
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
	
	public List<ArmorSkill> ArmorSkills {
		get;
		private set;
	}

	public Range ElementResisRange {
		get;
		set;
	}

    public bool hasResistance
    {
        get;
        set;
    }
	
	// the elements that the armor has certain resistence to
	public Dictionary<ElementType, float> ElementResisIndex {
		get;
		set;
	}

	// need to add skill and resis.
	public Armor(string type, float remaining, float cost, Range defense_range , Range health_range): base(type){
		Remaining = remaining;
		Cost = cost;

		DefenseRange = defense_range;
		HealthRange = health_range;
		ArmorSkills = new List<ArmorSkill> ();
		ElementResisIndex = new Dictionary<ElementType, float> ();
		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();
        hasResistance = false;
        GenerateProperties ();
	}

	public Armor(Armor a): base (a){
		Remaining = a.Remaining;
		Cost = a.Cost;

		DefenseRange = a.DefenseRange;
        HealthRange = a.HealthRange;
        ElementResisRange = a.ElementResisRange;
        hasResistance = a.hasResistance;

        ArmorSkills = new List<ArmorSkill> ();
        for(int i = 0; i < a.ArmorSkills.Count; i++)
        {
            ArmorSkills.Add(a.ArmorSkills[i]);
        }
        
		ElementResisIndex = new Dictionary<ElementType, float> ();
        /*
        foreach (ElementType key in a.ElementResisIndex.Keys)
        {
            ElementResisIndex[key] = a.ElementResisIndex[key];
        }
        */

        UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();

		GenerateProperties ();
	}

    public void AddArmorSkill(ArmorSkill skill)
    {
        ArmorSkills.Add(skill);
    }



	public void GenerateProperties(){
		Reset ();
		Defense = DefenseRange.generateValueInRange();
		Health = HealthRange.generateValueInRange();
        if (hasResistance)
        {
            generateElementResis();
        }
        
        //skill and resis
    }


    private void generateElementResis()
    {
        ElementType[] elements = { ElementType.Fire, ElementType.Ice, ElementType.Wind,
        ElementType.Holy, ElementType.Dark};
        System.Random rnd = new System.Random();
        int idx = rnd.Next(0, elements.Length);
        ElementResisIndex[elements[idx]] = ElementResisRange.generateValueInRange();
    }


	private void Reset(){
		Defense = 0;
        Health = 0;
        ElementResisIndex.Clear();
    }

	public override Item Clone(){
		return new Armor(this);
	}

    //gear specific functions
    public void Fix()
    {
        this.Remaining = 1f;
    }
}
