using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EvilChamberController : MonoBehaviour {
    private ElementType[] elements = {ElementType.Ice, ElementType.Fire, ElementType.Wind, ElementType.Dark, ElementType.Holy};

    private string[] fire_monsters = { MonsterType.FIRE_DOG,
    MonsterType.FIRE_DUMMY,MonsterType.FIRE_SOUL,MonsterType.FIRE_FOX,
    MonsterType.FIRE_ELF
    };
    private string[] ice_monsters = { MonsterType.ICE_BEAST,
    MonsterType.ICE_SOUL,MonsterType.FISH_LEADER,MonsterType.DEEP_MONSTER_FISH, MonsterType.YETI};
    private string[] wind_monsters = { MonsterType.WIND_GIANT_BAT, MonsterType.DUST_ELF,MonsterType.GIANT_SNAKE,
    MonsterType.VULTURE, MonsterType.BOSS_DAY_35};
    private string[] dark_monsters = {MonsterType.DEATH_DUMMY, MonsterType.EVIL_SHADOW, MonsterType.FOG_DAEMON, MonsterType.SKELETON_KING,
    MonsterType.DEATH_WITCH};
    private string[] holy_monsters = {MonsterType.HOLY_BLADER, MonsterType.STONE_PUPPY, MonsterType.STONE_GIANT,
    MonsterType.TREE_MAN,MonsterType.JUNIOR_KEEPER,MonsterType.HIGH_KEEPER};


    public Text LvlText;
    private int max_lvl = 5;
   

	void Update () {
        LvlText.text = "" + Game.Current.Hero.EvilChamberLevel;
	}

    public Monster CreateDynamicMonster(int current_lvl, int count)
    {
        System.Random rnd = new System.Random();
        string type = MonsterType.EVIL_SHADOW;
        ElementType e_type = elements[rnd.Next(0, elements.Length)];
        switch (e_type)
        {
            case ElementType.Fire:
                type = fire_monsters[rnd.Next(0, fire_monsters.Length)];
                break;
            case ElementType.Ice:
                type = ice_monsters[rnd.Next(0, ice_monsters.Length)];
                break;
            case ElementType.Wind:
                type = wind_monsters[rnd.Next(0, wind_monsters.Length)];
                break;
            case ElementType.Dark:
                type = dark_monsters[rnd.Next(0, dark_monsters.Length)];
                break;
            case ElementType.Holy:
                type = holy_monsters[rnd.Next(0, holy_monsters.Length)];
                break;
        }


        float dps = 110f * (Mathf.Pow(1.5f,current_lvl - 1)) / count * (1 + count/10);
        float defense = Random.Range(30, 50);
        float hp = 2000 * (Mathf.Pow(2f, current_lvl - 1)) / count *(1 + count / 10);
        float cool_down = Random.Range(1, 3);
        Monster monster = new Monster(type, cool_down, dps * cool_down, defense, hp, e_type);
        DropNode node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 30 * current_lvl / count);
        monster.DropList.Add(node1);
        monster.GeneralDrops.Add(GeneralDropType.Rare);
        monster.CanRunAwayFrom = false;
        monster.isInEvilChamber = true;

        //need to add personalized skills for monsters 
        switch (current_lvl)
        {
            case 1:
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_MULTI_ATTACK, 0.3f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.2f, 2f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.3f, 0.5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS,0.3f));
                break;

            case 2:
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_CHAOS, 15f, 7f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 30f, 10f, 0.05f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 40f, 16f, 0.5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                break;

            case 3:
                monster.Defense = 2000f;
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.25f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 1, 2f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_ULTI_ICE_SHIELD, 50f, 10f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                break;

            case 4:
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_MULTI_ATTACK, 0.4f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK, 0.1f, 5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_PURIFY, 5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_WEAK_SKILL, 20, 10, 0.1f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                break;

            case 5:
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK, 0.3f, 6f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.5f, 0.5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.ARMOR_SKILL_ABSORB, 0.2f, 1f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 50,20,0.05f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_SUPER_DEFENSE, 10f, 5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                break;

        }
       
        return monster;
    }


    public  void CreateBattle()
    {
        MainCharacter hero = Game.Current.Hero;
        if (hero.EvilChamberLevel < max_lvl)
        {
            System.Random rnd = new System.Random();
            int count = rnd.Next(1, 4);
            List<Monster> monsters = new List<Monster>();
            for (int i = 0; i < count; i++)
            {
                monsters.Add(CreateDynamicMonster(Game.Current.Hero.EvilChamberLevel, count));
            }

            Game.Current.Hero.CurrentBattleMonsters = monsters;

            Game.Current.ActionEngine.DestroyAllViewIndexing();
            SceneManager.LoadScene(3);
        }
        else
        {
            Game.Current.AddToast(max_lvl + Lang.Current["lvl_not_provided"]);
        }
    }



}
