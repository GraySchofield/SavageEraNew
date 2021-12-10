using System;
[System.Serializable]
public class GlobalState : TimedState
{
	public GlobalState (float duration, string state_type, float general_arg1):base(duration, state_type)
	{
		this.Arg1 = general_arg1;
			
	}	

	//generic argument used for all states to represent different meaning
	//for e.g. how much regenerate heals
	public float Arg1 {
		get;
		set;
	}
	

	public override void UpdateState(float period){
	
		this.currentTime += period;
		if (this.currentTime >= this.Duration) {
			//time over
			//deactivate the state, remove it from game
            Game.Current.Hero.RemoveGlobalState(this.Type);
        }
        else
        {
            TakeContinuosEffect();
        }
	}

    





    public void Activate(){
        MainCharacter hero = Game.Current.Hero;
        switch (this.Type)
        {

            case StateType.WEATHER_GLUE:
                Game.Current.RoutineInjector.RoutineIndex[EventType.GENERATE_CLIMATE].Probability = 0;
                break;

            case StateType.HARVEST_STOPPED:
                Game.Current.RoutineInjector.RoutineIndex[EventType.HARVEST].Probability = 0;
                break;

            case StateType.STATE_FROZEN:
                hero.DamageMultipler *= 0.5f; //damage reduced by half
                break;

            case StateType.STATE_HEATSTROKE:
                hero.SufferingMultiplier *= 2f; //suffers more damage
                break;

            case StateType.STATE_DIZZY:
                hero.EquippedTool = null;//can't equip any tool when you are dizzy
                break;

            case StateType.STATE_MUSIC_BOX:
                hero.FoodCdMultiplier *= 0.6f;
                break;


            case StateType.STATE_SONG_SINGING:
                hero.SufferingMultiplier *= 3f;
                hero.DamageMultipler *= 1.5f;
                break; 

        }
	}
	 
	public void Deactivate(){
        MainCharacter hero = Game.Current.Hero;

        switch (this.Type)
        {

            case StateType.WEATHER_GLUE:
                Game.Current.RoutineInjector.RoutineIndex[EventType.GENERATE_CLIMATE].Probability = 1f;
                break;

            case StateType.HARVEST_STOPPED:
                Game.Current.RoutineInjector.RoutineIndex[EventType.HARVEST].Probability = 1f;
                break;

            case StateType.STATE_FROZEN:
                {
                    hero.DamageMultipler *= 2f;
                }
                break;

            case StateType.STATE_HEATSTROKE:
                hero.SufferingMultiplier *= 0.5f; //suffers more damage
                break;

            case StateType.STATE_MUSIC_BOX:
                hero.FoodCdMultiplier /= 0.6f;
                break;


            case StateType.STATE_SONG_SINGING:
                hero.SufferingMultiplier /= 3f;
                hero.DamageMultipler /= 1.5f;
                break;

        }

	}

    

    //the effect that will happen every update cycle of the update,ie 0.5 s
    //this effect will only be valid outside battle
   private void TakeContinuosEffect()
    {
        MainCharacter hero = Game.Current.Hero;
        switch (this.Type)
        {
            case StateType.STATE_FROZEN:
                hero.CurrentHealth -= 0.15f;
                break;
            case StateType.STATE_HEATSTROKE:
                hero.CurrentHealth -= 0.15f;
                break;
            case StateType.STATE_DIZZY:
                hero.CurrentHealth -= 0.15f;
                break;

            case StateType.REGENERATE:
                hero.CurrentHealth += this.Arg1;
                break;

            case StateType.STATE_BEE_POISON:
                hero.CurrentHealth -= 0.5f;
                break;


            case StateType.STATE_TENT:
                if (Game.Current.IsAtNight)
                {
                    hero.CurrentHealth += 0.3f;
                }
                break;            


        }
    }


}


