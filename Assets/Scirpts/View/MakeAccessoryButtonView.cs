using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class MakeAccessoryButtonView: ButtonView  {

	public MakeAccessoryButtonView(GameObject parentObject, MakeAccessoryAction action, int index): base("Prefab/GearMakeButton", -1, 1, 10, 15){
		Accessory accessory = action.GetAccessory();

		Button btn = go.transform.FindChild ("Button").GetComponent<Button> ();
		Transform parent = parentObject.transform.FindChild("Detail").FindChild("AccessoryScrollView").FindChild("AccessoryShopContent");
		go.transform.SetParent (parent);
		
		MoveTo (index);
		
		btn.GetComponentInChildren<Text>().text = accessory.Name;
        switch (accessory.Tier)
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
				gear_detail.transform.FindChild("Title").GetComponent<Text>().text = accessory.Name;
				string content_text = "";

				content_text +=  Lang.Current["random_property"] + " * " + accessory.AttributeCount + "\n";
                content_text += "\n" + getDetailString(accessory);

                //content_text +=  Lang.Current["attack_range"] + ":" + accessory.AttackRange.Min + "~" + accessory.AttackRange.Max + "\n";
                //content_text +=  Lang.Current["defense_range"] + ":" + accessory.DefenseRange.Min + "~" + accessory.DefenseRange.Max + "\n";
                //content_text +=  Lang.Current["health_range"] + ":" + accessory.HealthRange.Min + "~" + accessory.HealthRange.Max + "\n";

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
            GameObject.Find("EquipAccessory").GetComponent<AudioSource>().Play();
        });

        action.OnFail(delegate {
            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
        });

    }


    private string getDetailString(Accessory acc)
    {
        Accessory ac = acc.FixedData;
        string content = "";
        if(ac != null)
        {
            if(ac.Attack > 0)
            {
                content += Lang.Current["attack_range"] + ":" + ac.Attack + "\n";
            }


            if(ac.Defense > 0)
            {
                content += Lang.Current["defense_range"] + ":" + ac.Defense + "\n";

            }

            if (ac.Health > 0)
            {
                content += Lang.Current["health_range"] + ":" + ac.Health + "\n";
            }


            foreach (ElementType key in ac.ElementResisIndex.Keys)
            {
                content += Lang.Current["element_resis"] + ": " + Element.getElementName(key) + (int)((1 - ac.ElementResisIndex[key]) * 100 )+ "%\n" ;
            }


            foreach (ElementType key in ac.ElementAttackIndex.Keys)
            {
                content += Lang.Current["element_attack_bonus"] + ": " + Element.getElementName(key) + (int) (ac.ElementAttackIndex[key] * 100) + "%\n";

            }


        }

        return content;
    }

}
