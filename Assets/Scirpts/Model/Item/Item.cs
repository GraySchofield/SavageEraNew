using UnityEngine;
using System.Collections;


[System.Serializable]
abstract public class Item: BaseModel {

	public Item(string type): base(type){
	}

	public Item(Item item): base(item){
	}

	public abstract Item Clone();


}
