using UnityEngine;
using System.Collections;

[System.Serializable]
public class GearsPack{
	//this class is used to represent the gears the palyer is wearing
	//weapon, armor and accessory, will alos handle state change when gear is equipped

	private Weapon equipped_weapon;
	public Weapon EquippedWeapon {
		get{
			return equipped_weapon;
		}
		set{
			//update palyer attack once weapon is equipped
			if(this.equipped_weapon == null){
				equipped_weapon = value;
				Game.Current.Hero.Attack += equipped_weapon.Attack;
			}else{
				Game.Current.Hero.Attack -= equipped_weapon.Attack;
				equipped_weapon = value;
				if(equipped_weapon != null)
					Game.Current.Hero.Attack += equipped_weapon.Attack;
			}
		}
	}
		
	private Armor equipped_armor;
	public Armor EquippedArmor {
		get{
			return equipped_armor;
		}
		set{
			//update player health and armor, once armor is equipped
			if(this.equipped_armor == null){
				equipped_armor = value;
				Game.Current.Hero.HealthUpperLimit += equipped_armor.Health;
				Game.Current.Hero.Defense += equipped_armor.Defense;
				foreach(ElementType key in equipped_armor.ElementResisIndex.Keys){
					Game.Current.Hero.ElementResisIndex[key] *= equipped_armor.ElementResisIndex[key];
				}
			}else{
				Game.Current.Hero.HealthUpperLimit -= equipped_armor.Health;
				Game.Current.Hero.Defense -= equipped_armor.Defense;
				foreach(ElementType key in equipped_armor.ElementResisIndex.Keys){
					Game.Current.Hero.ElementResisIndex[key] /= equipped_armor.ElementResisIndex[key];
				}
				equipped_armor = value;
				if(equipped_armor != null){
					Game.Current.Hero.HealthUpperLimit += equipped_armor.Health;
					Game.Current.Hero.Defense += equipped_armor.Defense;
					foreach(ElementType key in equipped_armor.ElementResisIndex.Keys){
						Game.Current.Hero.ElementResisIndex[key] *= equipped_armor.ElementResisIndex[key];
					}
				}
			}
		}
	}

	private Accessory equipped_accessory;
	public Accessory EquippedAccessory {
		get{
			return equipped_accessory;
		}
		set{
			if(this.equipped_accessory == null){

                equipped_accessory = value;
                if (equipped_accessory != null)
                {
                    Game.Current.Hero.HealthUpperLimit += equipped_accessory.Health;
                    Game.Current.Hero.Defense += equipped_accessory.Defense;
                    Game.Current.Hero.Attack += equipped_accessory.Attack;
                    foreach (ElementType key in equipped_accessory.ElementResisIndex.Keys)
                    {
                        Game.Current.Hero.ElementResisIndex[key] *= equipped_accessory.ElementResisIndex[key];
                    }

                    foreach (ElementType key in equipped_accessory.ElementAttackIndex.Keys)
                    {
                        Game.Current.Hero.ElementAttackBonus[key] += equipped_accessory.ElementAttackIndex[key];
                    }

                    equipped_accessory.TakeSpecialEffect();
                }

            }
            else{
				Game.Current.Hero.HealthUpperLimit -= equipped_accessory.Health;
				Game.Current.Hero.Defense -= equipped_accessory.Defense;
				Game.Current.Hero.Attack -= equipped_accessory.Attack;

				foreach(ElementType key in equipped_accessory.ElementResisIndex.Keys){
					Game.Current.Hero.ElementResisIndex[key] /= equipped_accessory.ElementResisIndex[key];
				}

                foreach (ElementType key in equipped_accessory.ElementAttackIndex.Keys)
                {
                    Game.Current.Hero.ElementAttackBonus[key] -= equipped_accessory.ElementAttackIndex[key];
                }
                equipped_accessory.RemoveSpecialEffect();

                equipped_accessory = value;
				if(equipped_accessory != null){
					Game.Current.Hero.HealthUpperLimit += equipped_accessory.Health;
					Game.Current.Hero.Defense += equipped_accessory.Defense;
					Game.Current.Hero.Attack += equipped_accessory.Attack;
					foreach(ElementType key in equipped_accessory.ElementResisIndex.Keys){
						Game.Current.Hero.ElementResisIndex[key] *= equipped_accessory.ElementResisIndex[key];
					}

                    foreach (ElementType key in equipped_accessory.ElementAttackIndex.Keys)
                    {
                        Game.Current.Hero.ElementAttackBonus[key] += equipped_accessory.ElementAttackIndex[key];
                    }
                    equipped_accessory.TakeSpecialEffect();
                }
            }
		}
	}
}
