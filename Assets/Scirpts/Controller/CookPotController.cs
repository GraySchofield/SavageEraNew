using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CookPotController : MonoBehaviour {
	[HideInInspector]
	public IngredientPack igp;
	
	public Transform CookListPrefab;
	private Text button_one_text;
	private Text button_two_text;
	private Text button_three_text;
	private Text result_text;
	private Button resultButton;
	private Image progress_bar;

	private bool isCooking = false;
	private float CookingTime = 0f; // from 0 to 3, use 3 seconds to cook a dish
	private float CookingLength = 1.5f;
    private CanvasGroup ChangePanelGroup;
    private CanvasGroup StoryPanelGroup;
    private AudioSource CookSound;

    private int CookingQty = 1; //how many pieces of food should be cooked
    public Text CookingQtyText;


    void Start(){
		button_one_text = transform.FindChild ("CookPotContent").FindChild ("ButtonMaterial1").GetComponentInChildren<Text> ();
		button_two_text = transform.FindChild ("CookPotContent").FindChild ("ButtonMaterial2").GetComponentInChildren<Text> ();
		button_three_text = transform.FindChild ("CookPotContent").FindChild ("ButtonMaterial3").GetComponentInChildren<Text> ();
		result_text = transform.FindChild ("CookPotContent").FindChild ("ButtonResult").GetComponentInChildren<Text> ();
		resultButton = transform.FindChild ("CookPotContent").FindChild ("ButtonResult").GetComponent<Button> ();
		progress_bar = transform.FindChild ("CookPotContent").FindChild ("ProgressBar").GetComponent<Image> ();
		progress_bar.fillAmount = 0;
        ChangePanelGroup = GameObject.Find("PanelChangeable").GetComponent<CanvasGroup>();
        StoryPanelGroup = GameObject.Find("StoryPannel").GetComponent<CanvasGroup>();
        ChangePanelGroup.alpha = 0;
        StoryPanelGroup.alpha = 0;
        CookSound = GetComponent<AudioSource>();
    }


	public void setIngredient(int idx){
		//pop up the list of food the user has
		//let the user choose
		if (Game.Current.Hero.UserInventory.AllFood.Count > 0
            && !isCooking) {
			//only show the list when there is ingredient left
            //when cooking starts it cannot be changed
			Transform cook_list = Instantiate (CookListPrefab);
			cook_list.GetComponent<CookMaterialList> ().ingredientIndex = idx;
			cook_list.SetParent (transform);
			RectTransform cook_list_rect = cook_list.GetComponent<RectTransform> ();
			cook_list_rect.localScale = new Vector3 (1f, 1f, 1f);
			cook_list_rect.offsetMax = new Vector2 (0f, 0f);
			cook_list_rect.offsetMin = new Vector2 (0f, 0f);
		}

	}



	public void Cook(){
		//show a cook loading interface then cook
		if (igp.isFull()){
           
			isCooking = true;
			resultButton.interactable = true;
            CookingLength = CookingQty * 0.8f;      
        }
        else
        {
            Game.Current.AddToast(Lang.Current["cook_material_not_enough"]);
        }
	}

   /*
    public void PutBackIngredients()
    {
        //need to put back ingredients not used
        if (igp.IngredientOne != null)
        {
            Game.Current.Hero.UserInventory.Add(igp.IngredientOne);
            igp.IngredientOne = null;
        }
        if (igp.IngredientTwo != null)
        {
            Game.Current.Hero.UserInventory.Add(igp.IngredientTwo);
            igp.IngredientTwo = null;
        }
        if (igp.IngredientThree != null)
        {
            Game.Current.Hero.UserInventory.Add(igp.IngredientThree);
            igp.IngredientThree = null;
        }
    }
    */


	public void CloseSelf(){
        ChangePanelGroup.alpha = 1;
        StoryPanelGroup.alpha = 1;
        Destroy (transform.parent.gameObject);
	}


    private void AddCookingQty()
    {
        if(CookingQty < igp.IngredientOne.Count
            && CookingQty < igp.IngredientTwo.Count
            && CookingQty < igp.IngredientThree.Count
            && !isCooking)
        {
            CookingQty++;
        }
        
    }

    public void StartAddQty()
    {
        if(igp.IngredientOne != null&&
            igp.IngredientTwo != null &&
            igp.IngredientThree != null)
        {  
            InvokeRepeating("AddCookingQty", 0, 0.1f);
        }
        else
        {
            Game.Current.AddToast(Lang.Current["cook_material_not_enough"]);
        }
    }

    public void StopAddQty()
    {
        CancelInvoke("AddCookingQty");
    }


    private void MinusCookingQty()
    {
        if(CookingQty >= 2 && !isCooking)
        {
            CookingQty --;
        }
    }

    public void StartMinusQty()
    {
        InvokeRepeating("MinusCookingQty", 0, 0.1f);
    }

    public void StopMinusQty()
    {
        CancelInvoke("MinusCookingQty");
    }






    public void Update(){
        CookingQtyText.text = "" + CookingQty;
        if (igp.IngredientOne != null) {
			button_one_text.text = igp.IngredientOne.Name;
		} else {
			button_one_text.text = Lang.Current["ingredient"] + "1";
		}
		if (igp.IngredientTwo != null) {
			button_two_text.text = igp.IngredientTwo.Name;
		} else {
			button_two_text.text = Lang.Current ["ingredient"] + "2";
		}
		if (igp.IngredientThree != null) {
			button_three_text.text = igp.IngredientThree.Name;
		} else {
			button_three_text.text = Lang.Current ["ingredient"] + "3";
		}


		if (isCooking) {
			CookingTime += Time.deltaTime;
			progress_bar.fillAmount = CookingTime/CookingLength;
            if(!CookSound.isPlaying)
                CookSound.Play();
            if (CookingTime >= CookingLength){
                //cook finished
                if(CookSound.isPlaying)
                    CookSound.Stop();
                isCooking = false;
				CookingTime = 0;
				progress_bar.fillAmount = 1;
				resultButton.interactable = false;
				Food result_food = Cookbook.Get(igp);
                result_food.Count = CookingQty;
                Game.Current.Hero.UserInventory.Add(result_food);
				//remove all the ingredients
				result_text.text = result_food.Name;
                if (igp.IngredientOne != null)
                {

                    igp.IngredientOne.Count -= CookingQty;
                    if (igp.IngredientOne.Count  <= 0)
                    {
                        Game.Current.Hero.UserInventory.Remove(igp.IngredientOne);
                        igp.IngredientOne = null;
                    }
                  
                }

                if (igp.IngredientTwo != null)
                {

                    igp.IngredientTwo.Count -= CookingQty;
                    if (igp.IngredientTwo.Count <= 0)
                    {
                        Game.Current.Hero.UserInventory.Remove(igp.IngredientTwo);
                        igp.IngredientTwo = null;
                    }
                    
                }
               
                if (igp.IngredientThree != null)
                {
                    igp.IngredientThree.Count -= CookingQty;
                    if (igp.IngredientThree.Count <= 0)
                    {
                        Game.Current.Hero.UserInventory.Remove(igp.IngredientThree);                    
                        igp.IngredientThree = null;
                    }
                    
                }
                CookingQty = 1;

                //igp.clearPack();
            }
		}

	}

}
