public class GameFactory
{
	// generate event/item/building for testing.
	public static void GenerateTestData(){

		
        for(int i = 0; i < Config.AllWeapons.Length; i++)
        {
            Game.Current.Hero.gains(ItemFactory.Get(Config.AllWeapons[i]));

        }

        for (int i = 0; i < Config.AllArmors.Length; i++)
        {
            Game.Current.Hero.gains(ItemFactory.Get(Config.AllArmors[i]));

        }

        for (int i = 0; i < Config.AllAccessories.Length; i++)
        {
            Game.Current.Hero.gains(ItemFactory.Get(Config.AllAccessories[i]));

        }



        Game.Current.Hero.gains(ItemFactory.Get(ItemType.GRASS_SUIT));

		Game.Current.Hero.UserInventory.Add (ItemFactory.Get(ItemType.TOOTH_NECKLACE));
	
	
		Map dust_land = MapBuilder.CreateMap (MapType.DUSTLAND);
		Game.Current.Hero.AllMaps.Add (dust_land);
        Game.Current.Hero.AllMaps.Add(MapBuilder.CreateMap(MapType.SWAMP));

		Item hay = ItemFactory.BuildResource (ItemType.HAY, 1000);
		Item flint = ItemFactory.BuildResource (ItemType.FLINT, 1000);
		Item branch = ItemFactory.BuildResource (ItemType.BRANCH, 1000);
		Item soul = ItemFactory.BuildResource (ItemType.SOUL, 1000);



		
		Food milk_shake = ItemFactory.Get (ItemType.FRUIT_MILLK_SHAKE) as Food;
		milk_shake.Count = 3;
		Food carrot = ItemFactory.Get (ItemType.TOMATO) as Food;
		carrot.Count = 100;
		Food potato = ItemFactory.Get (ItemType.POTATO) as Food;
		potato.Count = 1000;
		Food mushroom = ItemFactory.Get (ItemType.MUSHROOM) as Food;
		mushroom.Count = 100;
		Food tomato = ItemFactory.Get (ItemType.APPLE) as Food;
		tomato.Count = 100;
		Food lamp = ItemFactory.Get (ItemType.LAMP) as Food;
		lamp.Count = 1000;
		Food rabbit_meat = ItemFactory.Get (ItemType.RABBIT_MEAT) as Food;
		rabbit_meat.Count = 100;
		Food beef = ItemFactory.Get (ItemType.BEEF) as Food;
		beef.Count = 100;
		Food fish = ItemFactory.Get (ItemType.FISH) as Food;
		fish.Count = 100;
		Food shrimp = ItemFactory.Get (ItemType.SHRIMP) as Food;
		shrimp.Count = 100;
		Food crap = ItemFactory.Get (ItemType.CRAP) as Food;
		crap.Count = 100;
		Food milk = ItemFactory.Get (ItemType.MILK) as Food;
		milk.Count = 100;
		Food bat_poison = ItemFactory.Get (ItemType.BAT_POISON) as Food;
		bat_poison.Count = 100;
        Food bear_paw = ItemFactory.Get(ItemType.BEAR_PAW) as Food;
        bear_paw.Count = 100;
        Food t_w_m = ItemFactory.Get(ItemType.TWICE_COOKED_MEAT) as Food;
        t_w_m.Count = 100;

        Food snake_blood = ItemFactory.Get(ItemType.SNAKE_MEAT) as Food;
        snake_blood.Count = 100;

        Food fruit_c = ItemFactory.Get(ItemType.DREAM_FRUIT) as Food;
        fruit_c.Count = 5;

        Food honey = ItemFactory.Get(ItemType.HONEY) as Food;
        honey.Count = 100;

        Food snow_ball = ItemFactory.Get(ItemType.SNOW_BALL) as Food;
        snow_ball.Count = 100;



        Game.Current.Hero.gains(honey);
        Game.Current.Hero.gains(snow_ball);
        Game.Current.Hero.gains (t_w_m);
        Game.Current.Hero.gains (carrot);
        Game.Current.Hero.gains (potato);
        Game.Current.Hero.gains (mushroom);
        Game.Current.Hero.gains (tomato);
        Game.Current.Hero.gains (lamp);
        Game.Current.Hero.gains (rabbit_meat);
        Game.Current.Hero.gains (beef);
        Game.Current.Hero.gains (fish);
        Game.Current.Hero.gains (shrimp);
        Game.Current.Hero.gains (crap);
        Game.Current.Hero.gains (milk);
        Game.Current.Hero.gains (bear_paw);
        Game.Current.Hero.gains(fruit_c);
        Game.Current.Hero.gains(snake_blood);

        Food f = ItemFactory.Get(ItemType.SEAFOOD_SALAD) as Food;
        f.Count = 100;
        Game.Current.Hero.gains(f);
        f = ItemFactory.Get(ItemType.CHILI_CRAP) as Food;
        f.Count = 100;
        Game.Current.Hero.gains(f);


        //Game.Current.Hero.gains (camp_fire);
        Game.Current.Hero.gains (milk_shake);
        Game.Current.Hero.gains (bat_poison);


        Game.Current.Hero.gains(hay);
        Game.Current.Hero.gains(flint);
        Game.Current.Hero.gains(branch);
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.WOOL, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.PIG_TAIL, 1000));

        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.ICE_CORE, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.WIND_CORE, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.FIRE_CORE, 1000));

        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.DARK_CORE, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.HOLY_CORE, 1000));

        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.GOLD, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.REDWOOD, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.CRYSTAL, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.BLUE_CRYSTAL, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.RED_CRYSTAL, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.IRON, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.WOLF_SKIN, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.FIRE_TEETH, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.FISH_HAND, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.YETI_KEY, 1));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.FIRE_KEY, 1));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.WIND_KEY, 1));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.ALLIGATOR_SKIN, 1));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.TEETH, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.LAVA, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.DUST, 1000));
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.FLOWER, 1000));






      






	//	Game.Current.Hero.gains (cold_machine);
	//	Game.Current.Hero.gains (warm_machine);
