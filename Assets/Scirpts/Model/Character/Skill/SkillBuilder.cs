using UnityEngine;
using System.Collections;

//the skill builder only build player's ulti skill
//monster skills are created directly in MonsterBuilder
public class SkillBuilder  {
	public static Skill BuildSkill(string type){
		Skill skill = null;
        switch (type)
        {
            case SkillType.ULTI_SKILL_HARD_STRIKE:
                skill = new PlayerUltiSkill(type, 0.15f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.None, 30, false);
                break;

            case SkillType.ULTI_SKILL_SWIPE:
                skill = new PlayerUltiSkill(type, 0.2f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.None, 25, true);
                break;

            case SkillType.ULTI_SKILL_FIRE_PULSE:
                skill = new PlayerUltiSkill(type, 0.1f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Fire, 40, false);
                break;

            case SkillType.ULTI_SKILL_FIRE_WAVE:
                skill = new PlayerUltiSkill(type, 0.15f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Fire, 30, false);
                break;

            case SkillType.ULTI_SKILL_FIRE_SHOCK:
                skill = new PlayerUltiSkill(type, 0.18f, 10, 0.1f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Fire, 1f, true);
                break;

            case SkillType.ULTI_SKILL_FIRE_HELL:
                skill = new PlayerUltiSkill(type, 0.05f, 100);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Fire, 180f, false);
                break;

            case SkillType.ULTI_SKILL_ICE_PULSE:
                skill = new PlayerUltiSkill(type, 0.1f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Ice, 40, false);
                break;

            case SkillType.ULTI_SKILL_ICE_WAVE:
                skill = new PlayerUltiSkill(type, 0.15f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Ice, 30, false);
                break;

            case SkillType.ULTI_SKILL_ICE_BLIZZARD:
                skill = new PlayerUltiSkill(type, 0.1f, 20f, 0.5f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Ice, 100, true);
                break;

            case SkillType.ULTI_SKILL_ICE_SHOCK:
                skill = new PlayerUltiSkill(type, 0.15f, 5f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Ice, 30, true);
                break;

            case SkillType.ULTI_SKILL_WIND_PULSE:
                skill = new PlayerUltiSkill(type, 0.1f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Wind, 40, false);
                break;

            case SkillType.ULTI_SKILL_WIND_WAVE:
                skill = new PlayerUltiSkill(type, 0.15f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Wind, 30, false);
                break;

            case SkillType.ULTI_SKILL_WIND_BLADE:
                skill = new PlayerUltiSkill(type, 0.05f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Wind, 180f, false);
                break;

            case SkillType.ULTI_SKILL_WIND_BURY:
                skill = new PlayerUltiSkill(type, 0.03f, 0.3f);  // 0.03
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Wind, 150f, true);
                break;

            case SkillType.ULTI_SKILL_POISON_STRIKE:
                skill = new PlayerUltiSkill(type, 0.05f, 10f, 0.01f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.None, 1, true);
                break;

            case SkillType.ULTI_SKILL_WEAK_STRIKE:
                skill = new PlayerUltiSkill(type, 0.1f, 20f, 0.7f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.Dark, 10, true);
                break;

            case SkillType.ULTI_SKILL_HOLY_SPIRIT:
                skill = new PlayerUltiSkill(type, 0.06f, 5f);
                break;

            case SkillType.ULTI_SKILL_ARMOR_BREAK:
                skill = new PlayerUltiSkill(type, 0.1f, 5f);
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.None, 100, false);
                break;


            case SkillType.ULTI_SKILL_ICE_BODY:
                skill = new PlayerUltiSkill(type, 0.15f, 10f ,1.5f); //0.15
                break;


            case SkillType.ULTI_SKILL_FIRE_STUN:
                skill = new PlayerUltiSkill(type, 0.15f, 5f); 
                ((PlayerUltiSkill)skill).SkillDamage = new Damage(ElementType.None, 1, true);
                break;


            case SkillType.ULTI_SKILL_WIND_FAST:
                skill = new PlayerUltiSkill(type, 0.1f, 10f, 0.8f); //0.1
                break;


        }
		return skill;
	}

}
