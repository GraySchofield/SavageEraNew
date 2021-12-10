using System;

[Serializable]
public class AcademicConstruction: Building {
	public AcademicConstruction(string type):base(type){
	}

	public AcademicConstruction(AcademicConstruction a):base(a){
	}

	public override Building Clone(){
		return new AcademicConstruction(this);
	}
}