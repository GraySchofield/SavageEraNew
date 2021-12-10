using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnterTechnologyButtonView {

	private GameObject enter;

	public EnterTechnologyButtonView(GameObject parentObject, string construction_name, int index, UnityAction enter_construction){
		enter = GameObject.Instantiate (Resources.Load ("Prefab/EnterScienceButton")) as GameObject;
		int padding = 20;
		RectTransform rf = enter.GetComponent<RectTransform> ();
		Button btn = enter.GetComponentInChildren<Button> ();
		enter.transform.SetParent (parentObject.transform);
		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = index; //0 ,1
		float width = rf.rect.width;
		//float height = rf.rect.height;
		
		rf.anchoredPosition = new Vector2 ( width/2 + x * (width + padding), 
		                                   0);
		
		btn.GetComponentInChildren<Text>().text = construction_name;

		btn.onClick.AddListener (enter_construction); 
		//this function need to bring out a new interface 
		//for the science construction
	}
	
	
	public void Remove(){
		GameObject.Destroy(enter);
	}
	
	public void MoveTo(int index){
		int padding = 20;
		RectTransform rf = enter.GetComponent<RectTransform> ();
		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = index; //0 ,1
		float width = rf.rect.width;
		//float height = rf.rect.height;
		
		rf.anchoredPosition = new Vector2 ( width / 2 + x * (width + padding), 
		                                  0);
		
	}
}
