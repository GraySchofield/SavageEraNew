using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class EventEngine
{
	// mapping from trigger type ( how often the events should be checked ) to the list of event.
	private Dictionary<string, EventList> events = new Dictionary<string, EventList> ();
	
	// mapping from event object to the list of event that hodling this object
	// this index is used to remove a particular event from the event collection.
	private Dictionary<IEvent, EventList> event_to_eventlist = new Dictionary<IEvent, EventList>();
	
	// mapping from the type of event to the event object
	// this index should only be used for singleton event (an event that has at most one object per type)
	private Dictionary<string, IEvent> type_to_event = new Dictionary<string, IEvent> ();
	
	// List of event for a particular trigger type
	[Serializable]
	public class EventList {
		public float Interval;
		public float LastCheckTime;
		List<IEvent> Events = new List<IEvent> ();
		
		public void Process(float time){
			if (time - LastCheckTime < Interval)
				return;

			//Debug.Log("Refresh Events "+Interval+", total : " + Events.Count);
			
			for(int idx = Events.Count - 1; idx >= 0; idx--){
				IEvent e = Events[idx];
				if(e.TryTrigger()){
					Game.Current.EventEngine.RemoveEvent(e);
				}	
			}		
			LastCheckTime = time;
		}
		
		public void Add(IEvent e){
			Events.Add (e);
		}
		
		public void Remove(IEvent e){
			Events.Remove (e);
		}
		
		public int Count{
			get{
				return Events.Count;
			}
		}

        public string getEventNameAt(int i)
        {
            return (Events[i].Type);
    }

    }
	
	public void AddEvent(string triggerType, IEvent e){
		AddEvent (triggerType, e, float.Parse (triggerType));
	}
	
	public void AddEvent(string triggerType, IEvent e, float interval){
		if (events.ContainsKey (triggerType)) {
			events [triggerType].Add (e);
		} else {
			EventList el = new EventList ();
			el.LastCheckTime = Game.Current.GameTime;
			el.Interval = interval;
			events[triggerType] = el;
			el.Add(e);
		}
		event_to_eventlist [e] = events [triggerType];
		type_to_event [e.Type] = e;

        Game.Current.Recorder.Track(e.Type);
    }
	
	public void RemoveEvent(IEvent e){
		if (!event_to_eventlist.ContainsKey (e))
			return;
		EventList el = event_to_eventlist [e];
		el.Remove (e);
		event_to_eventlist.Remove (e);
		type_to_event.Remove(e.Type);
    }

    public void RemoveEvent(string type){
		if (type_to_event.ContainsKey (type)) {
			IEvent e = type_to_event [type];
			RemoveEvent(e);
		}
	}
	
	// try to check all events
	public void ProcessEvent()
	{
		foreach (EventList el in events.Values) {
			el.Process(Game.Current.GameTime);
		}
	}
	
	public int EventCount{
		get{
			int count = 0;
			foreach (EventList el in events.Values) {
				count += el.Count;             
			}
			return count;
		}
	}


   

}

