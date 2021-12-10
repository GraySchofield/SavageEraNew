using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;

[Serializable]
public class ActionEngine
{
	//Resource/Food are still using the old spawn event system.

	// List of available spawn resource events
	private List<SpawnResourceEvent> available_spawn_resource_events = new List<SpawnResourceEvent>();
	// Mapping from event type to current event objects
	private Dictionary<string, SpawnResourceEvent> available_spawn_resource_event_index = new Dictionary<string, SpawnResourceEvent> ();
	// Mapping from event object to button view, will be rebuilt when the game is restarted.
	[NonSerialized]
	private Dictionary<SpawnResourceEvent, WorkButtonView> work_buttons = new Dictionary<SpawnResourceEvent, WorkButtonView> ();
	//[OnDeserialized]
	//private void RebuildAvailableWorks(StreamingContext c){
	//	RebuildAvailableWorks ();
	//}
	public void RebuildAvailableWorks()
	{
		if (work_buttons == null) {
			work_buttons = new Dictionary<SpawnResourceEvent, WorkButtonView> ();
		}
		for (int i = 0; i < available_spawn_resource_events.Count; i++) {
			SpawnResourceEvent e = available_spawn_resource_events [i];
			if (work_buttons.ContainsKey (e)) {
				work_buttons [e].MoveTo (i);
			} else {
               // Debug.Log("Rebuild Button!");
				work_buttons [e] = new WorkButtonView (GameObject.Find ("WorkScrollContent"), e.Name, i, delegate {
					e.GetResource();
					RemoveWork(e);
				});
			}
		}
	}
	public void AddWork(SpawnResourceEvent e){
		available_spawn_resource_events.Add (e);
		available_spawn_resource_event_index [e.Type] = e;
		int index = available_spawn_resource_events.Count - 1;
		work_buttons [e] = new WorkButtonView (GameObject.Find ("WorkScrollContent"), e.Name, index, delegate {
			e.GetResource();
			// remove the work event button.
			RemoveWork(e);
		});
	}
	public void RemoveWork(SpawnResourceEvent e){
		// remove all the index
		available_spawn_resource_events.Remove (e);
		available_spawn_resource_event_index.Remove (e.Type);
		// remove button
		if (work_buttons.ContainsKey (e)) {
			work_buttons [e].Remove ();
			work_buttons.Remove (e);
		}
        // rearrange buttons
        if (Game.Current.IsAtHome)
        {
            RebuildAvailableWorks();
        }
	}
	public void RemoveWork(string type){
		// some type of work event may haven't been spawned.
		if (!available_spawn_resource_event_index.ContainsKey (type))
			return;
		
		SpawnResourceEvent e = available_spawn_resource_event_index [type];
		RemoveWork (e);
	}

	// make tool, weapon, armor, accessroy, build building and learn skill are using the new Action engine

	private List<Action> available_make_tool_actions = new List<Action>();
	private Dictionary<string, Action> available_make_tool_action_index = new Dictionary<string, Action> ();

    [NonSerialized]
	private Dictionary<Action, ButtonView> tool_buttons = new Dictionary<Action, ButtonView> ();

	private List<Action> available_build_construction_actions = new List<Action>();
	private Dictionary<string, Action> available_build_construction_action_index = new Dictionary<string, Action> ();

    [NonSerialized]
    private Dictionary<Action, ButtonView> construction_buttons = new Dictionary<Action, ButtonView>();

    private List<Action> available_make_weapon_actions = new List<Action>();
	private Dictionary<string, Action> available_make_weapon_action_index = new Dictionary<string, Action> ();
	
	private List<Action> available_make_armor_actions = new List<Action>();
	private Dictionary<string, Action> available_make_armor_action_index = new Dictionary<string, Action> ();
	
	private List<Action> available_make_accessory_actions = new List<Action>();
	private Dictionary<string, Action> available_make_accessory_action_index = new Dictionary<string, Action> ();
	
	private List<Action> available_learn_skill_actions = new List<Action>();
	private Dictionary<string, Action> available_learn_skill_action_index = new Dictionary<string, Action> ();

	public void RebuildActions(string clazz, GameObject parentObject = null){
		if (clazz == "Tool") {
			if(tool_buttons == null){
				tool_buttons = new Dictionary<Action, ButtonView> ();
			}
			RebuildActions(available_make_tool_actions, tool_buttons, (parentObject == null)?(GameObject.Find ("MakeToolScrollContent")):(parentObject));
		} else if (clazz == "Building") {

            if (construction_buttons == null){
				construction_buttons = new Dictionary<Action, ButtonView> ();
			}
			RebuildActions(available_build_construction_actions, construction_buttons, (parentObject == null)?(GameObject.Find ("ConstructionScrollContent")):(parentObject));
		} else if (clazz == "Weapon") {
			RebuildActions(available_make_weapon_actions, parentObject);
		} else if (clazz == "Skill") {
			RebuildActions(available_learn_skill_actions, parentObject);
		} else if (clazz == "Armor") {
			RebuildActions(available_make_armor_actions, parentObject);
		} else if (clazz == "Accessory") {
			RebuildActions(available_make_accessory_actions, parentObject);
		} else {
			Debug.LogError("Unkown class.");
			return;
		}
	}

