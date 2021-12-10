using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleResult{
	Win, 
	Lose,
	Runaway
}


public class BattleEngine : MonoBehaviour {
	//Battle engine controls the process of 

	public List<Monster> current_monsters; //the monsters for this battle 
	private List<Transform> current_monster_prefabs; // monsters ui reference, 1 to 1 reference with monster list

	public float refreshPeriod = 0.5f; // the calculate rate of the whole battle 
	private Transform theCavans; //the canvas for battle
	private Transform monsterPanel; //the canvas that holds monsters
    private GameObject monsterPrefab; //the prefab for monsters;
    public Transform DamageEffect;
    private Transform playerEffectPanel;
    private Transform playerDamagedText;
    private Transform playerDamagedEffect;
    private Transform monsterDamagedText;

	//some hero controlls
	public Transform HealthText;
	private Text health_text;
	public Transform HealthBar;
	private Image health_bar_image_bottom;
	private Image health_bar_image_top;
	public Text PlayerBattleStates;

	public Transform SkillValueButton;
	private Image skill_button_image;
	private Button skill_button;
	public Transform FoodRecover;
	private Image food_recover_image;

    public Text food_recover_text;
	public Transform PlayerBattleToastTextPrefab;

	private float previous_health; //used to lerp the health deduction process
	private float time_to_lerp = 0;
	private bool isHealthIncreasing = false;

	//Food
	private bool isFoodReady = false; // if food is ready to be used to heal
	public float current_food_cool_down ;
	private float FoodCD = 5f;
	public Transform CanvasOverlay;

	//Ulti
	private bool isUltiReady = false; // if the ulti is fully charged
	private float previous_ulti_charge = 0f; // use to determine when ulti charge is changed
	public float CurrentUltiCharge = 0f; // once it is 1, the uli will be regarded as charged
	public Transform ChargeUltiParticlePrefab;// just an effect
	public Transform UltiReadyParticle;
    public Transform WeaponSkillPrefab;
    public Transform ArmorSkillPrefab;

	//Escape
	private bool isEscaping = false;
	public float EscapeWaitTime = 5f;
	private float current_escape_time = 0f;
	public Transform EscapeButton;


    //drop list 
    private AsyncOperation async;
    public GameObject DropList;
    public Slider DropListLoadingBar;
	private List<Item> dropped_item_list;
    public Transform dropRow;
    public Transform dropListContent;

    //loading screen
    public GameObject LoadingScreen;

    //audios 
    public AudioSource BattleBGM;
    public AudioSource player_attack_soud;
    public AudioSource player_attack_miss_soud;
    public AudioSource player_heal_soud;
    public AudioSource player_charge_full_sound;


    private bool isBattling = false; // used to control the battle status

    private MainCharacter hero;

    public GameObject ReviveConfirm;

    void Awake()
    {
        hero = Game.Current.Hero;
        if(hero.FoodInBattle != null)
        {
            FoodCD = hero.FoodInBattle.CoolDown *  hero.FoodCdMultiplier;
        }
    }


    void Start(){
        theCavans = GameObject.Find("Canvas").transform;
        monsterPanel = theCavans.FindChild("MonstersPanel");
        playerEffectPanel = theCavans.FindChild("PlayerEffectPanel");
        playerDamagedText = (Resources.Load("Prefab/PlayerDamagedText") as GameObject).transform;
        playerDamagedEffect = (Resources.Load("Prefab/BleedingEffect") as GameObject).transform;
        health_bar_image_top = HealthBar.FindChild("HealthImageBottom").FindChild("HealthImageTop").GetComponent<Image>();
        health_bar_image_bottom = HealthBar.FindChild("HealthImageBottom").GetComponent<Image>();
        monsterDamagedText = (Resources.Load("Prefab/MonsterDamagedText") as GameObject).transform;

        skill_button_image = SkillValueButton.GetComponent<Image> ();
		skill_button = SkillValueButton.GetComponent<Button> ();
		food_recover_image = FoodRecover.GetComponent<Image> ();

        health_text = HealthText.GetComponent<Text> ();
        current_food_cool_down = 0;

     

		dropped_item_list = new List<Item>();

		previous_health = hero.CurrentHealth;

		if (hero.CurrentBattleMonsters != null) {
			this.initBattle(hero.CurrentBattleMonsters);
		} else {
			Debug.LogError("No monsters !");
		}

		UltiReadyParticle.gameObject.SetActive(false);

	}


