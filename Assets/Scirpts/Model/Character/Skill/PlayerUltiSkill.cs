using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerUltiSkill : Skill {
	//how much can be charged every single attack
	public float ChargeAmount {
		get;
		set;
	}
	
	public Damage SkillDamage {
		get;
		set;
	}

	public int SkillLevel {
		get;
		set;
	}

	public PlayerUltiSkill(string type, float charge_amount):base(type){
		ChargeAmount = charge_amount;
		SkillLevel = 0;
	}
	
	public PlayerUltiSkill(string type, float charge_amount, float arg1):base(type, arg1){
		ChargeAmount = charge_amount;
		SkillLevel = 0;
	}
	
	public PlayerUltiSkill(string type, float charge_amount, float arg1, float arg2):base(type, arg1, arg2){
		ChargeAmount = charge_amount;
		SkillLevel = 0;
	}

	public PlayerUltiSkill(string type): base(type){
		ChargeAmount = 0;
		SkillLevel = 0;
	}
	
	public PlayerUltiSkill(PlayerUltiSkill s):base(s){
		ChargeAmount = s.ChargeAmount;
		SkillLevel = s.SkillLevel;
		SkillDamage = s.SkillDamage.Clone ();
	}

	public override Skill Clone(){
		return new PlayerUltiSkill (this);	
	}

	public void TakeEffect(BattleEngine battle_engine){
		if (this.Type.Equals (SkillType.ULTI_SKILL_ICE_SHOCK)) {
            List<Monster> attacked_monsters = battle_engine.current_monsters;
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_STUN));
			}
            
            battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
        }

		else if (this.Type.Equals(SkillType.ULTI_SKILL_ICE_BLIZZARD)){
			List<Monster> attacked_monsters = battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_SLOW,this.Arg2));
			}
		}

		else if (this.Type.Equals(SkillType.ULTI_SKILL_POISON_STRIKE)){
            List<Monster> attacked_monsters = battle_engine.current_monsters;
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_POISON,this.Arg2));
			}
            battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
        }

		else if (this.Type.Equals(SkillType.ULTI_SKILL_WEAK_STRIKE)){
			List<Monster> attacked_monsters = battle_engine.current_monsters;
            for (int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_WEAK,this.Arg2));
			}
            battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
        }

		else if (this.Type.Equals(SkillType.ULTI_SKILL_FIRE_HELL)){
			List<Monster> attacked_monsters = battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_FIRE));
			}
		}

		else if (this.Type.Equals(SkillType.ULTI_SKILL_FIRE_SHOCK)){
			List<Monster> attacked_monsters = battle_engine.current_monsters;
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				attacked_monsters[i].AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_BURNT,this.Arg2));
			}
            battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
        }
        
        else if (this.Type.Equals(SkillType.ULTI_SKILL_ARMOR_BREAK))
        {
            List<Monster> attacked_monsters = battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
            for (int i = 0; i < attacked_monsters.Count; i++)
            {
                attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_ARMOR_BREAK));
            }
        }

        else if (this.Type.Equals(SkillType.ULTI_SKILL_WIND_BURY)){
			List<Monster> attacked_monsters = battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
			for(int i = 0 ; i < attacked_monsters.Count ; i ++){
				if(attacked_monsters[i].CurrentHealth <= attacked_monsters[i].HealthUpperLimit * Arg1){
						//directly kill the monster
					attacked_monsters[i].CurrentHealth = 0;
					attacked_monsters[i].isAlive = false;
				}
			}
		}
        else if (this.Type.Equals(SkillType.ULTI_SKILL_HOLY_SPIRIT))
        {
            BattleState state = new BattleState(this.Arg1, StateType.BATTLE_STATE_IMMORTAL);
            Game.Current.Hero.AddBattleState(state);
        }

        else if (this.Type.Equals(SkillType.ULTI_SKILL_FIRE_STUN))
        {
            if(Game.Current.Hero.Gears.EquippedWeapon.Element == ElementType.Fire)
            {
                List<Monster> attacked_monsters = battle_engine.current_monsters;

                for (int i = 0; i < attacked_monsters.Count; i++)
                {
                    attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STUN));
                }

                battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
            }
         
        }

        else if (this.Type.Equals(SkillType.ULTI_SKILL_ICE_BODY))
        {
            if (Game.Current.Hero.Gears.EquippedWeapon.Element == ElementType.Ice)
            {

                BattleState state = new BattleState(this.Arg1, StateType.BATTLE_STATE_STRONG, this.Arg2);
                Game.Current.Hero.AddBattleState(state);
            }
            
        }

        else if (this.Type.Equals(SkillType.ULTI_SKILL_WIND_FAST))
        {
            if (Game.Current.Hero.Gears.EquippedWeapon.Element == ElementType.Wind)
            {

                BattleState state = new BattleState(this.Arg1, StateType.BATTLE_STATE_FAST, this.Arg2);
                Game.Current.Hero.AddBattleState(state);
            }

        }
        else {
			battle_engine.AttackMonsters(SkillDamage.CloneWithSkillMultiplier());
		}

	}

	//upgrade the current skill
	public bool Upgrade(){
		MainCharacter hero = Game.Current.Hero;
		float probability;
		if (SkillLevel == 0) {
			probability = 1;
		} else {
			probability = 1/Mathf.Pow(1.2f,(float)SkillLevel);
		}

		//only cost souls when upgrading elements
		Resource soul_required = ItemFactory.BuildResource (ItemType.SOUL, CalculateSoulRequire());
		if (isFullLevel()) {
			Game.Current.AddToast (Lang.Current ["skill_full"]);
			return false;
		} else {
			if (hero.has (soul_required)) {
				hero.loses (soul_required);
				if (Random.value <= probability) {
					//upgrade success
					SkillLevel += 1;
                    switch (this.Type)
                    {
                        case SkillType.ULTI_SKILL_SWIPE:
                            SkillDamage.DamageAmount += 25f;
                            break;

                        case SkillType.ULTI_SKILL_HARD_STRIKE:
                            SkillDamage.DamageAmount += 20f;
                            break;
                        case SkillType.ULTI_SKILL_FIRE_PULSE:
                            ChargeAmount += 0.015f;
                            SkillDamage.DamageAmount += 35f;
                            break;

                        case SkillType.ULTI_SKILL_FIRE_WAVE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 20f;
                            break;

                        case SkillType.ULTI_SKILL_FIRE_SHOCK:
                            Arg1 += 1f;
                            Arg2 += 0.07f;
                            break;

                        case SkillType.ULTI_SKILL_FIRE_HELL:
                            SkillDamage.DamageAmount += 35f;
                            ChargeAmount += 0.01f;
                            break;

                        case SkillType.ULTI_SKILL_ICE_PULSE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 35f;
                            break;

                        case SkillType.ULTI_SKILL_ICE_WAVE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 20f;
                            break;

                        case SkillType.ULTI_SKILL_ICE_BLIZZARD:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 50f;
                            break;

                        case SkillType.ULTI_SKILL_WIND_PULSE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 35f;
                            break;

                        case SkillType.ULTI_SKILL_WIND_WAVE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 20f;
                            break;

                        case SkillType.ULTI_SKILL_WIND_BLADE:
                            ChargeAmount += 0.02f;
                            SkillDamage.DamageAmount += 50f;
                            break;

                        case SkillType.ULTI_SKILL_ICE_SHOCK:
                            SkillDamage.DamageAmount += 55f;
                            Arg1 += 0.34f;
                            break;

                        case SkillType.ULTI_SKILL_POISON_STRIKE:
                            ChargeAmount += 0.01f;
                            Arg2 += 0.002f;
                            break;

                        case SkillType.ULTI_SKILL_WEAK_STRIKE:
                            ChargeAmount += 0.02f;
                            Arg2 -= 0.05f;
                            SkillDamage.DamageAmount += 25f;
                            break;

                        case SkillType.ULTI_SKILL_HOLY_SPIRIT:
                            ChargeAmount += 0.005f;
                            Arg1 += 1;
                            break;

                        case SkillType.ULTI_SKILL_ARMOR_BREAK:
                            ChargeAmount += 0.01f;
                            Arg1 += 3;
                            SkillDamage.DamageAmount += 50f;
                            break;

                        case SkillType.ULTI_SKILL_FIRE_STUN:
                            ChargeAmount += 0.01f;
                            Arg1 += 0.25f;
                            break;

                        case SkillType.ULTI_SKILL_ICE_BODY:
                            Arg1 += 1;
                            Arg2 += 0.1f;
                            break;

                        case SkillType.ULTI_SKILL_WIND_FAST:
                            ChargeAmount += 0.01f;
                            Arg2 -= 0.05f;
                            break;

                        case SkillType.ULTI_SKILL_WIND_BURY:
                            ChargeAmount += 0.02f;
                            Arg1 += 0.02f;
                            SkillDamage.DamageAmount += 40f;
                            break;

                    }

					Game.Current.AddToast (Lang.Current ["upgrade_success"]);
					return true;
				} else {
					//upgrade fail
					Game.Current.AddToast (Lang.Current ["upgrade_fail"]);
					return false;
				}
			} else {
				Game.Current.AddToast (Lang.Current ["not_enough_soul"]);
				return false;
			}
		}
	
	}


	public int CalculateSoulRequire(){

        switch (this.Type)
        {
            case SkillType.ULTI_SKILL_SWIPE:
                return (SkillLevel * 2 + 1);
            case SkillType.ULTI_SKILL_HARD_STRIKE:
                return (SkillLevel * 2 + 1);

            case SkillType.ULTI_SKILL_FIRE_PULSE:
                return (SkillLevel * 3 + 2);

            case SkillType.ULTI_SKILL_FIRE_WAVE:
                return (SkillLevel * 3 + 2);

            case SkillType.ULTI_SKILL_FIRE_SHOCK:
                return (SkillLevel * 4 + 6);

            case SkillType.ULTI_SKILL_FIRE_HELL:
                return (SkillLevel * 4 + 10);

            case SkillType.ULTI_SKILL_ICE_PULSE:
                return (SkillLevel * 3 + 2);


            case SkillType.ULTI_SKILL_ICE_WAVE:
                return (SkillLevel * 3 + 2);

            case SkillType.ULTI_SKILL_ICE_SHOCK:
                return (SkillLevel * 4 + 6);

            case SkillType.ULTI_SKILL_ICE_BLIZZARD:
                return (SkillLevel * 4 + 10);


            case SkillType.ULTI_SKILL_WIND_PULSE:
                return (SkillLevel * 3 + 2);

            case SkillType.ULTI_SKILL_WIND_WAVE:
                return (SkillLevel * 3 + 2);

            case SkillType.ULTI_SKILL_WIND_BLADE:
                return (SkillLevel * 4 + 6);

            case SkillType.ULTI_SKILL_WIND_BURY:
                return (SkillLevel * 4 + 10);

            case SkillType.ULTI_SKILL_HOLY_SPIRIT:
                return (SkillLevel * 6 + 5);
 
            case SkillType.ULTI_SKILL_ARMOR_BREAK:
                return (SkillLevel * 8 + 4);

            default:
                return (SkillLevel * 4 + 5);
        }

	}

	//if the current skill is full 
	public bool isFullLevel(){
        switch (this.Type)
        {
            case SkillType.ULTI_SKILL_SWIPE:
                if (SkillLevel == 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case SkillType.ULTI_SKILL_HARD_STRIKE:
                if (SkillLevel == 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case SkillType.ULTI_SKILL_FIRE_PULSE:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_FIRE_WAVE:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_FIRE_SHOCK:
                if (SkillLevel == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_FIRE_HELL:
                if (SkillLevel == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            case SkillType.ULTI_SKILL_ICE_PULSE:
                if (SkillLevel == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_ICE_WAVE:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            case SkillType.ULTI_SKILL_ICE_BLIZZARD:
                if (SkillLevel == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            case SkillType.ULTI_SKILL_WIND_PULSE:
                if (SkillLevel == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_WIND_WAVE:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }

         

            case SkillType.ULTI_SKILL_ICE_SHOCK:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_POISON_STRIKE:
                if (SkillLevel == 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_WEAK_STRIKE:
                if (SkillLevel == 6)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_HOLY_SPIRIT:
                if (SkillLevel == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case SkillType.ULTI_SKILL_ARMOR_BREAK:
                if (SkillLevel == 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            default:
                if (SkillLevel == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }

        }
	}

}