	public void AddAction(Action action){
		if (action is MakeToolAction) {
			AddAction(action, available_make_tool_actions, available_make_tool_action_index, tool_buttons, GameObject.Find ("MakeToolScrollContent"));
		} else if (action is BuildConstructionAction) {
			AddAction(action, available_build_construction_actions, available_build_construction_action_index, construction_buttons, GameObject.Find ("ConstructionScrollContent"));
		} else if (action is MakeWeaponAction) {
			AddAction(action, available_make_weapon_actions, available_make_weapon_action_index);
		} else if (action is MakeArmorAction) {
			AddAction(action, available_make_armor_actions, available_make_armor_action_index);
		} else if (action is MakeAccessoryAction) {
			AddAction(action, available_make_accessory_actions, available_make_accessory_action_index);
		} else if (action is LearnSkillAction) {
			AddAction(action, available_learn_skill_actions, available_learn_skill_action_index);
		} else {
			Debug.LogError("Unkown action.");
			return;
		}

		Game.Current.Recorder.Track (action.Type);
	}

	public void RemoveAction(Action action){
		if (action is MakeToolAction) {
			RemoveAction(action, available_make_tool_actions, available_make_tool_action_index, tool_buttons, GameObject.Find ("ToolScrollContent"));
		} else if (action is BuildConstructionAction) {

            RemoveAction(action, available_build_construction_actions, available_build_construction_action_index, construction_buttons, GameObject.Find ("ConstructionScrollContent"));
		} else if (action is MakeWeaponAction) {
			RemoveAction(action, available_make_weapon_actions, available_make_weapon_action_index);
		} else if (action is MakeArmorAction) {
			RemoveAction(action, available_make_armor_actions, available_make_armor_action_index);
		} else if (action is MakeAccessoryAction) {
			RemoveAction(action, available_make_accessory_actions, available_make_accessory_action_index);
		} else if (action is LearnSkillAction) {
			RemoveAction(action, available_learn_skill_actions, available_learn_skill_action_index);
		} else {
			Debug.LogError("Unkown action.");
			return;
		}
	}

	public void DestroyAllViewIndexing(){
       
		work_buttons.Clear ();
		construction_buttons.Clear ();
		tool_buttons.Clear ();
      
	}

	// private generic functions

	private void RebuildActions(List<Action> availableActions, Dictionary<Action, ButtonView> actionButtons, GameObject parentObject)
	{
		for (int i = 0; i < availableActions.Count; i++) {
			Action action = availableActions [i];
			if (actionButtons.ContainsKey (action)) {
				actionButtons [action].MoveTo (i);
			} else {
				actionButtons [action] = action.CreateButtonView(parentObject, i);
			}
		}
	}
	private void RebuildActions(List<Action> availableActions, GameObject parentObject)
	{
		for (int i = 0; i < availableActions.Count; i++) {
			Action action = availableActions [i];
			action.CreateButtonView(parentObject, i);
		}
	}
	private void AddAction(Action action, List<Action> availableActions, Dictionary<string, Action> availableActionIndex, Dictionary<Action, ButtonView> actionButtons, GameObject parentObject){
		AddAction (action, availableActions, availableActionIndex);
        if (parentObject != null)
        {
            int index = availableActions.Count - 1;
            if(actionButtons == null)
            {
                actionButtons = new Dictionary<Action, ButtonView>();
            }
            actionButtons[action] = action.CreateButtonView(parentObject, index);
        }
	}
	private void AddAction(Action action, List<Action> availableActions, Dictionary<string, Action> availableActionIndex){
		availableActions.Add (action);
		availableActionIndex [action.Type] = action;
	}
	private void RemoveAction(Action action, List<Action> availableActions, Dictionary<string, Action> availableActionIndex, Dictionary<Action, ButtonView> actionButtons, GameObject parentObject){
		RemoveAction (action, availableActions, availableActionIndex);
		if (actionButtons.ContainsKey (action)) {
			actionButtons [action].Remove ();
			actionButtons.Remove (action);
		}
		RebuildActions (availableActions, actionButtons, parentObject);
	}
	private void RemoveAction(Action action, List<Action> availableActions, Dictionary<string, Action> availableActionIndex){
		availableActions.Remove (action);
		availableActionIndex.Remove (action.Type);
	}
}

