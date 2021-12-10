using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponSkill:Skill{
	//Weapon skills are attached with player's weapons
	public float TriggerProbability {
		get;
		set;
	}

	//the thickness of the attack ring
	public float Range {
		get;
		set;
	}

	public WeaponSkill(string type, float probability, float range):base(type){
		this.TriggerProbability = probability;
		this.Range = range;
	}

	public WeaponSkill(string type, float probability, float range, float arg1):base(type,arg1){
		this.TriggerProbability = probability;
		this.Range = range;
	}

	public WeaponSkill(string type, float probability, float range, float arg1, float arg2):base(type, arg1 , arg2){
		this.TriggerProbability = probability;
		this.Range = range;
	}

	public WeaponSkill(string type): base(type){
		TriggerProbability = 0;
		Range = 0;
	}
	
	public WeaponSkill(WeaponSkill s):base(s){
		TriggerProbability = s.TriggerProbability;
		Range = s.Range;
	}

	public void TakeEffect(BattleEngine battle_engine, Damage damage, List<Monster> attacked_monsters)
    {
		MainCharacter hero = Game.Current.Hero;
        if (Random.value <= TriggerProbability)
        {
            battle_engine.showWeaponSkillText(this.Name);
            switch (this.Type)
            {
                case SkillType.WEAPON_SKILL_CRITICAL:
                    damage.DamageAmount *= Arg1;
                    break;

                case SkillType.WEAPON_SKILL_BLOOD_SUCK:
                    hero.CurrentHealth += damage.DamageAmount * this.Arg1; //arg1 blood suck ratio
                    break;

                case SkillType.WEAPON_SKILL_ELEMENT_CHANGE:
                    ElementType[] eles = { ElementType.Dark, ElementType.Fire, ElementType.Holy, ElementType.Ice, ElementType.Wind, ElementType.None};
                    int r = (new System.Random()).Next(0, eles.Length);
                    damage.EType = eles[r];
                    break;

                case SkillType.WEAPON_SKILL_STUN:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STUN));
                    }
                    break;

                case SkillType.WEAPON_SKILL_SLOW:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_SLOW, this.Arg2));
                    }
                    break;

                case SkillType.WEAPON_SKILL_POISON:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_POISON, this.Arg2));
                    }
                    break;

                case SkillType.WEAPON_SKILL_VOID:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_WEAK, this.Arg2));
                    }
                    break;

                case SkillType.WEAPON_SKILL_BURNT:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_BURNT, this.Arg2));
                    }
                    break;

                case SkillType.WEAPON_SKILL_FIRE:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_FIRE));
                    }
                    break;

                case SkillType.WEAPON_SKILL_WIND:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_WIND));
                    }
                    break;

                case SkillType.WEAPON_SKILL_ICE:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_ICE));
                    }
                    break;

                case SkillType.WEAPON_SKILL_HOLY:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_HOLY));
                    }
                    break;

                case SkillType.WEAPON_SKILL_DARK:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_DARK));
                    }
                    break;

                case SkillType.WEAPON_SKILL_ARMOR_BREAK:
                    damage.DamageAmount = 1f;
                    for (int i = 0; i < attacked_monsters.Count; i++)
                    {
                        attacked_monsters[i].AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_ARMOR_BREAK));
                    }
                    break;


                case SkillType.WEAPON_SKILL_TOUGH:
                    damage.DamageAmount = 1f;
                    Game.Current.Hero.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_TOUGH, this.Arg2));
                    break;

                case SkillType.WEAPON_SKILL_STRONG:
                    damage.DamageAmount = 1f;
                    Game.Current.Hero.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STRONG, this.Arg2));
                    break;

                case SkillType.WEAPON_SKILL_BLOOD_RAGE:
                    damage.DamageAmount = 1f;
                    Game.Current.Hero.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_BLOOD_RAGE, this.Arg2));
                    break;

                case SkillType.WEAPON_SKILL_STRONG_SKILL:
                    damage.DamageAmount = 1f;
                    Game.Current.Hero.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STRONG_SKILL, this.Arg2));
                    break;

               


            }
        }
	}


	public override Skill Clone(){
		return new WeaponSkill (this);	
	}
}
