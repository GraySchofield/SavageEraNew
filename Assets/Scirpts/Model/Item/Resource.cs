using UnityEngine;
using System.Collections;

/// <summary>
/// Read only item is stackable.
/// </summary>
[System.Serializable]
public class Resource : StackableItem {

	public Resource(string type, int count): base(type, count)
	{
	}

	public Resource(Resource r): base(r){
	}

	public override Item Clone(){
		return new Resource(this);
	}
}
