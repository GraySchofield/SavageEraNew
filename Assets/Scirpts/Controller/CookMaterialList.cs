using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CookMaterialList : MonoBehaviour {
	public Transform foodRowPrefab;
	public float refreshPeriod = 0.5f;
	public Transform scrollContent;
	[HideInInspector]
	public int ingredientIndex = -1;


	public void renderFoodMaterialList(){
		List<Food> food = Game.Current.Hero.UserInventory.AllFood;
		foreach (Transform childTransform in scrollContent) Destroy(childTransform.gameObject);
		int position_idx = 0;
        IngredientPack the_igp = GetComponentInParent<CookPotController>().igp;

        for (int i = 0; i < food.Count; i ++) {
			//add the items to the view 
			Food current_food = food[i];
			if(current_food.IsIngredient && !the_igp.ContainsFood(current_food)){

				Transform clone = Instantiate(foodRowPrefab) as Transform;
				Text name = clone.GetComponentInChildren<Text>();
				RectTransform rf = clone.GetComponent<RectTransform>();
				name.text = current_food.Name  + " " + current_food.Count;
				clone.SetParent(scrollContent.transform);

				float height_constant = rf.rect.height;
				rf.anchorMin = new Vector2(0,1);
				rf.anchorMax = new Vector2(1,1);
				rf.localScale = new Vector3(1f,1f,1f);
				rf.offsetMin = new Vector2(22.5f, -(position_idx+1)*(5f + height_constant));
				rf.offsetMax = new Vector2(-22.5f, -5 - position_idx*(5f + height_constant));
                clone.FindChild("Add").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
				clone.GetComponentInChildren<Button>().onClick.AddListener(delegate {
					//Add Food to the controller and dismiss the controller
					if(ingredientIndex >= 0){
                        switch (ingredientIndex){
						case 0 :
                                /*
                                if(the_igp.IngredientOne != null)
                                    {
                                        Game.Current.Hero.gains(the_igp.IngredientOne, false);
                                    }
                                    */
                                the_igp.IngredientOne = current_food;
							break;
						case 1:
                                /*
                                if (the_igp.IngredientTwo != null)
                                {
                                    Game.Current.Hero.gains(the_igp.IngredientTwo, false);
                                }
                                */
                                the_igp.IngredientTwo = current_food;
							break;
						case 2:
                                /*
                                if (the_igp.IngredientThree != null)
                                {
                                    Game.Current.Hero.gains(the_igp.IngredientThree, false);
                                }
                                */
                      
                                the_igp.IngredientThree = current_food;
							break;
						}

						//Game.Current.Hero.UserInventory.Remove(current_food);
						Destroy(gameObject,0.1f);

					}
				});
				position_idx ++;
			}
		}             
	}

	public void CloseSelf(){
		Destroy (gameObject);
	}
	
	// Use this for initialization
	void Start () {
        renderFoodMaterialList();
	}

}
