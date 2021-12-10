using UnityEngine;
using System.Collections;
using System;

public class UniqueKeyGenerator{
	public static string GenerateStringHashKey(){
		Guid g = Guid.NewGuid();
		string GuidString = Convert.ToBase64String(g.ToByteArray());
		return GuidString;
	}
		
}
