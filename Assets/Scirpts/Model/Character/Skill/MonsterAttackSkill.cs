using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterAttackSkill : Skill {
	// Attack skill Takes effect when monster attacks, with certain probability
	public float TriggerProbability {
		get;
		private set;
	}


	public MonsterAttackSkill(string type, float probability):base(type){
		this.TriggerProbability = probability;
	}

	public MonsterAttackSkill(string type, float probability, float arg1):base(type,arg1){
		this.TriggerProbability = probability;
	}

	public MonsterAttackSkill(string type, float probability, float arg1, float arg2):base(type,arg1,arg2){
		this.TriggerProbability = probability;
	}

	public MonsterAttackSkill(string type):	base(type){
		this.TriggerProbability = 0;
	}

	public MonsterAttackSkill(MonsterAttackSkill s):base(s){
		TriggerProbability = s.TriggerProbability;
	}

	public override Skill Clone(){
		return new MonsterAttackSkill (this);
	}

	public  void TakeEffect(Monster monster, Damage damage , BattleEngine b_e, int idx){
		if (Random.value <= TriggerProbability) {
            //trigger the skills
            switch (this.Type)
            {
                case SkillType.MONSTER_CRITICAL:
                    damage.DamageAmount *= this.Arg1; //arg1 here is critcal ratio
                    break;

                case SkillType.MONSTER_BLOOD_SUCK:
                    monster.CurrentHealth += damage.DamageAmount * this.Arg1; //arg1 blood suck ratio
                    b_e.UpdateMonstersUI(idx);
                    break;

                case SkillType.MONSTER_ELEMENT_CHANGE:
                    //switch random attack element type
                    ElementType[] eles = { ElementType.Dark, ElementType.Fire, ElementType.Holy, ElementType.Ice, ElementType.Wind };
                    int r = (new System.Random()).Next(0, eles.Length);
                    damage.EType = eles[r];
                    break;
                case SkillType.MONSTER_MULTI_ATTACK:
                    //deal additional attacks
                    monster.AttackCoolDown = 0.5f;
                    break;
                case SkillType.MONSTER_STRONGER:
                    monster.Attack = monster.Attack * Arg1; //the longer the battle, the stronger the monster is
                    break;

                case SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK:
                    Game.Current.Hero.AddBattleState(new BattleState(this.Arg1,StateType.BATTLE_STATE_ARMOR_BREAK));
                    break;

                case SkillType.MONSTER_ATTACK_STUN:
                    if(!Game.Current.Hero.IsInBattleState(StateType.BATTLE_STATE_FOCUS))
                        Game.Current.Hero.AddBattleState(new BattleState(this.Arg1, StateType.BATTLE_STATE_STUN));
                    break;
            }

			b_e.ShowMonsterSkillTitle(this.Name,idx,"AttackSkill");

		}
	}

}
