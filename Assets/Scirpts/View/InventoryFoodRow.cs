using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryFoodRow {
	private GameObject food_row;
	private Text food_row_text;
	private RectTransform food_row_rect;

	public InventoryFoodRow(Food current_food, int idx, Transform parentTransform){
        //add the items to the view 
		food_row = GameObject.Instantiate (Resources.Load ("Prefab/FoodRow")) as GameObject;
		food_row_text = food_row.GetComponentInChildren<Text>();
		food_row_rect = food_row.GetComponent<RectTransform>();
		food_row_text.text = current_food.Name  + "*" + current_food.Count ;
		
		float height_constant = food_row_rect.rect.height;
		food_row_rect.anchorMin = new Vector2(0,1);
		food_row_rect.anchorMax = new Vector2(1,1);
		food_row_rect.localScale = new Vector3(1f,1f,1f);
		food_row_rect.sizeDelta = new Vector2( food_row_rect.rect.width, height_constant);
		food_row_rect.offsetMin = new Vector2(22.5f, -(idx+1)*(5f + height_constant));
		food_row_rect.offsetMax = new Vector2(-22.5f, -5 - idx*(5f + height_constant));
		
		food_row.transform.SetParent(parentTransform,false);
		food_row.GetComponent<Button>().onClick.AddListener(delegate {
			//inistantiate the detail Dialog based on the Food !
			GameObject food_detail_top = GameObject.Instantiate (Resources.Load ("Prefab/FoodDetailDialog")) as GameObject;
			food_detail_top.transform.SetParent(GameObject.Find ("Canvas").transform);
			food_detail_top.transform.SetAsLastSibling(); // move the dialog to the front of the View
			RectTransform food_detail_top_rect =  food_detail_top.GetComponent<RectTransform>();
			food_detail_top_rect.localScale = new Vector3(1f,1f,1f);
			food_detail_top_rect.offsetMax = new Vector2(0f,0f);
			food_detail_top_rect.offsetMin = new Vector2(0f,0f);
			
			
			Transform food_detail_view = food_detail_top.transform.FindChild("FoodDetail");
			//set title
			food_detail_view.FindChild("Header").GetComponentInChildren<Text>().text = current_food.Name;
			food_detail_view.FindChild("Description").GetComponent<Text>().text = current_food.Description + "\n" + Lang.Current["health_recover"] + ":" +
				current_food.HealValue + "\n"
                +Lang.Current["cool_down"] + ":" + current_food.CoolDown;
			food_detail_view.GetComponent<FoodDetailController>().theFood = current_food; // pass the item to the controller
			
			RectTransform tool_rect = food_detail_view.GetComponent<RectTransform>();
			
			tool_rect.anchoredPosition3D = new Vector3(0, 0, 0);
			tool_rect.localScale = new Vector3(1f,1f,1f);
			
		});

        food_row.GetComponent<Button>().onClick.AddListener(
           delegate {
               GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
           });



    }

	public void Remove(){
		GameObject.Destroy (food_row);
	}

	public void UpdateText(Food current_food, int idx){
		if (food_row_text != null) {
			food_row_text.text = current_food.Name  + "*" + current_food.Count ;
		}
		float height_constant = food_row_rect.rect.height;
		food_row_rect.offsetMin = new Vector2(22.5f, -(idx+1)*(5f + height_constant));
		food_row_rect.offsetMax = new Vector2(-22.5f, -5 - idx*(5f + height_constant));
	}

}
