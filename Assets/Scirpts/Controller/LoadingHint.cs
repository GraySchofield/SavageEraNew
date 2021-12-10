using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingHint : MonoBehaviour {
    private string[] hints = {"loading_hint_1", "loading_hint_2", "loading_hint_3",
 "loading_hint_4", "loading_hint_5", "loading_hint_6", "loading_hint_7",
 "loading_hint_8",  "loading_hint_9",  "loading_hint_10", "loading_hint_11"}; // all possible hints
    private Text content_text;
	// Use this for initialization
	void Start () {
        content_text = GetComponent<Text>();
        System.Random rnd = new System.Random();
        string type = hints[rnd.Next(0, hints.Length)];
        content_text.text = Lang.Current[type];
	}
	
	
}
