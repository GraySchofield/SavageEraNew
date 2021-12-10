using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapBuilder {
	public static Map CreateMap(string type){
        Map map = null;

		switch(type){
		case MapType.DUSTLAND:
            map = CreateMapDustLand();
            break;

        case MapType.SWAMP:
            map = CreateMapSwamp();
            break;
        }
		return map;
		
	}

    public static Map CreateMapDustLand()
    {
        Map map = null;
        int map_size;
        int center_x;
        int center_y;

        map_size = 50;
        center_x = map_size / 2;
        center_y = map_size / 2;
        map = new Map(map_size, MapType.DUSTLAND, "MapDustLand");
        map.RandomMonsterIndex.Add(1, new List<string>());
        map.RandomMonsterIndex.Add(2, new List<string>());
        map.RandomMonsterIndex.Add(3, new List<string>());
        map.RandomMonsterIndex.Add(4, new List<string>());

        //map indexes are clustered by their distance twords the origin
        //larger tiers will have more points
        List<int> AvailableIndex = new List<int>(); //list of all available indexes used for some fixed position stories
        for (int i = 0; i < map_size + (map_size - 1) * map_size; i++)
        {
            if (i != map_size / 2 + map_size / 2 * map_size)
                AvailableIndex.Add(i);
        }
        System.Random rnd = new System.Random();

        //Fixed position events
        AddEvent(38, 38, "story_map_event_young_witch", map, AvailableIndex);
        AddEvent(38, 10, "story_map_event_young_witch", map, AvailableIndex);
        AddEvent(10, 10, "story_map_event_young_witch", map, AvailableIndex);

        for (int i = 28; i <= 31; i++)
        {
            AddBlock(45, i, map, AvailableIndex);
            AddBlock(47, i, map, AvailableIndex);
        }
        AddBlock(46, 32, map, AvailableIndex);
        AddMonster(46, 28, 3, MonsterType.WOLF, map, AvailableIndex);
        AddMonster(46, 29, 3, MonsterType.Bat, map, AvailableIndex);
        AddMonster(46, 30, 2, MonsterType.VULTURE, map, AvailableIndex);
        AddEvent(46, 31, "story_map_event_gear_scroll_lvl1", map, AvailableIndex);


        for (int i = 28; i <= 31; i++)
        {
            AddBlock(12, i, map, AvailableIndex);
            AddBlock(14, i, map, AvailableIndex);
        }
        AddBlock(13, 32, map, AvailableIndex);
        AddMonster(13, 28, 3, MonsterType.WOLF, map, AvailableIndex);
        AddMonster(13, 29, 3, MonsterType.Bat, map, AvailableIndex);
        AddMonster(13, 30, 2, MonsterType.VULTURE, map, AvailableIndex);
        AddEvent(13, 31, "story_map_event_gear_scroll_lvl2", map, AvailableIndex);



        AddBlock(33, 12, map, AvailableIndex);
        AddBlock(34, 13, map, AvailableIndex);
        AddBlock(35, 12, map, AvailableIndex);
        AddEvent(34, 12, "story_soul_well", map, AvailableIndex);
        AddMonster(34, 11, 1, MonsterType.FIRE_DUMMY, map, AvailableIndex);

        AddBlock(12, 40, map, AvailableIndex);
        AddBlock(13, 41, map, AvailableIndex);
        AddBlock(14, 40, map, AvailableIndex);
        AddEvent(13, 40, "story_soul_well", map, AvailableIndex);
        AddMonster(13, 39, 1, MonsterType.FIRE_DUMMY, map, AvailableIndex);

        //Add a test boss for testing 
        // AddMonster(27, 27, 1, MonsterType.BOSS_DAY_20, map, AvailableIndex);

        AddEvent(30, 30, "story_map_event_mystery_box", map, AvailableIndex);

        AddBlock(11, 35, map, AvailableIndex);
        AddBlock(13, 35, map, AvailableIndex);
        AddBlock(12, 36, map, AvailableIndex);
        AddEvent(12, 35, "story_map_event_saved_man", map, AvailableIndex);
        AddMonster(12, 34, 1, MonsterType.WOLF_LEADER, map, AvailableIndex);



        for (int i = 0; i < 8; i++)
        {
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_fire_soul", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_ice_soul", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_wind_soul", map);
        }

        for (int i = 0; i < 2; i++)
        {
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_research_man", map);
        }


        



        List<int> Tier1Index = new List<int>();
        string[] Tier1Resource = { ItemType.REDWOOD, ItemType.IRON, ItemType.WOOL, ItemType.BRANCH};

        string[] Tier1Food = { ItemType.MUSHROOM };
        string[] Tier1Monster = { MonsterType.WOLF, MonsterType.TIGER };
        for(int i = 0; i < Tier1Monster.Length; i++)
        {
            map.RandomMonsterIndex[1].Add(Tier1Monster[i]);
        }        

        List<int> Tier2Index = new List<int>();
        string[] Tier2Resource = { ItemType.REDWOOD, ItemType.GOLD, ItemType.TEETH , ItemType.PIG_TAIL, ItemType.BRANCH};
        string[] Tier2Food = { ItemType.MUSHROOM, ItemType.APPLE , ItemType.PORK};
        string[] Tier2Monster = { MonsterType.BEAR, MonsterType.Bat};
        for (int i = 0; i < Tier2Monster.Length; i++)
        {
            map.RandomMonsterIndex[2].Add(Tier2Monster[i]);
        }


        List<int> Tier3Index = new List<int>();
        string[] Tier3Resource = { ItemType.CRYSTAL, ItemType.COWHIDE, ItemType.PIG_TAIL};
        string[] Tier3Food = { ItemType.APPLE, ItemType.PORK, ItemType.PEAR, ItemType.STRAWBERRY, ItemType.ORANGE };
        string[] Tier3Monster = { MonsterType.VULTURE};
        for (int i = 0; i < Tier3Monster.Length; i++)
        {
            map.RandomMonsterIndex[3].Add(Tier3Monster[i]);
        }


        List<int> Tier4Index = new List<int>();
        string[] Tier4Resource = { ItemType.WINTER_WOOD, ItemType.FLOWER};
        string[] Tier4Food = { ItemType.PEAR, ItemType.STRAWBERRY, ItemType.ORANGE, ItemType.APPLE};
        string[] Tier4Monster = { MonsterType.FIRE_DUMMY, MonsterType.DUST_ELF};
        for (int i = 0; i < Tier4Monster.Length; i++)
        {
            map.RandomMonsterIndex[4].Add(Tier4Monster[i]);
        }

        //the tiering calculation should actually use the 
        //tiering in the map object, will leave it hear
        //but should make better in future maps
        for (int i = 0; i < map_size + (map_size - 1) * map_size; i++)
        {
            if (i != map_size / 2 + map_size / 2 * map_size)
            {
                //while not center of the map
                int dis_index = Math.Abs(i % map_size - center_x) + Math.Abs(i / map_size - center_y);
                if (dis_index <= 15 && AvailableIndex.Contains(i))
                {
                    //tier 1
                    Tier1Index.Add(i);
                }
                else if (dis_index <= 25 && AvailableIndex.Contains(i))
                {
                    //tier 2
                    Tier2Index.Add(i);
                }
                else if (dis_index <= 35 && AvailableIndex.Contains(i))
                {
                    //tier 3
                    Tier3Index.Add(i);
                }
                else
                {
                    //tier 4
                    if (AvailableIndex.Contains(i))
                    {
                        Tier4Index.Add(i);
                    }
                }
            }
        }


        //generate things according to each tier seperately
        //tier 1 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 5; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            int j = rnd.Next(Tier1Food.Length);
            Food food = ItemFactory.Get(Tier1Food[j]) as Food;
            food.Count = rnd.Next(2, 8);
            map.AddItemToMap(idx, food);
            Tier1Index.RemoveAt(r);
        }


        for (int i = 0; i < 13; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            int j = rnd.Next(Tier1Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier1Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier1Index.RemoveAt(r);
        }

        for (int i = 0; i < 25; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            int j = rnd.Next(Tier1Monster.Length);
            List<Monster> temp_monsters = new List<Monster>();
            //monsters may also need a builder
            if (Tier1Monster[j].Equals(MonsterType.WOLF))
            {
                for (int k = 0; k < 3; k++)
                {
                    Monster monster = MonsterBuilder.BuildMonster(Tier1Monster[j]);
                    temp_monsters.Add(monster);
                }
            }
            else
            {
                Monster monster = MonsterBuilder.BuildMonster(Tier1Monster[j]);
                temp_monsters.Add(monster);
            }
            map.AddMonstersToMap(idx, temp_monsters);
            Tier1Index.RemoveAt(r);
        }

        for (int i = 0; i < 2; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_grave");
            Tier1Index.RemoveAt(r);
        }



        for (int i = 0; i < 2; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_yeti_house");
            Tier1Index.RemoveAt(r);
        }




        //tier 2 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            int j = rnd.Next(Tier2Food.Length);
            Food food = ItemFactory.Get(Tier2Food[j]) as Food;
            food.Count = rnd.Next(2, 8);

            map.AddItemToMap(idx, food);
            Tier2Index.RemoveAt(r);
        }


        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            int j = rnd.Next(Tier2Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier2Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier2Index.RemoveAt(r);
        }

        for (int i = 0; i < 28; i++)
        {
            int j = rnd.Next(Tier2Monster.Length);

            if (Tier2Monster[j].Equals(MonsterType.Bat))
            {
                AddMonsterToRandomPosition(rnd, 2, Tier2Monster[j], map, Tier2Index);

            }
            else
            {
                AddMonsterToRandomPosition(rnd, 1, Tier2Monster[j], map, Tier2Index);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_grave");
            Tier2Index.RemoveAt(r);
        }

        for (int i = 0; i < 3; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            map.AddStoryEventToMap(idx, "story_water_well");
            Tier2Index.RemoveAt(r);

            r = rnd.Next(0, Tier2Index.Count);
            idx = Tier2Index[r];
            map.AddStoryEventToMap(idx, "story_soul_dealer");
            Tier2Index.RemoveAt(r);
        }


        //tier 3 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            int j = rnd.Next(Tier3Food.Length);
            Food food = ItemFactory.Get(Tier3Food[j]) as Food;
            food.Count = rnd.Next(2, 8);
            map.AddItemToMap(idx, food);
            Tier3Index.RemoveAt(r);
        }


        for (int i = 0; i < 20; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            int j = rnd.Next(Tier3Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier3Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier3Index.RemoveAt(r);
        }

        for (int i = 0; i < 30; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            int j = rnd.Next(Tier3Monster.Length);
            List<Monster> temp_monsters = new List<Monster>();
            //monsters may also need a builder
          
           
            Monster monster = MonsterBuilder.BuildMonster(Tier3Monster[j]);
            temp_monsters.Add(monster);
            
            map.AddMonstersToMap(idx, temp_monsters);
            Tier3Index.RemoveAt(r);
        }

        for (int i = 0; i < 3; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_grave");
            Tier3Index.RemoveAt(r);
        }


        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_mine");
            Tier3Index.RemoveAt(r);
        }

        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_yeti_house");
            Tier3Index.RemoveAt(r);
        }

        for (int i = 0; i < 5; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            AddMonster(idx % map_size, idx / map_size, 1, MonsterType.DUST_ELF, map, Tier3Index);
        }

        for (int i = 0; i < 3; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            map.AddStoryEventToMap(idx, "story_water_well");
            Tier3Index.RemoveAt(r);

            r = rnd.Next(0, Tier3Index.Count);
            idx = Tier3Index[r];
            map.AddStoryEventToMap(idx, "story_soul_dealer");
            Tier3Index.RemoveAt(r);
        }


        //tier 4 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            int j = rnd.Next(Tier4Food.Length);
            Food food = ItemFactory.Get(Tier4Food[j]) as Food;
            food.Count = rnd.Next(2, 8);
            map.AddItemToMap(idx, food);
            Tier4Index.RemoveAt(r);
        }


        for (int i = 0; i < 25; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            int j = rnd.Next(Tier4Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier4Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier4Index.RemoveAt(r);
        }

        for (int i = 0; i < 40; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            int j = rnd.Next(Tier4Monster.Length);
            List<Monster> temp_monsters = new List<Monster>();
            //monsters may also need a builder
            Monster monster = MonsterBuilder.BuildMonster(Tier4Monster[j]);
            temp_monsters.Add(monster);

            map.AddMonstersToMap(idx, temp_monsters);
            Tier4Index.RemoveAt(r);
        }

        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_grave");
            Tier4Index.RemoveAt(r);
        }


        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            map.AddStoryEventToMap(idx, "story_map_event_dig_mine");
            Tier4Index.RemoveAt(r);
        }

        for (int i = 0; i < 2; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            map.AddStoryEventToMap(idx, "story_water_well");
            Tier4Index.RemoveAt(r);

            r = rnd.Next(0, Tier4Index.Count);
            idx = Tier4Index[r];
            map.AddStoryEventToMap(idx, "story_soul_dealer");
            Tier4Index.RemoveAt(r);
        }


        return map;


    }

    public static Map CreateMapSwamp()
    {
        Map map = null;
        int map_size;
        int center_x;
        int center_y;
        map_size = 50;
        center_x = map_size / 2;
        center_y = map_size / 2;
        map = new Map(map_size, MapType.SWAMP, "MapSwamp");
        map.RandomMonsterIndex.Add(1, new List<string>());
        map.RandomMonsterIndex.Add(2, new List<string>());
        map.RandomMonsterIndex.Add(3, new List<string>());
        map.RandomMonsterIndex.Add(4, new List<string>());
        //map indexes are clustered by their distance twords the origin
        //larger tiers will have more points
        List<int> AvailableIndex = new List<int>(); //list of all available indexes used for some fixed position stories
        for (int i = 0; i < map_size + (map_size - 1) * map_size; i++)
        {
            if (i != map_size / 2 + map_size / 2 * map_size)
                AvailableIndex.Add(i);
        }
        System.Random rnd = new System.Random();

        //Fixed position events-------------------------
      
        for (int i = 28; i <= 31; i++)
        {
            AddBlock(12, i, map, AvailableIndex);
            AddBlock(14, i, map, AvailableIndex);
        }
        AddBlock(13, 32, map, AvailableIndex);
        AddMonster(13, 28, 3, MonsterType.FISH_GUARDIAN, map, AvailableIndex);
        AddMonster(13, 29, 1, MonsterType.FISH_LEADER, map, AvailableIndex);
        AddMonster(13, 30, 2, MonsterType.DEATH_WITCH, map, AvailableIndex);
        AddEvent(13, 31, "story_map_event_blacksmith", map, AvailableIndex);


        AddBlock(39, 45, map, AvailableIndex);
        AddBlock(41, 45, map, AvailableIndex);
        AddBlock(40, 46, map, AvailableIndex);
        AddEvent(40, 45, "story_map_event_gaint_stone", map, AvailableIndex);

        AddBlock(4, 38, map, AvailableIndex);
        AddBlock(6, 38, map, AvailableIndex);
        AddBlock(5, 39, map, AvailableIndex);
        AddEvent(5, 38, "story_map_event_gaint_stone", map, AvailableIndex);



        for (int i = 9; i <= 11; i++)
        {
            AddBlock(i, 11, map, AvailableIndex);
            AddBlock(i, 13, map, AvailableIndex);
        }
        AddBlock(8, 12, map, AvailableIndex);
        AddEvent(9, 12, "story_map_event_treasure_ghost", map, AvailableIndex);


        //Random position events-------------------------

        for (int i = 0; i < 30; i++)
        {
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_fishing", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_chop_winter_tree", map);
        }
        

        for(int i = 0; i < 10; i++)
        {
            AddMonsterToRandomPosition(rnd, 1, MonsterType.BLACKSMITH_FIGHTER, map, AvailableIndex);
        }


        for (int i = 0; i < 3; i++)
        {
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_ice_cave", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_dry_soul", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_holy_water", map);
            AddMonsterToRandomPosition(rnd, 1, MonsterType.WIND_GIANT_BAT, map, AvailableIndex);
            AddMonsterToRandomPosition(rnd, 1, MonsterType.FIRE_ELF, map, AvailableIndex);

        }
        {
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_high_altar", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_ice_beast", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_fire_cave", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_medi_doctor", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_map_event_skeleton_king", map);
            AddEventToRandomPosition(rnd, AvailableIndex, "story_holy_fete", map);

        }



        List<int> Tier1Index = new List<int>();
        string[] Tier1Resource = { ItemType.REDWOOD, ItemType.IRON, ItemType.FLOWER, ItemType.BRANCH, ItemType.BRANCH};

        string[] Tier1Food = { ItemType.MUSHROOM, ItemType.STRAWBERRY, ItemType.ORANGE};
        string[] Tier1Monster = { MonsterType.ALLIGATOR, MonsterType.FISH_GUARDIAN, MonsterType.VIPER};
        for (int i = 0; i < Tier1Monster.Length; i++)
        {
            map.RandomMonsterIndex[1].Add(Tier1Monster[i]);
        }

        List<int> Tier2Index = new List<int>();
        string[] Tier2Resource = { ItemType.REDWOOD, ItemType.GOLD, ItemType.WINTER_WOOD, ItemType.BRANCH};
        string[] Tier2Food = { ItemType.MUSHROOM, ItemType.TOMATO, ItemType.PEAR};
        string[] Tier2Monster = { MonsterType.FISH_LEADER,MonsterType.GIANT_SNAKE, MonsterType.FIRE_FOX};
        for (int i = 0; i < Tier2Monster.Length; i++)
        {
            map.RandomMonsterIndex[2].Add(Tier2Monster[i]);
        }

        List<int> Tier3Index = new List<int>();
        string[] Tier3Resource = {ItemType.CRYSTAL, ItemType.BLUE_CRYSTAL, ItemType.RED_CRYSTAL};
        string[] Tier3Food = {  ItemType.LIFE_WATER};
        string[] Tier3Monster = {MonsterType.FIRE_DOG, MonsterType.DEATH_DUMMY,MonsterType.JUNIOR_KEEPER};
        for (int i = 0; i < Tier3Monster.Length; i++)
        {
            map.RandomMonsterIndex[3].Add(Tier3Monster[i]);
        }


        List<int> Tier4Index = new List<int>();
        string[] Tier4Resource = {ItemType.RED_CRYSTAL, ItemType.BLUE_CRYSTAL, ItemType.WINTER_WOOD};
        string[] Tier4Food = {ItemType.FIG};
        string[] Tier4Monster = {MonsterType.DEATH_WITCH, MonsterType.HIGH_KEEPER};
        for (int i = 0; i < Tier4Monster.Length; i++)
        {
            map.RandomMonsterIndex[4].Add(Tier4Monster[i]);
        }


        for (int i = 0; i < map_size + (map_size - 1) * map_size; i++)
        {
            if (i != map_size / 2 + map_size / 2 * map_size)
            {
                //while not center of the map
                int dis_index = Math.Abs(i % map_size - center_x) + Math.Abs(i / map_size - center_y);
                if (dis_index <= 15 && AvailableIndex.Contains(i))
                {
                    //tier 1
                    Tier1Index.Add(i);
                }
                else if (dis_index <= 25 && AvailableIndex.Contains(i))
                {
                    //tier 2
                    Tier2Index.Add(i);
                }
                else if (dis_index <= 35 && AvailableIndex.Contains(i))
                {
                    //tier 3
                    Tier3Index.Add(i);
                }
                else
                {
                    //tier 4
                    if (AvailableIndex.Contains(i))
                    {
                        Tier4Index.Add(i);
                    }
                }
            }
        }


        //generate things according to each tier seperately
        //tier 1 ----------------------------------------------------------------------------------------------------------

        AddEventToRandomPosition(rnd, Tier1Index, "story_map_event_crystal_armor", map);

        for (int i = 0; i < 3; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            int j = rnd.Next(Tier1Food.Length);
            Food food = ItemFactory.Get(Tier1Food[j]) as Food;
            food.Count = rnd.Next(5, 15);
            map.AddItemToMap(idx, food);
            Tier1Index.RemoveAt(r);
        }


        for (int i = 0; i < 13; i++)
        {
            int r = rnd.Next(0, Tier1Index.Count);
            int idx = Tier1Index[r];
            int j = rnd.Next(Tier1Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier1Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier1Index.RemoveAt(r);
        }

        for (int i = 0; i < 32; i++)
        {
            int j = rnd.Next(Tier1Monster.Length);
            if (Tier1Monster[j].Equals(MonsterType.FISH_GUARDIAN))
            {
                AddMonsterToRandomPosition(rnd, 2, Tier1Monster[j], map, Tier1Index);
            }
            else
            {
                AddMonsterToRandomPosition(rnd, 1, Tier1Monster[j], map, Tier1Index);
            }

        }

    



        //tier 2 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 5; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            int j = rnd.Next(Tier2Food.Length);
            Food food = ItemFactory.Get(Tier2Food[j]) as Food;
            food.Count = rnd.Next(5, 15);
            map.AddItemToMap(idx, food);
            Tier2Index.RemoveAt(r);
        }


        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier2Index.Count);
            int idx = Tier2Index[r];
            int j = rnd.Next(Tier2Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier2Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier2Index.RemoveAt(r);
        }

        for (int i = 0; i < 25; i++)
        {
            int j = rnd.Next(Tier2Monster.Length);
            if (Tier2Monster[j].Equals(MonsterType.FIRE_FOX))
            {
                AddMonsterToRandomPosition(rnd, 2, Tier2Monster[j], map, Tier2Index);
            }
            else
            {
                AddMonsterToRandomPosition(rnd, 1, Tier2Monster[j], map, Tier2Index);
            }
        }





        //tier 3 ----------------------------------------------------------------------------------------------------------

        AddEventToRandomPosition(rnd, Tier3Index, "story_map_event_normal_sword", map);
        for (int i = 0; i < 10; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            int j = rnd.Next(Tier3Food.Length);
            Food food = ItemFactory.Get(Tier3Food[j]) as Food;
            food.Count = rnd.Next(1, 3);
            map.AddItemToMap(idx, food);
            Tier3Index.RemoveAt(r);
        }


        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier3Index.Count);
            int idx = Tier3Index[r];
            int j = rnd.Next(Tier3Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier3Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier3Index.RemoveAt(r);
        }

        for (int i = 0; i < 40; i++)
        {
            int j = rnd.Next(Tier3Monster.Length);
            if (Tier3Monster[j].Equals(MonsterType.DEATH_DUMMY))
            {
                AddMonsterToRandomPosition(rnd, 3, Tier3Monster[j], map, Tier3Index);
            }
            else
            {
                AddMonsterToRandomPosition(rnd, 1, Tier3Monster[j], map, Tier3Index);
            }
        }


       



        //tier 4 ----------------------------------------------------------------------------------------------------------
        for (int i = 0; i < 16; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            int j = rnd.Next(Tier4Food.Length);
            Food food = ItemFactory.Get(Tier4Food[j]) as Food;
            food.Count = 1;
            map.AddItemToMap(idx, food);
            Tier4Index.RemoveAt(r);
        }


        for (int i = 0; i < 15; i++)
        {
            int r = rnd.Next(0, Tier4Index.Count);
            int idx = Tier4Index[r];
            int j = rnd.Next(Tier4Resource.Length);
            Resource res = ItemFactory.BuildResource(Tier4Resource[j], (int)UnityEngine.Random.Range(1, 6));
            map.AddItemToMap(idx, res);
            Tier4Index.RemoveAt(r);
        }

        for (int i = 0; i < 40; i++)
        {        
            int j = rnd.Next(Tier4Monster.Length);
            AddMonsterToRandomPosition(rnd, rnd.Next(1, 2), Tier4Monster[j], map, Tier4Index);
        }

       

        return map;
    }


    public static void AddMonster(int x, int y, int count, string type, Map map, List<int> AvailableIndex){
		int idx = x + y * map.MapSize;
		List<Monster> mons = new List<Monster>();
		mons.Clear();
		for(int k = 0 ; k < count ; k ++){
			Monster monster = MonsterBuilder.BuildMonster(type);
			mons.Add (monster);
		}
		
		map.AddMonstersToMap(idx, mons);
		AvailableIndex.Remove (idx);
	}


    public static void AddMonster(int idx, int count, string type, Map map, List<int> AvailableIndex)
    {
        List<Monster> mons = new List<Monster>();
        mons.Clear();
        for (int k = 0; k < count; k++)
        {
            Monster monster = MonsterBuilder.BuildMonster(type);
            mons.Add(monster);
        }

        map.AddMonstersToMap(idx, mons);
        AvailableIndex.Remove(idx);
    }
    public static void AddMonsterToRandomPosition(System.Random rnd, int count, string type, Map map, List<int> AvailableIndex)
    {
        int r = rnd.Next(0, AvailableIndex.Count);
        int idx = AvailableIndex[r];
        AddMonster(idx, count, type, map, AvailableIndex);
    }


    public static void AddEventToRandomPosition(System.Random rnd, List<int> AvailableIndex, string type, Map map)
    {
        int r = rnd.Next(0, AvailableIndex.Count);
        int idx = AvailableIndex[r];
        AddEvent(idx % map.MapSize, idx / map.MapSize, type, map, AvailableIndex);
    }

    public static void AddEvent(int x, int y, string type, Map map, List<int> AvailableIndex){
		int fixed_idx;
		fixed_idx = x + y*map.MapSize;
		map.AddStoryEventToMap(fixed_idx, type);
		AvailableIndex.Remove(fixed_idx);
	}


	public static void AddBlock(int x , int y, Map map, List<int> AvailableIndex){
		int fixed_idx;
		fixed_idx = x + y * map.MapSize;
		map.MapBlocks.Add (fixed_idx);
		AvailableIndex.Remove (fixed_idx);
	}

}
	