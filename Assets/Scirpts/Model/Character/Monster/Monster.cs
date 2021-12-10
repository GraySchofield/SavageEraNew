using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum GeneralDropType{
	Common,
	Uncommon,
	Rare
}

[System.Serializable]
public class Monster : Character {
	public float AttackCoolDown {
		get;
		set;
	}

	public float AttackCoolDownLimit {
		get;
		set;                
	}

	//for now the type will be just equal to the name
	public string Type {
		get;
		private set;
	}


	public bool isAlive {
		get;
		set;
	}


	//the current element type of the monsters, can be changed by skills
	public ElementType Element {
		set;
		get;
	}

	//the original true element type of the monster
	public ElementType OriginalElement {
		get;
		private set;
	}


	public Dictionary<string,BattleState> BattleStatesIndexs {
		get;
		private set;
	}

	private List<string> all_battle_states = new List<string> ();

	public Monster(string type , float cool_down_limit, float attack, float defense, float health, ElementType element_type ){
		this.Name = Lang.Current[type];
		this.Description = Lang.Current[type + "_des"];
		this.AttackCoolDownLimit = cool_down_limit;
		this.AttackCoolDown = Random.value * this.AttackCoolDownLimit;
		this.Attack = attack;
		this.Defense = defense;
		this.Type = type; //this may need to be changed
		this.HealthUpperLimit = health;
		this.CurrentHealth = this.HealthUpperLimit;
		this.AttackSkills = new List<MonsterAttackSkill> ();
		this.DefenseSkills = new List<MonsterDefenseSkill> ();
		this.UltiSkills = new List<MonsterUltiSkill> ();
		this.isAlive = true;
		this.Element = element_type;
		OriginalElement = element_type; 
		this.DropList = new List<DropNode> ();
		GeneralDrops = new List<GeneralDropType> ();
		CanRunAwayFrom = true;  //normal monsters can all be runaway from 
		BattleStatesIndexs = new Dictionary<string, BattleState>();
        this.isBoss = false; //default not boss
        this.isInEvilChamber = false;
	}


	public void AddBattleState(BattleState battle_state){
		if (BattleStatesIndexs.ContainsKey (battle_state.Type)) {
			BattleStatesIndexs[battle_state.Type] = battle_state;
		} else {
			BattleStatesIndexs.Add (battle_state.Type,battle_state);
			all_battle_states.Add (battle_state.Type);
			battle_state.Activate(this);
		}
	}

    //purify
    public void RemoveAllDebuffStates()
    {
        string[] de_buff_states = {
        StateType.BATTLE_STATE_STUN,StateType.BATTLE_STATE_POISON,
        StateType.BATTLE_STATE_WEAK, StateType.BATTLE_STATE_SLOW,
        StateType.BATTLE_STATE_BURNT,StateType.BATTLE_STATE_ICE,StateType.BATTLE_STATE_FIRE,
        StateType.BATTLE_STATE_WIND,StateType.BATTLE_STATE_DARK,StateType.BATTLE_STATE_HOLY,
        StateType.BATTLE_STATE_ARMOR_BREAK};

        for(int i = 0; i < de_buff_states.Length; i++)
        {
            RemoveBattleState(de_buff_states[i]);
        }
    }



	public void RemoveBattleState(string battle_state_type){
        if (BattleStatesIndexs.ContainsKey(battle_state_type))
        {
            BattleStatesIndexs[battle_state_type].Deactivate(this);
            BattleStatesIndexs.Remove(battle_state_type);
        }   
	}
    

	public void UpdateBattleStates(float period, BattleEngine battle_engine, int idx){
		for (int i = 0; i < all_battle_states.Count; i++) {
			string key = all_battle_states[i];
			if(BattleStatesIndexs.ContainsKey(key)){
				BattleStatesIndexs[key].UpdateMonsterState(period,this, battle_engine, idx);
			}else{
				all_battle_states.RemoveAt(i);
			}
		}
	}


	public bool isInBattleState(string battle_state_type){
		if (BattleStatesIndexs.ContainsKey (battle_state_type)) {
			return true;
		}else{
			return false;
		}
	}


	
	public  void DoBattle(float period , BattleEngine battle_engine, int idx){
		//attack the player, when cd is ready
		if (!isInBattleState (StateType.BATTLE_STATE_STUN)) {
			//Battle only when not stunned
			if (this.AttackCoolDown > period) {
				//cd not yet reached 
				this.AttackCoolDown -= period;
			} else {
				this.AttackCoolDown = 0f;
			}

			if (this.AttackCoolDown == 0) {
				//attack when cd is reached
				//run Over all the Attack Skills and Take Effect
				this.AttackCoolDown = this.AttackCoolDownLimit;// reset cd 
				Damage monster_damage = new Damage (this.Element, this.Attack);

				for (int i = 0; i < AttackSkills.Count; i ++) {
					AttackSkills [i].TakeEffect (this, monster_damage, battle_engine, idx);
				}
		
				battle_engine.ShowMonsterAttackEffect (idx);

				Game.Current.Hero.SufferDamage (monster_damage, battle_engine, this);
				battle_engine.PlayMonsterSound (idx, "Attack");

			} 

			//Ultis 
			for (int i = 0; i < UltiSkills.Count; i ++) {
				UltiSkills [i].TakeEffect (period, this, battle_engine, idx);
			}
		}
	}

