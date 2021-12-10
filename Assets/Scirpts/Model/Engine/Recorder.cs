using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Recorder
{
	[Serializable]
	public class Record
	{
		public float FirstTime;
		public float LastTime;
		public int Count;
		
		public Record(float gameTime){
			FirstTime = gameTime;
			LastTime = gameTime;
			Count = 1;
		}
	}

	private Dictionary<string, Record> records = new Dictionary<string, Record>();

	private List<IRecorderObserver> observers = new List<IRecorderObserver>();

	private void Clear(){
		records.Clear ();
	}

	public Record Get(string type){
		if (records.ContainsKey (type)) {
			return records[type];
		}
		return null;
	}
	
	/// <summary>
	/// Track the type.
	/// </summary>
	public void Track(string type){
		if (records.ContainsKey (type)) {
			Record rec = records [type];
			rec.LastTime = Game.Current.GameTime;
			rec.Count += 1;
		} else {
			records [type] = new Record (Game.Current.GameTime);
		}
		NotifyObserver (type);
	}

	private void NotifyObserver(string type){
		foreach (IRecorderObserver o in observers) {
			o.Notify(this, type);
		}
	}

	public void AddObserver(IRecorderObserver o){
		observers.Add (o);
	}
}


