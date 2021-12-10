using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CircleAttackButtonController : MonoBehaviour {
	private float current_radius = 0.3f; //the current total radius of all attack circle
	public Transform basicAttackCircle;
	private float sizePerScale;
	//the above are initialization for normal attack ring


	public Color[] testColors;

	// <-----------------------------Battle Related-------------------------------->//
	public Transform Battle;
	private BattleEngine the_battle_engine;
	//the following value is here only for test,should be reconstructed according to what 
	//weapons the hero has
	private Dictionary<string,AttackRange> AttackCircleIndex; //maps from skill_type, to its range
	private Dictionary<string,Transform> CirclePrefabIndex;//maps from the skill type to the circle prefab
	public Transform Anchor;//used to determine the radius of a moving ring
	private bool isAttacking = true;
	private float current_cd = 5f; // the current cd for player attack, when 0 player can attack
	private Weapon currentWeapon;
	private float currentCoolDownLimit;
	public Animator movingRingAnimator;

	public Transform AttackButton;
	private Image attack_button_image;
    public MainCharacter Hero;

	public void generateAttackCircle(float range,Color color ,string skill_type){
		Transform buttonClone = Instantiate ((Resources.Load("Prefab/AttackCircle") as GameObject).transform, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity) as Transform;
		buttonClone.SetParent (this.transform);
		buttonClone.GetComponent<SpriteRenderer> ().color = color;
		float scale = (range + current_radius) * 2 / sizePerScale ;
		buttonClone.localScale = new Vector3 (scale, scale, scale);
		current_radius += range; //update the current radius


		AttackCircleIndex.Add (skill_type, new AttackRange (current_radius - range, current_radius));
		CirclePrefabIndex.Add (skill_type, buttonClone);
	}

	void Start () {
        Hero = Game.Current.Hero;
        //.orientation = ScreenOrientation.LandscapeLeft;
        attack_button_image = AttackButton.GetComponent<Image> ();

		the_battle_engine = Battle.GetComponent<BattleEngine> ();
		AttackCircleIndex = new Dictionary<string,AttackRange>();
		CirclePrefabIndex = new Dictionary<string,  Transform> ();
		//initialize the basic attack circle
		float size = basicAttackCircle.GetComponent<SpriteRenderer>().bounds.size.x;
		sizePerScale = size / basicAttackCircle.localScale.x;  //local scale x, y should be the same
		current_radius = size / 2;

		AttackCircleIndex.Add("normal_attack",new AttackRange(0f,current_radius));
		CirclePrefabIndex.Add ("normal_attack", basicAttackCircle);
		//Spawn attack circles based on the weapon
		currentWeapon = Hero.Gears.EquippedWeapon;
		if (currentWeapon != null) {
			List<WeaponSkill> weapon_skills = currentWeapon.WeaponSkills;
			if (weapon_skills != null) {
				for (int i = 0; i < weapon_skills.Count; i ++) {
					generateAttackCircle (weapon_skills [i].Range, testColors [i], weapon_skills [i].Type);
					 
				}
			}
			//assign cool down
			current_cd = currentWeapon.CoolDown;
			//assing ring speed
			movingRingAnimator.speed = currentWeapon.RingSpeed;
			currentCoolDownLimit = currentWeapon.CoolDown * Hero.CoolDownMultiplier;
		} else {
			currentCoolDownLimit = 5f;
		}



		//update attack cool down
		//InvokeRepeating ("UpdateAttackCoolDown", 0, period);
	}
	
	void Update(){
        if (currentWeapon != null)
        {
            currentCoolDownLimit = currentWeapon.CoolDown * Hero.CoolDownMultiplier;
        }
        else
        {
            currentCoolDownLimit = 5 * Hero.CoolDownMultiplier;
        }
        UpdateAttackCoolDown(Time.deltaTime);
	}

	public void UpdateAttackCoolDown(float interval){
		if (!isAttacking  && !Game.Current.Hero.IsInBattleState(StateType.BATTLE_STATE_STUN)) {
			//cd is only update when not attacking
			if (current_cd <= interval) {
				//can attack now
				isAttacking = true;
				current_cd = currentCoolDownLimit;
				movingRingAnimator.SetBool ("Started", true);
				attack_button_image.fillAmount = 1;
			} else {
				current_cd -= interval;
				attack_button_image.fillAmount = (currentCoolDownLimit - current_cd)/currentCoolDownLimit;
			}
		}
	}
	

	public void Attack(){
		if (isAttacking) {
			this.isAttacking = false;
			float range = Anchor.position.y  - transform.position.y; // y of anchor is the radius of the ring
			//check each radius and see where the ring hits
			bool isInAnyRange = false;
			foreach (string key in AttackCircleIndex.Keys) {
				AttackRange a_r = AttackCircleIndex [key];
				if (a_r.IsInRange (range)) {
					isInAnyRange = true;
					the_battle_engine.player_attack_soud.Play();
					//Weapon currentWeapon = Game.Current.Hero.Gears.EquippedWeapon;
					CirclePrefabIndex[key].GetComponent<Animator>().SetTrigger("Activated");
					if (key.Equals ("normal_attack")){
						if(currentWeapon != null){
							Damage damage = new Damage (currentWeapon.Element, Game.Current.Hero.Attack);
							the_battle_engine.AttackMonsters (damage);
						}else{
							Damage damage = new Damage (ElementType.None, Game.Current.Hero.Attack);
							the_battle_engine.AttackMonsters (damage);
						}
					} else {
						WeaponSkill effectiveSkill = Game.Current.Hero.Gears.EquippedWeapon.WeaponSkillIndex [key];
						Damage damage = new Damage (Game.Current.Hero.Gears.EquippedWeapon.Element, Game.Current.Hero.Attack);
						effectiveSkill.TakeEffect (the_battle_engine, damage, the_battle_engine.current_monsters);
						the_battle_engine.AttackMonsters (damage);	
                        //set skill name
					}
                    //charge the ulti when clicked in range
                    if(!Game.Current.Hero.IsInBattleState(StateType.BATTLE_STATE_CHAOS))
                        ChargeUlti();

					
				}
			}

			if(!isInAnyRange){
				the_battle_engine.player_attack_miss_soud.Play();
			}

			movingRingAnimator.enabled = false;
			Invoke ("FinishAttackAnimation", 0.2f);
		} 
	}


    private void ChargeUlti()
    {
        if (Game.Current.Hero.CurrentUltiSkill != null)
        {
            the_battle_engine.CurrentUltiCharge += Game.Current.Hero.CurrentUltiSkill.ChargeAmount;
            if (the_battle_engine.CurrentUltiCharge > 1)
            {
                the_battle_engine.CurrentUltiCharge = 1f;
            }
        }
    }




	void FinishAttackAnimation(){
		movingRingAnimator.enabled = true;
		movingRingAnimator.SetBool ("Started", false);
	}


}
