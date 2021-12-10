using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewNotificationManager : MonoBehaviour {

	public Transform targetViewGroup;
	public float refreshPeriod;
	private Image image;
	private float timeToRefresh = 2.2f;
	private int lastChidrenCount;
	public int contentType = 0; //1 res, 2 food , 3 tool 
    private bool startRefresh = false;

    private Color white = new Color(1f,1f,1f,1f);

    void Awake(){
		image = GetComponent<Image>();
		if (contentType != 0) {
            lastChidrenCount = Game.Current.Hero.UserInventory.AllResources.Count
                + Game.Current.Hero.UserInventory.AllFood.Count +
                Game.Current.Hero.UserInventory.AllTools.Count;
		}
        Invoke("getInitialChildCount", 2f);
	}


    private void getInitialChildCount()
    {
        if(targetViewGroup != null)
            lastChidrenCount = targetViewGroup.childCount;
        startRefresh = true;
    }

	// Update is called once per frame
	void Update () {
		if (Time.time > timeToRefresh && startRefresh) {
			if(targetViewGroup != null){
				if(lastChidrenCount != targetViewGroup.childCount){
                    //number of children changed , new stuff 
                    
					image.color =  white;
					lastChidrenCount = targetViewGroup.childCount;
				}
			}
			else if(contentType != 0){
				int current_count = lastChidrenCount;
                MainCharacter hero = Game.Current.Hero;
                current_count = hero.UserInventory.AllResources.Count
                    + hero.UserInventory.AllFood.Count
                    + hero.UserInventory.AllTools.Count;

				if(lastChidrenCount != current_count){
					image.color =  white;
					lastChidrenCount = current_count;
				}
            }
           
			timeToRefresh = Time.time + refreshPeriod;
		}
	
	}
}
