public interface IEvent{
	/// <summary>
	/// Tries to trigger event.
	/// </summary>
	/// <returns><c>true</c>, if the event is triggered, <c>false</c> otherwise.</returns>
	bool TryTrigger(); 

	/// <summary>
	/// Adds the follow up event.
	/// </summary>
	/// <param name="e">event</param>
	/// <param name="triggerType">The type that how the event should be triggered</param>
	void AddFollowUpEvent(IEvent e, string triggerType);

	/// <summary>
	/// Flag to indicate whether a follow up event should run
	/// </summary>
	/// <value><c>true</c> if run follow up event; otherwise, <c>false</c>.</value>
	bool RunFollowUpEvent{
		get;
		set;
	}

	string Type {
		get;
		set;
	}

	string Name {
		get;
	}

	/// <summary>
	/// Reset the status of the event, mostly reset the start time
	/// </summary>
	void reset();

	IEvent Clone();
}