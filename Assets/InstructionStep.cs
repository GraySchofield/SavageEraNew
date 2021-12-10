using UnityEngine;
using System.Collections;

public class InstructionStep : MonoBehaviour {
    public int TargetStep = 1;

    public void StepUp()
    {
        if(Game.Current.GameTime < 300 && PlayerPrefs.GetInt("Step") == TargetStep)
        {
            PlayerPrefs.SetInt("Step", TargetStep + 1);
            PlayerPrefs.Save();
        }
    }


}
