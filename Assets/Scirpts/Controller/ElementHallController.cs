using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElementHallController : MonoBehaviour {
	private Text fire_lvl;
	private Text ice_lvl;
	private Text wind_lvl;
	private Text dark_lvl;
	private Text holy_lvl;


	private Image progress_bar;
	
	private bool isUpgrading = false;
	private float UpgradeTime = 0f; // from 0 to 3, use 3 seconds to cook a dish
	private float UpgradeLength = 3f;

	private ElementType current_upgrade_type = ElementType.None;
    private AudioSource SkillUpgrading;
    private AudioSource Fail;
    private AudioSource Success;



    void Start(){
        SkillUpgrading = GetComponents<AudioSource>()[0];
        Fail = GetComponents<AudioSource>()[1];
        Success = GetComponents<AudioSource>()[2];

        fire_lvl = transform.FindChild ("ElementHallContent").FindChild ("FirePanel").FindChild ("LvL").GetComponent<Text> ();
		ice_lvl = transform.FindChild ("ElementHallContent").FindChild ("IcePanel").FindChild ("LvL").GetComponent<Text> ();
		wind_lvl = transform.FindChild ("ElementHallContent").FindChild ("WindPanel").FindChild ("LvL").GetComponent<Text> ();
		holy_lvl = transform.FindChild ("ElementHallContent").FindChild ("HolyPanel").FindChild ("LvL").GetComponent<Text> ();
		dark_lvl = transform.FindChild ("ElementHallContent").FindChild ("DarkPanel").FindChild ("LvL").GetComponent<Text> ();

	
		progress_bar = transform.FindChild ("ElementHallContent").FindChild ("ProgressBar").GetComponent<Image> ();
		progress_bar.fillAmount = 0;
		RenderElementLevelText ();
	}
	

	private void RenderElementLevelText(){
		MainCharacter hero = Game.Current.Hero;
		fire_lvl.text = "LVL" + hero.ElementLevel [ElementType.Fire] + "\n" + Lang.Current["soul"]+ "*" + hero.CalculateElementSoulRequire(ElementType.Fire)
            + "\n" + Lang.Current["fire_core"] + "*1"; 

		ice_lvl.text = "LVL" + hero.ElementLevel [ElementType.Ice] + "\n" + Lang.Current["soul"] + "*" + hero.CalculateElementSoulRequire(ElementType.Ice) + 
			"\n" + Lang.Current["ice_core"] + "*1"; 

		wind_lvl.text = "LVL" + hero.ElementLevel [ElementType.Wind] + "\n" + Lang.Current["soul"]+ "*" + hero.CalculateElementSoulRequire(ElementType.Wind) + 
			"\n" + Lang.Current["wind_core"] + "*1"; 

		dark_lvl.text = "LVL" + hero.ElementLevel [ElementType.Dark] + "\n" + Lang.Current["soul"] + "*" +  hero.CalculateElementSoulRequire(ElementType.Dark) + 
			"\n" + Lang.Current["dark_core"] + "*1";

		holy_lvl.text = "LVL" + hero.ElementLevel [ElementType.Holy] + "\n" + Lang.Current["soul"] + "*" + hero.CalculateElementSoulRequire(ElementType.Holy) + 
			"\n" + Lang.Current["holy_core"] + "*1";
	}


	public void Upgrade(int type){
		switch (type) {
		case 0:
			current_upgrade_type = ElementType.Fire;
			break;
		case 1:
			current_upgrade_type = ElementType.Ice;
			break;
		case 2:
			current_upgrade_type = ElementType.Wind;
			break;
		case 3:
			current_upgrade_type = ElementType.Holy;
			break;
		case 4:
			current_upgrade_type = ElementType.Dark;
			break;
		}
		isUpgrading = true;
		progress_bar.color = new Color(0.282f, 0.816f, 0.329f);
		
	}
	
	public void Update(){
		if (isUpgrading) {
			UpgradeTime += Time.deltaTime;
			progress_bar.fillAmount = UpgradeTime/UpgradeLength;
            if (!SkillUpgrading.isPlaying)
                SkillUpgrading.Play();
            if (UpgradeTime >= UpgradeLength){
                //cook finished
                if (SkillUpgrading.isPlaying)
                    SkillUpgrading.Stop();
                isUpgrading = false;
				UpgradeTime = 0;
				progress_bar.fillAmount = 1;
				if(Game.Current.Hero.UpgradeElement(current_upgrade_type)){
					//upgrade success
					RenderElementLevelText();
                    Success.Play();
                }
                else{
                    //upgrade fail
                    Fail.Play();
					progress_bar.color = new Color(0.773f,0f,0f);
				}


			}
		}
		
	}
}
