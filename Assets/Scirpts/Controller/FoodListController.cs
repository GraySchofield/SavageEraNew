using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FoodListController : MonoBehaviour {
	public Transform generalListRowPrefab;
	public Transform generalListRowEquippedPrefab;
	public Transform generalListDetail;
	
	public void RenderFoodList(){
		List<Food> foods = Game.Current.Hero.UserInventory.AllFood;
		foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
		for (int i = 0; i < foods.Count; i ++) {
			//add the items to the view 
			Food current_food = foods[i];
			Transform clone;
			if(current_food == Game.Current.Hero.FoodInBattle){
				clone = Instantiate(generalListRowEquippedPrefab) as Transform;
			}else{
				clone = Instantiate(generalListRowPrefab) as Transform;
			}
			Text name = clone.GetComponentInChildren<Text>();
			RectTransform rf = clone.GetComponent<RectTransform>();
			name.text = current_food.Name  + " " + current_food.Count + " " + Lang.Current["hp_recover"] + ":" + current_food.HealValue + 
                " " + Lang.Current["cool_down"] + ":" + current_food.CoolDown;
			clone.SetParent(transform);
			float height_constant = rf.rect.height;
			rf.anchorMin = new Vector2(0,1);
			rf.anchorMax = new Vector2(1,1);
			rf.localScale = new Vector3(1f,1f,1f);
			rf.offsetMin = new Vector2(20f, -(i+1)*(8f + height_constant));
			rf.offsetMax = new Vector2(-20f, -5 - i*(8f + height_constant));
			clone.GetComponent<Button>().onClick.AddListener(delegate {
				clone.SetAsLastSibling();
				if(clone.FindChild("GeneralListDetail(Clone)") != null){
					//close 
					Destroy(clone.FindChild("GeneralListDetail(Clone)").gameObject);
				}else{
					//open
					Transform list_detail = Instantiate(generalListDetail) as Transform;
					list_detail.SetParent(clone);
					list_detail.FindChild("Title").GetComponent<Text>().text = current_food.Name;
					list_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = current_food.Description;
					RectTransform detail_rf  = list_detail.GetComponent<RectTransform>();
					detail_rf.localScale = new Vector3(1f,1f,1f);
					detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
					detail_rf.offsetMax = new Vector2(-5f, 0f);
					
				}
			});
			clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
				Game.Current.Hero.FoodInBattle = current_food;
                Game.Current.Hero.UserInventory.AllFood.Remove(current_food);
                Game.Current.Hero.UserInventory.AllFood.Insert(0,current_food);
                transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                GameObject.Find("Click").GetComponent<AudioSource>().Play();

            });
		} 
	}
}
