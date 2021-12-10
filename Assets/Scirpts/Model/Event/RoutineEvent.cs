using System.Collections;
using UnityEngine;

/// <summary>
/// Routine that will call a certain function in a period and then loop again.
/// </summary>
[System.Serializable]
public class RoutineEvent: BaseEvent{

	public delegate void Callback();
	private Callback callback;

	public RoutineEvent (string type, Callback callback): base(type)
	{
		this.callback = callback;

		StartTime = Game.Current.GameTime;
	}
	public RoutineEvent (RoutineEvent e):base(e)
	{
		callback = e.callback;
		
		StartTime = e.StartTime;
	}

	protected override bool condition(){
		if (Game.Current.GameTime - StartTime < CoolDown)
			return false;
		
		if (Random.value >= Probability) {
			StartTime = Game.Current.GameTime;
			return false;
		}

		return true;
	}

	// execute the callback
	protected override void action(){
		callback ();
		reset ();
	}

	protected override bool TriggerResult(){
		return false;
	}

	/// <summary>
	/// Reset the start time
	/// </summary>
	public override void reset(){
		StartTime = Game.Current.GameTime;
	}

	public override IEvent Clone(){
		RoutineEvent e = new RoutineEvent(this);
		// should be reset outside this cloen function?
		e.reset ();
		return e;
	}
}

