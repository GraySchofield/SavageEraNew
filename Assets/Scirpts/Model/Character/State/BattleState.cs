using UnityEngine;
using System.Collections;
[System.Serializable]
public class BattleState: TimedState{

	public BattleState (float duration, string state_type):base(duration, state_type)
	{

	}	

	public BattleState (float duration, string state_type, float arg1):base(duration, state_type)
	{
		this.Arg1 = arg1;
	}	


	//generic arguments
	public float Arg1 {
		get;
		set;
	}


	public override void UpdateState(float period){
		this.currentTime += period;
		if (this.currentTime >= this.Duration) {
			//time over
			//deactivate the state, remove it from hero
			Game.Current.Hero.RemoveBattleState (this.Type);
		} else {
			TakeContinousEffect();
		}
	}

	public  void UpdateMonsterState(float period , Monster monster, BattleEngine battle_engine, 
	                                int idx){
		this.currentTime += period;
		if (this.currentTime >= this.Duration) {
			//time over
			//deactivate the state, remove it from game once at end
			monster.RemoveBattleState (this.Type);
		} else {
			//continous effect
			TakeContinousEffect(monster, battle_engine, idx);
		}
	}

	//hero
	public void Activate(){
        switch (this.Type)
        {
            case StateType.BATTLE_STATE_WEAK:
                Game.Current.Hero.Attack *= Arg1;
                break;

            case StateType.BATTLE_STATE_STRONG:
                Game.Current.Hero.Attack *= Arg1;
                break;

            case StateType.BATTLE_STATE_TOUGH:
                Game.Current.Hero.Defense *= Arg1;
                break;

            case StateType.BATTLE_STATE_BLOOD_RAGE:
                Game.Current.Hero.Attack *= Arg1;
                Game.Current.Hero.Defense /= Arg1;
                break;

            case StateType.BATTLE_STATE_STRONG_SKILL:
                Game.Current.Hero.SkillMultiplier *= Arg1;
                break;

            case StateType.BATTLE_STATE_ARMOR_BREAK:
                Game.Current.Hero.Defense *= Config.armor_break_multiplier;
                break;

            case StateType.BATTLE_STATE_FAST:
                Game.Current.Hero.CoolDownMultiplier *= Arg1;
                break;

            case StateType.BATTLE_STATE_WEAK_SKILL:
                Game.Current.Hero.SkillMultiplier *= Arg1;
                break;
            
        }
	}

	//hero
	public void Deactivate(){
        switch (this.Type)
        {
            case StateType.BATTLE_STATE_WEAK:
                Game.Current.Hero.Attack /= Arg1;
                break;

            case StateType.BATTLE_STATE_STRONG:
                Game.Current.Hero.Attack /= Arg1;
                break;

            case StateType.BATTLE_STATE_TOUGH:
                Game.Current.Hero.Defense /= Arg1;
                break;

            case StateType.BATTLE_STATE_BLOOD_RAGE:
                Game.Current.Hero.Attack /= Arg1;
                Game.Current.Hero.Defense *= Arg1;
                break;

            case StateType.BATTLE_STATE_STRONG_SKILL:
                Game.Current.Hero.SkillMultiplier /= Arg1;
                break;

            case StateType.BATTLE_STATE_ARMOR_BREAK:
                Game.Current.Hero.Defense /= Config.armor_break_multiplier;
                break;

            case StateType.BATTLE_STATE_FAST:
                Game.Current.Hero.CoolDownMultiplier /= Arg1;
                break;

            case StateType.BATTLE_STATE_WEAK_SKILL:
                Game.Current.Hero.SkillMultiplier /= Arg1;
                break;
        }
	}

