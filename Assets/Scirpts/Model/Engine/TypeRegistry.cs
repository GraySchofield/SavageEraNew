using System;
using System.Collections.Generic;

[Serializable]
public class TypeRegistry
{
	[Serializable]
	public class Entry {
		public string Category;
		public string Clazz;
		public string SubClazz;
		public string Type;

		public Entry(string type, string category, string clazz, string subClazz){
			Category = category;
			Clazz = clazz;
			SubClazz = subClazz;
			Type = type;
		}
	}

	private Dictionary<string, Entry> registry = new Dictionary<string, Entry> ();

	public void Add(string type, string category, string clazz, string subClazz = null){
		registry[type] = new Entry (type, category, clazz, subClazz);
	}

	public Entry Get(string type){
		if (registry.ContainsKey (type)) {
			return registry [type];
		}
		return null;
	}
}

