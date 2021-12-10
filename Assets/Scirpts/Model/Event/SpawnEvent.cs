using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Reflection;
using System.Linq;

// event to create a clickable button that can generate items/buildings
[System.Serializable]
abstract public class SpawnEvent: BaseEvent
{
	protected List<Item> require_items;
	protected List<Building> require_buildings;

	// extra parameters
	public bool Repeatable;

	private bool game_time_reset = false;

	public SpawnEvent (string type, List<Item> requireItems, List<Building> requireBuildings, List<string> otherConditions):base(type)
	{
		this.require_items = requireItems;
		this.require_buildings = requireBuildings;

		if (otherConditions != null) {
			foreach (string conditionXML in otherConditions) { 
				XmlDocument conditionDoc = new XmlDocument();
				conditionDoc.LoadXml(conditionXML);
				XmlNode conditionNode = conditionDoc.DocumentElement;
				string methodName = conditionNode["MethodName"].InnerText;
				Type storyConditionFactoryType = typeof(StoryConditionFactory);
				MethodInfo info = storyConditionFactoryType.GetMethod(methodName);
				Condition condition = (Condition)Delegate.CreateDelegate(typeof(Condition), info);
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				foreach(XmlNode child in conditionNode.ChildNodes){
					parameters[child.Name] = child.InnerText;
				}
				AddCondition(new ConditionWithParams(condition, parameters));
			}
		}
			
		StartTime = Game.Current.GameTime;
	}
	public SpawnEvent (SpawnEvent e): base(e)
	{
		if (e.require_items != null) {
			require_items = e.require_items.Select (i => i).ToList ();
		}
		if (e.require_buildings != null) {
			require_buildings = e.require_buildings.Select (i => i).ToList ();
		}

        Repeatable = e.Repeatable;
        game_time_reset = e.game_time_reset;
		
		StartTime = e.StartTime;
	}

	protected override bool condition(){
		Game g = Game.Current;

		//spawnevents are not happening in adventure
		if (!g.IsAtHome) {
			return false;
		}

		// check cool down
		if ((g.Recorder.Get (Type) != null && g.Recorder.Get(Type).Count > 1 || game_time_reset) && g.GameTime - StartTime < CoolDown) {
			return false;
		}

		if (UnityEngine.Random.value >= Probability) {
			StartTime = g.GameTime;
			game_time_reset = true;
			return false;
		}

		if (!Doable (ignoreCount: true)) {
			return false;
		}

		foreach (ConditionWithParams c in conditions) {
			if (!c.Condition (c.Params)){
				return false;
			}
		}

		return true;
	}

	public virtual bool Doable(bool ignoreCount = false, bool spawnIgnore = true){
		MainCharacter h = Game.Current.Hero;

		// check require items
		if (require_items != null){
			foreach (Item require in require_items) {
				if (spawnIgnore && require.SpawnIgnore) continue;

				if(require is Tool && !((Tool)require).IsEquipment){
					if(h.EquippedTool != null && !h.EquippedTool.Type.Equals(require.Type)){
						return false;
					}
				}
				else if(require is Tool && ((Tool)require).IsEquipment){
					if(h.Equipment != null && !h.Equipment.Type.Equals(require.Type)){
						return false;
					}
				}
				else if (!Game.Current.Hero.UserInventory.Has (require, ignoreCount)) {
					// this logic looks not intuitive
					if(!ignoreCount){
						Game.Current.AddToast(require.Name + " " + Lang.Current["not_enough"]);
					}
					return false;
				}
			}
		}
		
		// check require buildings
		if (require_buildings != null) {
			foreach (Building require in require_buildings) {
				if (spawnIgnore && require.SpawnIgnore) continue;

				if (!Game.Current.Hero.UserConstructions.Has (require)) {
					if(!ignoreCount)
						Game.Current.AddToast(Lang.Current["need"] + " " +require.Name);
					return false;
				}
			}
		}
		
		return true;
	}

	protected virtual void ConsumeItem(out bool toolUseUp){
		toolUseUp = false;

		MainCharacter hero = Game.Current.Hero;
		if (require_items != null) {
			foreach (Item requireItem in require_items) {
				if (requireItem is Tool) {
					Tool tool = requireItem as Tool;
					// we have checked that the tool has been equipped.
					hero.UseTool (tool.IsEquipment, tool.Cost, out toolUseUp);
					//Game.Current.AddToast(Lang.Current["lose"] + " " + tool.Name + " " + (int)(tool.Cost*100) + "%");
				} else {
					hero.UserInventory.Remove (requireItem);
					if (requireItem is StackableItem) {		
						//Game.Current.AddToast(Lang.Current["lose"] + " " + requireItem.Name + "*" + ((StackableItem)requireItem).Count);
					}
				}
			}
		}
	}

	public override void reset(){
		StartTime = Game.Current.GameTime;
	}
}