	public void initBattle(List<Monster> monsters){
		current_monster_prefabs = new List<Transform> ();
		current_monsters = monsters;
        //init interfaces(add monster to panel)
        monsterPrefab = Resources.Load("Prefab/Monsters/" + current_monsters[0].Type) as GameObject;

        if(monsterPrefab == null)
        {
            monsterPrefab = Resources.Load("Prefab/Monsters/MonsterPrefab") as GameObject;
        }

        for (int i = 0; i < current_monsters.Count; i ++) {
			Monster temp_monster = current_monsters[i];
			Transform tempMonsterPrefab = Instantiate(monsterPrefab.transform) as Transform;
			current_monster_prefabs.Add(tempMonsterPrefab);
			//need to set title , image, skill titles , etc
			tempMonsterPrefab.FindChild("Title").GetComponent<Text>().text = temp_monster.Name;

			//config the monster graphics size
			tempMonsterPrefab.SetParent(monsterPanel);
			//float parent_width = monsterPanel.GetComponent<RectTransform>().rect.width;
            float parent_width =720f;

            float margin = 20f;
			float padding = 10f;
            float size = (parent_width - 2 * margin - (3 - 1) * padding) / 3;
            if (current_monsters.Count == 3)
            {
                margin = 20f;
            }
            else if (current_monsters.Count == 2)
            {
                margin = (parent_width - padding - 2 * size)/2;
            }
            else
            {
                margin = (parent_width  - size) / 2;
            }


			RectTransform tempRf = tempMonsterPrefab.GetComponent<RectTransform>();
            tempRf.anchoredPosition3D = new Vector3(margin + size/2 + (i)*(size + padding),
                                                    -20f - size/2, 0f);
			tempRf.localScale = new Vector3(1f,1f,1f);
			tempRf.sizeDelta = new Vector2(size,size);
			ParticleSystem ps =  tempMonsterPrefab.FindChild("MonsterParticle").GetComponent<ParticleSystem>();
            Transform damage_effect = Instantiate(DamageEffect) as Transform;
            damage_effect.SetParent(tempMonsterPrefab);
            RectTransform damage_rf = damage_effect.GetComponent<RectTransform>();
            damage_rf.localScale = new Vector3(1f, 1f, 1f);
            damage_rf.offsetMin = new Vector2(0f, 0f);
            damage_rf.offsetMax = new Vector2(0f, 0f);
            damage_rf.rotation = Quaternion.Euler(0, 0, 30);

            switch (temp_monster.Element){
			case ElementType.Dark:
				ps.startColor = new Color(0.498f, 0.275f, 0.561f);
                    break;
			case ElementType.Holy:
				ps.startColor = new Color(0.812f, 0.655f, 0.196f);
				break;
			case ElementType.Fire:
				ps.startColor = new Color(0.808f, 0.173f, 0.251f);
				break;
			case ElementType.Ice:
				ps.startColor = new Color(0.204f, 0.424f, 0.576f);
				break;
			case ElementType.Wind:
				ps.startColor = new Color(0.188f, 0.824f, 0.298f);
				break;
			case ElementType.None:
				ps.startColor = Color.white;
				break;
			}
	
		}

		for(int k = 0 ; k < current_monsters.Count ; k ++){
			UpdateMonstersUI(k);
		}
        isBattling = true;
        //start battle
        StartCoroutine(StartMonsterBattle());
       // InvokeRepeating("MonstersBattle",0 ,refreshPeriod);
		InvokeRepeating ("UpdatePlayerBattleState", 0, refreshPeriod);
        if (current_monsters[0].isBoss)
        {
            BattleBGM.clip = Resources.Load("Audio/monster_boss") as AudioClip;
        }
        else if (!current_monsters[0].CanRunAwayFrom)
        {
            BattleBGM.clip = Resources.Load("Audio/monster_cannot_run") as AudioClip; 
        }
        else
        {
            BattleBGM.clip = Resources.Load("Audio/BattlefieldLoop") as AudioClip;
        }
        BattleBGM.Play();
    }


	void Update(){
		MonitorPlayer (Time.deltaTime);
	}


	public void pauseBattle(){
        isBattling = false;
	}

    public void ResumeBattle()
    {
        isBattling = true;
        StartCoroutine(StartMonsterBattle());
    }


    public void DyInBattle()
    {
        //hero.DeactivateAllBattleStates();
        //ReduceGearsDurability();
        SceneManager.LoadScene(4, LoadSceneMode.Single);
        this.current_monsters = null;
        BattleBGM.Stop();
    }

