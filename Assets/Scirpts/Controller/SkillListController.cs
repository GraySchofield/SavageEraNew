using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillListController : MonoBehaviour {

	public Transform generalListRowPrefab;
	public Transform generalListRowEquippedPrefab;
	public Transform skillListDetail;
	
	public void RenderSkillList(){
		List<PlayerUltiSkill> skills = Game.Current.Hero.LearntUltiSkills;
		foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
		for (int i = 0; i < skills.Count; i ++) {
			//add the items to the view 
			PlayerUltiSkill current_skill = skills[i];
			Transform clone;
			if(current_skill == Game.Current.Hero.CurrentUltiSkill){
				clone = Instantiate(generalListRowEquippedPrefab) as Transform;
			}else{
				clone = Instantiate(generalListRowPrefab) as Transform;
			}
			Text name = clone.GetComponentInChildren<Text>();
			RectTransform rf = clone.GetComponent<RectTransform>();
			if(name == null){
				Debug.LogError("UI null!");
			}
            name.text = GetSkillTitleString(current_skill);
			clone.SetParent(transform);
			float height_constant = rf.rect.height;
			rf.anchorMin = new Vector2(0,1);
			rf.anchorMax = new Vector2(1,1);
			rf.localScale = new Vector3(1f,1f,1f);
			rf.offsetMin = new Vector2(20f, -(i+1)*(8f + height_constant));
			rf.offsetMax = new Vector2(-20f, -5 - i*(8f + height_constant));
			clone.GetComponent<Button>().onClick.AddListener(delegate {
				//show or hide gears detail
				clone.SetAsLastSibling(); //need to move front
				if(clone.FindChild("SkillListDetail(Clone)") != null){
					//close 
					Destroy(clone.FindChild("SkillListDetail(Clone)").gameObject);
				}else{
					//open
					Transform list_detail = Instantiate(skillListDetail) as Transform;
					list_detail.SetParent(clone);
					list_detail.FindChild("Title").GetComponent<Text>().text = current_skill.Name;					
					list_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = GetSkillDetailString(current_skill);
					Button btn_upgrade = list_detail.FindChild("Upgrade").GetComponent<Button>();
					Text upgrade_text = btn_upgrade.GetComponentInChildren<Text>();
					RectTransform detail_rf  = list_detail.GetComponent<RectTransform>();
					detail_rf.localScale = new Vector3(1f,1f,1f);
					detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
					detail_rf.offsetMax = new Vector2(-5f, 0f);

                    btn_upgrade.onClick.AddListener(delegate {
						if(current_skill.Upgrade()){
                            //name.text = current_skill.Name + " " + Lang.Current["level"] + current_skill.SkillLevel;
                            //RenderSkillList();
                            Achievement.Current.UnlockAchievement(Achievement.AchievementType.FIRST_SKILL_UPGRADE);
                            list_detail.GetComponent<AudioSource>().Play();
                            name.text = GetSkillTitleString(current_skill);
                            list_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = GetSkillDetailString(current_skill);

                            if (current_skill.isFullLevel())
                            {
                                upgrade_text.text = Lang.Current["level_max"];
                                Achievement.Current.UnlockAchievement(Achievement.AchievementType.SKILL_MAX);                               
                            }
                            else
                            {
                                upgrade_text.text = Lang.Current["upgrade"] + "(" + current_skill.CalculateSoulRequire() + Lang.Current["soul"] + ")";
                            }
                        }
                        else
                        {
                            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
                        }
					});

					if(current_skill.isFullLevel()){
						upgrade_text.text = Lang.Current["level_max"];
					}else{
						upgrade_text.text = Lang.Current["upgrade"] +"(" + current_skill.CalculateSoulRequire() + Lang.Current["soul"] + ")";
					}

				}
			});
			clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
				//add Weapon to player gear	
				Game.Current.Hero.CurrentUltiSkill =  current_skill;
                Game.Current.Hero.LearntUltiSkills.Remove(current_skill);
                Game.Current.Hero.LearntUltiSkills.Insert(0, current_skill);
                transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                GameObject.Find("Click").GetComponent<AudioSource>().Play();

            });
		} 
	}

    private string GetSkillDetailString(PlayerUltiSkill current_skill)
    {
        string des = "";
        des += current_skill.Description + "\n" + Lang.Current["skill_charge"] + ":" + (int)(current_skill.ChargeAmount * 100) + "%";

        switch (current_skill.Type)
        {
            case SkillType.ULTI_SKILL_FIRE_SHOCK:
                des += "\n" + Lang.Current["damage"] + ":" + Lang.Current["per_second"] + System.Math.Round(current_skill.Arg2 * 2, 1) + "*" + Lang.Current["current_attack"];
                break;

            case SkillType.ULTI_SKILL_ARMOR_BREAK:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_FIRE_HELL:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;


            case SkillType.ULTI_SKILL_FIRE_STUN:
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_HOLY_SPIRIT:
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_ICE_BLIZZARD:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;


            case SkillType.ULTI_SKILL_ICE_BODY:
                des += "\n" + Lang.Current["attack_bonus"] + ":" + (int)(current_skill.Arg2 * 100) + "%";
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

           
            case SkillType.ULTI_SKILL_WIND_FAST:
                des += "\n" + Lang.Current["cool_down_shorten"] + ":" + (int)(current_skill.Arg2 * 100) + "%";
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_WIND_BURY:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                des += "\n" + Lang.Current["kill_threshold"] + ":" + (int)(current_skill.Arg1 * 100) + "%";
                break;

            case SkillType.ULTI_SKILL_ICE_SHOCK:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_POISON_STRIKE:
                des += "\n" + Lang.Current["damage"] + ":"  + Lang.Current["per_second"] + System.Math.Round(current_skill.Arg2 * 2 * 100,1) + "%" + Lang.Current["monster_health"];
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                break;

            case SkillType.ULTI_SKILL_WEAK_STRIKE:
                des += "\n" + Lang.Current["monster_attack_reduced_to"] + (int)(current_skill.Arg2 * 100) + "%";
                des += "\n" + Lang.Current["lasting_time"] + ":" + current_skill.Arg1 + Lang.Current["second"];
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                break;
                
            default:
                des += "\n" + Lang.Current["damage"] + ":" + Element.getElementName(current_skill.SkillDamage.EType) + current_skill.SkillDamage.DamageAmount;
                break;
        }





        return des;
    }

    private string GetSkillTitleString(PlayerUltiSkill current_skill)
    {
        
       string text = current_skill.Name + " " + Lang.Current["level"] + current_skill.SkillLevel;
       return text;
    }


}
