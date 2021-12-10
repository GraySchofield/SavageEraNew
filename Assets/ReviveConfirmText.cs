using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReviveConfirmText : MonoBehaviour {

    void OnEnable()
    {
        StackableItem revive_stone = Shop.Current.GetProduct(ItemType.REVIVE_STONE);
        Text t = GetComponent<Text>();

        if (revive_stone != null)
        {
            t.text = Lang.Current["revive_confirm"] + "\n"
           + Lang.Current["left"] + ":" + revive_stone.Count;
        }
        else
        {
            t.text = Lang.Current["revive_confirm"] + "\n"
           + Lang.Current["left"] + ":0";
        }
    }


}