    public void ReviveInBattle()
    {
        if(Shop.Current.UseProduct(ItemType.REVIVE_STONE, 1))
        {
            //use a revive stone
            BattleBGM.volume = 1f;
            ReviveConfirm.SetActive(false);
            hero.CurrentHealth = hero.HealthUpperLimit; //revive
            player_heal_soud.Play();
            isBattling = true;
            StartCoroutine(StartMonsterBattle());// re start battle
        }
    }



    public void endBattle(BattleResult result){
		//CancelInvoke ("MonstersBattle");
        isBattling = false;
		
        switch (result)
        {
            case BattleResult.Win:
                hero.DeactivateAllBattleStates();
                ReduceGearsDurability();
                UltiReadyParticle.gameObject.SetActive(false);
                GenerateDropList();
                CheckSpecialMonsterEvents();
                ShowDropList();
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.FIRST_VICTORY);
                this.current_monsters = null;
                BattleBGM.Stop();
                break;
            case BattleResult.Lose:
                if(Game.Current.CurrentGameMode == GameMode.Normal)
                {
                    //normal mode
                    //revive will only be available in normal mode                   
                    BattleBGM.volume = 0.5f;
                    ReviveConfirm.SetActive(true);
                }
                else
                {
                    //survival mode
                    DyInBattle();
                }
                break;
            case BattleResult.Runaway:
                //restore all monsters
                hero.DeactivateAllBattleStates();
                ReduceGearsDurability();
                if (current_monsters != null)
                {
                    Achievement.Current.UnlockAchievement(Achievement.AchievementType.RUNAWAY);
                    for (int i = 0; i < current_monsters.Count; i++)
                    {
                        current_monsters[i] = MonsterBuilder.BuildMonster(current_monsters[i].Type);
                    }

                    if (Game.Current.IsAtHome)
                    {
                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                    else
                    {
                        SceneManager.LoadScene(2, LoadSceneMode.Single);
                    }
                }
                this.current_monsters = null;
                BattleBGM.Stop();
                break;
        }
       
        
    }


    private void CheckSpecialMonsterEvents()
    {
        string m_type = current_monsters[0].Type;
        if(System.Array.IndexOf(Config.SpecialEventMonters,m_type) > -1)
        {
            //defeated a special event monster, finish the map event
            if (Game.Current.Hero.CurrentMapEvent != null)
            {
                Game.Current.Hero.CurrentMapEvent.SetMapEventFinished();
            }
        }

        if (current_monsters[0].isInEvilChamber)
        {
            Game.Current.Hero.EvilChamberLevel++;
            Rank.Current.UpdateEvil(Game.Current.Hero.EvilChamberLevel, Game.Current.CurrentGameMode);
        }
    }


    private void ReduceGearsDurability(){
		//will reduce gear's durability for each battle
		GearsPack gears = hero.Gears;
		if (gears.EquippedWeapon != null) {
			gears.EquippedWeapon.Remaining -= gears.EquippedWeapon.Cost;
			if (gears.EquippedWeapon.Remaining <= 0) {
				//weapon is used up, destroy it 
				hero.UserInventory.AllWeapons.Remove(gears.EquippedWeapon);
				gears.EquippedWeapon = null;
			}
		}

		if (gears.EquippedAccessory != null) {
			gears.EquippedAccessory.Remaining -= gears.EquippedAccessory.Cost;
			if (gears.EquippedAccessory.Remaining <= 0) {
				//weapon is used up, destroy it 
				hero.UserInventory.AllAccessories.Remove (gears.EquippedAccessory);
				gears.EquippedAccessory = null;
			}
		}

		if (gears.EquippedArmor != null) {
			gears.EquippedArmor.Remaining -= gears.EquippedArmor.Cost;
			if (gears.EquippedArmor.Remaining <= 0) {
				//weapon is used up, destroy it 
				hero.UserInventory.AllArmors.Remove (gears.EquippedArmor);
				gears.EquippedArmor = null;
			}
		}
	}


