using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;


public class MakeWeaponButtonView: ButtonView  {

	public MakeWeaponButtonView(GameObject parentObject, MakeWeaponAction action, int index): base("Prefab/GearMakeButton", -1, 1, 10, 22){
		Weapon weapon = action.GetWeapon ();

		Button btn = go.transform.FindChild ("Button").GetComponent<Button> ();
		Transform parent = parentObject.transform.FindChild("Detail").FindChild("SmithContentScrollView").FindChild("SmithContent");
		go.transform.SetParent (parent);

		MoveTo (index);
		
		btn.GetComponentInChildren<Text>().text = weapon.Name;
        switch (weapon.Tier)
        {
            case 1:
                btn.GetComponentInChildren<Text>().color = Config.uncommonColor;
                break;

            case 2:
                btn.GetComponentInChildren<Text>().color = Config.rareColor;
                break;

            case 3:
                btn.GetComponentInChildren<Text>().color = Config.legendColor;
                break;
        }
        go.transform.FindChild ("WorkRequires").GetComponent<Text> ().text = action.GetRequireString ();

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
				gear_detail.transform.FindChild("Title").GetComponent<Text>().text = weapon.Name;
				string content_text = "";
				content_text +=  Lang.Current["attack_range"] + ":" + weapon.AttackRange.Min * weapon.CoolDown + "~" + weapon.AttackRange.Max * weapon.CoolDown + "\n";
				content_text +=  Lang.Current["cool_down"] + ":" + weapon.CoolDown + "\n";
				content_text += Lang.Current["ring_speed"] + ":" + weapon.RingSpeed + "\n";

				gear_detail.transform.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = content_text;
				RectTransform detail_rf  = gear_detail.GetComponent<RectTransform>();
				detail_rf.localScale = new Vector3(1f,1f,1f);
				detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
				detail_rf.offsetMax = new Vector2(-5f, 0f);
				
			}

		});


        row_button.onClick.AddListener(
            delegate {
                GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
            });


        btn.onClick.AddListener (delegate {
			action.Run();
		});


        action.OnSuccess(delegate {
            GameObject.Find("EquipWeapon").GetComponent<AudioSource>().Play();
        });

        action.OnFail(delegate {
            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
        });

    }
}
