using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Map
{
    //Map class is used to keep track of data in a map


    public Map(int size, string type, string prefab_name)
    {
        MapSize = size;

        //start at the center of the map, where the home is
        PlayerX = (int)size / 2;
        PlayerY = (int)size / 2;

        // whole map should be cover with war fogs when created
        WarFogIndexs = new bool[MapSize, MapSize];
        for (int x = 0; x < WarFogIndexs.GetLength(0); x++)
        {
            for (int y = 0; y < WarFogIndexs.GetLength(1); y++)
            {
                WarFogIndexs[x, y] = true; //all forgy
            }
        }

        Type = type;
        Name = Lang.Current[type];
        Description = Lang.Current[type + "_des"];
        MonstersInMap = new Dictionary<int, List<Monster>>();
        ItemsInMap = new Dictionary<int, Item>();
        RandomMonsterIndex = new Dictionary<int, List<string>>();
        EventsInMap = new Dictionary<int, MapEvent>();
        MapBlocks = new List<int>();
        PrefabName = prefab_name;
    }

    public string Type
    {
        get;
        private set;
    }


    public string Name
    {
        get;
        private set;
    }


    public string Description
    {
        get;
        private set;
    }

    //the name of the prefab of the Map base
    public string PrefabName
    {
        get;
        private set;
    }


    //all maps are squares, for e.g. 50*50
    public int MapSize
    {
        get;
        private set;
    }


    //war fogs of the map, if true than there is war fog, false realved
    public bool[,] WarFogIndexs
    {
        get;
        private set;
    }


    //x position
    public int PlayerX
    {
        get;
        set;
    }

    //y position
    public int PlayerY
    {
        get;
        set;
    }



    //maps from  monster indexes to mosnters
    //index is calculated as x + y * mapsize 
    public Dictionary<int, List<Monster>> MonstersInMap
    {
        get;
        private set;
    }

    public Dictionary<int, Item> ItemsInMap
    {
        get;
        private set;
    }


    public Dictionary<int, MapEvent> EventsInMap
    {
        get;
        private set;
    }


    

    //indexes on the map that are obstacles
    public List<int> MapBlocks
    {
        get;
        set;
    }


    public Dictionary<int, List<string>> RandomMonsterIndex
    {
        get;
        private set;
    }


    public void AddMonstersToMap(int x, int y, List<Monster> monsters)
    {
        int idx = x + y * MapSize;
        MonstersInMap.Add(idx, monsters);
    }

    public void AddMonstersToMap(int idx, List<Monster> monsters)
    {
        MonstersInMap.Add(idx, monsters);
    }

    public void AddItemToMap(int idx, Item item)
    {
        ItemsInMap.Add(idx, item);
    }



    public void AddStoryEventToMap(int idx, string story_type)
    {
        Story story = StoryFactory.Get(story_type);
        SpawnStoryEvent story_event = new SpawnStoryEvent("spawn" + story_type, null, null, null, story);
        MapEvent map_event = new MapEvent(story_event);
        this.EventsInMap.Add(idx, map_event);
    }


    //calculate which a certain 
    private int CalculateTierFromDist(float dist)
    {
        if (dist >= 0)
        {
            if (dist <= 0.3f * MapSize)
            {
                return 1;
            }
            else if (dist <= 0.5f * MapSize)
            {
                return 2;
            }
            else if (dist <= 0.7f * MapSize)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            return -1;
        }


    }

    public string GetRandomMonsterFromDist(float dist)
    {
        int tier = CalculateTierFromDist(dist);
        List<string> monsters = RandomMonsterIndex[tier];
        System.Random rnd = new System.Random();
        int idx = rnd.Next(0, monsters.Count);
        return monsters[idx];
    }


}