	private void ShowDropList(){
		Destroy (GameObject.Find("Battle").transform.FindChild("CircleAttackIndicator").gameObject);
        DropList.SetActive(true);
        Transform drop_list = DropList.transform;
		Text picked_hint_text = drop_list.FindChild ("PickedHint").GetComponent<Text> ();
		for (int i = 0; i < dropped_item_list.Count; i ++) {
            Transform drop_row = Instantiate(dropRow);
            RectTransform rf = drop_row.GetComponent<RectTransform>();
            rf.SetParent(dropListContent);
            rf.localScale = new Vector3(1f, 1f, 1f);
            float height = rf.rect.height;
            rf.offsetMin = new Vector2(0f, -(i + 1) * height);
            rf.offsetMax = new Vector2(0f, -(i) * height);
            Text text = drop_row.GetComponent<Text>();
            switch (dropped_item_list[i].Tier)
            {
                case 1:
                    text.color = Config.uncommonColor;
                    break;
                case 2:
                    text.color = Config.rareColor;
                    break;
                case 3:
                    text.color = Config.legendColor;
                    break;
            }

            if (dropped_item_list[i] is StackableItem){

                text.text = dropped_item_list[i].Name  + " X " + ((StackableItem)dropped_item_list[i]).Count + "\n";
			}else{
                text.text = dropped_item_list[i].Name  + "\n";
			}
		}
		Button pick_all = drop_list.FindChild ("PickAll").GetComponent<Button> ();

		pick_all.onClick.AddListener (delegate {
            //TODO: Collect the items in the drop list
            pick_all.enabled = false;
			for (int i = 0; i < dropped_item_list.Count; i ++) {
				Item item = dropped_item_list[i];
				hero.gains(item.Clone());
			}
			dropped_item_list.Clear();
			picked_hint_text.color = new Color(1f,1f,1f,1f);
            LoadBackLevelWithLoadingFromDropList();
        });

	}

    private void LoadBackLevelWithLoadingFromDropList()
    {
        if (Game.Current.IsAtHome)
        {
            StartCoroutine(loadLevelInBGFromDropList(1));
        }
        else
        {
            StartCoroutine(loadLevelInBGFromDropList(2));
        }
    }


    IEnumerator loadLevelInBGFromDropList(int level)
    {

        async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
        DropListLoadingBar.GetComponent<CanvasGroup>().alpha = 1f; //show the loading bar
        while (!async.isDone)
        {
            DropListLoadingBar.value = async.progress;
            yield return null;
        }
    }


    private void GenerateDropList(){
		//general drop are only for resources
		string[] commons = {ItemType.WOOD, ItemType.TEETH, ItemType.BRANCH};  //10 - 30
		string[] uncommons = {ItemType.REDWOOD, ItemType.GOLD, ItemType.CRYSTAL, ItemType.IRON}; //5 - 15
		string[] rares = {ItemType.BLUE_CRYSTAL,ItemType.WINTER_WOOD, ItemType.RED_CRYSTAL,
		ItemType.MOON_GRASS}; // 1 - 3 
		System.Random random = new System.Random ();
		for (int i = 0; i < current_monsters.Count; i ++) {
			Monster monster = current_monsters[i];

			if(monster.GeneralDrops.Contains(GeneralDropType.Common)){
				int idx = random.Next(0,commons.Length);
                if (DropListContains(commons[idx]) != null)
                {
                    ((StackableItem)DropListContains(commons[idx])).Count += random.Next(10, 30);
                }
                else
                {
                    dropped_item_list.Add(ItemFactory.BuildResource(commons[idx], random.Next(10, 30)));
                }
            }

			if(monster.GeneralDrops.Contains(GeneralDropType.Uncommon)){
				int idx = random.Next(0,uncommons.Length);
                if (DropListContains(uncommons[idx]) != null)
                {
                    ((StackableItem)DropListContains(uncommons[idx])).Count += random.Next(5, 15);
                }
                else
                {
                    dropped_item_list.Add(ItemFactory.BuildResource(uncommons[idx], random.Next(5, 15)));

                }
            }

			if(monster.GeneralDrops.Contains(GeneralDropType.Rare)){
				int idx = random.Next(0,rares.Length);
                if (DropListContains(rares[idx]) != null)
                {
                    ((StackableItem)DropListContains(rares[idx])).Count += random.Next(1, 5);
                }
                else
                {
                    dropped_item_list.Add(ItemFactory.BuildResource(rares[idx], random.Next(1, 5)));
                }
			}



			//monster specific drops
			List<DropNode> drop_nodes = monster.DropList;
			for(int j = 0 ; j < drop_nodes.Count ; j ++){
				Item item = drop_nodes[j].GenerateDroppedItem();
				if(item != null){
                    if(item is StackableItem)
                    {
                        if (DropListContains(item.Type) != null)
                        {
                            ((StackableItem)DropListContains(item.Type)).Count += ((StackableItem)item).Count;
                        }
                        else
                        {
                            dropped_item_list.Add(item);
                        }
                    }
                    else
                    {
                        dropped_item_list.Add(item);
                    }
                }
			}
		}
	}

