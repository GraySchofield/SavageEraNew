using System;
using UnityEngine;
public static class Config {
    public static bool NeedAchievement = true;

    public static bool IsDebugMode = false;
    //how many second for a day
    public static int SecondsPerDay = 300;

    public static int BackUpSavePeriod = 1;

    public static int SeasonLength = 25; // 15 days a season

    public static float InitialHealthUpperLimit = 100f;

    public static string DefaultHeroName = "Wild Hunter";

    public static string DefaultLanguage = Lang.LangType.CHINESE;

    public static string SpawnEventTriggerType = "1";

    public static bool DoRelod = true;

    public static string DataPath = "";

    public static Color uncommonColor = new Color(0f, 0.831f, 1f);
    public static Color rareColor = new Color(0.824f, 0.451f, 0.051f);
    public static Color legendColor = new Color(0.125f, 0.875f, 0.471f);
    public static float armor_multiplier = 0.06f;
    public static float armor_break_multiplier = 0.0001f;

    public static string[] SpecialEventMonters = {MonsterType.SKELETON_KING, MonsterType.ICE_BEAST,
        MonsterType.ACIENT_KEEPER, MonsterType.ACIENT_KEEPER_2,
        MonsterType.LIGHT_FETE, MonsterType.STONE_GIANT, MonsterType.TREASURE_GHOST};

    public static string[] AllWeapons = {
     ItemType.IRON_SWORD,
     ItemType.SPIKE_GLOVE,
     ItemType.YOUNG_SWORD,
     ItemType.OLD_SWORD,
     ItemType.GREAT_SWORD,
     ItemType.CRYSTAL_SWORD,
     ItemType.FISH_SWORD,
     ItemType.DEEP_FISH_SWORD,
     ItemType.ICE_SWORD,
     ItemType.ICE_GUN,
     ItemType.ICE_LANCER,
     ItemType.ICE_HAMMER,
     ItemType.ICE_REAP,
     ItemType.ICE_GIANT_SWORD,
     ItemType.FIRE_BOW,
     ItemType.FIRE_KNIFE,
     ItemType.FIRE_AXE,
     ItemType.FIRE_SWORD,
     ItemType.FIRE_BIG_SWORD,
     ItemType.FIRE_BIG_STAFF,
     ItemType.WIND_SWORD,
     ItemType.WIND_WHIP,
     ItemType.WIND_DART,
     ItemType.WIND_BIG_SWORD,
     ItemType.WIND_BIG_AXE,
     ItemType.WIND_BIG_DART,
     ItemType.DARK_CLAW,
     ItemType.DARK_WUDU_STAFF,
     ItemType.DARK_SWORD,
     ItemType.DARK_LANCER,
     ItemType.DARK_GIANT_AXE,
     ItemType.DARK_GIANT_REAPER,
     ItemType.DARK_DEMON_SWORD,
     ItemType.HOLY_JUSTICE_HAMMER,
     ItemType.HOLY_JUDGE_STAFF,
     ItemType.HOLY_STAFF,
     ItemType.HOLY_HAMMER,
     ItemType.HOLY_LIGHT_SWORD,
     ItemType.HOLY_LIGHT_LANCER,
     ItemType.HOLY_SAINT_BOOK,
  //   ItemType.LIGHT_SPEAR,
     ItemType.NORMAL_BLACK_IRON_SWORD,
     ItemType.SPECIAL_BLACK_IRON_SWORD,
     ItemType.MASTER_BLACK_IRON_SWORD,
    ItemType.ACIENT_SWORD,
    ItemType.NORMAL_SWORD,
    ItemType.METAL_SWORD,
    ItemType.SMALL_ICE_KNIFE,
    ItemType.CHUANJIA_SWORD,
    ItemType.NEW_FIRE_GUN,
    ItemType.QUMO_SWORD,
    ItemType.BLOOD_DRINK_SWORD};
    public static string[] AllArmors = {
    ItemType.GRASS_SUIT,
    ItemType.LOG_SUIT,
    ItemType.IRON_SUIT,
    ItemType.WOLF_SUIT,
    ItemType.TIGER_SUIT,
    ItemType.FUR_SUIT,
    ItemType.FOX_SUIT,
   // ItemType.JUSTICE_ARMOR,
    ItemType.ICE_BEAST_ARMOR,
    ItemType.NORMAL_BLACK_IRON_SUIT,
    ItemType.SPECIAL_BLACK_IRON_SUIT,
    ItemType.MASTER_BLACK_IRON_SUIT,
    ItemType.FOG_SUIT,
    ItemType.BREAK_BLADE_SWORD,
    ItemType.METAL_SUIT,
    ItemType.DEMON_ENERGY_SUIT,
    ItemType.GLORIOUS_SUIT

            };
    public static string[] AllAccessories = {
      ItemType.TOOTH_NECKLACE,
      ItemType.FLOWER_RING,
      ItemType.FEATHER_HAT,
      ItemType.ALLIGATOR_BELT,
      ItemType.FIRE_TEETH_NECKLACE,
      //ItemType.LIGHT_NECKLACE,
      ItemType.SKELETON_RING,
      ItemType.HOLY_HAT,
      ItemType.NORMAL_BLACK_IRON_NECKLACE,
      ItemType.SPECIAL_BLACK_IRON_NECKLACE,
      ItemType.MASTER_BLACK_IRON_NECKLACE,
      ItemType.ICE_NECKLACE,
      ItemType.METAL_NECKLACE,
      ItemType.OLD_GOLD_RING,
      ItemType.SHINY_EAR_RING,
      ItemType.BROZEN_METAL,
      ItemType.BLESSING_RING
    };


    public static string[] AllSkills =
    {
        SkillType.ULTI_SKILL_ARMOR_BREAK,
        SkillType.ULTI_SKILL_FIRE_HELL,
        SkillType.ULTI_SKILL_FIRE_PULSE,
        SkillType.ULTI_SKILL_FIRE_SHOCK,
        SkillType.ULTI_SKILL_FIRE_STUN,
        SkillType.ULTI_SKILL_FIRE_WAVE,
        SkillType.ULTI_SKILL_HARD_STRIKE,
        SkillType.ULTI_SKILL_HOLY_SPIRIT,
        SkillType.ULTI_SKILL_ICE_BLIZZARD,
        SkillType.ULTI_SKILL_ICE_BODY,
        SkillType.ULTI_SKILL_ICE_PULSE,
        SkillType.ULTI_SKILL_ICE_SHOCK,
        SkillType.ULTI_SKILL_ICE_WAVE,
        SkillType.ULTI_SKILL_POISON_STRIKE,
        SkillType.ULTI_SKILL_SWIPE,
        SkillType.ULTI_SKILL_WEAK_STRIKE,
        SkillType.ULTI_SKILL_WIND_BLADE,
        SkillType.ULTI_SKILL_WIND_BURY,
        SkillType.ULTI_SKILL_WIND_FAST,
        SkillType.ULTI_SKILL_WIND_PULSE,
        SkillType.ULTI_SKILL_WIND_WAVE,
    };

}