	//idx is the index of the monster in the current battle list
	public void SufferDamage(Damage damage, int idx, BattleEngine battle_engine){
		//TODO need to run over all the Defense Skills and Take Effect

		// the Damage object will be changed throughout the calculation
		//1. Skills
		for (int i = 0; i < DefenseSkills.Count; i ++) {
			DefenseSkills[i].TakeEffect(this,damage,battle_engine,idx);
		}

		//2. Element	
		CalculateElementDamage(damage);
		//3. Armor
		CalculateArmoredDamage(damage);

        damage.DamageAmount *= Game.Current.Hero.DamageMultipler;


        if (this.isInBattleState(StateType.BATTLE_STATE_IMMORTAL))
        {
            damage.DamageAmount = 0f;
        }

        if (this.isInBattleState(StateType.BATTLE_STATE_MAGIC_IMMUNE))
        {
            if(damage.EType != ElementType.None)
            {
                damage.DamageAmount = 0f;
            }
        }

		//apply the final damage to the monster
		if (this.CurrentHealth > damage.DamageAmount) {
			this.CurrentHealth -= damage.DamageAmount;
		} else {
			this.CurrentHealth = 0;
			this.isAlive = false;
			battle_engine.PlayMonsterSound(idx,"Die");

		}
		battle_engine.ShowMonsterDamagedText (damage.DamageAmount, damage.EType, idx);

		
	}

	//monster alive again and restore to full health
	public void Revive(){
		this.CurrentHealth = this.HealthUpperLimit;
		this.isAlive = true;
	}


	private void CalculateElementDamage(Damage damage){
		switch (damage.EType) {
		case ElementType.Ice:
			if(this.Element == ElementType.Fire){
				damage.DamageAmount = damage.DamageAmount * 2f; //150% damage
			}else if(this.Element == ElementType.Wind){
				damage.DamageAmount = damage.DamageAmount * 0.5f;
			}else{
				damage.DamageAmount = damage.DamageAmount;
			}
			break;
		case ElementType.Fire:
			if(this.Element == ElementType.Wind){
				damage.DamageAmount = damage.DamageAmount * 2f; //150% damage
			}else if(this.Element == ElementType.Ice){
				damage.DamageAmount = damage.DamageAmount * 0.5f;
			}else{
				damage.DamageAmount = damage.DamageAmount;
			}
			break;
		case ElementType.Wind:
			if(this.Element == ElementType.Ice){
				damage.DamageAmount = damage.DamageAmount * 2f; //150% damage
			}else if(this.Element == ElementType.Fire){
				damage.DamageAmount = damage.DamageAmount * 0.5f;
			}else{
				damage.DamageAmount = damage.DamageAmount;
			}
			break;
		case ElementType.Holy:
			if(this.Element == ElementType.Dark){
				damage.DamageAmount = damage.DamageAmount * 3f; //150% damage
			}else{
				damage.DamageAmount = damage.DamageAmount * 0.8f;
			}
			break;
		case ElementType.Dark:
			if(this.Element == ElementType.Holy){
				damage.DamageAmount = damage.DamageAmount * 0.3f; //150% damage
			}else{
				damage.DamageAmount = damage.DamageAmount * 1.5f;
			}
			break;
		case ElementType.None:
			damage.DamageAmount = damage.DamageAmount * 1f;
			break;
	
		}
	}

	private void CalculateArmoredDamage(Damage damage){
        float reduction = this.Defense * Config.armor_multiplier / (1 + this.Defense * Config.armor_multiplier);
        damage.DamageAmount = damage.DamageAmount * (1f - reduction);
        /*
        if (this.Defense >= damage.DamageAmount) {
			damage.DamageAmount = 0;
		} else {
			damage.DamageAmount -= this.Defense;
		}
        */
	}



	public List<MonsterAttackSkill> AttackSkills {
		get;
		set;
	}
	
	public List<MonsterDefenseSkill> DefenseSkills {
		get;
		set;
	}

	public List<MonsterUltiSkill> UltiSkills {
		get;
		set;
	}


	//wether you can run away from this monster or not
	public bool CanRunAwayFrom {
		get;
		set;
	}

    //wether the boss is a boss or not
    public bool isBoss
    {
        get;
        set;
    }


    //if the monster is from dark chamber
    public bool isInEvilChamber
    {
        get;
        set;
    }


	//monster may have a specific drop list
	public List<DropNode> DropList {
		get;
		private set;
	}

	//common, uncommon, rare
	//general drop is seperated from drop list
	public List<GeneralDropType> GeneralDrops {
		get;
		private set;
	}

}
