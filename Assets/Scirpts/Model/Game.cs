using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
using System.ComponentModel;
public enum GameMode
{
    Survial,
    Normal
}

[Serializable]
public class Game{
	
	public List<string> Logs {
		get;
		private set;
	}

    public List<string> BackUpLogs
    {
        get;
        private set;
    }

	public List<string> Toasts {
		get;
		private set;
	}
	
	public MainCharacter Hero {
		get;
		private set;
	}

	private float game_time = 0;
	public float GameTime {
		get{
			return game_time;
		}
		set{
			game_time = value;
			EventEngine.ProcessEvent();
		}
	}
	
	//between 0, 1  1 for most bright , 0 for total darkness
	public float Brightness {
		get {
			int time_of_day = (int)GameTime % Config.SecondsPerDay; //how many seconds passed within this day
			float bright = 0f;
			
			if (time_of_day >= Config.SecondsPerDay * 0.6f){
				bright = ((float)(time_of_day) )/ (Config.SecondsPerDay ) * 1.15f;
				if(bright >= 0.95f){
					bright = 0.95f;
				}
			}
			
			return bright;
		}
	}
	
	public bool IsLightOn {
		get;
		set;
	}
	
	//wether we are at home 
	//or we are out at adventure
	public bool IsAtHome {
		get;
		set;
	}

	// some additional utility functions
	public bool IsAtNight{
		get{
			int time_of_day = (int)Game.Current.GameTime % Config.SecondsPerDay; //how many seconds passed within this day
			
			if (time_of_day <= Config.SecondsPerDay * 0.6f) {
				return false;
			} else {
				return true;
			}
		}
	}

	public Recorder Recorder {
		get;
		private set;
	}
	
	public EventEngine EventEngine {
		get;
		private set;
	}

    public ActionEngine ActionEngine {
		get;
		private set;
	}

	public RoutineInjector RoutineInjector {
		get;
		private set;
	}

	public StoryInjector StoryInjector {
		get;
		private set;
	}

	public ActionInjector ActionInjector {
		get;
		private set;
	}

	public ResourceInjector ResourceInjector {
		get;
		private set;
	}

	public TypeRegistry TypeRegistry {
		get;
		private set;
	}


    public GameMode CurrentGameMode
    {
        get;
        set;
    }
    //game start/save/load


    public int EncryptionId
    {
        get;
        set;
    }


    private Game(){
		//initial the same here
		Logs = new List<String> ();
        BackUpLogs = new List<String>();
        Toasts = new List<String> ();
		Hero = new MainCharacter ();
		IsAtHome = true;
		IsLightOn = false;

		Recorder = new Recorder ();
		ActionEngine = new ActionEngine ();
		EventEngine = new EventEngine ();
		ActionInjector = new ActionInjector ();
		RoutineInjector = new RoutineInjector ();
		StoryInjector = new StoryInjector ();
		ResourceInjector = new ResourceInjector ();
		TypeRegistry = new TypeRegistry ();
		Recorder.AddObserver (ActionInjector);
		Recorder.AddObserver (StoryInjector);
        EncryptionId = SystemInfo.deviceUniqueIdentifier.GetHashCode();

        

    }

	// singleton
	private static Game current;
	public static Game Current{
		get 
		{
            return current;
		}
	}

	
	
	public void AddLog(String content){
		Logs.Add (content);
        AddBackUpLog(content);
	}

    //back up logs are used to get back the stories when scene is switched
    public void AddBackUpLog(String content)
    {
        if (BackUpLogs.Count >= 5)
        {
            //exceeds upper limit
            BackUpLogs.RemoveAt(0);
            BackUpLogs.Add(content);
        }
        else
        {
            BackUpLogs.Add(content);
        }
    }


	public void AddToast(string content){
		Toasts.Add (content);
	}

	public void LoadGameView(){
		ActionEngine.RebuildAvailableWorks ();
		ActionEngine.RebuildActions ("Tool");
		ActionEngine.RebuildActions ("Building");
	}
	

    public void UpdateDayCountRank()
    {
        int day_count = (int)(this.GameTime / Config.SecondsPerDay);
        Rank.Current.UpdateDayCount(day_count, this.CurrentGameMode);
    }

