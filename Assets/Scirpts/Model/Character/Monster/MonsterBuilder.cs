
using System;

public class MonsterBuilder
{
	public static Monster BuildMonster(string type){
		Monster monster = null;
		DropNode node1;
		DropNode node2;
		DropNode node3;
        switch (type)
        {

            case MonsterType.WOLF:
                monster = new Monster(type, 2, 3, 1, 15, ElementType.Fire);
                monster.GeneralDrops.Add(GeneralDropType.Common);
                node1 = new DropNode(ItemType.WOLF_SKIN, "Resource", 0.5f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.05f, 1);
                monster.DropList.Add(node1);                
                break;

            case MonsterType.TIGER:
                monster = new Monster(type, 5, 15, 1, 30, ElementType.Fire);
                monster.GeneralDrops.Add(GeneralDropType.Common);
                node1 = new DropNode(ItemType.COWHIDE, "Resource", 0.5f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.08f, 1);
                monster.DropList.Add(node1);
                break;


            case MonsterType.BEAR:
                monster = new Monster(type, 3.5f, 12, 3, 70, ElementType.Fire);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.5f, 2f));
                //monster.DefenseSkills.Add ( new MonsterDefenseSkill(SkillType.MONSTER_MAGIC_IMMUNE,1f));
                monster.GeneralDrops.Add(GeneralDropType.Common);
                node1 = new DropNode(ItemType.BEAR_PAW, "Food", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.1f, 1);
                monster.DropList.Add(node1);

                break;

