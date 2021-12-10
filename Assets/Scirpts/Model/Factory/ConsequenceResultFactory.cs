using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class ConsequenceResultFactory 
{

	public static void Dummy(XmlNode node){
		Debug.Log ("This is a Dummy method");
	}

	public static void getMap(XmlNode node){
		string map_type = node ["MapType"].InnerText;
		Map map = MapBuilder.CreateMap (map_type);
		Game.Current.Hero.AllMaps.Add (map);
        Achievement.Current.UnlockAchievement(Achievement.AchievementType.ADVENTURE_START);
	}


	public static void LogStory(XmlNode node){
		string content = node ["ResultContent"].InnerText;
		Game.Current.AddLog (Lang.Current[content]);
	}


	public static void LaunchBattle(XmlNode node){

		string monster_type = node ["MonsterType"].InnerText;
		int count = int.Parse(node ["Count"].InnerText);
        if (monster_type.Contains("boss_day"))
        {
            //it is a boss
            GameObject.Find("GM*").GetComponent<GameController>().isBossPopUp = false;
        }
        CreateBattle(true, monster_type, count);
	}


    public static void FightHolyKeeper(XmlNode node)
    {
        string[] monster_types = { MonsterType.HOLY_BLADER, MonsterType.HOLY_SOURCER, MonsterType.HOLY_WORRIER};
        string monsterType;
        System.Random random = new System.Random();
        monsterType = monster_types[random.Next(0, monster_types.Length)];
        CreateBattle(true, monsterType, 1);
    }



    public static void AttracksEvilMonsters(XmlNode node)
    {
        string[] lvl1_monsters = { MonsterType.WOLF, MonsterType.TIGER, MonsterType.Bat, MonsterType.BEAR, MonsterType.VULTURE};
        string[] lvl2_monsters = { MonsterType.FIRE_DUMMY, MonsterType.DUST_ELF, MonsterType.YETI, MonsterType.TREE_MAN, MonsterType.ZOMBIE};
        string[] lvl3_monsters = { MonsterType.FISH_GUARDIAN, MonsterType.ALLIGATOR, MonsterType.VIPER, MonsterType.FIRE_DOG, MonsterType.FIRE_FOX, MonsterType.GIANT_SNAKE};
        string[] lvl4_monsters = { MonsterType.HIGH_KEEPER, MonsterType.JUNIOR_KEEPER, MonsterType.DEATH_WITCH };
        string[] lvl6_monsters = { MonsterType.HELL_WORM, MonsterType.OGRE, MonsterType.WHITE_WALKER};

        int days = Mathf.FloorToInt(Game.Current.GameTime/Config.SecondsPerDay);
        System.Random random = new System.Random();
        string monsterType;
        if(days < 20)
        {
            monsterType = lvl1_monsters[random.Next(0, lvl1_monsters.Length)];
        }
        else if(days < 45)
        {
            monsterType = lvl2_monsters[random.Next(0, lvl2_monsters.Length)];
        }
        else if(days <  75)
        {
            monsterType = lvl3_monsters[random.Next(0, lvl3_monsters.Length)];
        }
        else if(days < 120)
        {
            monsterType = lvl4_monsters[random.Next(0, lvl4_monsters.Length)];
        }
        else 
        {
            monsterType = lvl4_monsters[random.Next(0, lvl4_monsters.Length)];
        }
        /*
        else
        {
            monsterType = high_lvl_monsters[random.Next(0, high_lvl_monsters.Length)];
        }

        */
        int count = random.Next(1, 3);

        CreateBattle(true, monsterType, count);
    }


    public static void LogThenBattle(XmlNode node){
		string content = node ["ResultContent"].InnerText;
		Game.Current.AddLog (Lang.Current[content]);
		string monster_type = node ["MonsterType"].InnerText;
		int count = int.Parse(node ["Count"].InnerText);
        StartDelayBattle(monster_type, count);
	}

    public static void BearDestroyTree(XmlNode node){
        Game.Current.Hero.MyPopulation.RemoveUpperLimit(5);
        Game.Current.Hero.MyHouse.Count--;
    }


	public static void HintRain(XmlNode node){
		string[] hints = {Lang.Current["hint_rain_one"], Lang.Current["hint_rain_two"]};
		System.Random random = new System.Random ();
		int idx = random.Next(0, hints.Length);
		Game.Current.AddLog (hints[idx]);
	}

	public static void HintSnow(XmlNode node){
		string[] hints = {Lang.Current["hint_snow_one"], Lang.Current["hint_snow_two"]};
		System.Random random = new System.Random ();
		int idx = random.Next(0, hints.Length);
		Game.Current.AddLog (hints[idx]);
	}

	public static void HintSunny(XmlNode node){
		string[] hints = {Lang.Current["hint_sunny_one"], Lang.Current["hint_sunny_two"]};
		System.Random random = new System.Random ();
		int idx = random.Next(0, hints.Length);
		Game.Current.AddLog (hints[idx]);
	}

	public static void HintFog(XmlNode node){
		string[] hints = {Lang.Current["hint_fog_one"], Lang.Current["hint_fog_two"]};
		System.Random random = new System.Random ();
		int idx = random.Next(0, hints.Length);
		Game.Current.AddLog (hints[idx]);
	}

	public static void HintNight(XmlNode node){
		string[] hints = {Lang.Current["hint_night_one"], Lang.Current["hint_night_two"]};
		System.Random random = new System.Random ();
		int idx = random.Next(0, hints.Length);
		Game.Current.AddLog (hints[idx]);
	}



	public static void GainRandomFood(XmlNode node){
		string[] possible_foods = {ItemType.RABBIT_MEAT, ItemType.BEEF, ItemType.PORK, ItemType.MUSHROOM, ItemType.POTATO, ItemType.APPLE,
			ItemType.ORANGE, ItemType.FISH, ItemType.SHRIMP};
		System.Random random = new System.Random ();
		int idx = random.Next(0, possible_foods.Length);
		int count = random.Next (1, 10);
		Food item = ItemFactory.Get (possible_foods [idx]) as Food;
		item.Count = count;
		Game.Current.Hero.gains (item);
		Game.Current.AddLog (Lang.Current ["luck_pick"] + item.Name + "*" + item.Count);
	}

	public static void GainRandomResourceNormal(XmlNode node){
		string[] possible_resources = {ItemType.FLINT, ItemType.HAY, ItemType.WOOD, ItemType.REDWOOD, ItemType.GOLD, ItemType.CRYSTAL,
		ItemType.TEETH};
		System.Random random = new System.Random ();
		int idx = random.Next(0, possible_resources.Length);
		int count = random.Next (1, 30);
		Resource item = ItemFactory.BuildResource (possible_resources [idx], count);
		Game.Current.Hero.gains (item);
		Game.Current.AddLog (Lang.Current ["luck_pick"] + item.Name + "*" + item.Count);
	}


	public static void GainRandomResourceRare(XmlNode node){
		string[] possible_resources = {ItemType.FEATHER,ItemType.PIG_TAIL, ItemType.FLOWER, ItemType.WINTER_WOOD,
        ItemType.BLUE_CRYSTAL, ItemType.RED_CRYSTAL};
		System.Random random = new System.Random ();
		int idx = random.Next(0, possible_resources.Length);
		int count = random.Next (1, 3);
		Resource item = ItemFactory.BuildResource (possible_resources [idx], count);
		Game.Current.Hero.gains (item);
		Game.Current.AddLog (Lang.Current ["luck_pick"] + item.Name + "*" + item.Count);
	}

	public static void GainRandomTool(XmlNode node){
		string[] possible_tool = {ItemType.AXE, ItemType.GOLD_AXE, ItemType.CHISEL, ItemType.WOOD_TRAP, ItemType.IRON_SHOVEL, ItemType.TORCH};
		System.Random random = new System.Random ();
		int idx = random.Next(0, possible_tool.Length);
		Tool item = ItemFactory.Get (possible_tool [idx]) as Tool;
		Game.Current.Hero.gains (item);
		Game.Current.AddLog (Lang.Current ["luck_pick"] + item.Name);
	}

    
    public static void AddGlobalState(XmlNode node)
    {
        string type = node["Type"].InnerText;
        float duration = float.Parse(node["Duration"].InnerText);
        GlobalState state = new GlobalState(duration, type, 0f);
        Game.Current.Hero.AddGlobalState(state);
    }


    public static void LooseRandomResource(XmlNode node){
        Resource res = ReduceRandomResource();
        Game.Current.AddLog (Lang.Current ["unlucky_stolen"] + res.Name + "*" + res.Count);
	}

    public static void LooseRandomResourceFromWet(XmlNode node)
    {
        Resource res = ReduceRandomResource();
        Game.Current.AddLog(res.Name + "*" + res.Count + Lang.Current["wet_broken"]);
    }

	public static void LooseRandomFood(XmlNode node){
        Food food = ReduceRandomFood();
        Game.Current.AddLog (Lang.Current ["unlucky_eaten"] + food.Name + "*" + food.Count);
	}

    public static void LooseRandomFoodFromHeat(XmlNode node)
    {
        Food food = ReduceRandomFood();
        Game.Current.AddLog(food.Name + "*" + food.Count + Lang.Current["hot_broken"]);
    }


    public static void IgnoreVulture(XmlNode node)
    {
        Food food = ReduceRandomFood();
        Game.Current.AddLog(Lang.Current["vulture_attack_food"] + food.Name + "*" + food.Count);
    }



    public static void ScholarTakeSeed(XmlNode node){
		float c = UnityEngine.Random.value;
		if ( UnityEngine.Random.value <= 0.5f) {
			Game.Current.AddLog(Lang.Current["cons_scholar_take_one_seed"]);
		} else {
			Item reward;

            if (c <= 0.3f){
				reward = ItemFactory.BuildScroll(ItemType.SCROLL_LOG_SUIT);	
			}else if (c <= 0.5f){
				reward = ItemFactory.BuildResource(ItemType.BLUE_CRYSTAL, (int)UnityEngine.Random.Range(1,5));
			}else if(c <= 0.7f){
				reward = ItemFactory.BuildResource(ItemType.RED_CRYSTAL, (int)UnityEngine.Random.Range(1, 5));
			}else{
                reward = ItemFactory.Get(ItemType.LOG_SUIT);
			}
			Game.Current.Hero.gains(reward);
			Game.Current.AddLog(Lang.Current["cons_scholar_take_one_seed_with_reward"]
			                              + reward.Name);
		}
	}


    public static void HealInjuredHunter(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;

        Resource souls = ItemFactory.BuildResource(ItemType.SOUL, 5);
        if (UnityEngine.Random.value <= 0.5f)
        {
            if (hero.has(souls))
            {
                hero.loses(souls);
                Game.Current.AddLog(Lang.Current["cons_hunter_leave"]);

            }
            else
            {
                Game.Current.AddToast(souls.Name + Lang.Current["not_enough"]);
            }

        }
        else
        {
            if (hero.has(souls))
            {
                hero.loses(souls);
                Game.Current.AddLog(Lang.Current["cons_hunter_leave_with_reward"]);
                hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_ULTI_ARMOR_BREAK));
            }
            else
            {
                Game.Current.AddToast(souls.Name + Lang.Current["not_enough"]);
            }

        }

    }



    public static void ConsShadowInFog(XmlNode node){
		float c = UnityEngine.Random.value;
        int days = Mathf.FloorToInt(Game.Current.GameTime / Config.SecondsPerDay);

        if (c <= 0.5f) {
			Game.Current.AddLog(Lang.Current["cons_cannot_follow"]);
		} else {
			Game.Current.AddLog(Lang.Current["cons_meet_mist_monster"]);

            if(days <= 35)
            {
                StartDelayBattle(MonsterType.FOG_DAEMON, 2);
            }
            else if(days <= 55)
            {
                StartDelayBattle(MonsterType.FOG_DAEMON_2, 2);
            }
            else
            {
                StartDelayBattle(MonsterType.FOG_DAEMON_3, 2);
            }
        }
	}


	public static void ConsLostSoldiers(XmlNode node){
		float c = UnityEngine.Random.value;
		Item reward;
		if (UnityEngine.Random.value <= 0.5f) {
			Game.Current.AddLog (Lang.Current ["cons_soldier_took_30_potato"]);
		} else {
			if (c <= 0.6f) {
				reward = ItemFactory.Get(ItemType.CRYSTAL_SWORD);
			} else if (c <= 0.7f) {
				reward = ItemFactory.Get(ItemType.IRON_SUIT);

			} else if (c <= 0.8f) {
				reward = ItemFactory.Get(ItemType.IRON_SWORD);

			} else {
				reward = ItemFactory.BuildScroll(ItemType.SCROLL_CRYSTAL_SWORD);
			}
			Game.Current.Hero.gains(reward);
			Game.Current.AddLog(Lang.Current["cons_soldier_took_potato_with_reward"]
			                              + reward.Name);
		}
	}


	public static void ConsOpenSnowBox(XmlNode node){
		float c = UnityEngine.Random.value;
		Item reward;
		if (UnityEngine.Random.value <= 0.5f) {
			Game.Current.AddLog (Lang.Current ["cons_nothing_inbox"]);
		} else {
			if (c <= 0.5f) {
				reward = ItemFactory.BuildResource(ItemType.ICE_CORE,1);
			} else {
				reward = ItemFactory.BuildResource (ItemType.BLUE_CRYSTAL,3);
			}
			Game.Current.Hero.gains(reward);
			Game.Current.AddLog(Lang.Current["cons_box_contains"]
			                              + reward.Name);
		}
	}



	public static void ConsOpenMudBox(XmlNode node){
		float c = UnityEngine.Random.value;
		Item reward;
		if (UnityEngine.Random.value <= 0.5f) {
			Game.Current.AddLog (Lang.Current ["cons_nothing_inbox"]);
		} else {
			if (c <= 0.3f) {
				reward = ItemFactory.BuildResource(ItemType.FIRE_CORE,1);
			} else if(c<=0.6f){
				reward = ItemFactory.BuildResource (ItemType.RED_CRYSTAL,3);
			}
			else {
				reward = ItemFactory.BuildResource (ItemType.WIND_CORE,1);
			}
			Game.Current.Hero.gains(reward);
			Game.Current.AddLog(Lang.Current["cons_box_contains"]
			                              + reward.Name);
		}
	}


    public static void ConsGoodnessUp(XmlNode node)
    {
        Game.Current.Hero.Goodness += UnityEngine.Random.Range(0.8f, 1f);
    }


    public static void ConsGoodnessDown(XmlNode node)
    {
        Game.Current.Hero.Goodness -= UnityEngine.Random.Range(0.8f, 1f);
    }



  


    public static void LostRandomStuff(XmlNode node)
    {
        string[] resource_list = {ItemType.WOOL, ItemType.WOOD, ItemType.GOLD, ItemType.REDWOOD, ItemType.CRYSTAL, ItemType.GOLD,
        ItemType.TEETH};
        System.Random random = new System.Random();
        int idx = random.Next(0, resource_list.Length);
        Resource res = ItemFactory.BuildResource(resource_list[idx], (int)UnityEngine.Random.Range(1,10));
        Game.Current.Hero.loses(res);
    }


    public static void SearchDeadBody(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        if(UnityEngine.Random.value <= 0.5f)
        {
            //spawn monster
            CreateBattle(true, MonsterType.ALIVE_ZOMBIE, 1); 
        }
        else
        {
            float c = UnityEngine.Random.value;
            string[] resource_list = { ItemType.BLUE_CRYSTAL, ItemType.GOLD, ItemType.CRYSTAL,
            ItemType.RED_CRYSTAL, ItemType.REDWOOD, ItemType.WINTER_WOOD};
            string[] weapon_list = { ItemType.CRYSTAL_SWORD, ItemType.FISH_SWORD,
            ItemType.YOUNG_SWORD};
            string[] armor_list = { ItemType.FUR_SUIT, ItemType.FOX_SUIT,
            ItemType.TIGER_SUIT, ItemType.WOLF_SUIT};
            string[] accessory_list = { ItemType.TOOTH_NECKLACE, ItemType.FLOWER_RING,
            ItemType.FEATHER_HAT, ItemType.ALLIGATOR_BELT};
            System.Random random = new System.Random();
            if (c <= 0.6f)
            {
                Item reward = ItemFactory.BuildResource(resource_list[random.Next(0,resource_list.Length)], (int)UnityEngine.Random.Range(1f,15f));
                hero.gains(reward);
            }
            else if (c <= 0.75f)
            {
                Item reward = ItemFactory.Get(weapon_list[random.Next(0, weapon_list.Length)]);
                hero.gains(reward);
            }
            else if (c <= 0.85f)
            {
                Item reward = ItemFactory.Get(accessory_list[random.Next(0, accessory_list.Length)]);
                hero.gains(reward);
            }
            else
            {
                Item reward = ItemFactory.Get(armor_list[random.Next(0, armor_list.Length)]);
                hero.gains(reward);
            }
          
        }
    }


    public static void FightVipers(XmlNode node)
    {
        CreateBattle(true, MonsterType.VIPER, 3);
    }


    public static void KillBear(XmlNode node)
    {
        Food bear_paw = ItemFactory.Get(ItemType.BEAR_PAW) as Food;
        bear_paw.Count = 2;
        Game.Current.Hero.gains(bear_paw);
    }


    public  static void AskProfessor(XmlNode node)
    {
        ReduceRandomFood();
        string[] hints_1 = {Lang.Current["story_professor_hint_map1_1"], Lang.Current["story_professor_hint_map1_2"] };
        string[] hints_2 = { Lang.Current["story_professor_hint_map2_1"], Lang.Current["story_professor_hint_map2_2"] };

        int days = Mathf.FloorToInt(Game.Current.GameTime / Config.SecondsPerDay);
        System.Random rnd = new System.Random();
        if(days < 30)
        {
            Game.Current.AddLog(hints_1[rnd.Next(0,hints_1.Length)]);
        }
        else if (days < 50)
        {
            Game.Current.AddLog(hints_2[rnd.Next(0, hints_2.Length)]);

        }
        else
        {

        }

    }


    public static void ExchangePotato(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Food food = ItemFactory.Get(ItemType.POTATO) as Food;
        food.Count = 300;
        string[] reward_food_1 = {ItemType.FISH, ItemType.CRAP, ItemType.SHRIMP, ItemType.SNAKE_MEAT};
        string[] reward_food_2 = { ItemType.BEAR_PAW, ItemType.BAT_POISON, ItemType.FIG};

        string[] reward_weapons = {ItemType.CHUANJIA_SWORD, ItemType.SMALL_ICE_KNIFE, ItemType.NEW_FIRE_GUN};

        if (hero.has(food))
        {
            hero.loses(food);
            float c = UnityEngine.Random.value;
            System.Random rnd = new System.Random();
            if(c <= 0.08)
            {
                Weapon reward = ItemFactory.Get(reward_weapons[rnd.Next(0, reward_weapons.Length)]) as Weapon;
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name);
            }
            else if(c <= 0.38f)
            {
                int count = Mathf.RoundToInt(UnityEngine.Random.Range(1, 4));
                Food reward = ItemFactory.Get(reward_food_2[rnd.Next(0, reward_food_2.Length)]) as Food;
                reward.Count = count;
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name + "*" + reward.Count);
            }
            else
            {
                int count = Mathf.RoundToInt(UnityEngine.Random.Range(10, 20));
                Food reward = ItemFactory.Get(reward_food_1[rnd.Next(0, reward_food_1.Length)]) as Food;
                reward.Count = count;
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name + "*" + reward.Count);

            }
        }
        else
        {
            Game.Current.AddToast(food.Name + Lang.Current[ "not_enough"]);
        }
    }




    public static void ExchangeLamp(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Food food = ItemFactory.Get(ItemType.LAMP) as Food;
        food.Count = 300;
        string[] reward_res_1 = {ItemType.BLUE_CRYSTAL, ItemType.RED_CRYSTAL};
        string[] reward_res_2 = { ItemType.ICE_CORE, ItemType.FIRE_CORE, ItemType.WIND_CORE,
        ItemType.HOLY_CORE, ItemType.DARK_CORE, ItemType.SOUL};
        string[] reward_accessory = {ItemType.OLD_GOLD_RING, ItemType.SHINY_EAR_RING,
        ItemType.BROZEN_METAL};


        if (hero.has(food))
        {
            hero.loses(food);
            float c = UnityEngine.Random.value;
            System.Random rnd = new System.Random();

            if(c <= 0.1f)
            {
                Accessory reward = ItemFactory.Get(reward_accessory[rnd.Next(0, reward_accessory.Length)]) as Accessory;
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name);


            }
            else if (c <= 0.5f)
            {
                int count = Mathf.RoundToInt(UnityEngine.Random.Range(5, 10));
                Resource reward = ItemFactory.BuildResource(reward_res_1[rnd.Next(0, reward_res_1.Length)], count);
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name + "*" + reward.Count);

            }
            else
            {
                int count = Mathf.RoundToInt(UnityEngine.Random.Range(1, 4));
                Resource reward = ItemFactory.BuildResource(reward_res_2[rnd.Next(0, reward_res_2.Length)], count);
                hero.gains(reward);
                Game.Current.AddLog(Lang.Current["you_have_got"] + reward.Name + "*" + reward.Count);

            }
        }
        else
        {
            Game.Current.AddToast(food.Name + Lang.Current["not_enough"]);
        }
    }



    public static void GiveHunterFood(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Food food = ReduceRandomFood();
        Game.Current.AddLog(Lang.Current["given_hunter"] + food.Name + "*" + food.Count);
        if(UnityEngine.Random.value <= 0.5f)
        {
            Scroll s = ItemFactory.BuildScroll(ItemType.SCROLL_QUMO_SWORD);
            hero.gains(s);
            Game.Current.AddLog(Lang.Current["hunter_grateful"]);    
        }
    }


    public static void OfferBlessing(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource res = ReduceRandomResource();
        Game.Current.AddLog(Lang.Current["you_offered"] + " " + res.Name + "*" + res.Count);

        if(UnityEngine.Random.value <= 0.35f)
        {
            Accessory a = ItemFactory.Get(ItemType.BLESSING_RING) as Accessory;
            hero.gains(a);
            Game.Current.AddLog(Lang.Current["holy_rewarded_with_blessing"]);
        }
        else
        {
            Game.Current.AddLog(Lang.Current["holy_left"]);
        }
       


    }

    public static void AcceptBlessing(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Game.Current.AddLog(Lang.Current["you_are_blessed"]);

        if(UnityEngine.Random.value < 0.5f)
        {
            GlobalState state = new GlobalState(1 * Config.SecondsPerDay, StateType.STATE_MUSIC_BOX, 0f);
            hero.AddGlobalState(state);
        }
      
    }

    public static void RobRichMan(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        hero.CurrentHealth = 5f;

        Armor a = ItemFactory.Get(ItemType.GLORIOUS_SUIT) as Armor;
        hero.gains(a);
        Game.Current.AddLog(Lang.Current["robbed_from_rich_man"] + a.Name);
    }


    public static void BegRichMan(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource r = ItemFactory.BuildResource(ItemType.RED_CRYSTAL, 10);
        hero.gains(r);

        r = ItemFactory.BuildResource(ItemType.BLUE_CRYSTAL, 10);
        hero.gains(r);
        Game.Current.AddLog(Lang.Current["rich_man_give"]);
    }


    public static void StealHoney(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Food Honey = ItemFactory.Get(ItemType.HONEY) as Food;
        Honey.Count = 20;
        GlobalState state = new GlobalState(3*Config.SecondsPerDay, StateType.STATE_BEE_POISON, 0f);
        hero.AddGlobalState(state);
        hero.gains(Honey);
        Game.Current.AddLog(Lang.Current["stoled_honey"]);
        
    }


    public static void SacrificeElfToWitch(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        if(UnityEngine.Random.value <= 0.3f)
        {
            //get the scroll
            Scroll reward = ItemFactory.BuildScroll(ItemType.SCROLL_DEMON_ENERGY_SUIT);
            hero.gains(reward);
            Game.Current.AddLog(Lang.Current["witch_give_reward"] + reward.Name);
        }
        else
        {
            //get the weapon
            Armor reward = ItemFactory.Get(ItemType.SCROLL_DARK_DEMON_SWORD) as Armor;
            hero.gains(reward);
            Game.Current.AddLog(Lang.Current["witch_give_reward"] + reward.Name);
        }

    }
    


    //consequences for map events---------------------------------------------------
    public static void DigGrave(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		if (!hero.isItemEquipped (ItemType.IRON_SHOVEL)) {
			Game.Current.AddToast(Lang.Current["need_iron_shovel"]);
		} else {
            bool toolUseUp;
            hero.UseEquippedTool(0.1f, out toolUseUp);
			float c = UnityEngine.Random.value;
            if(c <= 0.05f)
            {
                Item reward = ItemFactory.BuildResource(ItemType.RED_CRYSTAL,2);

                hero.gains(reward);
            }
            else if ( c <= 0.1f){
				Item reward = ItemFactory.BuildResource(ItemType.RED_CRYSTAL,2);

				hero.gains(reward);
            }
			else if (c <= 0.2f){
				Item reward = ItemFactory.BuildResource(ItemType.REDWOOD, 30);
                hero.gains(reward);
			}
			else if(c <= 0.3f){
				Item reward = ItemFactory.BuildResource(ItemType.SOUL,2);
                hero.gains(reward);
			}
			else{
				string monster_type = node ["MonsterType"].InnerText;
				int count = int.Parse(node ["Count"].InnerText);
                CreateBattle(false, monster_type, count);
            }
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            hero.CurrentMapEvent.SetMapEventFinished() ; //finished the event, the spot on the map will be invalidated
        }
		
	}

	public static void DigMine(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		if (!hero.isItemEquipped (ItemType.GOLD_CHISEL)) {
			Game.Current.AddToast(Lang.Current["need_gold_chisel"]);
		} else {
            bool toolUseUp;
            hero.UseEquippedTool(0.1f, out toolUseUp);
			float c = UnityEngine.Random.value;
			if( c <= 0.1f){
				Item reward = ItemFactory.BuildResource(ItemType.BLUE_CRYSTAL,3);
				hero.gains(reward);
			}

			else if( c <= 0.2f){
				Item reward = ItemFactory.BuildResource(ItemType.RED_CRYSTAL,3);
				hero.gains(reward);
			}
			else if (c <= 0.4f){
				Item reward = ItemFactory.BuildResource(ItemType.GOLD, 100);
				
				hero.gains(reward);
			}
			else if(c <= 0.6f){
				Item reward = ItemFactory.BuildResource(ItemType.CRYSTAL,60);
				hero.gains(reward);
			}
			else{
                CreateBattle(false, MonsterType.STONE_PUPPY, 2);
            }
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated

        }
    }


    
    public static void FishInThePool(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        if (!hero.isItemEquipped(ItemType.FISHING_ROD))
        {
            Game.Current.AddToast(Lang.Current["need_tool"] + Lang.Current[ItemType.FISHING_ROD] + "!");
        }
        else
        {
            bool toolUseUp;
            hero.UseEquippedTool(0.1f, out toolUseUp);
            float c = UnityEngine.Random.value;
            if (c <= 0.3f)
            {
                Food reward = ItemFactory.Get(ItemType.FISH) as Food;
                reward.Count = 10;
                hero.gains(reward);
            }
            else if (c <= 0.55f)
            {
                Food reward = ItemFactory.Get(ItemType.SHRIMP) as Food;
                reward.Count = 10;
                hero.gains(reward);
            }
            else if (c <= 0.7f)
            {
                Food reward = ItemFactory.Get(ItemType.CRAP) as Food;
                reward.Count = 10;
                hero.gains(reward);
            }
            else if(c <= 0.73f)
            {
                Item reward = ItemFactory.Get(ItemType.FISH_SWORD);
                hero.gains(reward);
            }
            else if(c < 0.74f)
            {
                Item reward = ItemFactory.Get(ItemType.DEEP_FISH_SWORD);
                hero.gains(reward);
            }
            else if(c < 0.78f)
            {
                Item reward = ItemFactory.BuildScroll(ItemType.SCROLL_IRON_SUIT);
                hero.gains(reward);
            }
            else
            {
                CreateBattle(false, MonsterType.DEEP_MONSTER_FISH, 1);
            }

            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated

        }
    }


    public static void MakeWithBlackSmith(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        string[] normal_gears = {ItemType.NORMAL_BLACK_IRON_SWORD, ItemType.NORMAL_BLACK_IRON_SUIT, ItemType.NORMAL_BLACK_IRON_NECKLACE};
        string[] uncommon_gears = {ItemType.SPECIAL_BLACK_IRON_NECKLACE, ItemType.SPECIAL_BLACK_IRON_SUIT, ItemType.SPECIAL_BLACK_IRON_SWORD};
        string[] rare_gears = {ItemType.MASTER_BLACK_IRON_NECKLACE, ItemType.MASTER_BLACK_IRON_SUIT, ItemType.MASTER_BLACK_IRON_SWORD};

        Resource item = ItemFactory.BuildResource(ItemType.BLACK_IRON, 5);
        if (hero.has(item))
        {
            hero.loses(item);
            float c = UnityEngine.Random.value;
            System.Random rnd = new System.Random();
            if (c <= 0.6f)
            {
                hero.gains(ItemFactory.Get(normal_gears[rnd.Next(0, normal_gears.Length)]));
            }
            else if (c <= 0.9f)
            {
                hero.gains(ItemFactory.Get(uncommon_gears[rnd.Next(0, normal_gears.Length)]));
            }
            else
            {
                hero.gains(ItemFactory.Get(rare_gears[rnd.Next(0, normal_gears.Length)]));
            }
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }
  
    }

    
    public static void TradeCrystalSuit(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.FISH_HAND, 10);
        if (hero.has(item))
        {
            hero.loses(item);
            hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_CRYSTAL_SUIT));
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }
    }

    public static void TradeFireTeeth(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.FIRE_TEETH, 10);
        if (hero.has(item))
        {
            hero.loses(item);
            hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_NORMAL_SWORD));
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }
    }





    public static void ChopWinterWood(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        if (!hero.isItemEquipped(ItemType.GOLD_AXE))
        {
            Game.Current.AddToast(Lang.Current["need_tool"] + Lang.Current[ItemType.GOLD_AXE] + "!");
        }
        else
        {
            bool toolUseUp;
            hero.UseEquippedTool(0.08f, out toolUseUp);
            float c = UnityEngine.Random.value;
            if (c <= 0.7f)
            {
                Item reward = ItemFactory.BuildResource(ItemType.WINTER_WOOD, (int)UnityEngine.Random.Range(10,23));
                hero.gains(reward);
            }
            else
            {
                CreateBattle(false, MonsterType.TREE_MONSTER, 1);
            }
 
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated

        }
    }




	public static void FightYoungWitch(XmlNode node){

        MainCharacter hero = Game.Current.Hero;
        if(hero.UserInventory.Get(ItemType.SCROLL_ULTI_WIND_FAST,"Scroll") == null)
        {
            //
            CreateBattle(false, MonsterType.YOUNG_WITCH_WIND, 1);
        }else if (hero.UserInventory.Get(ItemType.SCROLL_ULTI_FIRE_STUN, "Scroll") == null)
        {
            CreateBattle(false, MonsterType.YOUNG_WITCH_FIRE, 1);
        }
        else
        {
            CreateBattle(false, MonsterType.YOUNG_WITCH_ICE, 1);
        }


        hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated

	}


	public static void EnterYetiHouse(XmlNode node){
		MainCharacter hero = Game.Current.Hero;

		if (hero.UserClimate.theSeason != Season.Winter) {
			Game.Current.AddToast(Lang.Current["yeti_house_need_winter"]);
		} else {
            CreateBattle(false, MonsterType.YETI, 1);
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated
		}
	}

    public static void OpenYetiBox(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource ice_key = ItemFactory.BuildResource(ItemType.YETI_KEY, 1);
        Resource fire_key = ItemFactory.BuildResource(ItemType.FIRE_KEY, 1);
        Resource wind_key = ItemFactory.BuildResource(ItemType.WIND_KEY, 1);
    
        if (hero.has(ice_key)
            && hero.has(fire_key)
            && hero.has(wind_key))
        {
            Game.Current.Hero.AllMaps.Add(MapBuilder.CreateMap(MapType.SWAMP));
            Game.Current.AddToast(Lang.Current["gain_map"] +": " + Lang.Current[MapType.SWAMP]);
            hero.CurrentMapEvent.SetMapEventFinished(); //finished the event, the spot on the map will be invalidated
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            hero.loses(ice_key);
            hero.loses(fire_key);
            hero.loses(wind_key);
        }
        else
        {
            Game.Current.AddToast(Lang.Current["need"] + " " +  ice_key.Name +
                "," + fire_key.Name + "," + wind_key.Name);
        }

    }



    public static void ExchangeSoul(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		if (hero.CurrentHealth <= hero.HealthUpperLimit * 0.8f) {
			Game.Current.AddToast (Lang.Current ["need_enough_health"]);
		} else {
			Game.Current.Hero.CurrentHealth -= hero.HealthUpperLimit * 0.8f;
			Item Soul = ItemFactory.BuildResource(ItemType.SOUL,5);
			hero.gains(Soul);
            hero.CurrentMapEvent.SetMapEventFinished();
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }

	}


	public static void LifeWaterHeal(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		hero.CurrentHealth = hero.HealthUpperLimit; //restore to upper limit
		Game.Current.AddToast (Lang.Current ["restored_full_health"]);
        hero.CurrentMapEvent.SetMapEventFinished();
        GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);


    }

    public static void LifeWaterTake(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		Food water = (Food)ItemFactory.Get (ItemType.LIFE_WATER);
		water.Count = 3;
		hero.gains (water);
        hero.CurrentMapEvent.SetMapEventFinished();
        GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);	
	}


	public static void DealSoul(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		Resource soul = ItemFactory.BuildResource (ItemType.SOUL, 1);
		if (hero.has (soul, false)) {
			hero.loses(soul);
			float c = UnityEngine.Random.value;
			Item reward;
			if( c <= 0.1f){
				reward  = ItemFactory.BuildResource(ItemType.RED_CRYSTAL, 5);

			}else if(c <= 0.2f){
				reward  = ItemFactory.BuildResource(ItemType.BLUE_CRYSTAL, 5);

			}else if(c <= 0.3f){
                reward = ItemFactory.Get(ItemType.FUR_SUIT);
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.LUCKY_DEAL_SOUL);
			}else if(c <= 0.4f){
                reward = ItemFactory.Get(ItemType.FLOWER_RING);
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.LUCKY_DEAL_SOUL);
            }
            else if(c <= 0.5f){
                reward = ItemFactory.Get(ItemType.FEATHER_HAT);
                Achievement.Current.UnlockAchievement(Achievement.AchievementType.LUCKY_DEAL_SOUL);
            }
            else if(c <= 0.6f){

                reward  = ItemFactory.BuildResource(ItemType.CRYSTAL, 50);
				
			}else if(c <= 0.7f){
				reward  = ItemFactory.BuildResource(ItemType.GOLD, 100);
			}else{
				reward  = ItemFactory.BuildResource(ItemType.IRON, 200);
			}

			hero.gains(reward);
            hero.CurrentMapEvent.SetMapEventFinished();
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else {
			Game.Current.AddToast (Lang.Current ["not_enough_soul"]);
		}
	}

	public static void LaunchBattleMapEvent(XmlNode node){
		MainCharacter hero = Game.Current.Hero;
		string monster_type = node ["MonsterType"].InnerText;
		int count = int.Parse(node ["Count"].InnerText);
        bool need_finish = bool.Parse(node["NeedFinish"].InnerText);
        if (need_finish)
        {
            hero.CurrentMapEvent.SetMapEventFinished();
        }
        CreateBattle(false, monster_type, count);
	}




    public static void OpenHighAltar(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource light_flower = ItemFactory.BuildResource(ItemType.HIGH_SKY_FLOWER, 5);
        if (hero.has(light_flower))
        {
            hero.CurrentMapEvent.SetMapEventFinished();
            Game.Current.AddToast(Lang.Current["high_altar_opened"]);
            Game.Current.AddLog(Lang.Current["high_altar_opened"]);
            Game.Current.Hero.UserConstructions.Add(BuildingFactory.Get(BuildingType.EVIL_CHAMBER));
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else
        {
            Game.Current.AddToast(light_flower.Name + Lang.Current["not_enough"] + "!");

        }

    }


    public static void BreakIceCave(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource ice_stone = ItemFactory.BuildResource(ItemType.ICE_STONE, 1);
        hero.gains(ice_stone);
        hero.UserClimate.WeatherToday = Weather.Snow;
        hero.UserClimate.BaseTempature = -5;
        hero.CurrentMapEvent.SetMapEventFinished();
        GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
    }

    public static void AttracksIceBeast(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource ice_stone = ItemFactory.BuildResource(ItemType.ICE_STONE, 3);
        if (hero.has(ice_stone))
        {
            hero.loses(ice_stone);
            //lunch battle
            CreateBattle(false, MonsterType.ICE_BEAST, 1);
        }
        else
        {
            Game.Current.AddToast(ice_stone.Name + Lang.Current["not_enough"] + "!");
        }
    }


    public static void ForgeLightFlower(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource amber = ItemFactory.BuildResource(ItemType.FIRE_AMBER, 3);
        Resource light_flower = ItemFactory.BuildResource(ItemType.HIGH_SKY_FLOWER, 1);
        if (hero.has(amber))
        {
            hero.loses(amber);
            hero.gains(light_flower);
            hero.CurrentMapEvent.SetMapEventFinished();
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else
        {
            Game.Current.AddToast(amber.Name + Lang.Current["not_enough"] + "!");
        }
    }

    public static void TradeLightFlower(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.BAT_WING, 3);
        Resource light_flower = ItemFactory.BuildResource(ItemType.HIGH_SKY_FLOWER, 1);
        if (hero.has(item))
        {
            hero.loses(item);
            hero.gains(light_flower);
            hero.CurrentMapEvent.SetMapEventFinished();
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }
    }

    public static void TakeRewardFromSavedMan(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Scroll re = ItemFactory.BuildScroll(ItemType.SCROLL_METAL_NECKLACE);
        hero.gains(re);
        hero.Goodness -= 1f;
        hero.CurrentMapEvent.SetMapEventFinished();
        GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
    }

    public static void TradeWithReasearchMan(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item1 = ItemFactory.BuildResource(ItemType.DUST, 5);
        Resource item2 = ItemFactory.BuildResource(ItemType.LAVA, 5);

        if (hero.has(item1))
        {
            if (hero.has(item2))
            {
                hero.loses(item1);
                hero.loses(item2);
                Scroll s1 = ItemFactory.BuildScroll(ItemType.SCROLL_METAL_SWORD);
                Scroll s2 = ItemFactory.BuildScroll(ItemType.SCROLL_METAL_SUIT);

                if (hero.has(s1))
                {
                    hero.gains(s2);
                }
                else
                {
                    hero.gains(s1);
                }
                hero.CurrentMapEvent.SetMapEventFinished();
                GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
            }
            else
            {
                Game.Current.AddToast(item2.Name + Lang.Current["not_enough"] + "!");
            }
        }
        else
        {
            Game.Current.AddToast(item1.Name + Lang.Current["not_enough"] + "!");

        }

    }



    public static void FillDeadSpirit(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.FAT_SOUL, 1);
        hero.CurrentHealth -= hero.HealthUpperLimit * 0.5f;
        hero.gains(item);
        hero.CurrentMapEvent.SetMapEventFinished();
        GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);
    }

    public static void SummonSkelectionKing(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.FAT_SOUL, 3);
        if (hero.has(item))
        {
            hero.loses(item);
            CreateBattle(false, MonsterType.SKELETON_KING, 1);
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }

    }

    public static void PullLightWater(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.SOUL, 3);
        Resource water = ItemFactory.BuildResource(ItemType.LIGHT_WATER, 1);
        if (hero.has(item))
        {
            hero.loses(item);
            hero.gains(water);
            hero.CurrentMapEvent.SetMapEventFinished();
            GameObject.Destroy(GameObject.Find("Player").GetComponent<MapController>().CurrentEventObject);

        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");

        }

    }


    public static void SummonLightFete(XmlNode node)
    {
        MainCharacter hero = Game.Current.Hero;
        Resource item = ItemFactory.BuildResource(ItemType.LIGHT_WATER, 3);

        if (hero.has(item))
        {
            hero.loses(item);
            CreateBattle(false, MonsterType.LIGHT_FETE, 1);
        }
        else
        {
            Game.Current.AddToast(item.Name + Lang.Current["not_enough"] + "!");
        }
    }







    //utility--------------------------------------------------------------------------->
    public static void CreateBattle(bool isAtHome, String monster_type, int count)
    {

        List<Monster> monsters = new List<Monster>();
        for (int i = 0; i < count; i++)
        {
            monsters.Add(MonsterBuilder.BuildMonster(monster_type));
        }

        Game.Current.Hero.CurrentBattleMonsters = monsters;
        if (isAtHome)
        {
           
            Game.Current.ActionEngine.DestroyAllViewIndexing();
            SceneManager.LoadScene(3);
        }
        else
        {
            //not at home, start battle with a loading
            MapController mc = GameObject.Find("Player").GetComponent<MapController>();
            mc.LoadWithLoadingScreen(3);
        }
    }


