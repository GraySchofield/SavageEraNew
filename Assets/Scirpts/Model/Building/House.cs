using System;

[Serializable]
public class House: Building
{
	//define how many house in this object
	public int Count {
		get;
		set;
	}

	//define how many people can live in ONE house
	public int Capacity {
		get;
		private set;
	}

	public House(string type, int capacity, int count = 1): base(type){
		Capacity = capacity;
		Count = count;
	}

	public House(House h):base(h){
		Capacity = h.Capacity;
		Count = h.Count;
	}

	public override Building Clone(){
		return new House(this);
	}
}