            case MonsterType.Bat:
                monster = new Monster(type, 1, 5, 3, 25, ElementType.Wind);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                node1 = new DropNode(ItemType.BAT_POISON, "Food", 0.18f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.08f, 1);
                monster.DropList.Add(node1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_FAST, 5f, 10f, 0.5f));
                break;

            case MonsterType.VULTURE:
                monster = new Monster(type, 3, 30, 5, 110, ElementType.Wind);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                node1 = new DropNode(ItemType.FEATHER, "Resource", 0.5f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.15f));
                break;

            case MonsterType.YOUNG_BLADE_MASTER:
                monster = new Monster(type, 3, 18, 5, 150, ElementType.Holy);
                node1 = new DropNode(ItemType.YOUNG_SWORD, "Weapon", 1f, 1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_FAST, 24f, 6f, 0.7f));
                monster.DropList.Add(node1);
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.OLD_BLADE_MASTER:
                monster = new Monster(type, 3, 35, 20, 1200, ElementType.Holy);
                node1 = new DropNode(ItemType.OLD_SWORD, "Weapon", 1f, 1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_FAST, 24f, 9f, 0.5f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK, 0.5f, 4f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.15f, 1f));
                monster.DropList.Add(node1);
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.GREAT_BLADE_MASTER:
                monster = new Monster(type, 3, 103, 80, 3500, ElementType.Holy);
                node1 = new DropNode(ItemType.GREAT_SWORD, "Weapon", 1f, 1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 24f, 10f , 0.5f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.2f, 0.5f));
                monster.DropList.Add(node1);
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.ZOMBIE:
                monster = new Monster(type, 5, 40, 0, 80, ElementType.Dark);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.8f, 2);
                monster.DropList.Add(node1);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 1f, 0.3f));
                break;

            case MonsterType.STONE_PUPPY:
                monster = new Monster(type, 5, 25, 0, 150, ElementType.Wind);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 1f, 1f));
                break;


            case MonsterType.YOUNG_WITCH_WIND:
                monster = new Monster(type, 1, 8, 15, 500, ElementType.Wind);
                node1 = new DropNode(ItemType.SCROLL_ULTI_WIND_FAST, "Scroll", 1, 1);
                monster.DropList.Add(node1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ENERGY_PULSE, 17f));
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.YOUNG_WITCH_FIRE:
                monster = new Monster(type, 1, 10, 10, 450, ElementType.Fire);
                node1 = new DropNode(ItemType.SCROLL_ULTI_FIRE_STUN, "Scroll", 1, 1);
                monster.DropList.Add(node1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ENERGY_PULSE, 17f));
                monster.CanRunAwayFrom = false;

                break;

            case MonsterType.YOUNG_WITCH_ICE:
                monster = new Monster(type, 1, 6, 10, 600, ElementType.Ice);
                node1 = new DropNode(ItemType.SCROLL_ULTI_ICE_BODY, "Scroll", 1, 1);
                monster.DropList.Add(node1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ENERGY_PULSE, 17f));
                monster.CanRunAwayFrom = false;
                break;



            case MonsterType.HUNGARY_HUNTER:
                monster = new Monster(type, 3, 22, 5, 450, ElementType.Fire);
                node1 = new DropNode(ItemType.QUMO_SWORD, "Weapon", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 3);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ELEMENT_CHANGE, 0.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_RAGE, 50, 1.5f));
                break;

            case MonsterType.FURY_HUNTER:
                monster = new Monster(type, 1.5f, 3, 2, 660, ElementType.Dark);
                node1 = new DropNode(ItemType.BLOOD_DRINK_SWORD, "Weapon", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1, 1);
                monster.DropList.Add(node1);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_STRONGER, 0.25f, 2f));
                monster.CanRunAwayFrom = false;
                break;



            case MonsterType.ACIENT_KEEPER:
                monster = new Monster(type, 5, 80, 10, 1239, ElementType.Fire);
                float c = UnityEngine.Random.value;
                if(c <= 0.33)
                {
                    node1 = new DropNode(ItemType.SCROLL_ICE_SWORD, "Scroll", 1f, 1);
                    monster.DropList.Add(node1);
                }
                else if (c <= 0.66f)
                {
                    node1 = new DropNode(ItemType.SCROLL_FIRE_BOW, "Scroll", 1f, 1);
                    monster.DropList.Add(node1);
                }
                else
                {
                    node1 = new DropNode(ItemType.SCROLL_WIND_SWORD, "Scroll", 1f, 1);
                    monster.DropList.Add(node1);
                }                       
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FIRE_KEY, "Resource", 1, 1);
                monster.DropList.Add(node1);
                node3 = new DropNode(ItemType.YETI_KEY, "Resource", 1f, 1);
                monster.DropList.Add(node3);


                monster.CanRunAwayFrom = false;
                monster.isBoss = true;
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.3f, 0.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DISPERSE, 10f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DOOM, 32f));
                break;


            case MonsterType.ACIENT_KEEPER_2:
                monster = new Monster(type, 3, 65, 10, 1248, ElementType.Wind);
                node1 = new DropNode(ItemType.SCROLL_FUR_SUIT, "Scroll", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SCROLL_FLOWER_RING, "Scroll", 1f, 1);
                monster.DropList.Add(node1);
  
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.WIND_KEY, "Resource", 1, 1);
                monster.DropList.Add(node1);

                monster.CanRunAwayFrom = false;
                monster.isBoss = true;

                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 1, 1f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.5f, 0.5f));
                break;



            case MonsterType.WOLF_LEADER:
                monster = new Monster(type, 1, 18, 10, 400, ElementType.Fire);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FIRE_CORE, "Resource", 1f, 1);
                monster.DropList.Add(node1);              
                monster.isBoss = true;
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DEATH_BITE, 50f));
                break;


            case MonsterType.FOG_DAEMON:
                monster = new Monster(type, 2, 30, 5, 220, ElementType.Dark);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 25f, 5f, 0.5f));
                monster.isBoss = true;
                break;


            case MonsterType.FOG_DAEMON_2:
                monster = new Monster(type, 2, 62, 5, 450, ElementType.Dark);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1, 2);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 25f, 5f, 0.5f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK,0.2f,1f));
                monster.isBoss = true;
                break;



            case MonsterType.FOG_DAEMON_3:
                monster = new Monster(type, 2, 88, 25, 700, ElementType.Dark);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 20);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1, 3);
                monster.DropList.Add(node1);

                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.4f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 20f, 8f, 0.5f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.3f, 1f));
                monster.isBoss = true;

                break;

            case MonsterType.FIRE_SOUL:
                monster = new Monster(type, 3, 27, 3, 100, ElementType.Fire);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FIRE_CORE, "Resource", 1, 1);
                monster.DropList.Add(node1);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 1f, 0.3f));
                break;

            case MonsterType.ICE_SOUL:
                monster = new Monster(type, 5, 50, 3, 100, ElementType.Ice);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 1, 1);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.45f, 4f));
                monster.DropList.Add(node1);
                break;

            case MonsterType.WIND_SOUL:
                monster = new Monster(type, 2, 18, 3, 100, ElementType.Wind);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.WIND_CORE, "Resource", 1, 1);
                monster.DropList.Add(node1);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_FAST, 17f, 10f, 0.5f));
                break;

            case MonsterType.TREE_MAN:
                monster = new Monster(type, 5, 30, 3, 110, ElementType.Holy);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.8f, 3);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 10f, 5f, 0.02f));
                monster.DropList.Add(node1);
                break;

            case MonsterType.ALIVE_STATUE:
                monster = new Monster(type, 5, 51, 25, 300, ElementType.Holy);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.8f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.BLUE_CRYSTAL, "Resource", 1, 5);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.3f, 5f));
                break;

            case MonsterType.YETI:
                monster = new Monster(type, 3, 34, 5, 200, ElementType.Ice);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 0.8f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                node2 = new DropNode(ItemType.YETI_HEART, "Food", 0.2f, 1);
                monster.DropList.Add(node2);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.3f, 2f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MAGIC_IMMUNE, 1f));
                break;

            case MonsterType.FIRE_DUMMY:
                monster = new Monster(type, 3, 30, 12, 220, ElementType.Fire);
                node1 = new DropNode(ItemType.FIRE_CORE, "Resource", 0.7f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.LAVA, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_STRONGER,0.25f,1.3f));
                break;

            case MonsterType.DUST_ELF:
                monster = new Monster(type, 2, 28, 8, 190, ElementType.Wind);
                node1 = new DropNode(ItemType.WIND_CORE, "Resource", 0.4f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DUST, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.2f));

                break;

            case MonsterType.BOSS_DAY_20:
                monster = new Monster(type, 4, 45, 15, 922, ElementType.Ice);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 15);
                node2 = new DropNode(ItemType.YETI_HEART, "Food", 1f, 3);
                node3 = new DropNode(ItemType.ICE_CORE, "Resource", 1f, 3);

                monster.DropList.Add(node1);
                monster.DropList.Add(node2);
                monster.DropList.Add(node3);

                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.5f, 0.3f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.4f, 0.7f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ENERGY_PULSE, 18f));
                monster.isBoss = true;
                if(Game.Current.CurrentGameMode == GameMode.Survial)
                {
                    monster.CanRunAwayFrom = false;
                }

                break;

            //map two difficulty ----------------------------------------------------------
            case MonsterType.ALIVE_ZOMBIE:
                monster = new Monster(type, 3, 76, 5, 220, ElementType.Dark);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.3f, 0.5f));
                break;

            case MonsterType.ALLIGATOR:
                monster = new Monster(type, 5, 120, 20, 180, ElementType.Ice);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 0.1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.3f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.ALLIGATOR_SKIN, "Resource", 0.75f, 2);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 0.5f, 1.2f));
                break;

            case MonsterType.FISH_GUARDIAN:
                monster = new Monster(type, 1, 18f, 5, 160, ElementType.Ice);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 0.1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.3f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FISH_HAND, "Resource", 0.75f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FISH_SWORD, "Weapon", 0.02f, 1);
                monster.DropList.Add(node1);

                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_MULTI_ATTACK, 0.1f));
                break;

            case MonsterType.VIPER:
                monster = new Monster(type, 2, 25, 10, 210, ElementType.Wind);
                node1 = new DropNode(ItemType.WIND_CORE, "Resource", 0.1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SNAKE_BLOOD, "Food", 0.13f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SNAKE_MEAT, "Food", 0.75f, 5);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Uncommon);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 5f, 3.5f, 0.025f));
                break;

            case MonsterType.FISH_LEADER:
                monster = new Monster(type, 1, 45, 10, 360, ElementType.Ice);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FISH_HAND, "Resource", 0.75f, 1);
                monster.DropList.Add(node1);

                node1 = new DropNode(ItemType.FISH_SWORD, "Weapon", 0.04f, 1);
                monster.DropList.Add(node1);

                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_MULTI_ATTACK, 0.12f));
                break;


            case MonsterType.DEEP_MONSTER_FISH:
                monster = new Monster(type, 1, 45, 20, 480, ElementType.Ice);
                node1 = new DropNode(ItemType.ICE_CORE, "Resource", 0.5f, 2);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FISH, "Food", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DEEP_FISH_SWORD, "Weapon", 0.1f, 1);
                monster.DropList.Add(node1);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_STRONGER, 0.2f, 1.3f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.2f, 0.5f));
                break;


            case MonsterType.GIANT_SNAKE:
                monster = new Monster(type, 5, 210, 15, 520, ElementType.Wind);
                node1 = new DropNode(ItemType.WIND_CORE, "Resource", 0.1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.3f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SNAKE_BLOOD, "Food", 0.1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SNAKE_MEAT, "Food", 0.5f, 10);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 0.4f, 1.5f));

                break;


            case MonsterType.FIRE_FOX:
                monster = new Monster(type, 3, 75, 10, 290, ElementType.Fire);
                node1 = new DropNode(ItemType.FIRE_CORE, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.3f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FOX_FUR, "Resource", 0.6f, 3);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.3f, 1.5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_BURNT_RESIS, 1f));
                break;


            case MonsterType.TREE_MONSTER:
                monster = new Monster(type, 1, 45, 10000, 400, ElementType.Dark);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1f, 1);
                monster.DropList.Add(node1);

                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 8);
                monster.DropList.Add(node1);

                node1 = new DropNode(ItemType.WINTER_WOOD, "Resource", 1f, 15);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);

                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 10f, 5f, 0.035f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.2f, 0.5f));
                break;



            case MonsterType.BLACKSMITH_FIGHTER:
                monster = new Monster(type, 1, 65, 20, 750, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);

                node1 = new DropNode(ItemType.SOUL, "Resource", 0.3f, 5);
                monster.DropList.Add(node1);

                node1 = new DropNode(ItemType.BLACK_IRON, "Resource", 1, (int)UnityEngine.Random.Range(1f, 10f));
                monster.DropList.Add(node1);

                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 0.3f, 1f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.7f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.4f, 1f));
                break;


            case MonsterType.FIRE_DOG:
                monster = new Monster(type, 3, 200, 15, 600, ElementType.Fire);
                node1 = new DropNode(ItemType.FIRE_CORE, "Resource", 0.2f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.FIRE_TEETH, "Resource", 0.45f, 3);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ENERGY_PULSE, 8f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_BURNT_RESIS, 1));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_HEAL, 50f));

                break;

          
            case MonsterType.DEATH_DUMMY:
                monster = new Monster(type, 2, 45, 5, 366, ElementType.Dark);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 0.25f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CLAW, "Weapon", 0.05f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.2f, 1.5f));
                break;


            case MonsterType.JUNIOR_KEEPER:
                monster = new Monster(type, 1, 65, 22, 633, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.HOLY_JUSTICE_HAMMER, "Weapon", 0.05f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.2f, 0.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_HEAL, 300));
                break;


            case MonsterType.DEATH_WITCH:
                monster = new Monster(type, 1, 105, 10, 1020, ElementType.Dark);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_WUDU_STAFF, "Weapon", 0.12f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.1f, 1.2f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 15, 10, 0.7f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_WEAK_RESIS, 0.5f));
                break;

            case MonsterType.HIGH_KEEPER:
                monster = new Monster(type, 1, 100, 30, 1150, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 0.5f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 0.5f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.HOLY_JUDGE_STAFF, "Weapon", 0.12f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.4f, 0.45f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_HEAL, 300));
                break;

            case MonsterType.STONE_GIANT:
                monster = new Monster(type, 3, 150, 10, 999, ElementType.Wind);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.BREAK_BLADE_SWORD, "Weapon", 1f, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MAGIC_IMMUNE, 1f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK, 0.25f, 9f));
                monster.isBoss = true;
                break;

            case MonsterType.ICE_BEAST:
                monster = new Monster(type, 3, 310, 15, 2666, ElementType.Ice);
                node1 = new DropNode(ItemType.HIGH_SKY_FLOWER, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.ICE_BEAST_ARMOR, "Armor", 0.3f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_SHIELD, 0.3f, 0.45f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.8f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.25f, 1.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ICE_SHIELD, 30f, 6f));
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.FIRE_ELF:
                monster = new Monster(type, 1, 65, 15, 650, ElementType.Fire);
                node1 = new DropNode(ItemType.FIRE_AMBER, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.15f, 1f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ELEMENT_CHANGE, 0.25f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_SUPER_DEFENSE, 10, 5));
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.WIND_GIANT_BAT:
                monster = new Monster(type, 2, 105, 20, 750, ElementType.Wind);
                node1 = new DropNode(ItemType.BAT_WING, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_CHAOS, 10, 5));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_PAIN, 19f, 10f));
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.SKELETON_KING:
                monster = new Monster(type, 2, 230, 40, 2900, ElementType.Dark);
                node1 = new DropNode(ItemType.HIGH_SKY_FLOWER, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SKELETON_RING, "Accessory", 0.3f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_VOID_BREATH, 30f, 10f , 0.3f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 0.3f, 2f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_PAIN, 40f, 10f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_WEAK_RESIS,1f));
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.LIGHT_FETE:
                monster = new Monster(type, 1, 120, 15, 2500, ElementType.Holy);
                node1 = new DropNode(ItemType.HIGH_SKY_FLOWER, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.HOLY_HAT, "Accessory", 0.3f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_CRITICAL, 0.1f, 2f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MAGIC_IMMUNE, 1f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.3f, 2f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_POISON_RESIS, 1f));
                monster.isBoss = true;
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.TREASURE_GHOST:
                monster = new Monster(type, 2, 135, 1000f, 1600, ElementType.Dark);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SCROLL_ULTI_HOLY_SPIRIT, "Scroll", 1, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DISPERSE, 15f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.35f, 0.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_CHAOS, 20f, 10f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_POISON_RESIS, 1f));
                monster.isBoss = true;
                break;

            case MonsterType.HOLY_WORRIER:
                monster = new Monster(type, 1, 95, 150, 1950, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.LIGHT_SPEAR, "Weapon", 1f, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 1f, 1f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DISPERSE, 60));
                break;


            case MonsterType.HOLY_BLADER:
                monster = new Monster(type, 1, 85, 15, 1950, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.JUSTICE_ARMOR, "Accessory", 1f, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                break;


            case MonsterType.HOLY_SOURCER:
                monster = new Monster(type, 3, 220, 10, 1950, ElementType.Holy);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 3);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.LIGHT_NECKLACE, "Accessory", 1f, 1);
                monster.DropList.Add(node1);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DOOM, 31));
                break;


        

            case MonsterType.HELL_WORM:
                monster = new Monster(type, 2, 56, 10, 600, ElementType.Fire);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.EVIL_BREATH, "Food", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.3f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_POISON_SMOKE, 5, 4f, 0.05f));
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.OGRE:
                monster = new Monster(type, 5, 135, 15, 750, ElementType.Ice);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.EVIL_BREATH, "Food", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_REFLECT, 0.3f, 5f));
                monster.CanRunAwayFrom = false;
                break;

            case MonsterType.WHITE_WALKER:
                monster = new Monster(type, 1, 42, 0, 886, ElementType.Wind);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.EVIL_BREATH, "Food", 0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.4f, 0.3f));
                monster.CanRunAwayFrom = false;
                break;


            case MonsterType.SHADOW_GHOST:
                monster = new Monster(type, 2, 53, 50, 540, ElementType.Dark);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1, 1);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.SHADOW_HEART, "Food",  0.5f, 1);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_BLOOD_SUCK, 0.3f, 0.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_DISPERSE, 40));
                monster.CanRunAwayFrom = false;
                break;


            case MonsterType.BOSS_DAY_35:
                monster = new Monster(type, 3, 136, 22, 2100, ElementType.Wind);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 45);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.BLUE_CRYSTAL, "Resource", 0.5f, 20);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.RED_CRYSTAL, "Resource", 0.5f, 20);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.WIND_CORE, "Resource", 1f, 5);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.isBoss = true;
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_SKILL_ARMOR_BREAK, 0.15f, 4));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_PURIFY, 4.5f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ELEMENT_SHIELD, 43.5f, 10f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_SUPER_DEFENSE, 25.5f, 7f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.15f));
                if (Game.Current.CurrentGameMode == GameMode.Survial)
                {
                    monster.CanRunAwayFrom = false;
                }
                break;

            case MonsterType.BOSS_DAY_50:
                monster = new Monster(type, 1, 145, 40, 9999, ElementType.Holy);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 100);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.DARK_CORE, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                node1 = new DropNode(ItemType.HOLY_CORE, "Resource", 1f, 10);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.isBoss = true;
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_POISON_RESIS, 1f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_STUN_RESIS, 0.5f));
                monster.DefenseSkills.Add(new MonsterDefenseSkill(SkillType.MONSTER_MISS, 0.2f));
                monster.AttackSkills.Add(new MonsterAttackSkill(SkillType.MONSTER_ATTACK_STUN, 0.2f, 2));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_CHAOS, 30.1f, 10f));
                monster.UltiSkills.Add(new MonsterUltiSkill(SkillType.MONSTER_ULTI_ELEMENT_SHIELD, 50.2f, 15f));
                if (Game.Current.CurrentGameMode == GameMode.Survial)
                {
                    monster.CanRunAwayFrom = false;
                }
                break;

            case MonsterType.BOSS_DAY_65:
                monster = new Monster(type, 3, 300, 50, 5000, ElementType.Dark);
                node1 = new DropNode(ItemType.SOUL, "Resource", 1f, 30);
                monster.DropList.Add(node1);
                monster.GeneralDrops.Add(GeneralDropType.Rare);
                monster.CanRunAwayFrom = false;
                monster.isBoss = true;

                break;

          

        }

		return monster;
	}


}


