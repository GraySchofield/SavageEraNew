using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

public class Cookbook{

	private static Dictionary<string, Food> cookbook ;
	private static Dictionary<string, Food> single_cook_book ;

	
	private static Dictionary<string, Food> Init(){
   
		Dictionary<string, Food> cb = new Dictionary<string, Food> ();
		single_cook_book = new Dictionary<string, Food>();
		TextAsset resource = (TextAsset)Resources.Load("Config/Cookbook");
		XmlDocument cookbookXML = new XmlDocument ();
		cookbookXML.LoadXml (resource.text);
		XmlNodeList recipes = cookbookXML.SelectNodes ("/Recipes/Recipe");
		foreach (XmlNode recipe in recipes) {
			XmlNodeList ingredients = recipe.SelectNodes("Ingredient");
			List<string> ingredientList = new List<string>();
			foreach(XmlNode ingredient in ingredients){
				ingredientList.Add(ingredient.InnerText);
			}
			ingredientList.Sort();
			string foodType = recipe["Food"].InnerText;
			Food food = ItemFactory.Get(foodType) as Food;
			if(recipe["IsSingle"] != null){
                single_cook_book[ingredientList[0]] = food;
            }
            string finalRecipe = string.Join("_", ingredientList.ToArray());
            cb[finalRecipe] = food;
        }
        return cb;
	}
	
	public static void ReloadXML(){
		cookbook = Init();
	}

	public static Food Get(IngredientPack igp){
		if (igp.isEmpty ()) {
			return null;
		}

        Dictionary<string, string> ingredientSubTypes = new Dictionary<string, string>();

        List<string> ingredientList = new List<string> ();
		if (igp.IngredientOne != null) {
			ingredientList.Add (igp.IngredientOne.Type);
            ingredientSubTypes[igp.IngredientOne.Type] = igp.IngredientOne.SubClass;

        }
		if (igp.IngredientTwo != null) {
			ingredientList.Add (igp.IngredientTwo.Type);
            ingredientSubTypes[igp.IngredientTwo.Type] = igp.IngredientTwo.SubClass;
        }
		if (igp.IngredientThree != null) {
			ingredientList.Add (igp.IngredientThree.Type);
            ingredientSubTypes[igp.IngredientThree.Type] = igp.IngredientThree.SubClass;
        }
		ingredientList.Sort ();

        Food f = GetFood(ingredientList, ingredientSubTypes, new List<string>(), 0);

        if (f != null)
        {
            return f;
        }
        else
        {
			foreach(string type in ingredientList)
            {
				if(single_cook_book.ContainsKey(type) && ingredientList.Count > 1)
                {
					return single_cook_book[type];
				}
			}

            Achievement.Current.UnlockAchievement(Achievement.AchievementType.FIRST_BAD_COOK);
            return ItemFactory.Get(ItemType.WETGOOP) as Food;
        }
      
	}

    private static Food GetFood(List<string> ingredientList, Dictionary<string, string> ingredientSubTypes, List<string> finalIngredientList, int step)
    {
        if (step >= ingredientList.Count)
        {
            finalIngredientList.Sort();
            string finalIngredients = string.Join("_", finalIngredientList.ToArray());
            if (cookbook.ContainsKey(finalIngredients))
            {
                return cookbook[finalIngredients];
            }
            else
            {
                return null;
            }
        }

        finalIngredientList.Add(ingredientList[step]);
        Food f = GetFood(ingredientList, ingredientSubTypes, finalIngredientList, step + 1);
        if (f != null) return f;
        finalIngredientList.Remove(ingredientList[step]);

        string subtype = ingredientSubTypes[ingredientList[step]];
        if (subtype != null)
        {
            finalIngredientList.Add(subtype);
            f = GetFood(ingredientList, ingredientSubTypes, finalIngredientList, step + 1);
            if (f != null) return f;
            finalIngredientList.Remove(subtype);
        }

        return null;
    }
}

