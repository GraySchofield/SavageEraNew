using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainCharacter : Character {	
	
	//the constructor
	public MainCharacter(){
		Name = Config.DefaultHeroName;
		HealthUpperLimit = Config.InitialHealthUpperLimit;
		CurrentHealth = HealthUpperLimit;

		UserInventory = new Inventory();
		this.UserConstructions = new Constructions ();
		this.UserClimate = new Climate ();
		MyPopulation = new Population ();
	//  MyHouse = (House)BuildingFactory.Get(BuildingType.WOODEN_HOUSE);
        MyHouse = new House("wooden_house", 5);
		MyHouse.Count = 0;
		Gears = new GearsPack ();
		Defense = 1f;
		Attack = 5f;

		AllMaps = new List<Map> ();
		MapSight = 2;
        user_global_states = new List<GlobalState>();
        UserGlobalStateIndex = new Dictionary<string, GlobalState>();
		ElementResisIndex = new Dictionary<ElementType, float> ();
		ElementResisIndex.Add (ElementType.Fire, 1f);
		ElementResisIndex.Add (ElementType.Ice, 1f);
		ElementResisIndex.Add (ElementType.Wind, 1f);
		ElementResisIndex.Add (ElementType.Dark, 1f);
		ElementResisIndex.Add (ElementType.Holy, 1f);


        ElementAttackBonus = new Dictionary<ElementType, float>();
        ElementAttackBonus.Add(ElementType.Fire, 0f);
        ElementAttackBonus.Add(ElementType.Ice,  0f);
        ElementAttackBonus.Add(ElementType.Wind, 0f);
        ElementAttackBonus.Add(ElementType.Dark, 0f);
        ElementAttackBonus.Add(ElementType.Holy, 0f);


        ElementLevel = new Dictionary<ElementType,int> ();
		ElementLevel.Add (ElementType.Fire, 0);
		ElementLevel.Add (ElementType.Ice,  0);
		ElementLevel.Add (ElementType.Wind, 0);
		ElementLevel.Add (ElementType.Dark, 0);
		ElementLevel.Add (ElementType.Holy, 0);

		LearntUltiSkills = new List<PlayerUltiSkill> ();
		PlayerUltiSkill ulti_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_HARD_STRIKE);
		LearntUltiSkills.Add (ulti_skill);

		BattleStatesIndexs = new Dictionary<string, BattleState> ();
        //this.CurrentUltiSkill = ulti_skill;
        Goodness = 0;
        isInPanic = false;
        SkillMultiplier = 1f;
        DamageMultipler = 1f;
        SufferingMultiplier = 1f;
        FoodCdMultiplier = 1f;
        CoolDownMultiplier = 1f;
        EvilChamberLevel = 1;
      
    }

	

    //for all buff and debuff states
    private List<GlobalState> user_global_states;
	

    public Dictionary<string, GlobalState> UserGlobalStateIndex
    {
        get;
        private set;
    }



	// the inventory for this user to put items
	public Inventory UserInventory {
		get;
		private set;
	}

	// the set of construction this user has
	// for holding academic construction and technology construction only.
	public Constructions UserConstructions {
		get;
		private set;
	}

	// manage population and workers
	public Population MyPopulation {
		get;
		private set;
	}

	// manage houses
	// should this be put inside constructions or population?
	public House MyHouse {
		get;
		private set;
	}

	// the climate
	public Climate UserClimate {
		get;
		private set;
	}

	//the Gears the Player has equipped
	public GearsPack Gears {
		get;
		private set;
	}

	// manage the hand tool that is curretly equipped.
	private Tool equipped_tool = null;
	public Tool EquippedTool {
		get{
			return equipped_tool;
		}
		set {
			if(equipped_tool != null){
				Game.Current.ResourceInjector.ToolRemoved(equipped_tool.Type);
				equipped_tool.RemoveEffect(); 
			}
			equipped_tool = value;
			if(equipped_tool != null){
				Game.Current.ResourceInjector.ToolEquipped(equipped_tool.Type);
				equipped_tool.TakeEffect();
			}
		}
	}

	// manage the equipment tool that is curretly equipped
	private Tool equipment = null;
	public Tool Equipment {
		get{
			return equipment;
		}
		set{
			if(equipment != null){
				Game.Current.ResourceInjector.ToolRemoved(equipment.Type);
                equipment.RemoveEffect();

            }
            equipment = value;
			if(equipment != null){
				Game.Current.ResourceInjector.ToolEquipped(equipment.Type);
                equipment.TakeEffect();

            }
        }
	}



	// check whether the current user has the corresponding item.
	// If it is a tool, then it must be equipped.
	// If it is a stackable item, then both type and count will be checked.
	public bool has(Item item, bool ignoreCount = false) {
		if (item is Tool && !((Tool)item).IsEquipment) {
			if(EquippedTool != null && EquippedTool.Type.Equals(item.Type)){
				return true;
			}else{
				return false;
			}
		}else if(item is Tool && ((Tool)item).IsEquipment){ 
			if(Equipment != null && Equipment.Type.Equals(item.Type)){
				return true;
			}else{
				return false;
			}
		}else {
			return UserInventory.Has (item, ignoreCount);
		}
	}

	// check whether the current user has the corresponding building
    /*
	public bool has(Building building){
		if (building is ProductionConstruction) {
			return MyPopulation.Has ((ProductionConstruction)building);
		} else {
			return UserConstructions.Has (building);
		}
	}
    */

	// check whether a certain type of tool is equipped.
	public bool isItemEquipped(string t){
		if(EquippedTool != null && EquippedTool.Type.Equals(t)){
			return true;
		}else if (Equipment != null && Equipment.Type.Equals(t)){
			return true;
		}else{
			return false;
		}
	}
	                                                                                                                                                                   
	// method to call when this hero gets item.
	public void gains(Item item , bool need_toast = true) {
        if (need_toast)
        {
            if (item is StackableItem)
            {
               Game.Current.AddToast(Lang.Current["gain"] + " " + item.Name + "X" + ((StackableItem)item).Count);
            }
            else
            {
               Game.Current.AddToast(Lang.Current["gain"] + " " + item.Name);

            }
        }
        UserInventory.Add (item);
		// tell the injector that an item is gained. This can help to determine if this hero can make/build new stuff.
		Game.Current.Recorder.Track (item.Type);
	}



	// method to call when this hero loses an item, including a tool is used up.
	public void loses(Item item) {
		UserInventory.Remove (item);
	}

	// method to call when a building of this hero is destroyed
	public void destroy(Building building){
		if (building is House) {
			House house = building as House;
			MyHouse.Count -= house.Count;
			// recalculate the total population
			MyPopulation.RemoveUpperLimit(house.Count * MyHouse.Capacity);
			//MyPopulation.RemovePopulation(house.Count * MyHouse.Capacity);
		} else if (building is ProductionConstruction) {
			ProductionConstruction pc = building as ProductionConstruction;
			MyPopulation.RemoveOccupation(pc.Occupation);
		} else {
			UserConstructions.Remove (building);
		}
	}

	// return an item object from the inventory
	public Item Show(string type, string clazz){
		return UserInventory.Get(type, clazz);
	}

	public void UseTool(bool isEquipment, float cost, out bool toolUseUp){
		toolUseUp = false;

		if (isEquipment) {
			UseEquipment (cost, out toolUseUp);
		} else {
			UseEquippedTool (cost, out toolUseUp);
		}
	}

	public void UseEquippedTool(float cost, out bool toolUseUp){
        toolUseUp = false;
        EquippedTool.Use (cost);
		if (EquippedTool.Remaining <= 0) {
            toolUseUp = true;
			Game.Current.ResourceInjector.ToolRemoved (EquippedTool.Type); //remove events
			UserInventory.Remove (EquippedTool); //remove from inventory
            Tool new_tool = this.UserInventory.getFirstToolOfType(EquippedTool.Type);
            if (new_tool != null && Game.Current.IsAtHome)
            {
                this.EquippedTool = new_tool;
            }
            else
            {
                this.EquippedTool = null;
            }
		}
	}

	public void UseEquipment(float cost, out bool toolUseUp){
        toolUseUp = false;
        Equipment.Use (cost);
		if (Equipment.Remaining <= 0) {
            toolUseUp = true;
            Game.Current.ResourceInjector.ToolRemoved (Equipment.Type); //remove events
			UserInventory.Remove (Equipment); //remove from inventory
            Tool new_tool = this.UserInventory.getFirstToolOfType(Equipment.Type);
            if (new_tool != null && Game.Current.IsAtHome)
            {
                this.Equipment = new_tool;
            }
            else
            {
                this.Equipment = null;
            }
		}
	}

	// some routine event functions ..................................................................................

	//the following corotine runs every 1 seconds and use up tools
	public void UpdateEquippedTools(){
		bool toolUseUp;
		bool is_light_turned_on = false;
		if (Game.Current.Hero.isItemEquipped (ItemType.CAMP_FIRE)) {
			Game.Current.Hero.UseTool(true, 0.0015f, out toolUseUp); //1s
			is_light_turned_on = true;
		}
		
		if (Game.Current.Hero.isItemEquipped (ItemType.TORCH)) {
			Game.Current.Hero.UseTool(false, 0.0040f, out toolUseUp); //1s
			is_light_turned_on = true;
		}
		
		if (Game.Current.Hero.isItemEquipped (ItemType.WARM_MACHINE)) {
			is_light_turned_on = true;
			Game.Current.Hero.UseTool(true, 0.001f, out toolUseUp); //1s
		}
		
		if (Game.Current.Hero.isItemEquipped (ItemType.COLD_MACHINE)) {
			Game.Current.Hero.UseTool(true, 0.001f, out toolUseUp); //1s
		}

		if (Game.Current.Hero.isItemEquipped (ItemType.DRY_MACHINE)) {
			Game.Current.Hero.UseTool(true, 0.001f, out toolUseUp); //1s
		}

		if (Game.Current.Hero.isItemEquipped (ItemType.WOOLGLOVE)) {
			Game.Current.Hero.UseTool(false, 0.005f, out toolUseUp); //1s
		}

		if (Game.Current.Hero.isItemEquipped (ItemType.UMBRELLA)) {
			Game.Current.Hero.UseTool(false, 0.003f, out toolUseUp); //1s
		}

		if (Game.Current.Hero.isItemEquipped (ItemType.MUSIC_BOX)) {
			Game.Current.Hero.UseTool(true, 0.0002f, out toolUseUp); //1s
		}

        if (Game.Current.Hero.isItemEquipped(ItemType.FISH_MADE_FAN))
        {
            Game.Current.Hero.UseTool(false, 0.003f, out toolUseUp); //1s
        }

        if (Game.Current.Hero.isItemEquipped(ItemType.MOONLIGHT_TOTEM))
        {
            Game.Current.Hero.UseTool(true, 0.003f, out toolUseUp); //1s
        }

        if (is_light_turned_on || this.hasTimedState(StateType.STATE_ELF_LIGHT)) {
			Game.Current.IsLightOn = true;
		} else {
			Game.Current.IsLightOn = false;
		}


	}

	//update player health every 1 second, more and more hungary
	public void UpdateHealth(){
		Game.Current.Hero.CurrentHealth -= 0.15f; //decrease by this evey 0.5 seconds, so full health will last 2.5 day
		//update additional health state if any anormally exists		

		if (!Game.Current.IsAtHome) {
			//additional health consumption out in the wild
			Game.Current.Hero.CurrentHealth -= 0.15f;
		}


        if (Game.Current.IsAtNight)
        {
            //health drops faster at night
            Game.Current.Hero.CurrentHealth -= 0.15f;
        }


        if (Game.Current.Hero.isItemEquipped (ItemType.TENT) 
			&& Game.Current.IsAtNight) {
			Game.Current.Hero.CurrentHealth += 0.3f;
		}


        if (Game.Current.IsAtNight && !Game.Current.IsLightOn) {
            //dark with no light
           // if (!Config.IsDebugMode)
            {
                Game.Current.Hero.CurrentHealth -= 5f; //super fast drop at night
                isInPanic = true;
            }

        }else
        {
            isInPanic = false;
        }

		if (this.UserClimate.Tempature <= 30 ) {

            this.RemoveGlobalState(StateType.STATE_HEATSTROKE);
		}

        if (this.UserClimate.Tempature >= 10 )
        {
            this.RemoveGlobalState(StateType.STATE_FROZEN);
        }


        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            //player is dead ,should call game over
            if(Game.Current.CurrentGameMode == GameMode.Normal)
            {
                if (Game.Current.IsAtHome)
                {
                    GameObject.Find("GM*").GetComponent<GameController>().isGamePaused = true;
                    GameObject.Find("Audios").transform.FindChild("BGM").gameObject.SetActive(false);

                }
                else
                {
                    GameObject.Find("Player").GetComponent<MapController>().isGamePaused = true;
                    GameObject.Find("Audios").transform.FindChild("Mapbgm").gameObject.SetActive(false);

                }

                GameObject.Find("Canvas").transform.FindChild("ReviveConfirmPanel").gameObject.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(4, LoadSceneMode.Single);
            }
        }

    }

    //every 0.5s 


    //states will only be refreshed not stackeds
    public void AddGlobalState(GlobalState g_state)
    {
        if (UserGlobalStateIndex.ContainsKey(g_state.Type))
        {
            UserGlobalStateIndex[g_state.Type] = g_state;
        }
        else
        {
            UserGlobalStateIndex.Add(g_state.Type, g_state);
            user_global_states.Add(g_state);
            g_state.Activate(); //activate the battle state just after add the state
        }
    }


    public void RemoveGlobalState(string global_state_type)
    {
        if (UserGlobalStateIndex.ContainsKey(global_state_type))
        {
            UserGlobalStateIndex[global_state_type].Deactivate();
           // user_global_states.Remove(UserGlobalStateIndex[global_state_type]);
            UserGlobalStateIndex.Remove(global_state_type);
        }
        else
        {
           // Debug.LogError("no state to remove!");
        }
    }



    /*
    public void UpdateAllGlobalStates(){
		for (int i = 0; i < user_global_states.Count; i ++) {
            user_global_states[i].UpdateState(0.5f);
		}
	}
    */
    
   
    public void UpdateAllGlobalStates()
    {
        for (int i = 0; i < user_global_states.Count; i++)
        {
            string key = user_global_states[i].Type;
            if (UserGlobalStateIndex.ContainsKey(key))
            {
                UserGlobalStateIndex[key].UpdateState(0.5f);
            }
            else
            {
                user_global_states.RemoveAt(i);
            }
        }
    }
    



    //check if we have a global state
    public bool hasTimedState(string state_type){
		return UserGlobalStateIndex.ContainsKey(state_type);
	}
	                           

	//some more state Fields -----------------------------------------------------

    public int EvilChamberLevel
    {
        get;
        set;
    }	



    //the total goodness measure of the player, will be changed through out the game by events and players decisions
    //tier by -5 -4 -3 -2 -1 0 1 2 3 4 5,a normal event would change goodness approximately by 0.3 0.1 etc
    public float Goodness  
    {
        get;
        set;
    }

    //used to control bgm
    public bool isInPanic
    {
        get;
        set;
    }

  

	//Battle Related Functions And Fields ------------------------------------------------------------------------------------>

    public float CoolDownMultiplier
    {
        get;
        set;
    }



    public float SkillMultiplier
    {
        get;
        set;
    }


    //additional damage multiplier by the play
    //all player damages will be multiplied by this before monsters take it
    public float DamageMultipler
    {
        get;
        set;
    }


    //all damages to the player will be multiplied by this, before player takes it
    public float SufferingMultiplier
    {
        get;
        set;
    }


    public float FoodCdMultiplier
    {
        get;
        set;
    }

	public List<Map> AllMaps {
		get;
		private set;
	}

	public Map CurrentEquippedMap {
		get;
		set;
	}


	//how many blocks the player can see
	public int MapSight {
		get;
		set;
	}

	//the food you use to heal in battle
	public Food FoodInBattle {
		get;
		set;
	}

	// the elements  resistence 
	public Dictionary<ElementType, float> ElementResisIndex {
		get;
		set;
	}


    public Dictionary<ElementType, float> ElementAttackBonus{
        get;
        set;
    }
   

	//the current enemies you are facing ,used to pass data between scenes
	public List<Monster> CurrentBattleMonsters {
		get;
		set;
	}


	//the current map event the player is going through
	public MapEvent CurrentMapEvent {
		get;
		set;
	}
		
	public PlayerUltiSkill CurrentUltiSkill {
		get;
		set;
	}


	public List<PlayerUltiSkill> LearntUltiSkills {
		get;
		private set;
	}

	public Dictionary<ElementType,int> ElementLevel {
		get;
		private set;
	}


	public Dictionary<string,BattleState> BattleStatesIndexs {
		get;
		private set;
	}
	
	private List<string> all_battle_states = new List<string> ();


	public void AddBattleState(BattleState battle_state){
		if (BattleStatesIndexs.ContainsKey (battle_state.Type)){
			BattleStatesIndexs[battle_state.Type] = battle_state;
		} else {
			BattleStatesIndexs.Add (battle_state.Type,battle_state);
			all_battle_states.Add (battle_state.Type);
			battle_state.Activate(); //activate the battle state just after add the state
		}
	}
	
	
	public void RemoveBattleState(string battle_state_type){
        if (BattleStatesIndexs.ContainsKey(battle_state_type))
        {
            BattleStatesIndexs[battle_state_type].Deactivate();
            BattleStatesIndexs.Remove(battle_state_type);
        }
	}
	
	
	public void UpdateBattleStates(float period){
		for (int i = 0; i < all_battle_states.Count; i++) {
			string key = all_battle_states[i];
			if(BattleStatesIndexs.ContainsKey(key)){
				BattleStatesIndexs[key].UpdateState(period);
			}else{
				all_battle_states.RemoveAt(i);
			}
		}
	}


	//deactivate all the battle states if they didn't time out in battle
	public void DeactivateAllBattleStates(){
		foreach(string key in BattleStatesIndexs.Keys){
			BattleStatesIndexs[key].Deactivate();                                               
		}
		//clear all the records
		all_battle_states.Clear ();
		BattleStatesIndexs.Clear ();
	}



	public bool IsSkillLearnt(string skill_type){
		for (int i = 0; i < LearntUltiSkills.Count; i ++) {
			if(LearntUltiSkills[i].Type.Equals(skill_type)){
				return true;
			}
		}
		return false;
	
	}


	public void SufferDamage(Damage damage, BattleEngine battle_engine, Monster monster, bool ignorArmorSkill = false){
		Armor currentArmor = Game.Current.Hero.Gears.EquippedArmor;

		

		//1：calculate element damage resistence
		damage.DamageAmount *= ElementResisIndex[damage.EType];


        //2.Armor
        float reduction = this.Defense * Config.armor_multiplier / ( 1 + this.Defense * Config.armor_multiplier);
       

        damage.DamageAmount = damage.DamageAmount * (1f - reduction);
        /*
        if (this.Defense < damage.DamageAmount) {
			damage.DamageAmount = damage.DamageAmount - this.Defense;
		} else {
			damage.DamageAmount = 0;
		}
        */

        //3.Skill
        if (!ignorArmorSkill)
        {
            if (currentArmor != null)
            {
                for (int i = 0; i < currentArmor.ArmorSkills.Count; i++)
                {
                    currentArmor.ArmorSkills[i].TakeEffect(damage, battle_engine, monster);
                }
            }
        }

        if (IsInBattleState(StateType.BATTLE_STATE_IMMORTAL))
        {
            damage.DamageAmount = 0f;
        }

        damage.DamageAmount = damage.DamageAmount * this.SufferingMultiplier;
        this.CurrentHealth -= damage.DamageAmount;  
        battle_engine.ShowPlayerDamagedText (damage.DamageAmount, damage.EType);

	}
	

    public bool IsInBattleState(string type)
    {
        if (this.BattleStatesIndexs.ContainsKey(type))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



	//may need to return a bool here
	public bool UpgradeElement(ElementType e_type){
		int current_lvl = ElementLevel [e_type];
		float probability;
		if (current_lvl == 0) {
			probability = 1;
		} else {
			probability = 1/Mathf.Pow(1.3f,(float)current_lvl);
		}

		//only cost souls when upgrading elements
		Resource soul_required = ItemFactory.BuildResource (ItemType.SOUL, CalculateElementSoulRequire(e_type));
		if (current_lvl == 10) {
			Game.Current.AddToast (Lang.Current ["element_full"]);
			return false;
		} else {
			if (this.has (soul_required)) {
                Resource core = null;
                switch (e_type)
                {
                    case ElementType.Ice:
                        core = ItemFactory.BuildResource(ItemType.ICE_CORE, 1);
                       
                        break;
                    case ElementType.Fire:
                        core = ItemFactory.BuildResource(ItemType.FIRE_CORE, 1);

                        break;
                    case ElementType.Wind:
                        core = ItemFactory.BuildResource(ItemType.WIND_CORE, 1);

                        break;
                    case ElementType.Dark:
                        core = ItemFactory.BuildResource(ItemType.DARK_CORE, 1);

                        break;
                    case ElementType.Holy:
                        core = ItemFactory.BuildResource(ItemType.HOLY_CORE, 1);

                        break;            
                }

                if (!this.has(core))
                {
                    Game.Current.AddToast(core.Name + Lang.Current["not_enough"]);
                    return false;
                }
               

                this.loses (soul_required);
                this.loses (core);
				if (Random.value <= probability) {
					//upgrade success
					ElementLevel [e_type] = current_lvl + 1;
                    ElementAttackBonus[e_type] += 0.1f;
                    Game.Current.AddToast (Lang.Current ["upgrade_success"]);
					Game.Current.AddToast(Element.getElementName(e_type) + Lang.Current["element_damage_increase"]);
					ElementUpgradeReward(e_type);
                    if(current_lvl == 2)
                    {
                        Achievement.Current.UnlockAchievement(Achievement.AchievementType.ELEMENT_LVL_3);
                    }
                    return true;
				} else {
					//upgrade fail
					Game.Current.AddToast (Lang.Current ["upgrade_fail"]);
					return false;
				}
			} else {
				Game.Current.AddToast (Lang.Current ["not_enough_soul"]);
				return false;
			}
		}

	}


	private void ElementUpgradeReward(ElementType e_type){
		string[] ice_weapons = {ItemType.SCROLL_ICE_GUN, ItemType.SCROLL_ICE_LANCER, ItemType.SCROLL_ICE_HAMMER, ItemType.SCROLL_ICE_REAP, ItemType.SCROLL_ICE_GIANT_SWORD};
		string[] fire_weapons = {ItemType.SCROLL_FIRE_KNIFE, ItemType.SCROLL_FIRE_AXE, ItemType.SCROLL_FIRE_SWORD, ItemType.SCROLL_FIRE_BIG_SWORD, ItemType.SCROLL_FIRE_BIG_STAFF};
		string[] wind_weapons = {ItemType.SCROLL_WIND_WHIP, ItemType.SCROLL_WIND_DART, ItemType.SCROLL_WIND_BIG_SWORD, ItemType.SCROLL_WIND_BIG_AXE,ItemType.SCROLL_WIND_BIG_DART};
		string[] holy_weapons = {ItemType.SCROLL_HOLY_STAFF, ItemType.SCROLL_HOLY_HAMMER, ItemType.SCROLL_HOLY_LIGHT_SWORD, ItemType.SCROLL_HOLY_LIGHT_LANCER, ItemType .SCROLL_HOLY_SAINT_BOOK};
		string[] dark_weapons = { ItemType.SCROLL_DARK_SWORD, ItemType.SCROLL_DARK_LANCER, ItemType.SCROLL_DARK_GIANT_AXE, ItemType.SCROLL_DARK_GIANT_REAPER, ItemType.SCROLL_DARK_DEMON_SWORD};

		int lvl = ElementLevel [e_type];
        switch (e_type)
        {
            case ElementType.Fire:
                if (lvl <= fire_weapons.Length)
                {
                    Scroll scroll = ItemFactory.BuildScroll(fire_weapons[lvl - 1]);
                    this.gains(scroll, false);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + scroll.Name);
                }
                //reward some skills when level element level is up
                //not all levels will have a skill reward
                if (lvl == 1)
                {
                    PlayerUltiSkill reward_skill;
                    if (Random.value < 0.5f)
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_FIRE_WAVE);

                    }
                    else
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_FIRE_PULSE);
                    }
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                if (lvl == 4)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_FIRE_SHOCK);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                if (lvl == 7)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_FIRE_HELL);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                break;
            case ElementType.Ice:
                if (lvl <= ice_weapons.Length)
                {
                    Scroll scroll = ItemFactory.BuildScroll(ice_weapons[lvl - 1]);
                    this.gains(scroll, false);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + scroll.Name);

                }
                if (lvl == 1)
                {
                    PlayerUltiSkill reward_skill;
                    if (Random.value < 0.5)
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_ICE_WAVE);
                    }
                    else
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_ICE_PULSE);
                    }

                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                if (lvl == 4)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_ICE_SHOCK);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                if (lvl == 7)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_ICE_BLIZZARD);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                break;
            case ElementType.Wind:
                if (lvl <= wind_weapons.Length)
                {
                    Scroll scroll = ItemFactory.BuildScroll(wind_weapons[lvl - 1]);
                    this.gains(scroll, false);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + scroll.Name);

                }

                if (lvl == 1)
                {
                    PlayerUltiSkill reward_skill;
                    if (Random.value < 0.5)
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_WIND_WAVE);
                    }
                    else
                    {
                        reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_WIND_PULSE);
                    }

                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }


                if (lvl == 4)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_WIND_BLADE);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }


                if (lvl == 7)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_WIND_BURY);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                break;
            case ElementType.Holy:
                if (lvl <= holy_weapons.Length)
                {
                    Scroll scroll = ItemFactory.BuildScroll(holy_weapons[lvl - 1]);
                    this.gains(scroll, false);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + scroll.Name);
                }
                if (lvl == 2)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_POISON_STRIKE);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }

                break;
            case ElementType.Dark:
                if (lvl <= dark_weapons.Length)
                {
                    Scroll scroll = ItemFactory.BuildScroll(dark_weapons[lvl - 1]);
                    this.gains(scroll, false);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + scroll.Name);
                }

                if (lvl == 2)
                {
                    PlayerUltiSkill reward_skill = (PlayerUltiSkill)SkillBuilder.BuildSkill(SkillType.ULTI_SKILL_WEAK_STRIKE);
                    this.LearntUltiSkills.Add(reward_skill);
                    Game.Current.AddToast(Lang.Current["skill_learnt"] + " " + reward_skill.Name);
                }
                break;
        }


	}



	public int CalculateElementSoulRequire(ElementType e_type){
		return (ElementLevel [e_type] * 5 + 1);
	}




	
}