    private Item DropListContains(string type)
    {
        for (int i = 0; i < dropped_item_list.Count; i++)
        {
            if (dropped_item_list[i].Type.Equals(type))
            {
                return dropped_item_list[i];
            }
        }

        return null;
    }

    IEnumerator StartMonsterBattle()
    {
        while (isBattling)
        {
            MonstersBattle();
            yield return new WaitForSeconds(refreshPeriod);
        }
    }
	// monster attack players
	private void MonstersBattle(){
		//battle all the monsters
		bool is_any_monster_alive = false;  // if false ,all monsters dead
		for (int i = 0; i < current_monsters.Count; i ++) {
			Monster temp_monster = current_monsters[i];
			if(temp_monster.isAlive){
				temp_monster.UpdateBattleStates(refreshPeriod,this,i);
				UpdageMonstersBattleStateUI(i);
				temp_monster.DoBattle(refreshPeriod,this,i); // just pass the period , monster will handle the attack by itself
				is_any_monster_alive = true;
			}
			UpdateMonstersUI(i);
		}

		if (!is_any_monster_alive) {
			//all monsters dead, Battle ended
			endBattle(BattleResult.Win);
		}

	}


	public void ShowMonsterAttackEffect(int idx){
        
		current_monster_prefabs [idx].GetComponent<Animator> ().SetTrigger ("Attack");
        
	}

	public void ShowMonsterDamagedEffect(int idx){
        current_monster_prefabs[idx].FindChild("damage_effect(Clone)").GetComponent<Animator>().SetTrigger("Damaged");
        //current_monster_prefabs [idx].GetComponent<Animator> ().SetTrigger ("Attacked");
	}


	public void PlayMonsterSound(int idx,  string sound_type){
		Transform audios =  current_monster_prefabs[idx].FindChild("MonsterAudios");
		if (sound_type.Equals ("Attack")) {
			audios.Find("MonsterAttack").GetComponent<AudioSource>().Play();
		}
		else if(sound_type.Equals("Die")){
			audios.Find("MonsterDead").GetComponent<AudioSource>().Play();
		}
	}

	//start in invoker for every 0.5 second
	public void UpdatePlayerBattleState(){
		hero.UpdateBattleStates (refreshPeriod);
	}
	


	//monitor player status, if dead , end battle
	private void MonitorPlayer(float period){
		//update health

		if (previous_health != hero.CurrentHealth) {
			time_to_lerp = Time.time;
			if(hero.CurrentHealth > previous_health){
				isHealthIncreasing = true;
			}else{
				isHealthIncreasing = false;
			}
			previous_health = hero.CurrentHealth;
		}
		float current_fill = health_bar_image_bottom.fillAmount;
		float target_fill = hero.CurrentHealth / hero.HealthUpperLimit;

		health_bar_image_bottom.fillAmount = Mathf.Lerp (current_fill, target_fill, (Time.time - time_to_lerp) );
		if (isHealthIncreasing) {
			health_bar_image_top.fillAmount = Mathf.Lerp (current_fill, target_fill, (Time.time - time_to_lerp) );
		} else {
			health_bar_image_top.fillAmount = target_fill;
		}

		health_text.text = Lang.Current["health_value"] + ": " + (int)hero.CurrentHealth + "/" +  (int)hero.HealthUpperLimit;

		//update food CD
		if (!isFoodReady) {
			//cd is only update when not attacking
			if (current_food_cool_down <= period) {
				//can attack now
				isFoodReady = true;
                //may do a refresh for FoodCD, since food change may happen
                if(hero.FoodInBattle != null)
                {
                    FoodCD = hero.FoodInBattle.CoolDown * hero.FoodCdMultiplier;
                }
                else
                {
                    FoodCD = 5;
                }
                current_food_cool_down = FoodCD;
				food_recover_image.fillAmount = 1f;

			} else {
				current_food_cool_down -= period;
				food_recover_image.fillAmount = (FoodCD - current_food_cool_down) / FoodCD;

			}
		}
		if (hero.FoodInBattle != null) {
			food_recover_text.text = hero.FoodInBattle.Name + "*" + hero.FoodInBattle.Count;
		} else {
			food_recover_text.text = Lang.Current["please_choose_food"];
		}

		//monitor health
		if (hero.CurrentHealth <= 0) {
			//player dead, game over
			endBattle(BattleResult.Lose);
		}

		//battle state text
		string states = "";
		foreach (string key in hero.BattleStatesIndexs.Keys) {
			states += "\n" + hero.BattleStatesIndexs[key].Name;
		}
        PlayerBattleStates.text = states;

		//escape
		if (isEscaping) {
			if(current_escape_time < EscapeWaitTime){
				current_escape_time += period;
                EscapeButton.GetComponent<Image>().fillAmount = current_escape_time / EscapeWaitTime;
			}else{
				//time to escape
				endBattle(BattleResult.Runaway);
				isEscaping = false;
			}
		}


		//the charge bar
		if (CurrentUltiCharge != previous_ulti_charge) {
			//charge changed , update 
			skill_button_image.fillAmount = CurrentUltiCharge;
			previous_ulti_charge = CurrentUltiCharge;
			Transform effect_clone = Instantiate(ChargeUltiParticlePrefab);
			effect_clone.SetParent(SkillValueButton);
			RectTransform rf = effect_clone.GetComponent<RectTransform>();
			rf.localScale = new Vector3(1f,1f,1f);
			float x_pos = skill_button.GetComponent<RectTransform>().rect.width * skill_button_image.fillAmount;
			rf.anchoredPosition3D = new Vector3(x_pos,0,0);
			Destroy(effect_clone.gameObject, 1);
			if (CurrentUltiCharge >= 1f) {
				//fully charge 
				isUltiReady = true;
				UltiReadyParticle.gameObject.SetActive(true);
				player_charge_full_sound.Play();
			}else{
				UltiReadyParticle.gameObject.SetActive(false);
			}
		}

	}


