using UnityEngine;
using System.Collections;

public class InstructionController : MonoBehaviour {
    public GameObject[] Steps;
	// Update is called once per frame
	void Update () {
        if(Game.Current.GameTime <= Config.SecondsPerDay)
        {

            for (int i = 0; i < Steps.Length; i++)
            {
                int step_count = PlayerPrefs.GetInt("Step");
                if (step_count == i && Game.Current.GameTime <= Config.SecondsPerDay)
                {
                    Steps[i].SetActive(true);
                }
                else
                {
                    Steps[i].SetActive(false);
                }
            }
        }
        else
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
