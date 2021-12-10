using UnityEngine;
using System.Collections;

[System.Serializable]
public class Food : StackableItem {
	public float HealValue {
		get;
		set;
	}
	
	//whether the food can be used to cook other food
	public bool IsIngredient {
		get;
		set;
	}

	public string SubClass {
		get;
		set;
	}

    //cool down of the food in battle;
    public float CoolDown
    {
        get;
        set;
    }


	public Food(string type, int count, float heal_value, bool is_ingredient, string subclass): base(type, count)
	{
		HealValue = heal_value;
		IsIngredient = is_ingredient;
		SubClass = subclass;
        CoolDown = 5f; // default 5 seconds
	}

	public Food(Food f):base(f){
		HealValue = f.HealValue;
		IsIngredient = f.IsIngredient;
		SubClass = f.SubClass;
        CoolDown = f.CoolDown;
	}
	

	public override Item Clone(){
		return new Food (this);
	}

	public void Consume(){
		//eat different food will have very different result
		//for food with special effects, do them one by one
		TimedState state;
        MainCharacter hero = Game.Current.Hero;
        switch (Type)
        {
          
            case ItemType.MEAT_STEW:
                hero.RemoveGlobalState(StateType.STATE_FROZEN);
                break;
            case ItemType.VEGETABLE_CARNIVAL:
                hero.RemoveGlobalState(StateType.STATE_DIZZY);
                hero.RemoveBattleState(StateType.BATTLE_STATE_WEAK);
                break;
            case ItemType.UNICORN_SOUP:
                hero.HealthUpperLimit += 50;
                break;
            case ItemType.FANTASY_CARNIVAL:
                hero.HealthUpperLimit += 100;
                hero.Attack += 15;
                hero.Defense += 15;
                break;

            case ItemType.YETI_HEART:
                hero.Attack += Random.Range(0f, 5f);
                hero.HealthUpperLimit += Random.Range(0f, 10f);
                break;

            case ItemType.DREAM_FRUIT:
                hero.HealthUpperLimit += Random.Range(-10f, 20f);
                hero.Attack += Random.Range(-3f, 5f);
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.TAKE_A_DREAM);
                break;

        
            case ItemType.BEAR_PAW_STEW:
                hero.HealthUpperLimit += Random.Range(10, 20);
                break;


            case ItemType.THREE_XIAN_SOUP:
                hero.HealthUpperLimit += Random.Range(20, 50);
                break;

            case ItemType.BIG_HEAL_PILL:
                hero.HealthUpperLimit += Random.Range(20, 60);
                hero.Attack += Random.Range(2f, 8f);
                hero.Defense += Random.Range(2f, 5f);
                break;

            case ItemType.SASHIMI:
                hero.RemoveGlobalState(StateType.STATE_HEATSTROKE);

                break;
            case ItemType.ICY_DRINK:
                hero.RemoveGlobalState(StateType.STATE_HEATSTROKE);
                break;
         

            case ItemType.FRUIT_MILLK_SHAKE:
                //regenerate for 100Ss
                state = new GlobalState(100f, StateType.REGENERATE, 0.16f);
                hero.AddGlobalState((GlobalState)state);
                break;

            case ItemType.APPLE_PIE:
                state = new GlobalState(100f, StateType.REGENERATE, 0.13f);
                hero.AddGlobalState((GlobalState)state);
                break;
          
            case ItemType.FRUIT_CARNIVAL:
                hero.RemoveBattleState(StateType.BATTLE_STATE_POISON);
                hero.RemoveGlobalState(StateType.STATE_DIZZY);
                break;

            

            case ItemType.CHILI_CRAP:
                hero.RemoveBattleState(StateType.BATTLE_STATE_STUN);
                break;

            case ItemType.SEAFOOD_SALAD:
                hero.RemoveBattleState(StateType.BATTLE_STATE_POISON);
                break;
          
            case ItemType.FIG:
                state = new GlobalState(0.5f * Config.SecondsPerDay, StateType.LUCKY, 0f);
                hero.AddGlobalState((GlobalState)state);
                break;

            case ItemType.POISON_ENHANCER:
                hero.Attack += Random.Range(-2.5f, 4f);
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.TAKE_POISON_ENHANCER);
                break;


            case ItemType.EVIL_BREATH:
                hero.Attack += Random.Range(5f, 25f);
                break;


            case ItemType.SHADOW_HEART:
                hero.ElementAttackBonus[ElementType.Dark]
                    += Random.Range(0.01f, 0.3f);
                break;


            case ItemType.FRUIT_MILK_PUDDING:
                hero.CurrentHealth += hero.HealthUpperLimit * 0.1f;
                break;

            case ItemType.DONG_XIAO_XIAN:
                hero.CurrentHealth += hero.HealthUpperLimit * 0.15f;
                break;

            case ItemType.HONEY_SNAKE:
                hero.CurrentHealth += hero.HealthUpperLimit * 0.15f;
                break;

        }

		//normal food just heal the player
		float current_health = Game.Current.Hero.CurrentHealth;
		float limit = Game.Current.Hero.HealthUpperLimit;


        if (!hero.IsInBattleState(StateType.BATTLE_STATE_PAIN))
        {
            if ((current_health + this.HealValue) >= limit)
            {
                hero.CurrentHealth = limit;
            }
            else
            {
                hero.CurrentHealth += this.HealValue;
            }
        }
		
		Game.Current.Hero.loses (ItemFactory.Get (this.Type)); //only consume 1 

	}

}
