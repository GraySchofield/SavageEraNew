using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour {
	public float blockSize = 1.6f;
	private int MapSize;
	private Vector2 current_index;
	private Vector2 current_position;
	public Transform WarFog; //the war fog object

	private WarFogMaster fog_master;
	private Map currentMap;

	public Transform mainCamera; // used to let the camera follow the player


	//Monster Related stuffs
	public Transform monsterPrefab; //the prefab used to create monsters, may have different prefab latter, now use this to test
	public Transform MapObjects;

	private Dictionary<int,GameObject> itemObjects;
    private Dictionary<int, GameObject> eventObjects;
    [HideInInspector]
    public GameObject CurrentEventObject;

	//some UI componets
	public Transform ToastPanel;
	private ToastManager toast_manager;
	public Transform ClockSpinner;

	public Transform HealthBar;
	private Text HealthBarText;
	private RectTransform HealthBarRect;
	public GameObject Rain;
	public GameObject Fog;
	public GameObject Snow;

	private bool isMoving = false; //used to control the moving state, so you can't turn to other direction while already moving in some direction
	private bool hasMetRandomMonster = false; //stop you from moving when you meet a monster

    private AsyncOperation async;
    public GameObject LoadingScreen;
    public Slider LoadingBar;

    public Text WeaponText;
    public Text ArmorText;
    public Text AccessoryText;

    public Text StateText;

    public GameObject Frozen;
    public GameObject Heatstroke;
    public AudioSource FootStep;

    public Text TeleportCount;

    public Text current_equiped_tool_text;
    public Transform FireLight;
    public GameObject FireLightEffect;
    private Animator fire_light_animator;
    private SpriteRenderer light_shadow_render;
    private AudioSource fire_light_audio;
    public AudioSource BGM;

    [HideInInspector]
    public bool isGamePaused = false;


    /*
    void Awake(){
		Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}
    */

    void Start(){
		itemObjects = new Dictionary<int,GameObject> ();
        eventObjects = new Dictionary<int, GameObject>();
        fog_master = WarFog.GetComponent<WarFogMaster> ();
		toast_manager = ToastPanel.GetComponent<ToastManager> ();
		HealthBarText = HealthBar.FindChild ("HealthText").GetComponent<Text> ();
		HealthBarRect = HealthBar.FindChild ("HealthQty").GetComponent<RectTransform> ();
        fire_light_animator = FireLightEffect.GetComponentInChildren<Animator>();
        light_shadow_render = FireLightEffect.GetComponent<SpriteRenderer>();
        fire_light_audio = FireLight.GetComponent<AudioSource>();

        //load some map data from model
        currentMap = Game.Current.Hero.CurrentEquippedMap;
		MapSize = currentMap.MapSize;
		fog_master.generateWarFog (currentMap.WarFogIndexs);
		current_index = new Vector2 (currentMap.PlayerX, currentMap.PlayerY);
		current_position = MapIndexing.getPositionFromIndex ((int)current_index.x,(int)current_index.y, blockSize, MapSize);
		this.transform.position = new Vector3 (current_position.x, current_position.y, 0); //move player to its initial place

		//originally reaveal the sight of the player
		fog_master.UpdateFogWithPlayer (transform.position, Game.Current.Hero.MapSight * blockSize);

        //create the map base
        GameObject mapBase = Instantiate(Resources.Load("Prefab/Map/" + currentMap.PrefabName)) as GameObject;
        float pos = currentMap.MapSize / 2 * blockSize;
        mapBase.transform.position = new Vector3(-pos, pos, 0);



        //Create monsters from the model and put them into the map
        Dictionary<int, List<Monster>> AllMonsters = currentMap.MonstersInMap;
	
		foreach (int key in AllMonsters.Keys) {
			List<Monster> monsters = AllMonsters[key];
			int monster_x = key % MapSize;
			int monster_y = key / MapSize;
			Vector2 monster_pos = MapIndexing.getPositionFromIndex ((int)monster_x,(int)monster_y, blockSize, MapSize);
            GameObject monster_clone;
            if (monsters[0].isAlive){
                try
                {
                    monster_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/" + monsters[0].Type)) as GameObject;
                }
                catch (Exception e)
                {
                    monster_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/MonsterTypeOne")) as GameObject;
                    //Debug.LogError(e.Message);
                }
                monster_clone.transform.position = new Vector3(monster_pos.x, monster_pos.y, 0f);
                monster_clone.transform.SetParent(MapObjects);
            }
            /*
			if(!monsters[0].isAlive){
				monster_clone.GetComponent<SpriteRenderer>().color = new Color(0.259f, 0.259f, 0.259f);
			}*/
		}

		//create home
		Vector2 home_pos  = MapIndexing.getPositionFromIndex ((int)(MapSize/2),(int)(MapSize/2), blockSize, MapSize);
		GameObject home_clone = GameObject.Instantiate (Resources.Load ("Prefab/MapItems/home")) as GameObject;
		home_clone.transform.position =  new Vector3(home_pos.x, home_pos.y, 0f);
		home_clone.transform.SetParent(MapObjects);

		//create items
		Dictionary<int, Item> AllItems = currentMap.ItemsInMap;
		foreach (int key in AllItems.Keys) {
			int item_x = key % MapSize;
			int item_y = key / MapSize;
			Vector2 item_pos = MapIndexing.getPositionFromIndex ((int)item_x,(int)item_y, blockSize, MapSize);
			
			GameObject item_clone;
            Item item = AllItems[key];
            if(item is Food)
            {
                item_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/general_food")) as GameObject;

            }
            else
            {
                item_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/general")) as GameObject;

            }

            //try{
            //		item_clone = GameObject.Instantiate (Resources.Load ("Prefab/MapItems/" + AllItems[key].Type)) as GameObject;
            //}catch(Exception e){
            //item_clone = GameObject.Instantiate (Resources.Load ("Prefab/MapItems/general"))as GameObject;
			//}
			item_clone.transform.position =  new Vector3(item_pos.x, item_pos.y, 0f);
			item_clone.transform.SetParent(MapObjects);
			itemObjects.Add(key, item_clone);
		}

		//create events 
		Dictionary<int, MapEvent> AllEvents = currentMap.EventsInMap;
		foreach (int key in AllEvents.Keys){
            if (!AllEvents[key].IsFinished)
            {
                int event_x = key % MapSize;
                int event_y = key / MapSize;
                Vector2 event_pos = MapIndexing.getPositionFromIndex((int)event_x, (int)event_y, blockSize, MapSize);

                GameObject event_clone;
                try
                {
                    event_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/" + AllEvents[key].theStory.Type)) as GameObject;
                }
                catch (Exception e)
                {
                    event_clone = GameObject.Instantiate(Resources.Load("Prefab/MapItems/event")) as GameObject;
                }

                event_clone.transform.position = new Vector3(event_pos.x, event_pos.y, 0f);
                event_clone.transform.SetParent(MapObjects);
                eventObjects.Add(key, event_clone);
            }
		}
		
		List<int> AllBlocks = currentMap.MapBlocks;
		foreach (int index in AllBlocks) {
			int block_x = index % MapSize;
			int block_y = index / MapSize;
			Vector2 block_pos = MapIndexing.getPositionFromIndex ((int)block_x,(int)block_y, blockSize, MapSize);
			
			GameObject block_clone;
			
			block_clone = GameObject.Instantiate (Resources.Load ("Prefab/MapItems/block")) as GameObject;
			
			block_clone.transform.position =  new Vector3(block_pos.x, block_pos.y, 0f);
			block_clone.transform.SetParent(MapObjects);
		}


		//start all ui rendering routines
		InvokeRepeating ("ReadToastFromQueue", 0, 0.5f);
		InvokeRepeating ("RenderUI", 0, 0.5f);
		InvokeRepeating ("RenderWeather", 0, 1f);
        InvokeRepeating ("UpdateContents", 0, 0.5f);
        InvokeRepeating ("UpdateHints", 0, 2f);


        if (currentMap.Type.Equals(MapType.SWAMP))
        {
            BGM.clip = Resources.Load("Audio/map_swap_bgm") as AudioClip;
        }
        BGM.Play();

    }


    private bool isBlocked(int x, int y){
		int idx = x + y * currentMap.MapSize;
		if (currentMap.MapBlocks.Contains (idx)) {
			return true;
		} else {
			return false;
		}
	}


    public void MovingLeft()
    {
        InvokeRepeating("moveLeft", 0, 0.5f);
    }

    public void StopMovingLeft()
    {
        CancelInvoke("moveLeft");
    }

    public void MovingRight()
    {
        InvokeRepeating("moveRight", 0, 0.5f);
    }

    public void StopMovingRight()
    {
        CancelInvoke("moveRight");
    }


    public void MovingUp()
    {
        InvokeRepeating("moveUp", 0, 0.5f);
    }

    public void StopMovingUp()
    {
        CancelInvoke("moveUp");
    }

    public void MovingDown()
    {
        InvokeRepeating("moveDown", 0, 0.5f);
    }

    public void StopMovingDown()
    {
        CancelInvoke("moveDown");
    }


    public void moveLeft(){
		if (current_index.x > 0 && !isMoving && !hasMetRandomMonster
		    && !isBlocked((int)current_index.x -1 ,(int)current_index.y)) {
			isMoving = true;
            iTween.MoveBy (gameObject, iTween.Hash ("x", -blockSize, "easeType", "linear", 
		                                      "delay", 0, "time", 0.3, "oncomplete", "UpdatePositionState"));
		}
	}
	
	public void moveRight(){
		if (current_index.x < MapSize - 1 && !isMoving && !hasMetRandomMonster
		    && !isBlocked((int)current_index.x + 1 ,(int)current_index.y)) {
			isMoving = true;
			iTween.MoveBy (gameObject, iTween.Hash ("x", blockSize, "easeType", "linear", 
		                                      "delay", 0, "time", 0.3, "oncomplete", "UpdatePositionState"));
		}
	}

	public void moveUp(){
		if (current_index.y < MapSize - 1 && !isMoving && !hasMetRandomMonster
		    && !isBlocked((int)current_index.x, (int)current_index.y + 1)) {
			isMoving = true;
			iTween.MoveBy (gameObject, iTween.Hash ("y", blockSize, "easeType", "linear", 
		                                      "delay", 0, "time", 0.3, "oncomplete", "UpdatePositionState"));	
		}
	}

	public void moveDown(){
		if (current_index.y > 0 && ! isMoving && !hasMetRandomMonster
		    && !isBlocked((int)current_index.x, (int)current_index.y - 1 )) {
			isMoving = true;
			iTween.MoveBy (gameObject, iTween.Hash ("y", -blockSize, "easeType", "linear", 
		                                        "delay", 0, "time", 0.3, "oncomplete", "UpdatePositionState"));
		}
	}


    private void CancelMoving()
    {
        CancelInvoke("moveRight");
        CancelInvoke("moveLeft");
        CancelInvoke("moveUp");
        CancelInvoke("moveDown");

    }


    private void UpdatePositionState(){
        FootStep.Play();
        isMoving = false;
		current_position = new Vector2 (transform.position.x, transform.position.y);
		current_index = MapIndexing.getIndexFromPosition (current_position.x, current_position.y, blockSize, 50);
		currentMap.PlayerX = (int)current_index.x;
		currentMap.PlayerY = (int)current_index.y;
	//	Debug.Log ("position  x:" + current_position.x + "  y:" + current_position.y);
		//Debug.Log ("index  x:" + currentMap.PlayerX + "  y:" + currentMap.PlayerY );
		mainCamera.GetComponent<CameraFollowMe> ().isDragging = false;
		fog_master.UpdateFogWithPlayer (transform.position, Game.Current.Hero.MapSight * blockSize); //update war fog
		int index_key = currentMap.PlayerX + currentMap.PlayerY * MapSize;
		bool isAnythingOnMap = false;

		//check if there is a monster at the current position, if so lauch a battle
		if (currentMap.MonstersInMap.ContainsKey (index_key)) {
            isAnythingOnMap = true;
            
            List<Monster> current_monsters = currentMap.MonstersInMap[index_key];
			//launch battle
			Game.Current.Hero.CurrentBattleMonsters = current_monsters;
			if(current_monsters[0].isAlive){
                //only lunch battle when the monster is still alive
                CancelInvoke();
                SceneManager.LoadScene(3,LoadSceneMode.Single);
            }

		}


		//check if there is an event at the current position, if so show the event 
		if (currentMap.EventsInMap.ContainsKey (index_key)) {
			isAnythingOnMap = true;
			MapEvent map_event = currentMap.EventsInMap[index_key];
			if(!map_event.IsFinished){
                CancelMoving();
                Game.Current.Hero.CurrentMapEvent = map_event;
				map_event.theStory.JustRunStory();
                CurrentEventObject = eventObjects[index_key];
            }
		}


		//check it there is an item at the current position, if so pick it
		if (currentMap.ItemsInMap.ContainsKey (index_key)) {
			isAnythingOnMap = true;
			Item current_item = currentMap.ItemsInMap[index_key];
			Game.Current.Hero.gains(current_item);
			currentMap.ItemsInMap.Remove(index_key);
			Destroy(itemObjects[index_key]);			
		}

		//check if went home
		if (currentMap.PlayerX == (int)MapSize / 2 && currentMap.PlayerY == (int)MapSize / 2) {
            isAnythingOnMap = true;
            //go home 
            Game.Current.SaveGame(); // for cases where you could quit in battle
            LoadWithLoadingScreen(1);
        }


		if (!isAnythingOnMap) {
            //nothing on this spot, probabily generate a random monster
            float chance = 0.08f;
            if (Game.Current.Hero.hasTimedState(StateType.STATE_EVIL))
            {
                chance *= 2;
            }
            if (Game.Current.Hero.hasTimedState(StateType.STATE_ELF_LIGHT))
            {
                chance /= 2;
            }
			if(UnityEngine.Random.value  <= chance){  //0.04
				//create the random monster here
				List<Monster> random_monsters = new List<Monster>();
				int key = Math.Abs(currentMap.PlayerX - currentMap.MapSize /2) + Math.Abs(currentMap.PlayerY - currentMap.MapSize/2);
				Monster mon = MonsterBuilder.BuildMonster(currentMap.GetRandomMonsterFromDist(key));
				random_monsters.Add(mon);
				Game.Current.Hero.CurrentBattleMonsters = random_monsters;
				Game.Current.AddToast(Lang.Current["run_into_monster"]);
				//load battle by delay of 1 second
				hasMetRandomMonster = true;
				Invoke("LoadBattle",1);
			}
		}


	}


    
    public void TeleportHome()
    {
        Shop shop = Shop.Current;
        if (shop.UseProduct(ItemType.TELEPORT, 1))
        {
            //successful teleport
            Achievement.Current.UnlockAchievement(Achievement.AchievementType.TELEPORT);
            currentMap.PlayerX = (int)MapSize / 2;
            currentMap.PlayerY = (int)MapSize / 2;
            LoadWithLoadingScreen(1); 
        }
        else
        {
            Game.Current.AddToast(Lang.Current[ItemType.TELEPORT] + Lang.Current["not_enough"]);
        }
    }


    public void LoadWithLoadingScreen(int level)
    {
        CancelInvoke();
        LoadingScreen.SetActive(true);
        StartCoroutine(loadLevelInBG(level));
    }


    IEnumerator loadLevelInBG(int level)
    {

        async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            yield return null;
        }

       
    }


    private void LoadBattle(){

		hasMetRandomMonster = false;
        CancelInvoke();
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }


    //run every 0.5 seconds
    private void UpdateContents()
    {
        if (!isGamePaused)
        {
            Game.Current.GameTime += 0.5f;
            SpinnClock();
        }

        if(Game.Current.Hero.Gears.EquippedWeapon != null)
        {
            WeaponText.text = Game.Current.Hero.Gears.EquippedWeapon.Name + " " +
               Math.Round(Game.Current.Hero.Gears.EquippedWeapon.Remaining * 100,1) + "%";
            switch (Game.Current.Hero.Gears.EquippedWeapon.Tier)
            {
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
        else
        {
            WeaponText.text = Lang.Current["none"];
        }

        if (Game.Current.Hero.Gears.EquippedArmor != null)
        {
            ArmorText.text = Game.Current.Hero.Gears.EquippedArmor.Name + " " +
                 Math.Round(Game.Current.Hero.Gears.EquippedArmor.Remaining * 100,1) + "%";
            switch (Game.Current.Hero.Gears.EquippedArmor.Tier)
            {
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
        else
        {
            ArmorText.text = Lang.Current["none"];
        }

        if (Game.Current.Hero.Gears.EquippedAccessory != null)
        {
            AccessoryText.text = Game.Current.Hero.Gears.EquippedAccessory.Name + " " +
                 Math.Round(Game.Current.Hero.Gears.EquippedAccessory.Remaining * 100,1) + "%";
            switch (Game.Current.Hero.Gears.EquippedAccessory.Tier)
            {
                case 1:
                    AccessoryText.color = Config.uncommonColor;
                    break;

                case 2:
                    AccessoryText.color = Config.rareColor;
                    break;

                case 3:
                    AccessoryText.color = Config.legendColor;
                    break;
            }
        }
        else
        {
            AccessoryText.text = Lang.Current["none"];
        }


        if (Shop.Current.HasProduct(ItemType.TELEPORT))
        {
            TeleportCount.text = "" + Shop.Current.GetProduct(ItemType.TELEPORT).Count;
        }
        else
        {
            TeleportCount.text = "0";

        }
    }

    //run every 2 seconds
    private void UpdateHints()
    {
        int time_of_day = (int)Game.Current.GameTime % Config.SecondsPerDay; //how many seconds passed within this day

        if (time_of_day >= Config.SecondsPerDay * 0.53f && time_of_day <= Config.SecondsPerDay * 0.59f)
        {
            Game.Current.AddToast(Lang.Current["hint_night_comming"]);
        }
       
    }
	

	private void ReadToastFromQueue(){
		if(Game.Current.Toasts.Count > 0){
			//Toast the newest toast
			string toast_content = Game.Current.Toasts[0];
			Game.Current.Toasts.RemoveAt(0);
			toast_manager.ToastMessageInMap(toast_content);
		}
	}


	private void SpinnClock(){
		int day_passed = (int)Game.Current.GameTime / Config.SecondsPerDay;
		float time_of_day = Game.Current.GameTime - day_passed * Config.SecondsPerDay;
		float time_percentage = time_of_day / Config.SecondsPerDay;
		
		ClockSpinner.rotation = Quaternion.Euler (0, 0, 45 - time_percentage * 360);
	}


	private void RenderUI(){
		float currentHealth = Game.Current.Hero.CurrentHealth;
		float totalHealth = Game.Current.Hero.HealthUpperLimit;
		//text 
		HealthBarText.text = "" + Mathf.RoundToInt(currentHealth);
		//image 
		HealthBarRect.offsetMax = new Vector2 (-15f, -20f -  (1f - (currentHealth / totalHealth)) * 70f);
        RenderTimedStates();
        RenderEquippedTools();
        RenderFire();
        RenderBrightness();
    }


    private void RenderEquippedTools()
    {
        if (Game.Current.Hero.EquippedTool != null) {
            current_equiped_tool_text.text = Game.Current.Hero.EquippedTool.Name + Mathf.Round(Game.Current.Hero.EquippedTool.Remaining * 100) + "%";
        } else {
            current_equiped_tool_text.text = Lang.Current["tool"];
        }
    }


	private void RenderWeather(){
		switch (Game.Current.Hero.UserClimate.WeatherToday) {
		case Weather.Sunny:
			Rain.SetActive(false);
			Snow.SetActive(false);
			Fog.SetActive(false);
			break;
		case Weather.Rain:
			if(!Rain.activeSelf){
				Rain.SetActive(true);
				Rain.GetComponent<AudioSource>().Play();
			}
			Snow.SetActive(false);
			Fog.SetActive(false);
			break;
		case Weather.Snow:
			Rain.SetActive(false);
			if(!Snow.activeSelf){
				Snow.SetActive(true);
				Snow.GetComponent<AudioSource>().Play();
			}
			Fog.SetActive(false);
			break;
		case Weather.Fog:
			Rain.SetActive(false);
			Snow.SetActive(false);
			if(!Fog.activeSelf){
				Fog.SetActive(true);
			}
			break;
		}
	}


    private void RenderTimedStates()
    {
        string states_value = "";
        Dictionary<string, GlobalState> states = Game.Current.Hero.UserGlobalStateIndex;
        foreach (string key in states.Keys)
        {
            states_value += states[key].Name + "\n";

        }
        StateText.text = states_value;
    }



    private void RenderBrightness()
    {
        if (Game.Current.IsAtNight)
        {
            FireLightEffect.SetActive(true);
            if (Game.Current.IsLightOn)
            {
                light_shadow_render.color = new Color(light_shadow_render.color.r, light_shadow_render.color.g, light_shadow_render.color.b, Game.Current.Brightness * 0.7f);
            }
            else
            {
                light_shadow_render.color = new Color(light_shadow_render.color.r, light_shadow_render.color.g, light_shadow_render.color.b, Game.Current.Brightness);
            }
        }
        else
        {
            light_shadow_render.gameObject.SetActive(false);
        }
    }




    private void RenderFire()
    {
        //render can calculate fire durability when torch is on
        if (Game.Current.IsLightOn)
        {
            if (!fire_light_audio.isPlaying)
                fire_light_audio.Play();
        }
        else
        {
            if (fire_light_audio.isPlaying)
                fire_light_audio.Stop();
        }

        if (Game.Current.IsLightOn && Game.Current.IsAtNight)
        {
            //light is on
            FireLight.gameObject.SetActive(true);
            fire_light_animator.SetBool("FireLight", true);        
        }
        else
        {
            //light is off
            if (FireLight.gameObject.activeSelf)
            {
                fire_light_animator.SetBool("FireLight", false);
             
                FireLight.gameObject.SetActive(false);
            }
        }
    }



    void OnApplicationPause(bool pauseStatus) {
        Game.Current.SaveGameInMainThread();
	}

	/*
	void OnApplicationQuit() {
        Game.Current.SaveGameInMainThread();
	}
    */

    private void RenderHealthState()
    {

        if (Game.Current.Hero.hasTimedState(StateType.STATE_HEATSTROKE))
        {
            Heatstroke.SetActive(true);
        }
        else
        {
            Heatstroke.SetActive(false);
        }


        if (Game.Current.Hero.hasTimedState(StateType.STATE_FROZEN))
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


}
