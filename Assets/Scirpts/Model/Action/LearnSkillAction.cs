using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LearnSkillAction: Action
{
	private Skill skill;
	
	public LearnSkillAction (string type, List<Item> requireItems, List<Building> requireBuildings, Skill produce): base(type, requireItems, requireBuildings){
		skill = produce;
	}
	public LearnSkillAction (LearnSkillAction a):base(a){
		skill = a.skill;
	}

	protected override bool TryRun(){
		Game g = Game.Current;

		if (g.Hero.IsSkillLearnt (skill.Type)) {
			g.AddToast(Lang.Current["skill_already_learnt"]);
			return false;
		}

		if (Doable ()) {
			ConsumeItem ();
			g.Hero.LearntUltiSkills.Add((PlayerUltiSkill)skill);
			g.AddToast(Lang.Current["skill_learnt"] + ":" + skill.Name);
			return true;
		}

		return false;
	}

	public Skill GetSkill(){
		return skill;
	}
	
	public override BaseModel GetProduce(){
		return GetSkill ();
	}
	
	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new LearnSkillButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new LearnSkillAction(this);
	}
}