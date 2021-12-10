using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FoodDetailController : MonoBehaviour {

	public Food theFood; //the tool object that is passed to the controller
    private Text quantity;
    public AudioSource EatSound;

	void Awake(){
        quantity = transform.FindChild ("Quantity").GetComponent<Text> ();
	}

	void Update(){
		if (theFood != null) {
            Food theFoodInInventory = (Food)Game.Current.Hero.UserInventory.Get(theFood.Type, "Food");
            if(theFoodInInventory != null)
            {
                quantity.text = Lang.Current["qty"] + ": " + theFoodInInventory.Count;

            }
            else
            {
                quantity.text = Lang.Current["qty"] + ": 0";

            }
		}
	}


	public void EatCurrentFood(){
       Food theFoodInInventory = (Food)Game.Current.Hero.UserInventory.Get(theFood.Type, "Food");
        if (theFoodInInventory != null)
        {
            theFoodInInventory.Consume();
            if (!Game.Current.Hero.has(theFoodInInventory))
                DelayCloseDialog();
            if (!EatSound.isPlaying)
            {
                EatSound.Play();
            }
        }
        else
        {
            Debug.LogError("Food is null!");
        }


        if(Game.Current.GameTime < 300 && PlayerPrefs.GetInt("Step") == 6)
        {
            PlayerPrefs.SetInt("Step", 7);
            PlayerPrefs.Save();
        }

    }
	
    private void DelayCloseDialog()
    {
        Destroy(transform.parent.gameObject, 0.5f); // close the dialog

    }


    public void CloseDialog(){
		Destroy (transform.parent.gameObject); // close the dialog
	}
}