	public void Escape(){
		bool can_run = true;
		for (int i = 0; i < current_monsters.Count; i ++) {
			if(!current_monsters[i].CanRunAwayFrom)
				can_run = false;
		}

		if (can_run) {
			isEscaping = true;
		} else {
			ShowPlayerToast(Lang.Current["cannot_runaway"]);
		}
	}

	public void CastPlayerUlti(){
		if (isUltiReady) {
			//TODO: Cast the real ulti skill
			if(hero.CurrentUltiSkill != null){
				hero.CurrentUltiSkill.TakeEffect(this);
				ShowPlayerToast( hero.CurrentUltiSkill.Name);
			}
			player_attack_soud.Play();
			//reset ulti bar
			CurrentUltiCharge = 0;
			isUltiReady = false;
		}
	}


	private void ShowPlayerToast(string content){
		Transform title_clone = Instantiate(PlayerBattleToastTextPrefab);
		title_clone.SetParent(theCavans.FindChild("PlayerPanel"));
		title_clone.GetComponent<Text> ().text = content;
		RectTransform rf = title_clone.GetComponent<RectTransform>();
		rf.localScale = new Vector3(1f,1f,1f);
		rf.anchoredPosition3D = new Vector3(0,200,0);
		Destroy(title_clone.gameObject, 1f);
	}
	
	
	
	
	public void EatFood(){
		//only eat when it is ready
		if(isFoodReady){
			food_recover_image.fillAmount = 0f;  
			if(hero.FoodInBattle != null){
				hero.FoodInBattle.Consume();
			}
			player_heal_soud.Play();
			isFoodReady = false;
		}
	}
	
	//player attack monsters
	public List<Monster> AttackMonsters(Damage damage){
		if (damage.EType != ElementType.None) {
			//element damage, should be enlarged by player element level
			damage.DamageAmount *= (1 + hero.ElementAttackBonus[damage.EType]);
		}
		List<Monster> AttackedMonster = new List<Monster> (); //reference to all the monsters that are attacked
		if (damage.isAOE) {
            //aoe
			for(int i = 0 ; i < current_monsters.Count ; i ++){
				if(current_monsters[i].isAlive){
					current_monsters[i].SufferDamage(damage.Clone(),i,this);
					AttackedMonster.Add(current_monsters[i]);
				}
			}
			return AttackedMonster;
		} else {
			//not aoe
			for(int i = 0 ; i < current_monsters.Count ; i ++){
				//attack the first alive monster
				if(current_monsters[i].isAlive){
					current_monsters[i].SufferDamage(damage,i,this);
					AttackedMonster.Add(current_monsters[i]);
					return AttackedMonster;
				}

			}
			return null;
		}

	}

