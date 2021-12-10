using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Story: BaseModel
{
	protected List<Option> options = new List<Option> ();

	public Story (string type, List<Option> ops):base(type)
	{
		foreach (Option op in ops) {
			options.Add (op);
		}
        needPop = false;
	}

	public Story(Story s):base(s){
		foreach (Option op in s.options) {
			options.Add (op);
		}
        needPop = s.needPop;
	}

    public bool needPop
    {
        get;
        set;
    }

	public void Run()
	{
		Game g = Game.Current;

		if (options.Count == 0) {
			// no consequence'
			g.AddLog(Description);  // log story
		} 
		else if (options.Count == 1 && !needPop) {
			// Only one set of consequence
			if(Description.Length > 0)
				g.AddLog(Description);
			options[0].Run();
		} 
		else {
			// Multiple sets of options
			// no need to log stories in this case
			new PopupStoryView(this.Description, options);
            if (this.Type.Contains("story_boss_day"))
            {
                //it is a boss event the player can't run away or force close
                GameObject.Find("GM*").GetComponent<GameController>().isBossPopUp = true;
            }
		}

		g.Recorder.Track (Type);

	}

	public Story Clone(){
		return new Story (this);
	}
}

