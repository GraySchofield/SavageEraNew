using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorkButtonView
{
	private GameObject go;
	private float constantWidth;
	private float constantHeight;
	private int index;

	public delegate void Callback();

	public WorkButtonView(GameObject parentObject, string name, int index, UnityAction c){
		this.index = index;
		go = GameObject.Instantiate (Resources.Load ("Prefab/WorkButton")) as GameObject;
		int padding = 10;
		RectTransform rf = go.GetComponent<RectTransform> ();
		Button btn = go.GetComponent<Button> ();
		go.transform.SetParent (parentObject.transform);

		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = index % 4; //0 ,1
		int y = index / 4; // how many rows
		float width = rf.rect.width;
		float height = rf.rect.height;
		constantWidth = width;
		constantHeight = height;

		rf.anchoredPosition3D = new Vector3 (padding + width / 2 + x * (padding + width), 
		                                   -padding - height/2 - y*(padding + height),0);
		
		go.GetComponentInChildren<Text>().text = name;
		go.GetComponent<Animator> ().SetTrigger ("spawn");

		// pass callback to button listener
		btn.onClick.AddListener (c);
		btn.onClick.AddListener (delegate {
			GameObject.Find("Click").GetComponent<AudioSource>().Play();
            if(Game.Current.GameTime < Config.SecondsPerDay && PlayerPrefs.GetInt("Step") == 0)
            {
                Resource branch = Game.Current.Hero.UserInventory.Get(ItemType.BRANCH, "Resource") as Resource;
                if(branch != null)
                {
                    if(branch.Count >= 12 && Game.Current.Hero.UserInventory.Get(ItemType.CARROT, "Food") != null)
                    {
                        PlayerPrefs.SetInt("Step", 1);
                        PlayerPrefs.Save();
                    }                    
                }
            }
		});
	}


	public void Remove(){
		GameObject.Destroy(go);
	}

	public void MoveTo(int newIndex){
		if(index == newIndex){
			return;
		}
  
		index = newIndex;

		int padding = 10;
		RectTransform rf = go.GetComponent<RectTransform> ();
		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = index % 4; //0 ,1
		int y = index / 4; // how many rows
		float width = constantWidth;
		float height = constantHeight;
		
		rf.anchoredPosition = new Vector2 (padding + width / 2 + x * (padding + width), 
		                                   -padding - height/2 - y*(padding + height));

	}
}