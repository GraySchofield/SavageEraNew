using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RevivePanelController : MonoBehaviour {

	public void Revive()
    {
        if (Shop.Current.UseProduct(ItemType.REVIVE_STONE, 1))
        {
            Game.Current.Hero.CurrentHealth = Game.Current.Hero.HealthUpperLimit;
            transform.gameObject.SetActive(false);
            if (Game.Current.IsAtHome)
            {
                GameObject.Find("GM*").GetComponent<GameController>().isGamePaused = false;
                GameObject.Find("Audios").transform.FindChild("BGM").gameObject.SetActive(true);

            }
            else
            {
                GameObject.Find("Player").GetComponent<MapController>().isGamePaused = false;
                GameObject.Find("Audios").transform.FindChild("Mapbgm").gameObject.SetActive(true);
            }



        }
    }


    public void JustGoDie()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

}
