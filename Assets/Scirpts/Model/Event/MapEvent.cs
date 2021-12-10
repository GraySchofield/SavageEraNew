using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapEvent{
	//Map event is actually  not a subcalss of base event
	public SpawnStoryEvent theStory {
		get;
		private set;
	}
	
	//whether this event has been finished by the player
	public bool IsFinished {
		get;
		set;
	}
	
	public MapEvent(SpawnStoryEvent storyEvent){
		theStory = storyEvent;
		IsFinished = false;
	}


    public void SetMapEventFinished()
    {
        this.IsFinished = true;
    }
}