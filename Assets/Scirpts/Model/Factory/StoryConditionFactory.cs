using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

public class StoryConditionFactory
{
	public static bool AlwaysTrue(Dictionary<string, string> parameters){
		return true;
	}

	public static bool isPopulationVacancyAbove(Dictionary<string, string> parameters){
		int require_number = int.Parse(parameters["RequireVacancy"]);
		int actual_vacant = Game.Current.Hero.MyPopulation.UpperLimit - Game.Current.Hero.MyPopulation.Total;
		if (require_number <= actual_vacant) {
			//have enough spot
			return true;
		} else {
			//not having enough spot
			return false;
		}
	}

	public static bool notEquippedWith(Dictionary<string, string> parameters){
		string cate = parameters ["ToolCategory"];
		string type = parameters ["ToolType"];
		if (cate.Equals ("Tool")) {
			if (Game.Current.Hero.EquippedTool != null && Game.Current.Hero.EquippedTool.Type.Equals (type)) {
				return false;
			}
		} else if (cate.Equals ("Equipment")) {
			if (Game.Current.Hero.Equipment != null && Game.Current.Hero.Equipment.Type.Equals (type)) {
				return false;
			}
		}

		return true;
	}


	public static bool notAlreadyHaveScroll(Dictionary<string, string> parameters){
		string scroll_type = parameters ["ScrollType"];
		if (Game.Current.Hero.UserInventory.Get (scroll_type, "Scroll") != null) {
			return  false;
		} else {
			return true;
		}
	}


	public static bool isDarknessApproaching(Dictionary<string, string> parameters){
		int time_of_day = (int)Game.Current.GameTime % Config.SecondsPerDay; //how many seconds passed within this day
		
		if (time_of_day >= Config.SecondsPerDay * 0.57f
            && time_of_day <= Config.SecondsPerDay * 0.59f) {
			return true;
		} else {
			return false;
		}
	}

	public static bool isAtNight(Dictionary<string, string> parameters){
		int time_of_day = (int)Game.Current.GameTime % Config.SecondsPerDay; //how many seconds passed within this day
		
		if (time_of_day <= Config.SecondsPerDay * 0.6f) {
			return false;
		} else {
			return true;
		}
	}


	public static bool NotAtNight(Dictionary<string, string> parameters){
		int time_of_day = (int)Game.Current.GameTime % Config.SecondsPerDay; //how many seconds passed within this day
		
		if (time_of_day <= Config.SecondsPerDay * 0.6f) {
			return true;
		} else {
			return false;
		}
	}

	public static bool NotWearingWarmProtection(Dictionary<string, string> parameters){
		if (Game.Current.Hero.isItemEquipped (ItemType.WOOLGLOVE)) {
			return false;
			
		}
		return true;
	}


	public static bool IsInLuckyState(Dictionary<string, string> parameters){
		if (Game.Current.Hero.hasTimedState (StateType.LUCKY)) {
			return true;
		} else {
			return false;
		}
	}

	public static bool NotInLuckyState(Dictionary<string, string> parameters){
		if (Game.Current.Hero.hasTimedState (StateType.LUCKY)) {
			return false;
		} else {
			return true;
		}
	}

	public static bool IsInUnLuckyState(Dictionary<string, string> parameters){
		if (Game.Current.Hero.hasTimedState (StateType.UNLUCKY)) {
			return true;
		} else {
			return false;
		}
	}

	public static bool NotInUnLuckyState(Dictionary<string, string> parameters){
		if (Game.Current.Hero.hasTimedState (StateType.UNLUCKY)) {
			return false;
		} else {
			return true;
		}
	}



	public static bool IsInTimedState(Dictionary<string, string> parameters){
		string state_type = parameters ["StateType"];
		if (Game.Current.Hero.hasTimedState (state_type)) {
			return true;
		} else {
			return false;
		}
	}

	public static bool NotInTimedState(Dictionary<string, string> parameters){
		string state_type = parameters ["StateType"];
		if (Game.Current.Hero.hasTimedState (state_type)) {
			return false;
		} else {
			return true;
		}
	}

    public static bool IsInNormalGameMode(Dictionary<string, string> parameters)
    {
        if (Game.Current.CurrentGameMode == GameMode.Normal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