	//show when monster is attacked
	public void ShowMonsterDamagedText(float damage_amount,ElementType damage_element_type, int idx){
		Transform text_clone = Instantiate (monsterDamagedText) as Transform;
		text_clone.SetParent (current_monster_prefabs [idx]);
		RectTransform rf = text_clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.anchoredPosition3D = new Vector3 (0, -300f, 0);
		switch (damage_element_type) {
		case ElementType.Dark:
			text_clone.GetComponent<Text> ().material.color = new Color(0.498f, 0.275f, 0.561f);
                break;
		case ElementType.Holy:
			text_clone.GetComponent<Text> ().material.color = new Color(0.812f, 0.655f, 0.196f);
			break;
		case ElementType.Fire:
			text_clone.GetComponent<Text> ().material.color = new Color(0.808f, 0.173f, 0.251f);
			break;
		case ElementType.Ice:
			text_clone.GetComponent<Text> ().material.color = new Color(0.204f, 0.424f, 0.576f);
			break;
		case ElementType.Wind:
			text_clone.GetComponent<Text> ().material.color = new Color(0.188f, 0.824f, 0.298f);
			break;
		case ElementType.None:
			text_clone.GetComponent<Text> ().material.color = Color.white;
			break;
		}
		text_clone.GetComponent<Text>().text = "-" + System.Math.Round(damage_amount,1);
		//Destroy (text_clone.gameObject, 0.6f); //destroy the text after 2 seconds
	//	if(!current_monsters[idx].isAlive){
			//TODO： need to change UI, when monster is dead
	//		current_monster_prefabs[idx].FindChild("Title").GetComponent<Text>().text = "GG!";
	//	}

		UpdateMonstersUI (idx);
		ShowMonsterDamagedEffect (idx);
		Destroy (text_clone.gameObject, 0.6f);
	}


	public void UpdateMonstersUI(int idx){
		Transform current_transform = current_monster_prefabs[idx];
		Monster current_monster = current_monsters[idx];
        Transform DefenseAttack = current_transform.FindChild("defense_attack");
        Transform HealthBar = DefenseAttack.FindChild("MonsterHealthBar");
        HealthBar.GetComponent<Image>().fillAmount
			= current_monster.CurrentHealth / current_monster.HealthUpperLimit;
        HealthBar.GetComponentInChildren<Text>().text = System.Math.Round(current_monster.CurrentHealth,1) + "/" + System.Math.Round(current_monster.HealthUpperLimit,1);

        if(current_monster.Defense > 500) {
            DefenseAttack.GetComponent<Text>().text = Lang.Current["attack_one"] + current_monster.Attack +
             " " + Lang.Current["defense_one"] + Lang.Current["infinite"];
        }
        else
        {
            DefenseAttack.GetComponent<Text>().text = Lang.Current["attack_one"] + current_monster.Attack +
             " " + Lang.Current["defense_one"] + current_monster.Defense;
        }
       
        if (!current_monster.isAlive){
			current_transform.FindChild("MonsterImage").GetComponent<Image>().color = new Color(1f,1f,1f,0.5f);
			current_transform.FindChild("Title").GetComponent<Text>().text = Lang.Current["dead"];         
			current_transform.GetComponent<Animator>().SetTrigger("Dead");
			current_transform.FindChild("MonsterParticle").GetComponent<ParticleSystem>().Stop();
		}
	}

	public void UpdageMonstersBattleStateUI(int idx){
		Transform current_transform = current_monster_prefabs[idx];
		Monster current_monster = current_monsters[idx];
		Text tile = current_transform.FindChild ("Title").GetComponent<Text> ();
        Text state_text = current_transform.FindChild("MonsterImage").FindChild("states").GetComponent<Text>();
        tile.text = current_monster.Name;
        state_text.text = "";
        foreach (string key in current_monster.BattleStatesIndexs.Keys) {
            state_text.text += "\n" + current_monster.BattleStatesIndexs[key].Name;
		}
	}



	public void ShowMonsterSkillTitle(string title, int idx, string category){
		Transform text_clone = Instantiate ((Resources.Load("Prefab/MonsterUltiTitle") as GameObject).transform) as Transform;
		text_clone.SetParent (current_monster_prefabs [idx]);
		RectTransform rf = text_clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		text_clone.GetComponent<Text> ().text = title;

        if (category.Equals("AttackSkill"))
        {
            rf.anchoredPosition3D = new Vector3(0f, -100f, 0);
            text_clone.GetComponent<Text>().fontSize = 100;
           // text_clone.GetComponent<Text>().color = Color.red;

        }
        else if (category.Equals("DefenseSkill"))
        {
            rf.anchoredPosition3D = new Vector3(0f, -100f, 0);
            text_clone.GetComponent<Text>().fontSize = 100;
           // text_clone.GetComponent<Text>().color = Color.green;

        }
        else if (category.Equals("UltiSkill"))
        {
            rf.anchoredPosition3D = new Vector3(0f, -160f, 0);
            text_clone.GetComponent<Text>().fontSize = 130;
            //text_clone.GetComponent<Text>().color = Color.yellow;
        }
        /*
		if (category.Equals ("AttackSkill")) {
			rf.anchoredPosition3D = new Vector3 (0f, -350f, 0);
			text_clone.GetComponent<Text> ().fontSize = 100;
		} else if (category.Equals ("DefenseSkill")) {
			rf.anchoredPosition3D = new Vector3 (0f, -450, 0);
			text_clone.GetComponent<Text> ().fontSize = 100;
		} else if(category.Equals("UltiSkill")){
			rf.anchoredPosition3D = new Vector3 (0f, -550f, 0);
			text_clone.GetComponent<Text> ().fontSize = 130;
		}
        */
        Destroy (text_clone.gameObject, 1.55f);
	}