    public void SaveGameInMainThread()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        try
        {
            if (this.CurrentGameMode == GameMode.Normal)
            {
                file = File.Create(Config.DataPath + "/sg_n3.gd");

            }
            else
            {
                file = File.Create(Config.DataPath + "/sg_s3.gd");

            }
            bf.Serialize(file, this);
        }
        finally
        {
            if (file != null) file.Close();
        }
    }

	public void SaveGame(){
        BackgroundWorker bw = new BackgroundWorker();
        bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                if (this.CurrentGameMode == GameMode.Normal)
                {
                    file = File.Create(Config.DataPath + "/sg_n3.gd");

                }
                else
                {
                    file = File.Create(Config.DataPath + "/sg_s3.gd");

                }
                bf.Serialize(file, this);
            }
            finally
            {
                if (file != null) file.Close();
            }
        });

        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate (object o, RunWorkerCompletedEventArgs args)
        {
           // Game.Current.AddToast("Save Game completed!");
        });

        bw.RunWorkerAsync();
    }


    public void SaveBackUpGameInMainThread()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        try
        {
            file = File.Create(Config.DataPath + "/5d_bu.gd");
            bf.Serialize(file, this);
        }
        finally
        {
            if (file != null) file.Close();
        }
    }

    public void SaveBackUpGame()
    {
        BackgroundWorker bw = new BackgroundWorker();
        bw.DoWork += new DoWorkEventHandler(delegate (object o, DoWorkEventArgs args)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Create(Config.DataPath + "/5d_bu.gd");
                bf.Serialize(file, this);
            }
            finally
            {
                if (file != null) file.Close();
            }
        });

        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object o,RunWorkerCompletedEventArgs args)
        {
           // Game.Current.AddToast("Save Backup completed!");
        });

        bw.RunWorkerAsync();
       
    }


	public static void LoadGame(GameMode mode){
        string filename;
        if(mode == GameMode.Normal)
        {
            filename = "/sg_n3.gd";
        }
        else
        {
            filename = "/sg_s3.gd";

        }
        if (File.Exists (Application.persistentDataPath + filename)) {
			//Debug.Log(Application.persistentDataPath + filename);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = null;
			try{
				file = File.Open (Application.persistentDataPath + filename, FileMode.Open);
				Game g = (Game)bf.Deserialize (file);
                //vallidate
                if(g.EncryptionId == SystemInfo.deviceUniqueIdentifier.GetHashCode())
                {
                    //passed
                    current = g;
                }
                else
                {
                    //fail, restart the game

                    current = new Game();
                    current.CurrentGameMode = mode;
                   // current.Initialze();
                }
				return ;
			}
            catch(Exception e)
            {
                //when data cannot be loaded, load a back up save
                //need to test this
                if (mode == GameMode.Normal)
                {
                    //only normal mode has backup save
                    LoadBackUpGame();
                }
                else
                {
                    //************************
                    //in survival mode if original data is corruptted
                    //restart the game
                    current = new Game();
                    current.CurrentGameMode = mode;
                }

                Debug.LogError(e.Message);
            }
            finally {
				if(file != null) file.Close ();
			}
		}
        if(mode == GameMode.Normal)
        {
            //no data file at all in normal mode
            //this happens at first launch, or when file is corrupted
            //in android 6.0 +
            LoadBackUpGame();
        }
        else
        {
            //******************
            //no data file at all in survival mode
            //first time play survial mode, or corruptted
            //but nor 
            current = new Game();
            current.CurrentGameMode = mode;
        }
		
        return;
	}


    public static void LoadBackUpGame()
    {
        if (File.Exists(Application.persistentDataPath + "/5d_bu.gd"))
        {
            //Debug.Log(Application.persistentDataPath + "/5d_bu.gd");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Open(Application.persistentDataPath + "/5d_bu.gd", FileMode.Open);
                Game g = (Game)bf.Deserialize(file);
                //vallidate
                if (g.EncryptionId == SystemInfo.deviceUniqueIdentifier.GetHashCode())
                {
                    //passed
                    current = g;
                }
                else
                {
                    //fail, restart the game
                    
                    current = new Game();
                    current.CurrentGameMode = GameMode.Normal;
                }
                return ;
            }
            finally
            {
                if (file != null) file.Close();
            }
        }
        current = new Game();
        current.CurrentGameMode = GameMode.Normal;
        return;
    }

    public static void ResetGame()
    {
        current = new Game();
        current.CurrentGameMode = GameStateManager.current_mode;
        current.SaveGameInMainThread();
        current = null;
    }

    public static void ClearGame()
    {
        current = null;
    }

    public void ReloadGameData()
    {
        EventFactory.Reload();
        ActionInjector.Run();
        ResourceInjector.Run();
        RoutineInjector.Run();
        StoryInjector.Run();
    }


    public static int GetTime()
    {
        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        return ((int)t.TotalSeconds);
    }

}
