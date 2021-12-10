using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolDetailDialogController : MonoBehaviour {
	//public Tool theTool; //the tool object that is passed to the controller
    public Button MainButtonText;
    public string theToolType;
    private List<Tool> tools = new List<Tool>();

    void Start()
    {
        //get the list of all tools of this type 
        for(int i = 0; i < Game.Current.Hero.UserInventory.AllTools.Count; i++)
        {
            if (Game.Current.Hero.UserInventory.AllTools[i].Type.Equals(theToolType))
            {
                tools.Add(Game.Current.Hero.UserInventory.AllTools[i]);
            }
        }

        if (!tools[0].IsOneTime)
        {
            if (tools[0].IsEquipment)
            {
                if(tools.Contains(Game.Current.Hero.Equipment))
                {
                    MainButtonText.GetComponentInChildren<Text>().text = Lang.Current["remove"];
                    MainButtonText.onClick.RemoveAllListeners();
                    MainButtonText.onClick.AddListener(delegate { UnequipTool(); });

                }
            }
            else
            {
                if (tools.Contains(Game.Current.Hero.EquippedTool))
                {
                    MainButtonText.GetComponentInChildren<Text>().text = Lang.Current["remove"];
                    MainButtonText.onClick.RemoveAllListeners();
                    MainButtonText.onClick.AddListener(delegate { UnequipTool(); });
                }
            }



        }

    }

	public void EquipCurrrentTool(){
        //equip the curretn tool to the main Character
        MainCharacter hero = Game.Current.Hero;
		if (tools[0].IsOneTime) {
            //one time tool, just use it up
            if (tools[0].TakeEffect())
            {
                hero.loses(tools[0]);
            }
            CloseDialog();
        } else {
            if (tools[0].IsEquipment) {
                //the tool is Equipment
                hero.Equipment = tools[0]; //equip the tool
				Invoke ("CloseDialog", 0.2f); //close after 0.5 seconds 
			} else {
                //the tool is Handtool
                if (hero.hasTimedState(StateType.STATE_DIZZY))
                {
                    Game.Current.AddLog(Lang.Current["too_dizzy_to_work"]);
                }
                else
                {
                    hero.EquippedTool = tools[0]; //equip the tool
                }
				Invoke ("CloseDialog", 0.2f); //close after 0.5 seconds 
			}

		}

        if(PlayerPrefs.GetInt("Step") == 4 && Game.Current.GameTime < 300)
        {
            PlayerPrefs.SetInt("Step", 5);
            PlayerPrefs.Save();
        }


    }

    public void UnequipTool()
    {
        if (tools[0].IsEquipment)
        {
            Game.Current.Hero.Equipment = null;
        }else
        {
            Game.Current.Hero.EquippedTool = null;
        }
        CloseDialog();
    }


   public void ThrowAwayAllTools()
    {
        if (tools.Contains(Game.Current.Hero.Equipment))
        {
            Game.Current.Hero.Equipment = null;
        }

        if (tools.Contains(Game.Current.Hero.EquippedTool))
        {
            Game.Current.Hero.EquippedTool = null;
        }


        foreach(Tool t in tools)
        {
            Game.Current.Hero.loses(t);
        }
        CloseDialog();

    }



    public void CloseDialog(){
		Destroy (transform.parent.gameObject); // close the dialog
	}
}