//may need to add a not at home option for start delay battle
    public static void StartDelayBattle(string monster_type, int count)
    {
        List<Monster> monsters = new List<Monster>();
        for (int i = 0; i < count; i++)
        {
            monsters.Add(MonsterBuilder.BuildMonster(monster_type));
        }
        Game.Current.Hero.CurrentBattleMonsters = monsters;
        //.Current.ActionEngine.DestroyAllViewIndexing();
        GameController GC = GameObject.Find("GM*").GetComponent<GameController>();
        GC.Invoke("LoadBattle", 1.5f);
    }


    public static Food ReduceRandomFood()
    {
        List<Food> all_food = Game.Current.Hero.UserInventory.AllFood;
        System.Random random = new System.Random();
        int idx = random.Next(0, all_food.Count);
        Debug.LogError("Food indx : " + idx);
        int count;
        if(all_food[idx].Count > 3)
        {
            count = random.Next(1, all_food[idx].Count / 3);
        }
        else
        {
            count = 1;
        }
        if (count > 30)
        {
            count = 29; //max loose this much
        }
        string type = all_food[idx].Type;

        Food food = ItemFactory.Get(type) as Food;
        food.Count = count;
        Game.Current.Hero.loses(food);
        return food;
    }

    public static Resource ReduceRandomResource()
    {
        List<Resource> all_res = Game.Current.Hero.UserInventory.AllResources;
        System.Random random = new System.Random();
        int idx = random.Next(0, all_res.Count);
        int count;
        if(all_res[idx].Count > 3)
        {
            count = random.Next(1, all_res[idx].Count / 3);
        }
        else{
            count = 1;
        }

        if (count > 30)
        {
            count = 29; //max loose this much
        }
        string type = all_res[idx].Type;
        string[] special_items = {ItemType.YETI_KEY, ItemType.FIRE_KEY,
        ItemType.WIND_KEY, ItemType.HIGH_SKY_FLOWER, ItemType.ICE_STONE,
        ItemType.FAT_SOUL, ItemType.LIGHT_WATER,
        ItemType.BAT_WING, ItemType.FIRE_AMBER};
        if (Array.IndexOf(special_items,type) != -1)
        {
            type = ItemType.BRANCH;
            count = 1;
        }
        Resource item_to_lose = ItemFactory.BuildResource(type, count);
        Game.Current.Hero.loses(item_to_lose);
        return item_to_lose;
    }




}

