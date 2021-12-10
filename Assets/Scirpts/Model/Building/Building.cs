using System;

[Serializable]
abstract public class Building: BaseModel {
	
	public Building(string type):base(type){
	}

	public Building(Building b):base(b){
	}

	public abstract Building Clone ();
}