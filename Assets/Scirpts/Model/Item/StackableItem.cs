using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// stackable.
/// </summary>
[Serializable]
public class StackableItem : Item {

	public int Count {
		get;
		set;
	}
	
	public StackableItem(string type, int count): base(type)
	{
		Count = count;
	}

	public StackableItem(StackableItem s): base(s){
		Count = s.Count;
	}

	public override Item Clone(){
		return new StackableItem (this);
	}
}
