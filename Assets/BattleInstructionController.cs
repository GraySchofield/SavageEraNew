using UnityEngine;
using System.Collections;

public class BattleInstructionController : MonoBehaviour {
    public Animator MovingRingAnimator;
    public BattleEngine battle_engine;
    public GameObject page1;
    public GameObject page2;

	// Use this for initialization
	void Start () {
	    if(PlayerPrefs.GetInt("IsFirstBattle") == 0)
        {
            //first time
            MovingRingAnimator.enabled = false;
            battle_engine.pauseBattle();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
	
    

    public void StepUp()
    {
        if(PlayerPrefs.GetInt("IsFirstBattle") == 0)
        {
            page1.SetActive(false);
            page2.SetActive(true);
            PlayerPrefs.SetInt("IsFirstBattle", 1);
        }
        else if (PlayerPrefs.GetInt("IsFirstBattle") == 1)
        {
            gameObject.SetActive(false);
            MovingRingAnimator.enabled = true;
            battle_engine.ResumeBattle();
            PlayerPrefs.SetInt("IsFirstBattle", 2);
            PlayerPrefs.Save();

        }      
    }




}
