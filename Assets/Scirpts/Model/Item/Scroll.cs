using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Scroll.
/// </summary>
[Serializable]
public class Scroll : Item {
	
	public Scroll(string type): base(type)
	{
	}
	
	public Scroll(Scroll s): base(s){
	}

	
	public override Item Clone(){
		return new Scroll (this);
	}
}