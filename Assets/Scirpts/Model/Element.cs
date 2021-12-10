using UnityEngine;
using System.Collections;
[System.Serializable]
public enum ElementType{
	Ice,
	Fire,
	Wind,
	Dark,
	Holy,
	None // no type
}

public class Element  {
	public static string getElementName(ElementType etype){
		switch(etype) {
		case ElementType.Ice:
			return Lang.Current["element_ice"];
		case ElementType.Fire:
			return Lang.Current["element_fire"];
		case ElementType.Wind:
			return Lang.Current["element_wind"];
		case ElementType.Dark:
			return Lang.Current["element_dark"];
		case ElementType.Holy:
			return Lang.Current["element_holy"];
		case ElementType.None:
			return Lang.Current["element_none"];
		}
		return "";
	}

}
