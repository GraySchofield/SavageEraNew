using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public float refreshPeriod = 0.5f; //0.5s
		
	public GameObject resourceScrollContent;
	private Dictionary<string, InventoryResourceRow> ResourceIndex; // type to view
	public GameObject foodScrollContent;
	private Dictionary<string, InventoryFoodRow> FoodIndex; //type to view
	public GameObject toolScrollContent;
	private Dictionary<string, InventoryToolRow> ToolIndex; //unique key to view, different from stackable items
    private Dictionary<string, List<Tool>> TypeToToolList;
    public GameObject pageStorage;


	public Transform StoryTextPrefab;
	public Transform StoryScrollContent;

	public Transform CloudingShadow;

    private SpriteRenderer CloudingShadowRenderer;
	public Transform ClockSpinner;
	public Transform DayCount;
	private Text DayCountText;

	public Transform CurrentToolView;
	public Transform CurrentEquipmentView;
	private Text tool_text ;
	private Text equipment_text;
	public Transform FireLight;
	private Animator fire_light_animator;
	private AudioSource fire_light_audio;

	public Transform HealthBar;
	private Text HealthBarText;
    public Image HealthBarImage;

	public Transform PopulationText;

	public Transform PageVillage;
	public Transform ProductionScrollContent;
	public Transform ProductivityText;
	private Text product_text;
	public Transform WorkerRowPrefab;
	private Dictionary<string,WorkerRowView>  OccupationIndex; //used to index what view has already been rendered

	public Transform TechnologyScrollContent;
	public Transform BlackSmithDialog;
	public Transform IronCookPotDialog;
	public Transform GearShopDialog;
	public Transform ResearchDialog;
	public Transform ElementHallDialog;
    public Transform EvilChamberDialog;

	private Dictionary<string,EnterTechnologyButtonView> TechnologyIndex;  //used to index what view has already been rendered


	public Transform PageAdventure;
	public Text WeaponText;
	public Text ArmorText;
	public Text AccesoryText;
	public Transform AdventureScrollContent;
	private Text attack_text;
	private Text defense_text;
	private Text health_limit_text;
	private Text additional_text;


	public Transform GoToTool;
	private CanvasGroup GoToToolCanvasGroup;
	public Transform MakeToolScrollContent;
	public Transform GoToConstruction;
	private CanvasGroup GoToConstructionCanvasGroup;
	public Transform ConstructionScrollContent;
	public Transform GoToAdventure;
	private CanvasGroup GoToAdventureCanvasGroup;
	public Transform GoToVillage;
	private CanvasGroup GoToVillageCanvasGroup;

	public Transform GearListPrefab;
	public Transform MapListPrefab;
	public Transform SkillListPrefab;
	public Transform FoodListPrefab;
	
	public GameObject Sunstroke;
	public GameObject Dizzy;
	public GameObject Frozen;
	private Game currentGame;


	private Image weather_icon;
	public Sprite Sunny;
	public Sprite Snow;
	public Sprite Rain;
	private Text temparature_text;
	private Text humidity_text;
	public Transform WeatherPanel;
	private CanvasGroup WeatherPanelCanvasGroup;


	public Transform WeatherParticles;
	private Transform RainParticle;
	private Transform SnowParticle;
	private Transform FogPrefab;

	public Transform PanelToast;
	private ToastManager toast_manager;


	public Transform StatesText;
	private Text states_text;
	public Transform HealthBarGlow;


	public AudioClip[] DaySounds;
	public AudioClip[] NightSounds;
	public AudioSource DayNightSound;
	private bool isDay = true; //true for day, false for night



	public Transform ButtonMapSelect;
	private Text button_map_text;
	public Transform ButtonChooseSkill;
	private Text button_choose_skill_text;
	public Transform ButtonChooseFood;
	private Text button_choose_food_text;


    public GameObject LoadingScreen;
    private AsyncOperation async;
    public Slider LoadingBar;
    public AudioSource StoryPopUpSound;

    public Animator MainCameraAnimator;

    [HideInInspector]
    public bool isGamePaused = false;


    public Animator SettingPanelAnimator;

    /*
	void Awake(){
		Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}
    */

    public List<GameObject> Toasts;

    [HideInInspector]
    public bool isBossPopUp = false;

    // Use this for initialization 
    void Start () {
        Application.targetFrameRate = 60;
        Game.Current.IsAtHome = true;
		currentGame = Game.Current;

	
		GoToToolCanvasGroup = GoToTool.GetComponent<CanvasGroup> ();
		GoToConstructionCanvasGroup = GoToConstruction.GetComponent<CanvasGroup> ();
		GoToAdventureCanvasGroup = GoToAdventure.GetComponent<CanvasGroup> ();
		GoToVillageCanvasGroup = GoToVillage.GetComponent<CanvasGroup> ();
		weather_icon = WeatherPanel.FindChild ("WeatherIcon").GetComponent<Image> ();
		temparature_text = WeatherPanel.FindChild ("Temparature").GetComponent<Text> ();
		humidity_text = WeatherPanel.FindChild ("Humidity").GetComponent<Text> ();
		RainParticle = WeatherParticles.FindChild("Rain");
		RainParticle.gameObject.SetActive (false);
		SnowParticle = WeatherParticles.FindChild("Snow");
		SnowParticle.gameObject.SetActive (false);
		FogPrefab = WeatherParticles.FindChild("Fog");
		FogPrefab.gameObject.SetActive (false);
		OccupationIndex = new Dictionary<string,WorkerRowView> ();
		TechnologyIndex = new Dictionary<string,EnterTechnologyButtonView> ();
		ResourceIndex = new Dictionary<string, InventoryResourceRow> ();
		FoodIndex = new Dictionary<string,InventoryFoodRow> ();
		ToolIndex = new Dictionary<string, InventoryToolRow> ();
        TypeToToolList = new Dictionary<string, List<Tool>>();
        toast_manager = PanelToast.GetComponent<ToastManager> ();

        CloudingShadowRenderer = CloudingShadow.GetComponent<SpriteRenderer> ();
		HealthBarText = HealthBar.FindChild ("HealthText").GetComponent<Text> ();
		product_text = ProductivityText.GetComponent<Text> ();
		DayCountText = DayCount.GetComponent<Text> ();
		tool_text = CurrentToolView.GetComponent<Text> ();
		equipment_text = CurrentEquipmentView.GetComponent<Text> ();
		fire_light_animator = FireLight.GetComponent<Animator> ();
		fire_light_audio = FireLight.GetComponent<AudioSource> ();
		states_text = StatesText.GetComponent<Text> ();
		WeatherPanelCanvasGroup = WeatherPanel.GetComponent<CanvasGroup> ();
		attack_text = AdventureScrollContent.FindChild ("AttackText").GetComponent<Text> ();
		defense_text = AdventureScrollContent.FindChild ("DefenseText").GetComponent<Text> ();
		health_limit_text = AdventureScrollContent.FindChild ("HealthText").GetComponent<Text> ();
		additional_text = AdventureScrollContent.FindChild ("AdditionalText").GetComponent<Text> ();
		button_map_text = ButtonMapSelect.GetComponentInChildren<Text> ();
		button_choose_skill_text = ButtonChooseSkill.GetComponentInChildren<Text> ();
		button_choose_food_text = ButtonChooseFood.GetComponentInChildren<Text> ();
		//Game routines
        
		InvokeRepeating ("RenderInventory", 0, refreshPeriod);
		InvokeRepeating ("RenderUtility", 0, refreshPeriod);
		InvokeRepeating ("RenderVillagePage", 0, refreshPeriod);
		InvokeRepeating ("RenderTransactionButtons", 0, 1f);
		InvokeRepeating ("RenderHealthState", 0, 1f);
		InvokeRepeating ("RenderAdventureGears", 0, 0.8f);
		InvokeRepeating ("ReadToastFromQueue2", 0, refreshPeriod);
		InvokeRepeating ("RenderClimate", 0, 1); // update rate may be revised
		InvokeRepeating ("UpdateGameTime", 0, refreshPeriod);
          
        currentGame.LoadGameView();
        RecoverBackUpStories();


        if(Game.Current.GameTime < Config.SecondsPerDay && PlayerPrefs.GetInt("Step") == 0)
        {
            Food carrot = new Food(ItemType.CARROT, 1, 3, true, "~vegetable");
            currentGame.Hero.gains(carrot, false);
        }

    }

    


    private void UpdateGameTime(){
        if (!isGamePaused)
        {
            currentGame.GameTime += refreshPeriod;
            SpinnClock();
            ReadStoryFromQueue();
        }
    }


	private void RenderDayNightSound(){
		if (Game.Current.IsAtNight) {
			//now at night
			if(isDay){
				//switch from day to night
				System.Random random = new System.Random();
				int idx = random.Next(0, NightSounds.Length);

				DayNightSound.clip =  NightSounds[idx];
				DayNightSound.Play();
				isDay = false;
			}
		} else {
			//now at day
			if(!isDay){
				//switch from night to day
				System.Random random = new System.Random();
				int idx = random.Next(0, DaySounds.Length);
				DayNightSound.clip =  DaySounds[idx];
				DayNightSound.Play();

				isDay = true;
			}
		
		}
	}


	private void RenderAdventureGears(){
		if (PageAdventure.gameObject.activeSelf) {
			if(Game.Current.Hero.Gears.EquippedWeapon != null){
				WeaponText.text = Game.Current.Hero.Gears.EquippedWeapon.Name;
                switch (Game.Current.Hero.Gears.EquippedWeapon.Tier)
                {
                    case 0:
                        WeaponText.color = Color.white;
                        break;

                    case 1:
                        WeaponText.color = Config.uncommonColor;
                        break;

                    case 2:
                        WeaponText.color = Config.rareColor;
                        break;

                    case 3:
                        WeaponText.color = Config.legendColor;
                        break;
                }
            }
            else{
                WeaponText.color = Color.white;
                WeaponText.text = Lang.Current["weapon"];
               
            }

			if(Game.Current.Hero.Gears.EquippedArmor != null){
				ArmorText.text = Game.Current.Hero.Gears.EquippedArmor.Name;
                switch (Game.Current.Hero.Gears.EquippedArmor.Tier)
                {
                    case 0:
                        ArmorText.color = Color.white;
                        break;

                    case 1:
                        ArmorText.color = Config.uncommonColor;
                        break;

                    case 2:
                        ArmorText.color = Config.rareColor;
                        break;

                    case 3:
                        ArmorText.color = Config.legendColor;
                        break;
                }
            }
            else{
                ArmorText.color = Color.white;
                ArmorText.text = Lang.Current["armor"];
            }

			if(Game.Current.Hero.Gears.EquippedAccessory != null){
				AccesoryText.text = Game.Current.Hero.Gears.EquippedAccessory.Name;
                switch (Game.Current.Hero.Gears.EquippedAccessory.Tier)
                {
                    case 0:
                        AccesoryText.color = Color.white;
                        break;

                    case 1:
                        AccesoryText.color = Config.uncommonColor;
                        break;

                    case 2:
                        AccesoryText.color = Config.rareColor;
                        break;

                    case 3:
                        AccesoryText.color = Config.legendColor;
                        break;
                }
            }
            else{
                AccesoryText.color = Color.white;
                AccesoryText.text = Lang.Current["accessory"];                
            }

			//render some player states
			attack_text.text = Lang.Current["attack_range"] + ":" + Math.Round(Game.Current.Hero.Attack,1);
			defense_text.text = Lang.Current["defense_range"] + ":" +  Math.Round(Game.Current.Hero.Defense,1);
			health_limit_text.text = Lang.Current["health_upper_limit"] + ":" + Math.Round(Game.Current.Hero.HealthUpperLimit,1);
			string content = Lang.Current["element_resis"] + ":" + "\n";
			foreach(ElementType type in Game.Current.Hero.ElementResisIndex.Keys){
				content += Element.getElementName(type) + ":" + (int)((1 - Game.Current.Hero.ElementResisIndex[type]) * 100) + "%" + "\n";
			}

            string attack_bonus = Lang.Current["element_attack_bonus"] + ":" + "\n";
            foreach (ElementType type in Game.Current.Hero.ElementAttackBonus.Keys)
            {
                attack_bonus += Element.getElementName(type) + ":" + (int)( Game.Current.Hero.ElementAttackBonus[type] * 100) + "%" + "\n";
            }

            additional_text.text = content + "\n" + attack_bonus;


            //render the button text
            if (Game.Current.Hero.CurrentEquippedMap != null)
            {
                button_map_text.text = Game.Current.Hero.CurrentEquippedMap.Name;
            }
            else
            {
                button_map_text.text = Lang.Current["pick_map"];
            }

            if (Game.Current.Hero.CurrentUltiSkill != null)
            {
                button_choose_skill_text.text = Game.Current.Hero.CurrentUltiSkill.Name;
            }
            else
            {
                button_choose_skill_text.text = Lang.Current["pick_skill"];

            }

            if (Game.Current.Hero.FoodInBattle != null)
            {
                button_choose_food_text.text = Game.Current.Hero.FoodInBattle.Name;
            }
            else
            {
                button_choose_food_text.text = Lang.Current["pick_food"];
            }
		}
	}
	

	private void RenderHealthState(){

        if (currentGame.Hero.hasTimedState(StateType.STATE_HEATSTROKE))
        {
            Sunstroke.SetActive(true);
        }
        else
        {
            Sunstroke.SetActive(false);
        }


        if (currentGame.Hero.hasTimedState(StateType.STATE_FROZEN))
        {
            Frozen.SetActive(true);
        }
        else
        {
            Frozen.SetActive(false);
        }

        /*
        if (currentGame.Hero.hasTimedState(StateType.STATE_DIZZY))
        {
            MainCameraAnimator.SetBool("isDizzy", true);
        }
        else
        {
            MainCameraAnimator.SetBool("isDizzy", false);
        }
        */
    }


    private void RenderTransactionButtons() {
        if (!GoToToolCanvasGroup.interactable) {
            if (MakeToolScrollContent.childCount > 0) {
               // GoToToolCanvasGroup.alpha = 1;
                GoToToolCanvasGroup.interactable = true;
            }
        }

        if (!GoToConstructionCanvasGroup.interactable) {
            if (ConstructionScrollContent.childCount > 0) {
               // GoToConstructionCanvasGroup.alpha = 1;
                GoToConstructionCanvasGroup.interactable = true;
            }
        }

        if (!GoToVillageCanvasGroup.interactable) {
            if (Game.Current.Hero.UserConstructions.Has(BuildingType.SCIENCE_LVL_ONE, "AcademicConstruction")
                || Game.Current.Hero.MyPopulation.UpperLimit > 0){

				//GoToVillageCanvasGroup.alpha = 1;
				GoToVillageCanvasGroup.interactable = true;
			}
		}

		if (!GoToAdventureCanvasGroup.interactable) {
			if(Game.Current.Hero.AllMaps.Count > 0){
				//GoToAdventureCanvasGroup.alpha =1;
				GoToAdventureCanvasGroup.interactable = true;
			}
		}

	}

    private void RenderVillagePage()
    {
        if (PageVillage.gameObject.activeSelf)
        {
            RenderPopulation();
            RenderTechnologies();
            RenderProductions();
        }

    }



	private void RenderPopulation(){
		if (PageVillage.GetComponent<CanvasGroup> ().alpha > 0) {
			PopulationText.GetComponent<Text> ().text = Lang.Current ["population"] + ": " 
				+ Game.Current.Hero.MyPopulation.Idles + "/" + Game.Current.Hero.MyPopulation.Total
					+ " "  +  Lang.Current["upper_limit"] + ":" +  Game.Current.Hero.MyPopulation.UpperLimit; 
		}
	}


	public void RenderProductions(){
		if (PageVillage.GetComponent<CanvasGroup> ().alpha > 0) {
			//only render when visible

			Dictionary<string, ProductionConstruction> occupations = Game.Current.Hero.MyPopulation.Occupations;
			Dictionary<string, int> Productivity = Game.Current.Hero.MyPopulation.Productivity;

			//production
			for (int i = 0; i < occupations.Count; i ++) {
				//each key is the type of worker
				//instantiate the worker row and add to the scroll view;
				var element = occupations.ElementAt (i);
				string key = element.Key;
				ProductionConstruction pc = element.Value;


				if(OccupationIndex.ContainsKey(key)){
					//occupation UI existed
					//update it
					OccupationIndex[key].UpdateWorkerNumber(pc.Workers);
				}else{
					//occupation UI not existed
					//add it
					WorkerRowView row = new WorkerRowView (
						ProductionScrollContent.gameObject,                                  
						Lang.Current [key], pc.Workers, i,
						delegate {
						//add Worker
						Game.Current.Hero.MyPopulation.AddWorker (key, 1);
					},
					delegate {
						//minus Worker
						Game.Current.Hero.MyPopulation.RemoveWorker (key, 1);
					});
					OccupationIndex.Add(key, row);
				}


			}
			List<string> keys_to_remove = new List<string>();
			foreach(string key in OccupationIndex.Keys){
				if(!occupations.ContainsKey(key)){
					//item not exist, destroy view 
					keys_to_remove.Add(key);
				}
			}
			
			foreach(string key in keys_to_remove){
				OccupationIndex[key].Remove();
				OccupationIndex.Remove(key);
			}

	
			//productivity 

			//the following few lines will cause massive performance downgrade
			string text_content = "";
			foreach(string key in Productivity.Keys ){
				text_content +=  " " + Lang.Current[key] +  ": " + Productivity[key] + "\n";
			}
			product_text.text = text_content;


		}
	}

	//render the technology buildings that can be clicked
	public void RenderTechnologies(){
		if (PageVillage.GetComponent<CanvasGroup> ().alpha > 0) {
			List<TechnologyConstruction> all_techs = Game.Current.Hero.UserConstructions.AllTechnologyConstructions;
			for (int i = 0; i < all_techs.Count; i ++) {
				TechnologyConstruction temp = all_techs [i];

				if(!TechnologyIndex.ContainsKey(temp.Type))
				{
					EnterTechnologyButtonView enter_button = new EnterTechnologyButtonView (TechnologyScrollContent.gameObject, temp.Name, i,
                                                               delegate {
					if (temp.Type.Equals (BuildingType.BLACK_SMITH)) {
						//black smith clicked
						//create and render the black smith view
						Transform black_smith = Instantiate (BlackSmithDialog) as Transform;
						black_smith.SetParent (GameObject.Find ("Canvas").transform);
						black_smith.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform black_smith_top_rect = black_smith.GetComponent<RectTransform> ();
						black_smith_top_rect.localScale = new Vector3 (1f, 1f, 1f);
						black_smith_top_rect.offsetMax = new Vector2 (0f, 0f);
						black_smith_top_rect.offsetMin = new Vector2 (0f, 0f);

						Game.Current.ActionEngine.RebuildActions("Weapon", black_smith.gameObject);
					}
					else if(temp.Type.Equals(BuildingType.IRON_COOK_POT)){
						//create and render cook pot
						Transform cook_pot = Instantiate (IronCookPotDialog) as Transform;
						cook_pot.SetParent (GameObject.Find ("Canvas").transform);
						cook_pot.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform cook_pot_top_rect = cook_pot.GetComponent<RectTransform> ();
						cook_pot_top_rect.localScale = new Vector3 (1f, 1f, 1f);
						cook_pot_top_rect.offsetMax = new Vector2 (0f, 0f);
						cook_pot_top_rect.offsetMin = new Vector2 (0f, 0f);
					}
					else if(temp.Type.Equals(BuildingType.GEAR_SHOP)){
						//create and render gear shop
						Transform gear_shop = Instantiate (GearShopDialog) as Transform;
						gear_shop.SetParent (GameObject.Find ("Canvas").transform);
						gear_shop.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform gear_shop_rect = gear_shop.GetComponent<RectTransform> ();
						gear_shop_rect.localScale = new Vector3 (1f, 1f, 1f);
						gear_shop_rect.offsetMax = new Vector2 (0f, 0f);
						gear_shop_rect.offsetMin = new Vector2 (0f, 0f);
						
						Game.Current.ActionEngine.RebuildActions("Armor",gear_shop.gameObject);
						Game.Current.ActionEngine.RebuildActions("Accessory",gear_shop.gameObject);
					}
					else if(temp.Type.Equals(BuildingType.RESEARCH_FACILITY)){
						//create and render research facility
						Transform research_shop = Instantiate (ResearchDialog) as Transform;
						research_shop.SetParent (GameObject.Find ("Canvas").transform);
						research_shop.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform research_shop_rect = research_shop.GetComponent<RectTransform> ();
						research_shop_rect.localScale = new Vector3 (1f, 1f, 1f);
						research_shop_rect.offsetMax = new Vector2 (0f, 0f);
						research_shop_rect.offsetMin = new Vector2 (0f, 0f);

						Game.Current.ActionEngine.RebuildActions("Skill", research_shop.gameObject);
					}

					else if(temp.Type.Equals(BuildingType.ELEMENT_HALL)){
						Transform element_hall = Instantiate (ElementHallDialog) as Transform;
						element_hall.SetParent (GameObject.Find ("Canvas").transform);
						element_hall.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform element_hall_rect = element_hall.GetComponent<RectTransform> ();
						element_hall_rect.localScale = new Vector3 (1f, 1f, 1f);
						element_hall_rect.offsetMax = new Vector2 (0f, 0f);
						element_hall_rect.offsetMin = new Vector2 (0f, 0f);
						
					}

                    else if(temp.Type.Equals(BuildingType.EVIL_CHAMBER)){
						Transform evil_chamber = Instantiate (EvilChamberDialog) as Transform;
						evil_chamber.SetParent (GameObject.Find ("Canvas").transform);
						evil_chamber.SetAsLastSibling (); // move the dialog to the front of the View
						RectTransform evil_chamber_rect = evil_chamber.GetComponent<RectTransform> ();
						evil_chamber_rect.localScale = new Vector3 (1f, 1f, 1f);
						evil_chamber_rect.offsetMax = new Vector2 (0f, 0f);
						evil_chamber_rect.offsetMin = new Vector2 (0f, 0f);
						
					}

						
				});

					TechnologyIndex.Add(temp.Type, enter_button);
			 }

			}
		}
	}

    private void RenderBrightness(){
        if (Game.Current.IsAtNight)
        {
            CloudingShadowRenderer.gameObject.SetActive(true);
            Color c = CloudingShadowRenderer.color;
            if (Game.Current.IsLightOn)
            {
                c.a = currentGame.Brightness * 0.7f;
            }
            else
            {
                c.a = currentGame.Brightness;
            }
            CloudingShadowRenderer.color = c;
        }
        else
        {
            CloudingShadowRenderer.gameObject.SetActive(false);
        }

		
	}


	private void SpinnClock(){
		int day_passed = (int)currentGame.GameTime / Config.SecondsPerDay;
		float time_of_day = currentGame.GameTime - day_passed * Config.SecondsPerDay;
		float time_percentage = time_of_day / Config.SecondsPerDay;

		ClockSpinner.rotation = Quaternion.Euler (0, 0, 45 - time_percentage * 360);
		DayCountText.text = "" + day_passed ;
        if(currentGame.CurrentGameMode == GameMode.Survial)
        {
            if(day_passed == 10)
            {
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.SURVIVAL_10_DAY);
            }

            if(day_passed == 30)
            {
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.SURVIVAL_30_DAY);

            }


            if (day_passed == 60)
            {
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.SURVIVAL_60_DAY);

            }

            if(day_passed == 100)
            {
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.SURVIVAL_100_DAY);
            }

        }
	}


    private void RenderInventory()
    {
        //List<Item> items = Game.Current.Hero.UserInvetory.getItems;
        List<Resource> resouces = currentGame.Hero.UserInventory.AllResources;
        List<Food> food = currentGame.Hero.UserInventory.AllFood;
        List<Tool> tools = currentGame.Hero.UserInventory.AllTools;
        if (pageStorage.GetComponent<CanvasGroup>().alpha == 1)
        {
            //only render when active 
            for (int i = 0; i < resouces.Count; i++)
            {
                //add the items to the view 
                Resource res = resouces[i];
                if (ResourceIndex.ContainsKey(res.Type))
                {
                    //update text
                    ResourceIndex[res.Type].UpdateText(res, i);
                }
                else
                {
                    //add new row
                    InventoryResourceRow resourceRow = new InventoryResourceRow(resourceScrollContent, res, i);
                    ResourceIndex.Add(res.Type, resourceRow);

                }
            }

            List<string> keys_remove = new List<string>();
            foreach (string key in ResourceIndex.Keys)
            {
                if (Game.Current.Hero.UserInventory.Get(key, "Resource") == null)
                {
                    //item not exist, destroy view 
                    keys_remove.Add(key);
                }
            }

            foreach (string key in keys_remove)
            {
                ResourceIndex[key].Remove();
                ResourceIndex.Remove(key);
            }



            if (foodScrollContent.activeSelf)
            {
                //only render when active 
                for (int i = 0; i < food.Count; i++)
                {
                    Food current_food = food[i];
                    if (FoodIndex.ContainsKey(current_food.Type))
                    {
                        //update text 
                        FoodIndex[current_food.Type].UpdateText(current_food, i);
                    }
                    else
                    {
                        //add new row
                        InventoryFoodRow foodRow = new InventoryFoodRow(current_food, i, foodScrollContent.transform);
                        FoodIndex.Add(current_food.Type, foodRow);
                    }
                }

                List<string> keys_to_remove = new List<string>();
                foreach (string key in FoodIndex.Keys)
                {
                    if (Game.Current.Hero.UserInventory.Get(key, "Food") == null)
                    {
                        //item not exist, destroy view 
                        keys_to_remove.Add(key);
                    }
                }

                foreach (string key in keys_to_remove)
                {
                    FoodIndex[key].Remove();
                    FoodIndex.Remove(key);
                }

            }

            if (toolScrollContent.activeSelf)
            {
                //only render when active 
                TypeToToolList.Clear();
                for (int i = 0; i < tools.Count; i++)
                {
                    //add the items to the view 
                    Tool current_tool = tools[i];
                    /*
                    if (ToolIndex.ContainsKey(current_tool.UniqueKey))
                    {
                        //update tool row
                        ToolIndex[current_tool.UniqueKey].UpdateToolRow(current_tool, i);
                    }
                    else
                    {
                        //add new row
                        InventoryToolRow toolRow = new InventoryToolRow(toolScrollContent.transform, current_tool, i);
                        ToolIndex.Add(current_tool.UniqueKey, toolRow);
                    }
                    */

                    if (TypeToToolList.ContainsKey(current_tool.Type))
                    {
                        TypeToToolList[current_tool.Type].Add(current_tool);
                    }
                    else
                    {
                        TypeToToolList[current_tool.Type] = new List<Tool>();
                        TypeToToolList[current_tool.Type].Add(current_tool);
                    }

                }

                for(int i = 0; i < TypeToToolList.Count; i++)
                {
                    string key = TypeToToolList.ElementAt(i).Key;
                    if (ToolIndex.ContainsKey(key))
                    {
                        //alreay have this type of weapon in the inventory
                        ToolIndex[key].UpdateToolRow(TypeToToolList[key], i);
                    }
                    else
                    {
                        //add new type of weapons
                        InventoryToolRow toolRow = new InventoryToolRow(toolScrollContent.transform, TypeToToolList[key], i);
                        ToolIndex.Add(key, toolRow);
                    }
                }
                

                List<string> keys_to_remove = new List<string>();
                foreach (string key in ToolIndex.Keys)
                {
                    if (!Game.Current.Hero.UserInventory.ContainsToolOfType(key))
                    {
                        //item type not exist, destroy view 
                        keys_to_remove.Add(key);
                    }
                }

                foreach (string key in keys_to_remove)
                {
                    ToolIndex[key].Remove();
                    ToolIndex.Remove(key);
                }
            }

        }


    }

    private void RenderUtility()
    {
        RenderHealth();
        RenderCurrentTool();
        RenderFire();
        RenderBrightness();
        RenderDayNightSound();
        RenderTimedStates();
    }


	private void RenderCurrentTool(){
		//show what tool is equipped
		if (currentGame.Hero.EquippedTool != null) {
			tool_text.text = currentGame.Hero.EquippedTool.Name + Mathf.Round(currentGame.Hero.EquippedTool.Remaining * 100) + "%";
		} else {
			tool_text.text = Lang.Current["tool"];
		}

		if (currentGame.Hero.Equipment != null) {
			equipment_text.text = currentGame.Hero.Equipment.Name + Mathf.Round(currentGame.Hero.Equipment.Remaining * 100) + "%";
		} else {
			equipment_text.text = Lang.Current["equipment"];
		}

	}


	private void RenderFire(){
		//render can calculate fire durability when torch is on
		if (Game.Current.IsLightOn && Game.Current.IsAtNight
            && CloudingShadowRenderer.gameObject.activeSelf) {
            //light is on
            FireLight.gameObject.SetActive(true);
			fire_light_animator.SetBool("FireLight", true);
			if(!fire_light_audio.isPlaying)
				fire_light_audio.Play();
		
		}else {
            //light is off
            if (FireLight.gameObject.activeSelf)
            {
                fire_light_animator.SetBool("FireLight", false);
                if (fire_light_audio.isPlaying)
                    fire_light_audio.Stop();
                FireLight.gameObject.SetActive(false);
            }
        }
	}


	private void RenderHealth(){
		float currentHealth = Game.Current.Hero.CurrentHealth;
		float totalHealth = Game.Current.Hero.HealthUpperLimit;
		//text 
		HealthBarText.text = "" + Mathf.RoundToInt(currentHealth);
        //image 
        HealthBarImage.fillAmount = currentHealth / totalHealth;
        if (Game.Current.Hero.CurrentHealth <= Game.Current.Hero.HealthUpperLimit * 0.2f)
        {
            HealthBarGlow.gameObject.SetActive(true);
        }
        else
        {
            HealthBarGlow.gameObject.SetActive(false);
        }
    }



	private void ReadStoryFromQueue(){
		if (currentGame.Logs.Count > 0) {
			string latest_story = currentGame.Logs [0];
			currentGame.Logs.RemoveAt (0);
            StoryPopUpSound.Play();
            Transform text_clone = Instantiate (StoryTextPrefab) as Transform;
			RectTransform rf = text_clone.GetComponent<RectTransform> ();
			text_clone.GetComponent<Text> ().text = latest_story;
			StartCoroutine(RearangeStoryText(rf,text_clone));
		
		}

	}

    //recover the stories when
    private void RecoverBackUpStories()
    {
        for (int i = 0; i < currentGame.BackUpLogs.Count; i++)
        {
            Transform text_clone = Instantiate(StoryTextPrefab) as Transform;
            RectTransform rf = text_clone.GetComponent<RectTransform>();
            text_clone.GetComponent<Text>().text = currentGame.BackUpLogs[i];
            StartCoroutine(RearangeStoryText(rf, text_clone));
        }
    }


	IEnumerator RearangeStoryText(RectTransform rf, Transform text_clone){
		yield return new WaitForEndOfFrame();
		//shift all the stories down by the height of the inserted story
		for(int i = 0 ; i < StoryScrollContent.childCount ; i ++){
			Transform child = StoryScrollContent.GetChild(i);
			float original_y =  child.GetComponent<RectTransform>().anchoredPosition3D.y;
			child.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,original_y - rf.rect.height, 0);
		}
		
		text_clone.SetParent(StoryScrollContent);
		text_clone.SetAsLastSibling ();
		rf.localScale = new Vector3(1f,1f,1f);
		rf.anchoredPosition3D = new Vector3(0,-20f, 0);
		//need to remove the oldest child when the story list is too long

		if (StoryScrollContent.childCount >= 10) {
			Destroy(StoryScrollContent.GetChild(0).gameObject);
		}
	}


	private void ReadToastFromQueue(){
		if(currentGame.Toasts.Count > 0){
			//Toast the newest toast
			string toast_content = currentGame.Toasts[0];
			currentGame.Toasts.RemoveAt(0);
			toast_manager.ToastMessage(toast_content);
		}
	}


    private void ReadToastFromQueue2()
    {
        if (currentGame.Toasts.Count > 0)
        {
           for(int i=0; i < Toasts.Count; i++)
            {
                GameObject toast = Toasts[i];
                if (!toast.activeSelf)
                {
                    string toast_content = currentGame.Toasts[0];
                  //  toast.GetComponent<Text>().text = toast_content;
                    toast.SetActive(true);
                    toast.GetComponent<Text>().text = toast_content;
                    currentGame.Toasts.RemoveAt(0);

                    return;
                }
            }
        }
    }




    public void ShowGearList(string gear_type){
		Transform clone = Instantiate (GearListPrefab) as Transform;
		clone.SetParent (GameObject.Find("Canvas").transform);
		clone.SetAsLastSibling ();
		RectTransform rf = clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.offsetMin = new Vector2 (0, 0);
		rf.offsetMax = new Vector2 (0, -140f);
		GearListController glc = clone.FindChild ("GearScrollView")
			.FindChild ("GearScrollContent").GetComponent<GearListController> ();
		glc.GearType = gear_type;
		glc.renderGearList();

	}


	public void ShowMapList(){
		Transform clone = Instantiate (MapListPrefab) as Transform;
		clone.SetParent (GameObject.Find("Canvas").transform);
		clone.SetAsLastSibling ();
		RectTransform rf = clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.offsetMin = new Vector2 (0, 0);
		rf.offsetMax = new Vector2 (0, -140f);
		MapListController mlc = clone.FindChild ("MapScrollView")
			.FindChild ("MapScrollContent").GetComponent<MapListController> ();
		mlc.RenderMapList();
	}


	public void ShowSkillList(){
		Transform clone = Instantiate (SkillListPrefab) as Transform;
		clone.SetParent (GameObject.Find("Canvas").transform);
		clone.SetAsLastSibling ();
		RectTransform rf = clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.offsetMin = new Vector2 (0, 0);
		rf.offsetMax = new Vector2 (0, -140f);
		SkillListController slc = clone.FindChild ("SkillScrollView")
			.FindChild ("SkillScrollContent").GetComponent<SkillListController> ();
		slc.RenderSkillList ();
	}


	public void ShowFoodList(){
		Transform clone = Instantiate (FoodListPrefab) as Transform;
		clone.SetParent (GameObject.Find("Canvas").transform);
		clone.SetAsLastSibling ();
		RectTransform rf = clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.offsetMin = new Vector2 (0, 0);
		rf.offsetMax = new Vector2 (0, -140f);
		FoodListController flc = clone.FindChild ("FoodScrollView")
			.FindChild ("FoodScrollContent").GetComponent<FoodListController> ();
		flc.RenderFoodList ();
		
	}



	public void RenderClimate(){
	
		if (Game.Current.Hero.UserConstructions.Has (BuildingType.WEATHER_INSTRUMENT, "AcademicConstruction")) {

			temparature_text.text ="T:" + Mathf.RoundToInt(Game.Current.Hero.UserClimate.Tempature) + "°";
			humidity_text.text = "H:" + Mathf.RoundToInt(Game.Current.Hero.UserClimate.Humidity) + "";
			WeatherPanelCanvasGroup.alpha = 1;

		} else {
            WeatherPanelCanvasGroup.alpha = 0;    
        }

		switch (Game.Current.Hero.UserClimate.WeatherToday) {
		case Weather.Sunny:
			weather_icon.sprite =  Sunny;
			RainParticle.gameObject.SetActive(false);
			SnowParticle.gameObject.SetActive(false);
			FogPrefab.gameObject.SetActive(false);
			break;
		case Weather.Rain:
			weather_icon.sprite = Rain;
			if(!RainParticle.gameObject.activeSelf){
				RainParticle.gameObject.SetActive(true);
				RainParticle.GetComponent<AudioSource>().Play();
			}
			SnowParticle.gameObject.SetActive(false);
			FogPrefab.gameObject.SetActive(false);
			break;
		case Weather.Snow:
			weather_icon.sprite = Snow;
			RainParticle.gameObject.SetActive(false);
			if(!SnowParticle.gameObject.activeSelf){
				SnowParticle.gameObject.SetActive(true);
				SnowParticle.GetComponent<AudioSource>().Play();
			}
			FogPrefab.gameObject.SetActive(false);
			break;
		case Weather.Fog:
			weather_icon.sprite = Rain;
			RainParticle.gameObject.SetActive(false);
			SnowParticle.gameObject.SetActive(false);
			if(!FogPrefab.gameObject.activeSelf){
				FogPrefab.gameObject.SetActive(true);
				FogPrefab.GetComponent<AudioSource>().Play();
			}
			break;

		}

	}


    
	private void RenderTimedStates(){
		string states_value = "";
        Dictionary<string, GlobalState> states = Game.Current.Hero.UserGlobalStateIndex;
        foreach(string key in states.Keys)
        {
            states_value += states[key].Name + "\n";

        }
		states_text.text = states_value;
	}
    


    public void goShopping()
    {
        Game.Current.ActionEngine.DestroyAllViewIndexing();
        Game.Current.IsAtHome = false;
        LoadingScreen.SetActive(true);
        StartCoroutine(loadLevelInBG(5));
    }

    public void goBackLoading()
    {
        //save and quit the game
        Game.Current.SaveGameInMainThread();
        Game.Current.ActionEngine.DestroyAllViewIndexing();
       // Game.Current.IsAtHome = false;
        LoadingScreen.SetActive(true);
        StartCoroutine(loadLevelInBG(0));
    }



    public void goAdeventure(){
        MainCharacter hero = Game.Current.Hero;
		if (hero.CurrentEquippedMap != null){
            if (!Game.Current.IsAtNight)
            {
                goOutWithCondition();
            }
            else
            {
                if (Game.Current.Hero.hasTimedState(StateType.STATE_ELF_LIGHT))
                {

                    goOutWithCondition();
                }
                else
                {
                    Game.Current.AddToast(Lang.Current["cant_go_out_at_night"]);
                }
            }
        } else {
			Game.Current.AddToast(Lang.Current["please_choose_map"]);
		}
	}


    private void goOutWithCondition()
    {
        MainCharacter hero = Game.Current.Hero;
        if (hero.UserClimate.WeatherToday == Weather.Rain
                  && !hero.isItemEquipped(ItemType.UMBRELLA))
        {
            Game.Current.AddToast(Lang.Current["please_take_umbrella"]);
            return;
        }


        if (hero.UserClimate.WeatherToday == Weather.Snow
            && !hero.isItemEquipped(ItemType.WOOLGLOVE))
        {
            Game.Current.AddToast(Lang.Current["please_take_wool_glove"]);
            return;
        }


        if (hero.UserClimate.Tempature >= 30
            && hero.UserClimate.theSeason == Season.Summer
            && !hero.isItemEquipped(ItemType.FISH_MADE_FAN))
        {
            Game.Current.AddToast(Lang.Current["need_take_fan"]);
            return;
        }

        //remove water ever equipment
        Game.Current.Hero.Equipment = null;
        //if there is a torch remove it

        if (Game.Current.Hero.EquippedTool != null)
        {
            if (Game.Current.Hero.EquippedTool.Type.Equals(ItemType.TORCH))
            {
                Game.Current.Hero.EquippedTool = null;
            }
        }


        Game.Current.ActionEngine.DestroyAllViewIndexing();
        Game.Current.IsAtHome = false;
        CancelInvoke(); // stop all timers and events when loading
        LoadingScreen.SetActive(true);
        StartCoroutine(loadLevelInBG(2));
        Game.Current.SaveGame(); // for cases where you could quit in battle
    }
  

    IEnumerator loadLevelInBG(int level)
    {

        async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            yield return null;
        }
    }

    void OnApplicationPause(bool pauseStatus){
        if(!isBossPopUp)
            Game.Current.SaveGameInMainThread();
	}


    private void LoadBattle(){
		Game.Current.ActionEngine.DestroyAllViewIndexing();
        SceneManager.LoadScene(3);
	}

    public void ToggleSetting()
    {
        if (SettingPanelAnimator.GetBool("isOpen"))
        {
            SettingPanelAnimator.SetBool("isOpen", false);
        }
        else
        {
            SettingPanelAnimator.SetBool("isOpen", true);
        }
    }
   

}
