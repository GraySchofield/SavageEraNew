using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterDefenseSkill  : Skill {
	//defense skill takes effect when monsters are attacked
	public float TriggerProbability {
		get;
		private set;
	}

	public MonsterDefenseSkill(string type, float probability):base(type){
		this.TriggerProbability = probability;
	}
	
	public MonsterDefenseSkill(string type, float probability, float arg1):base(type, arg1){
		this.TriggerProbability = probability;
	}
	
	public MonsterDefenseSkill(string type, float probability, float arg1, float arg2):base(type, arg1, arg2){
		this.TriggerProbability = probability;
	}
	
	public MonsterDefenseSkill(string type): base(type){
		this.TriggerProbability = 0;
	}
	
	public MonsterDefenseSkill(MonsterDefenseSkill s):base(s){
		TriggerProbability = s.TriggerProbability;
	}
	
	public override Skill Clone(){
		return new MonsterDefenseSkill (this);	
	}

	public  void TakeEffect(Monster monster, Damage damage, BattleEngine b_e, int idx){
		if (Random.value <= TriggerProbability) {
            //trigger the skills
            switch (this.Type)
            {
                case SkillType.MONSTER_MAGIC_IMMUNE:
  
                    if (damage.EType != ElementType.None)
                    {
                        damage.DamageAmount = 0; //the skill is immune to all element damage
                        b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");

                    }
                    break;

                case SkillType.MONSTER_REFLECT:
                    Damage clone = new Damage(monster.Element, damage.DamageAmount * Arg1);
                    Game.Current.Hero.SufferDamage(clone, b_e, monster, true);
                    b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    break;

                case SkillType.MONSTER_MISS:
                    damage.DamageAmount = 0f;
                    b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    break;

                case SkillType.MONSTER_ABSORB:
                    monster.CurrentHealth += damage.DamageAmount * Arg1;
                    damage.DamageAmount = 0f;
                    b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    break;

                case SkillType.MONSTER_SHIELD:
                    damage.DamageAmount = damage.DamageAmount * Arg1;
                    b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");                
                    break;

                case SkillType.MONSTER_STUN_RESIS:
                    if (monster.isInBattleState(StateType.BATTLE_STATE_STUN))
                    {
                        monster.RemoveBattleState(StateType.BATTLE_STATE_STUN); //we can change 
                        //it here to remove all monsters stun state, this depends
                        b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    }
                    break;

                case SkillType.MONSTER_WEAK_RESIS:
                    if (monster.isInBattleState(StateType.BATTLE_STATE_WEAK))
                    {
                        monster.RemoveBattleState(StateType.BATTLE_STATE_WEAK); 
                        b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    }
                    break;

                case SkillType.MONSTER_POISON_RESIS:
                    if (monster.isInBattleState(StateType.BATTLE_STATE_POISON))
                    {
                        monster.RemoveBattleState(StateType.BATTLE_STATE_POISON);
                        b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    }
                    break;

                case SkillType.MONSTER_BURNT_RESIS:
                    if (monster.isInBattleState(StateType.BATTLE_STATE_BURNT))
                    {
                        monster.RemoveBattleState(StateType.BATTLE_STATE_BURNT);
                        b_e.ShowMonsterSkillTitle(this.Name, idx, "DefenseSkill");
                    }
                    break;
            }
		} 
	}
}