	//show when player is attacked
    public void ShowPlayerDamagedText(float damage_amount, ElementType damage_element_type){
		Transform text_clone = Instantiate (playerDamagedText) as Transform;
		text_clone.SetParent (playerEffectPanel);
		RectTransform rf = text_clone.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);
		rf.anchoredPosition3D = new Vector3 (0, -120f, 0);

		Transform bleeding_clone = Instantiate (playerDamagedEffect) as Transform;
		bleeding_clone.SetParent (playerEffectPanel);
		RectTransform brf = bleeding_clone.GetComponent<RectTransform> ();
		float random_scale = Random.Range (0.7f, 2.0f);
		brf.localScale = new Vector3 (random_scale, random_scale, random_scale);
		float random_x = Random.Range (-120f, 120f);
		float random_y = Random.Range (0, 300f);
		brf.anchoredPosition3D = new Vector3 (random_x, random_y, 0);
		brf.Rotate (0, 0, Random.Range(0,180));

		switch(damage_element_type){
		case ElementType.Dark:
			text_clone.GetComponent<Text> ().material.color = new Color(0.498f, 0.275f, 0.561f);
			break;
		case ElementType.Holy:
			text_clone.GetComponent<Text> ().material.color = new Color(0.812f, 0.655f, 0.196f);
			break;
		case ElementType.Fire:
			text_clone.GetComponent<Text> ().material.color = new Color(0.808f, 0.173f, 0.251f);
			break;
		case ElementType.Ice:
			text_clone.GetComponent<Text> ().material.color = new Color(0.204f, 0.424f, 0.576f);
			break;
		case ElementType.Wind:
			text_clone.GetComponent<Text> ().material.color = new Color(0.188f, 0.824f, 0.298f);
			break;
		case ElementType.None:
			text_clone.GetComponent<Text> ().material.color = Color.white;
			break;
		}
		text_clone.GetComponent<Text> ().text = "-" + System.Math.Round(damage_amount, 1);

		Destroy (text_clone.gameObject, 0.6f); //destroy the text after 2 seconds
		Destroy (bleeding_clone.gameObject, 0.4f); //destroy the text after 2 seconds
	}

    public void showWeaponSkillText(string content)
    {
        Transform clone = Instantiate(WeaponSkillPrefab) as Transform;
        clone.SetParent(playerEffectPanel);
        RectTransform rf = clone.GetComponent<RectTransform>();
        rf.localScale = new Vector3(1f, 1f, 1f);
        rf.anchoredPosition3D = new Vector3(0f, 0f, 0f);
        clone.GetComponent<Text>().text = content;
        Destroy(clone.gameObject, 2); // need to destroy the effect
    }

    public void showArmorSkillText(string content)
    {
        Transform clone = Instantiate(ArmorSkillPrefab) as Transform;
        clone.SetParent(playerEffectPanel);
        RectTransform rf = clone.GetComponent<RectTransform>();
        rf.localScale = new Vector3(1f, 1f, 1f);
        rf.anchoredPosition3D = new Vector3(0f, 0f, 0f);
        clone.GetComponent<Text>().text = content;
        Destroy(clone.gameObject, 2); // need to destroy the effect
    }
    
	

	public void ShowFoodChooseList(){
        if(GameObject.Find("CanvasOverlay(Clone)") != null)
        {
            Destroy(GameObject.Find("CanvasOverlay(Clone)"));
        }
        else
        {
            Transform clone = Instantiate(CanvasOverlay) as Transform;
            clone.GetComponent<Canvas>().worldCamera = Camera.main;
        }

	}

	public void ResetUltiCharge(){
		this.CurrentUltiCharge = 0;
	}

}
