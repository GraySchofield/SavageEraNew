using UnityEngine;
using UnityEngine.UI;

public class MultiLanguageText : MonoBehaviour {
    public string string_code;

	void Awake()
    {
        Text t = GetComponent<Text>();
        t.text = Lang.Current[string_code];  
        
    }


    

}
