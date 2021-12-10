using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleFoodList : MonoBehaviour {

	public Transform foodRowPrefab;
	public Transform scrollContent;

	
	
	public void renderFoodList(){
		List<Food> food = Game.Current.Hero.UserInventory.AllFood;
		foreach (Transform childTransform in scrollContent) Destroy(childTransform.gameObject);
		int position_idx = 0;
		for (int i = 0; i < food.Count; i ++) {
			//add the items to the view 
			Food current_food = food[i];
			Transform clone = Instantiate(foodRowPrefab) as Transform;
			Text name = clone.GetComponentInChildren<Text>();
			RectTransform rf = clone.GetComponent<RectTransform>();
			name.text = current_food.Name  + " " + current_food.Count + " " + Lang.Current["hp_recover"] + ":" + current_food.HealValue;
			clone.SetParent(scrollContent.transform);
			float height_constant = 120f;
			rf.anchorMin = new Vector2(0,1);
			rf.anchorMax = new Vector2(1,1);
			rf.localScale = new Vector3(1f,1f,1f);
			rf.offsetMin = new Vector2(20, -(position_idx+1)*(5f + height_constant));
			rf.offsetMax = new Vector2(-20, -5 - position_idx*(5f + height_constant));
			clone.GetComponentInChildren<Button>().onClick.AddListener(delegate {
				Game.Current.Hero.FoodInBattle = current_food;
				CloseSelf();
			});
			position_idx ++;
			
		}             
	}
	
	public void CloseSelf(){
		Destroy (transform.parent.parent.gameObject);
	}
	
	// Use this for initialization
	void Start () {
		renderFoodList ();
	}

}
