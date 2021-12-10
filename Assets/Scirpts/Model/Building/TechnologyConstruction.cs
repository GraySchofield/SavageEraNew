using System;

[Serializable]
public class TechnologyConstruction: Building {
	public TechnologyConstruction(string type):base(type){
	}
	
	public TechnologyConstruction(TechnologyConstruction a):base(a){
	}
	
	public override Building Clone(){
		return new TechnologyConstruction(this);
	}
}