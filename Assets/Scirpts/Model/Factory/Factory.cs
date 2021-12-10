using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public abstract class Factory
{
	public Factory ()
	{
	}

	// helper functions for building require/produce list
	protected static List<Item> BuildItemList(XmlNodeList nodeList){
		List<Item> items = new List<Item> ();
		foreach (XmlNode node in nodeList) {
			items.Add (ItemFactory.BuildFromXMLNode(node));
		}
		return items;
	}

	protected static List<StackableItem> BuildStackableItemList(XmlNodeList nodeList){
		List<StackableItem> stackables = new List<StackableItem> ();
		foreach (XmlNode node in nodeList) {
			stackables.Add (ItemFactory.BuildFromXMLNode(node) as StackableItem);
		}
		return stackables;
	}

	protected static List<Building> BuildBuildingList(XmlNodeList nodeList){
		List<Building> buildings = new List<Building> ();
		foreach (XmlNode node in nodeList) {
			buildings.Add (BuildingFactory.BuildFromXMLNode(node));
		}
		return buildings;
	}

	protected static List<Story> BuildStoryList(XmlNodeList nodeList){
		List<Story> stories = new List<Story> ();
		foreach (XmlNode node in nodeList) {
			stories.Add (StoryFactory.BuildFromXMLNode(node));
		}
		return stories;
	}

	protected static bool IsXParam(string name){
		if (name == "Require")
			return false;
		if (name == "Produce")
			return false;
		if (name == "Condition")
			return false;
		if (name == "Story")
			return false;
		if (name == "Result")
			return false;
		return true;
	}
}

