using System;

[System.Serializable]
public abstract class TimedState
{
	public TimedState(float duration, string type){
		this.Duration = duration;
		this.currentTime = 0;
		this.Type = type;
		this.Name = Lang.Current [type];
	}

	//how long the duration can last
	public float Duration {
		get;
		set;
	}

	//how much time has the 
	public float currentTime {
		get;
		set;
	}


	public string Type {
		get;
		private set;
	}


	public string Name {
		get;
		set;
	}


	public abstract void UpdateState(float period);
}


