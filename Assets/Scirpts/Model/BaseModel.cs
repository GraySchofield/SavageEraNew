using System;

[Serializable]
abstract public class BaseModel {
	
	public string Name {
		get;
		set;
	}
	
	public string Description {
		get;
		set;
	}
	
	public string Type {
		get;
		set;
	}

	/*
	 * Determine whether this model can be ingnored when 
	 * we try to display an action button that requires it.
	 * */
	public bool SpawnIgnore {
		get;
		set;
	}

    //0 ,1, 2, 3 the rarity of the item
    public int Tier
    {
        get;
        set;
    }

	public BaseModel(BaseModel m){
		Name = m.Name;
		Description = m.Description;
		Type = m.Type;
		// SpawnIgnore should defalut to be false and alert by explictly set.
		// Maybe not... this method is used by clone, we should exactly duplicate the whole obejct.
		SpawnIgnore = m.SpawnIgnore;
        Tier = m.Tier;
	}
	
	public BaseModel(string type){
		//name and desciption will be set based on the correspond language tag for the type.
		Name = Lang.Current [type];
		Description = Lang.Current [type + "_des"];
		Type = type;
        Tier = 0;
	}
}