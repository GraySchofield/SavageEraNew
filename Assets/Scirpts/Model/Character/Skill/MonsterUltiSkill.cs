using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterUltiSkill:Skill{
	public float CoolDownLimit {
		get;
		set;
	}

	public float CurrentCoolDown {
		get;
		set;
	}

	public MonsterUltiSkill(string type, float cool_down_limit):base(type){
		this.CoolDownLimit = cool_down_limit;
		this.CurrentCoolDown = Random.Range(cool_down_limit /3, cool_down_limit);
	}

	public MonsterUltiSkill(string type, float cool_down_limit, float arg1):base(type, arg1){
		this.CoolDownLimit = cool_down_limit;
		this.CurrentCoolDown = Random.Range(cool_down_limit /3, cool_down_limit);
    }

	public MonsterUltiSkill(string type, float cool_down_limit, float arg1, float arg2):base(type, arg1, arg2){
		this.CoolDownLimit = cool_down_limit;
		this.CurrentCoolDown = Random.Range(cool_down_limit /3, cool_down_limit);
    }

	public MonsterUltiSkill(string type): base(type){
		this.CoolDownLimit = 0;
		this.CurrentCoolDown = 0;	
	}
	
	public MonsterUltiSkill(MonsterUltiSkill s):base(s){
		this.CoolDownLimit = s.CoolDownLimit;
		this.CurrentCoolDown = s.CurrentCoolDown;	
	}

	public override Skill Clone(){
		return new MonsterUltiSkill (this);	
	}

	// this function may need to be revised, wether the damage should be re newed here
	public void TakeEffect(float period, Monster monster, BattleEngine battle_engine, int idx){
		if (CurrentCoolDown <= period){
			//time to cast ulti
			Damage damage;
            switch (this.Type)
            {
                case SkillType.MONSTER_ULTI_DEATH_BITE: //5 time critical hit
                                                        //5 here can be changed to arg1, but hardcode is also fine
                    Damage d = new Damage(monster.Element, monster.Attack * 5);
                    Game.Current.Hero.SufferDamage(d, battle_engine, monster);
                    break;

                case SkillType.MONSTER_ULTI_ENERGY_PULSE:  //fixed 50 damage
                    damage = new Damage(monster.Element, 50f);
                    Game.Current.Hero.SufferDamage(damage, battle_engine, monster);
                    break;

                case SkillType.MONSTER_ULTI_HEAL: //heal
                    monster.CurrentHealth = monster.HealthUpperLimit;
                    battle_engine.UpdateMonstersUI(idx);
                    break;

                case SkillType.MONSTER_ULTI_RAGE: //Increase attack
                    monster.Attack *= this.Arg1;
                    break;

                case SkillType.MONSTER_ULTI_DOOM:
                    Game.Current.Hero.CurrentHealth = Game.Current.Hero.HealthUpperLimit * 0.1f;
                    break;

                case SkillType.MONSTER_ULTI_DISPERSE: //reset player ulti charge
                    battle_engine.ResetUltiCharge();
                    break;

                case SkillType.MONSTER_ULTI_FAST:
                    //Arg1 is duration, Arg2 < 1, Cd reduction
                    monster.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_FAST, Arg2));
                    break;

                case SkillType.MONSTER_ULTI_POISON_SMOKE:
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_POISON, Arg2));
                    break;

                case SkillType.MONSTER_ULTI_VOID_BREATH:
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_WEAK, Arg2));
                    break;

                case SkillType.MONSTER_ULTI_PAIN:
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_PAIN));
                    break;

                case SkillType.MONSTER_ULTI_CHAOS:
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_CHAOS));
                    break;

                case SkillType.MONSTER_ULTI_ICE_SHIELD:
                    monster.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_IMMORTAL));
                    break;

                case SkillType.MONSTER_ULTI_SUPER_DEFENSE:
                    monster.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_TOUGH));
                    break;

                case SkillType.MONSTER_ULTI_ELEMENT_SHIELD:
                    monster.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_MAGIC_IMMUNE));
                    break;

                case SkillType.MONSTER_ULTI_STRONG_ATTACK:
                    monster.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_STRONG, Arg2));
                    break;

                case SkillType.MONSTER_ULTI_FEAR_ROAR:
                    damage = new Damage(monster.Element, monster.Attack * 2);
                    Game.Current.Hero.SufferDamage(damage, battle_engine, monster);
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_FEAR));
                    break;

                case SkillType.MONSTER_ULTI_WEAK_SKILL:
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_WEAK_SKILL, Arg2));
                    break;


                case SkillType.MONSTER_ULTI_STUN_STRIKE:
                    damage = new Damage(monster.Element, monster.Attack * 2);
                    Game.Current.Hero.SufferDamage(damage, battle_engine, monster);
                    Game.Current.Hero.AddBattleState(new BattleState(Arg1, StateType.BATTLE_STATE_STUN));
                    break;

                case SkillType.MONSTER_ULTI_PURIFY:
                    monster.RemoveAllDebuffStates();
                    break;                                             

            }
			battle_engine.ShowMonsterSkillTitle(this.Name,idx,"UltiSkill");
			CurrentCoolDown = CoolDownLimit; //reset the cool down
		} else {
			CurrentCoolDown -= period;
		}
	}
}
