using UnityEngine;
using UnityEngine.UI;
public class MultiLanguageFont : MonoBehaviour {


    void Awake()
    {
        Text t = GetComponent<Text>();

        if (Config.DefaultLanguage.Equals(Lang.LangType.CHINESE))
        {
            t.font = Resources.Load<Font>("Fonts/font_chinese_simply");
        }
        else if (Config.DefaultLanguage.Equals(Lang.LangType.TRADITIONALCHINESE))
        {
        }
        else
        {
            t.font = Resources.Load<Font>("Fonts/font_chinese_simply");
        }
        
        if (Config.DefaultLanguage == Lang.LangType.ENGLISH)
        {
            t.resizeTextForBestFit = true;
        }

    }
}
