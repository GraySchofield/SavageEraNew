using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class LearnSkillButtonView: ButtonView  {

	public LearnSkillButtonView(GameObject parentObject, LearnSkillAction action, int index): base("Prefab/GeneralMakeButton", -1, 1, 10, 5){
		PlayerUltiSkill skill = (PlayerUltiSkill)action.GetSkill();

		Button btn = go.transform.FindChild ("Button").GetComponent<Button> ();
		Transform parent = parentObject.transform.FindChild("Detail").FindChild("ResearchContentScrollView").FindChild("ResearchContent");
		go.transform.SetParent (parent);
		
		MoveTo (index);

		btn.GetComponentInChildren<Text>().text = skill.Name;
		go.transform.FindChild("WorkRequires").GetComponent<Text> ().text = action.GetRequireString();
		
		Button row_button = go.GetComponent<Button> ();
		
		row_button.onClick.AddListener(delegate {
			//show or hide gears deta
			go.transform.SetAsLastSibling(); //need to move front
			if(go.transform.FindChild("GeneralListDetail(Clone)") != null){
				//close 
				GameObject.Destroy(go.transform.FindChild("GeneralListDetail(Clone)").gameObject);
			}else{
				//open
				GameObject gear_detail =  GameObject.Instantiate (Resources.Load ("Prefab/GeneralListDetail")) as GameObject;
				gear_detail.transform.SetParent(go.transform);
				gear_detail.transform.FindChild("Title").GetComponent<Text>().text = skill.Name;
				gear_detail.transform.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = skill.Description 
					+ "\n" + Lang.Current["skill_charge"] + ":" + (int) (skill.ChargeAmount * 100) + "%";
				RectTransform detail_rf  = gear_detail.GetComponent<RectTransform>();
				detail_rf.localScale = new Vector3(1f,1f,1f);
				detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
				detail_rf.offsetMax = new Vector2(-5f, 0f);
			}
		});

		btn.onClick.AddListener (delegate {
			action.Run();
		});

        row_button.onClick.AddListener(
            delegate {
                GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
            });


        action.OnSuccess(delegate {
            GameObject.Find("Click").GetComponent<AudioSource>().Play();
        });

        action.OnFail(delegate {
            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
        });

    }
}
