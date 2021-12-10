using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Option:BaseModel
{
	List<Consequence> consequences = new List<Consequence> ();

	private bool Exclusive {
		get;
		set;
	}
	
	private bool Weighted {
		get;
		set;
	}
	
	public Option (string type, List<Consequence> cons): base(type)
	{
		foreach (Consequence con in cons) {
			consequences.Add (con);
		}
	}

	public void Run(){
		if (Exclusive) {
			System.Random rnd = new System.Random();
			if(Weighted){
				double hint = rnd.NextDouble();
				float sum = 0;
				foreach (Consequence con in consequences) {
					sum += con.Probability;
					if(sum >= hint){
						con.Run();
						break;
					}
				}
			}else{
				int index = rnd.Next(consequences.Count);
				consequences[index].Run();
			}
		} else {
			foreach (Consequence con in consequences) {
				con.Run ();
			}
		}
	}
}


