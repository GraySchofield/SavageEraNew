using UnityEngine;
using System.Collections;

[System.Serializable]
public class ArmorSkill : Skill {
	//armor skill is triggerred with certain probability when player is attacked
	public float TriggerProbability {
		get;
		set;
	}

	public ArmorSkill(string type, float probability): base(type){
		this.TriggerProbability = probability;
	}
	
	public ArmorSkill(string type, float probability, float arg1): base(type, arg1){
		this.TriggerProbability = probability;
	}
	
	public ArmorSkill(string type, float probability, float arg1, float arg2): base(type, arg1, arg2){
		this.TriggerProbability = probability;
	}

	public ArmorSkill(string type): base(type){
		this.TriggerProbability = 0;
	}

	public ArmorSkill(ArmorSkill s): base(s){
		TriggerProbability = s.TriggerProbability;
	}
	
	public void TakeEffect(Damage damage, BattleEngine battle_engine, Monster monster){
		MainCharacter hero = Game.Current.Hero;
		if (Random.value <= TriggerProbability
            && !hero.IsInBattleState(StateType.BATTLE_STATE_FEAR)) {
            //trigger the armor skill, when chance is right
            //and not in fear
            switch (this.Type)
            {
                case SkillType.ARMOR_SKILL_MISS:
                    damage.DamageAmount = 0f;
                    battle_engine.showArmorSkillText(this.Name);
                    break;

                case SkillType.ARMOR_SKILL_ABSORB:
                    hero.CurrentHealth += damage.DamageAmount * Arg1;
                    damage.DamageAmount = 0f;
                    battle_engine.showArmorSkillText(this.Name);
                    break;

                case SkillType.ARMOR_SKILL_ICE_SHIELD:
                    if(damage.EType == ElementType.Ice)
                    {
                        hero.CurrentHealth += damage.DamageAmount * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_FIRE_SHIELD:
                    if (damage.EType == ElementType.Fire)
                    {
                        hero.CurrentHealth += damage.DamageAmount * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);

                    }
                    break;

                case SkillType.ARMOR_SKILL_WIND_SHIELD:
                    if (damage.EType == ElementType.Wind)
                    {
                        hero.CurrentHealth += damage.DamageAmount * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_DARK_SHIELD:
                    if (damage.EType == ElementType.Dark)
                    {
                        hero.CurrentHealth += damage.DamageAmount * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_HOLY_SHIELD:
                    if (damage.EType == ElementType.Holy)
                    {
                        hero.CurrentHealth += damage.DamageAmount * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_REVIVE:
                    if(damage.DamageAmount >= hero.CurrentHealth)
                    {
                        //lethal attack
                        hero.CurrentHealth = hero.HealthUpperLimit * Arg1;
                        damage.DamageAmount = 0f;
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_DARK_REFLECT:
                    if (hero.Gears.EquippedWeapon != null)
                    {
                       if(hero.Gears.EquippedWeapon.Element == ElementType.Dark)
                        {
                            Damage refect = damage.Clone();
                            refect.DamageAmount *= Arg1;
                            refect.EType = ElementType.Dark;
                            battle_engine.AttackMonsters(refect);
                            battle_engine.showArmorSkillText(this.Name);
                        }
                    }
                    break;

                case SkillType.ARMOR_SKILL_HOLY_REFLECT:
                    if (hero.Gears.EquippedWeapon != null)
                    {
                        if (hero.Gears.EquippedWeapon.Element == ElementType.Holy)
                        {
                            Damage refect = damage.Clone();
                            refect.DamageAmount *= Arg1;
                            refect.EType = ElementType.Holy;
                            battle_engine.AttackMonsters(refect);
                            battle_engine.showArmorSkillText(this.Name);
                        }
                    }
                    break;

                case SkillType.ARMOR_SKILL_ICE_REFLECT:
                    if (hero.Gears.EquippedWeapon != null)
                    {
                        if (hero.Gears.EquippedWeapon.Element == ElementType.Ice)
                        {
                            Damage refect = damage.Clone();
                            refect.DamageAmount *= Arg1;
                            refect.EType = ElementType.Ice;
                            battle_engine.AttackMonsters(refect);
                            battle_engine.showArmorSkillText(this.Name);
                        }
                    }
                    break;

                case SkillType.ARMOR_SKILL_FIRE_REFLECT:
                    if (hero.Gears.EquippedWeapon != null)
                    {
                        if (hero.Gears.EquippedWeapon.Element == ElementType.Fire)
                        {
                            Damage refect = damage.Clone();
                            refect.DamageAmount *= Arg1;
                            refect.EType = ElementType.Fire;
                            battle_engine.AttackMonsters(refect);
                            battle_engine.showArmorSkillText(this.Name);
                        }
                    }
                    break;

                case SkillType.ARMOR_SKILL_WIND_REFLECT:
                    if (hero.Gears.EquippedWeapon != null)
                    {
                        if (hero.Gears.EquippedWeapon.Element == ElementType.Wind)
                        {
                            Damage refect = damage.Clone();
                            refect.DamageAmount *= Arg1;
                            refect.EType = ElementType.Wind;
                            battle_engine.AttackMonsters(refect);
                            battle_engine.showArmorSkillText(this.Name);
                        }
                    }
                    break;


                case SkillType.ARMOR_SKILL_IRON_WALL:
                    monster.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STUN));
                    battle_engine.showArmorSkillText(this.Name);
                    break;

                case SkillType.ARMOR_SKILL_INSTANT_HEAL:
                    battle_engine.current_food_cool_down = 0;
                    battle_engine.showArmorSkillText(this.Name);
                    break;

                case SkillType.ARMOR_SKILL_INSTANT_ULTI:
                    battle_engine.CurrentUltiCharge = 1;
                    battle_engine.showArmorSkillText(this.Name);
                    break;

                case SkillType.ARMOR_SKILL_DIVINE_SHIELD:
                    {
                        BattleState state = new BattleState(this.Arg1, StateType.BATTLE_STATE_IMMORTAL);
                        Game.Current.Hero.AddBattleState(state);
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;

                case SkillType.ARMOR_SKILL_FOCUS:
                    {
                        BattleState state = new BattleState(this.Arg1, StateType.BATTLE_STATE_FOCUS);
                        Game.Current.Hero.AddBattleState(state);
                        battle_engine.showArmorSkillText(this.Name);
                    }
                    break;
                

            }
		}
	}
	
	public override Skill Clone(){
		return new ArmorSkill (this);
	}

}
