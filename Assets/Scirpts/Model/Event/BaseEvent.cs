using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// The base class for all events
/// </summary>
[Serializable]
abstract public class BaseEvent: BaseModel, IEvent
{
	//wrap an event with a trigger type
	[System.Serializable]
	class EventWithType {
		public IEvent Event;
		//trigger type, e.g "0", "1", "5" etc
		public string Type;
		public EventWithType(IEvent e, string type){
			Event = e;
			Type = type;
		}
	}

	// extra parameters
	public float? StartTime = null;
	public float CoolDown = 0;
	public float Probability = 1; //0 - 1
	
	private List<EventWithType> followup_events = new List<EventWithType>();
	
	// this property determine whether the follow up event should be created. the default is Yes.
	public bool RunFollowUpEvent {
		get;
		set;
	}

	public BaseEvent(string type):base(type){
		RunFollowUpEvent = true;
	}

	public BaseEvent(BaseEvent e):base(e){
		StartTime = e.StartTime;
		CoolDown = e.CoolDown;
		Probability = e.Probability;
		followup_events = e.followup_events.Select(i => i).ToList();
		RunFollowUpEvent = e.RunFollowUpEvent;
		conditions = e.conditions.Select(i => i).ToList();
	}

	// this method will be called to check whether a event can be triggered.
	protected abstract bool condition ();
	// this method will be called as the body of the event
	protected abstract void action ();

	// the event spawned after this event is triggerred.
	public void AddFollowUpEvent(IEvent e, string type){
		followup_events.Add (new EventWithType(e, type));
	}

	// try to trigger the event
	public bool TryTrigger(){
		// check whether the condiction is met
		if (condition ()) {
			// execute the body
			action ();
			// try create follow up events.
			if(RunFollowUpEvent){
				foreach(EventWithType ewt in followup_events){
					ewt.Event.reset();
					Game.Current.EventEngine.AddEvent(ewt.Type, ewt.Event);
				}
                followup_events.Clear();
            }

			// return the result when the event is executed.
			return TriggerResult();
		}
		// the event is not executed.
		return false;
	}

	/// <summary>
	/// Specify what should be returned if the condition is met in TriTrigger
	/// </summary>
	protected virtual bool TriggerResult(){
		return true;
	}

	public virtual void reset(){
	}

	abstract public IEvent Clone();

	#region conditions

	public delegate bool Condition(Dictionary<string, string> parameters);

	[System.Serializable]
	public class ConditionWithParams{
		public Condition Condition;
		public Dictionary<string, string> Params;
		public ConditionWithParams(Condition condiction, Dictionary<string, string> parameters){
			Condition = condiction;
			Params = parameters;
		}
	}

	protected List<ConditionWithParams> conditions = new List<ConditionWithParams>();

	public void AddCondition(ConditionWithParams c){
		conditions.Add(c);
	}


    public int FollowUpEventCount()
    {
        return followup_events.Count;
    }

	#endregion
}


