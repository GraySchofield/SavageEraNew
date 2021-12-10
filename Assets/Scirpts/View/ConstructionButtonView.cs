using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConstructionButtonView: ButtonView  {

	private Building building;

	public ConstructionButtonView(GameObject parentObject, BuildConstructionAction action, int index): base("Prefab/BuildingButton", -1, 1, 15,15){
		Button btn = go.transform.FindChild ("ButtonBuild").GetComponent<Button>();
		Transform cost_des = go.transform.FindChild("CostDescription");
        Button info = go.GetComponent<Button>();

		go.transform.SetParent (parentObject.transform);

		MoveTo (index);
		
		btn.GetComponentInChildren<Text>().text = action.Name;
		cost_des.GetComponent<Text> ().text = action.GetRequireString();

        info.onClick.AddListener (delegate {
			GameObject detail_dialog = GameObject.Instantiate (Resources.Load ("Prefab/PopUpInfoDialog")) as GameObject;
			detail_dialog.transform.SetParent(GameObject.Find("Canvas").transform,false);
			detail_dialog.transform.SetAsLastSibling();

			RectTransform dialog_rect_transform = detail_dialog.GetComponent<RectTransform>();
			dialog_rect_transform.offsetMax = new Vector2(0f,0f);
			dialog_rect_transform.offsetMin = new Vector2(0f,0f);
			Transform  detail = detail_dialog.transform.FindChild("Detail");
			detail.FindChild("Header").GetComponentInChildren<Text>().text = action.Name;
			detail.FindChild("Description").GetComponent<Text>().text = action.GetProduce().Description;

		});

        info.onClick.AddListener(
           delegate {
               GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
           });

        building = action.GetBuilding ();
		if (building.Type != BuildingType.WOODEN_HOUSE) {
			//those constructions that are not wooden house may only be built once
			if (Game.Current.Hero.UserConstructions.Has (building)) {
				//already built
				RedrawContent ();
			}
		} else {
			if(Game.Current.Hero.MyHouse.Count > 0)
				RedrawContent ();
		}

		action.OnSuccess (delegate {
			GameObject.Find("Click").GetComponent<AudioSource>().Play();
			RedrawContent();
		});

        action.OnFail(delegate {
            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
        });

        btn.onClick.AddListener (delegate {
			action.Run();
		});
	}

	// this is called when the building is built
	public void RedrawContent(){
		//Button btn = go.transform.FindChild ("ButtonBuild").GetComponent<Button>();
		Transform cost_des = go.transform.FindChild("CostDescription");
		Transform building_state = go.transform.FindChild("BuildingState");

		//the building is built
		building_state.GetComponent<CanvasGroup>().alpha = 1;

		if (building.Type.Equals (BuildingType.WOODEN_HOUSE)) {
			//countable
			Sprite all_sprites = Resources.Load<Sprite> ("Sprites/confirm_circle");
			building_state.GetComponent<Image> ().sprite = all_sprites;
			building_state.GetComponentInChildren<Text> ().text =  Game.Current.Hero.MyHouse.Count + "";
		} else {
			building_state.GetComponentInChildren<Text> ().text = "";
			cost_des.GetComponent<Text> ().text = ""; // this need to be related to the new cost, or no value
		}

	}
}