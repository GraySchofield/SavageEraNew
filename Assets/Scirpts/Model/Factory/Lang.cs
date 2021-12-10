using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;


public class Lang {
	public class LangType{
		public static readonly string CHINESE = "Chinese";
		public static readonly string ENGLISH = "English";
        public static readonly string TRADITIONALCHINESE = "TraditionalChinese";
    }

	private XmlDocument languages;
	private XmlNode curLang;
	private Dictionary<string, string> theLanguage;
	private string curLangType;
	
	public string this[string key]{
		get {
			if(!theLanguage.ContainsKey(key)){
				//Debug.Log(key + " is not defined in language.xml");
				return "";
			}else{
				return theLanguage[key]; 
			}
		}
	}

	public bool Contains(string key){
		if (theLanguage.ContainsKey (key)) {
			return true;
		} else {
			return false;
		}
	}


	public void SetLang(string lang){
		curLangType = lang;
		curLang = languages.SelectSingleNode("/languages/languages[@Name='"+curLangType+"']");
	}

	private Lang(){
		theLanguage = new Dictionary<string, string> ();
		theLanguage.Clear ();
		TextAsset resource = (TextAsset)Resources.Load("Config/Language");
		languages = new XmlDocument ();
		languages.LoadXml (resource.text);
        switch (Application.systemLanguage)
        {
            case SystemLanguage.ChineseSimplified:
                curLangType = LangType.CHINESE;
                Config.DefaultLanguage = LangType.CHINESE;
                break;

            case SystemLanguage.ChineseTraditional:
                curLangType = LangType.TRADITIONALCHINESE;
                Config.DefaultLanguage = LangType.TRADITIONALCHINESE;

                break;

            case SystemLanguage.English:
                curLangType = LangType.ENGLISH;
                Config.DefaultLanguage = LangType.ENGLISH;
                break;


            default:
                curLangType = LangType.CHINESE;
                Config.DefaultLanguage = LangType.CHINESE;
                break;
        }
        //if 0, then just use the default system language 
        if(PlayerPrefs.GetInt("Lang") != 0)
        {
            switch (PlayerPrefs.GetInt("Lang"))
            {
                case 1:
                    //cn
                    curLangType = LangType.CHINESE;
                    Config.DefaultLanguage = LangType.CHINESE;
                    break;
                case 2:
                    //tw
                    curLangType = LangType.TRADITIONALCHINESE;
                    Config.DefaultLanguage = LangType.TRADITIONALCHINESE;
                    break;
                case 3:
                    //en
                    curLangType = LangType.ENGLISH;
                    Config.DefaultLanguage = LangType.ENGLISH;
                    break;
            }
        }


		curLang = languages.SelectSingleNode("/languages/language[@Name='"+curLangType+"']");

		XmlNodeList curLangList = curLang.ChildNodes;


		foreach(XmlNode languageValue in curLangList){
			//Debug.Log ("name :::::: " + languageValue.Name);
			if(!theLanguage.ContainsKey(languageValue.Name))
				theLanguage.Add (languageValue.Name, languageValue.InnerText);
		}
		
	}
	
	private static Lang current;
	public static Lang Current{
		get{
			if (current == null){
				current = new Lang ();
            }
			return current;
		}
	}

	public static void ReloadXML(){
		current = new Lang();
	}
}