//		Game.Current.Hero.gains (dry_machine);
//		Game.Current.Hero.gains (humid_machine);
		Game.Current.Hero.gains (ItemFactory.Get (ItemType.IRON_SHOVEL));
        Game.Current.Hero.gains(ItemFactory.Get(ItemType.MUSIC_BOX));

        Game.Current.Hero.gains (soul);
        //Scroll s = ItemFactory.Get (ItemType.SCROLL_CRYSTAL_SWORD) as Scroll;
        //Game.Current.Hero.gains (s);
        Game.Current.Hero.gains(ItemFactory.BuildResource(ItemType.HIGH_SKY_FLOWER, 5));

        
        for(int i = 0; i < Config.AllSkills.Length; i++)
        {
            Game.Current.Hero.LearntUltiSkills.Add((PlayerUltiSkill)SkillBuilder.BuildSkill(Config.AllSkills[i]));
        }
	
        Game.Current.Hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_FUR_SUIT));
        Game.Current.Hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_FLOWER_RING));
        Game.Current.Hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_ULTI_HOLY_SPIRIT));
        Game.Current.Hero.gains(ItemFactory.BuildScroll(ItemType.SCROLL_ULTI_ARMOR_BREAK));
        Game.Current.Hero.UserConstructions.Add(BuildingFactory.Get(BuildingType.EVIL_CHAMBER));

        Game.Current.Hero.gains(ItemFactory.Get(ItemType.FISH_MADE_FAN));

        Shop.Current.AddProduct(ItemType.REVIVE_STONE, 100);
        //Add newlly added events from xml
        //Game.Current.AddEvent (Config.SpawnEventTriggerType, EventFactory.BuildEvent (potential_unlockable, clazz));
    }
}