	//monster
	public void Activate(Monster monster){
        switch (this.Type)
        {
            case StateType.BATTLE_STATE_WEAK:
                monster.Attack *= Arg1; //weaken the monster
                break;
            case StateType.BATTLE_STATE_FAST:
                monster.AttackCoolDownLimit *= Arg1; //arg1 should be < 1
                break;

            case StateType.BATTLE_STATE_SLOW:
                monster.AttackCoolDownLimit *= Arg1; //arg1 should be > 1
                break;

            case StateType.BATTLE_STATE_FIRE:
                monster.Element = ElementType.Fire;
                break;

            case StateType.BATTLE_STATE_ICE:
                monster.Element = ElementType.Ice;
                break;

            case StateType.BATTLE_STATE_WIND:
                monster.Element = ElementType.Wind;
                break;

            case StateType.BATTLE_STATE_DARK:
                monster.Element = ElementType.Dark;
                break;

            case StateType.BATTLE_STATE_HOLY:
                monster.Element = ElementType.Holy;
                break;

            case StateType.BATTLE_STATE_ARMOR_BREAK:
                monster.Defense *= Config.armor_break_multiplier;
                break;

            case StateType.BATTLE_STATE_TOUGH:
                monster.Defense *= 1000;
                break;


            case StateType.BATTLE_STATE_STRONG:
                monster.Attack *= Arg1;
                break;               
           
        }
	}

	//monster
	public void Deactivate(Monster monster){
        switch (this.Type)
        {
            case StateType.BATTLE_STATE_WEAK:
                monster.Attack /= Arg1; //restore attack
                break;

            case StateType.BATTLE_STATE_FAST:
                monster.AttackCoolDownLimit /= Arg1; //arg1 should be < 1
                break;

            case StateType.BATTLE_STATE_SLOW:
                monster.AttackCoolDownLimit /= Arg1; //arg1 should be > 1
                break;

            case StateType.BATTLE_STATE_FIRE:
                monster.Element = monster.OriginalElement;
                break;

            case StateType.BATTLE_STATE_ICE:
                monster.Element = monster.OriginalElement;
                break;

            case StateType.BATTLE_STATE_WIND:
                monster.Element = monster.OriginalElement;
                break;

            case StateType.BATTLE_STATE_DARK:
                monster.Element = monster.OriginalElement;
                break;

            case StateType.BATTLE_STATE_HOLY:
                monster.Element = monster.OriginalElement;
                break;

            case StateType.BATTLE_STATE_ARMOR_BREAK:
                monster.Defense /= Config.armor_break_multiplier;
                break;

            case StateType.BATTLE_STATE_TOUGH:
                monster.Defense /= 1000;
                break;

            case StateType.BATTLE_STATE_STRONG:
                monster.Attack /= Arg1;
                break;
        }
	}


	//monster
	private void TakeContinousEffect(Monster monster, BattleEngine battle_engine, int idx){
		switch(this.Type){
		case StateType.BATTLE_STATE_POISON:
			//continuos percentage drop
		{	
			monster.CurrentHealth -= monster.HealthUpperLimit * Arg1;
			if(monster.CurrentHealth <= 0){
				monster.CurrentHealth = 0;
				monster.isAlive = false;
			}
		}
			break;

		case StateType.BATTLE_STATE_HEAL:
			//continuos percentage heal
			monster.CurrentHealth += monster.HealthUpperLimit * Arg1;
			if(monster.CurrentHealth >= monster.HealthUpperLimit){
				monster.CurrentHealth = monster.HealthUpperLimit;
			}
			break;


		case StateType.BATTLE_STATE_BURNT:
		{	
			Damage damage = new Damage(ElementType.Fire, Game.Current.Hero.Attack * Arg1);
			monster.SufferDamage(damage,idx,battle_engine);
		}
			break;

		}
	}


	//player
	private void TakeContinousEffect(){
		switch(this.Type){
		case StateType.BATTLE_STATE_POISON:
		{
			Game.Current.Hero.CurrentHealth -= Game.Current.Hero.HealthUpperLimit * Arg1; //direct damage, no reduction
			if(Game.Current.Hero.CurrentHealth <= 0){
				Game.Current.Hero.CurrentHealth = 0;
			}
		}
			break;
		case StateType.BATTLE_STATE_HEAL:
		{
			Game.Current.Hero.CurrentHealth += Game.Current.Hero.HealthUpperLimit * Arg1; //direct damage, no reduction
			if(Game.Current.Hero.CurrentHealth >= Game.Current.Hero.HealthUpperLimit){
				Game.Current.Hero.CurrentHealth = Game.Current.Hero.HealthUpperLimit;
			}
		}
			break;

		}
	}
	





}
