using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;


[System.Serializable]
public class Achievement  {

    // the subclass hold all the 
    // type to id mappings
    public class AchievementType
    {
#if UNITY_ANDROID
        public static string SURVIVAL_10_DAY = "CgkIop-T-Y4IEAIQBQ";
        public static string SURVIVAL_30_DAY = "CgkIop-T-Y4IEAIQBg";
        public static string SURVIVAL_60_DAY = "CgkIop-T-Y4IEAIQBw";
        public static string SURVIVAL_100_DAY = "CgkIop-T-Y4IEAIQCQ";
        public static string FIRST_BAD_COOK = "CgkIop-T-Y4IEAIQCg";
        public static string FIRST_VICTORY = "CgkIop-T-Y4IEAIQCw";
        public static string FIRST_SKILL_UPGRADE = "CgkIop-T-Y4IEAIQDA";
        public static string FIRST_REPAIR = "CgkIop-T-Y4IEAIQDQ";
        public static string FIRST_REBUID = "CgkIop-T-Y4IEAIQDg";
        public static string TAKE_POISON_ENHANCER = "CgkIop-T-Y4IEAIQDw";
        public static string TAKE_A_DREAM = "CgkIop-T-Y4IEAIQEA";
        public static string ADVENTURE_START = "CgkIop-T-Y4IEAIQEQ";
        public static string RUNAWAY = "CgkIop-T-Y4IEAIQEg";
        public static string TELEPORT = "CgkIop-T-Y4IEAIQEw";
        public static string ELEMENT_LVL_3 = "CgkIop-T-Y4IEAIQFA";
        public static string SKILL_MAX = "CgkIop-T-Y4IEAIQFQ";
        public static string LUCKY_DEAL_SOUL = "CgkIop-T-Y4IEAIQFg";
#elif UNITY_IPHONE
        public static string SURVIVAL_10_DAY = "CgkIopTY4IEAIQBQ";
        public static string SURVIVAL_30_DAY = "CgkIopTY4IEAIQBg";
        public static string SURVIVAL_60_DAY = "CgkIopTY4IEAIQBw";
        public static string SURVIVAL_100_DAY = "CgkIopTY4IEAIQCQ";
        public static string FIRST_BAD_COOK = "CgkIopTY4IEAIQCg";
        public static string FIRST_VICTORY = "CgkIopTY4IEAIQCw";
        public static string FIRST_SKILL_UPGRADE = "CgkIopTY4IEAIQDA";
        public static string FIRST_REPAIR = "CgkIopTY4IEAIQDQ";
        public static string FIRST_REBUID = "CgkIopTY4IEAIQDg";
        public static string TAKE_POISON_ENHANCER = "CgkIopTY4IEAIQDw";
        public static string TAKE_A_DREAM = "CgkIopTY4IEAIQEA";
        public static string ADVENTURE_START = "CgkIopTY4IEAIQEQ";
        public static string RUNAWAY = "CgkIopTY4IEAIQEg";
        public static string TELEPORT = "CgkIopTY4IEAIQEw";
        public static string ELEMENT_LVL_3 = "CgkIopTY4IEAIQFA";
        public static string SKILL_MAX = "CgkIopTY4IEAIQFQ";
        public static string LUCKY_DEAL_SOUL = "CgkIopTY4IEAIQFg";
#endif
    }


    public Achievement()
    {
        UnlockedAchievements = new List<string>();

    }

    // singleton
    private static Achievement current;
    public static Achievement Current
    {
        get
        {
            return current;
        }
    }

    public List<string> UnlockedAchievements
    {
        get;
        private set;
    }
    
    public void UnlockAchievement(string achieve_type)
    {
        if (!UnlockedAchievements.Contains(achieve_type)
            && Config.NeedAchievement)
        {
            //only unlock when not unlocked 
            //update achieve to google play
            //save local data
            Social.ReportProgress(achieve_type, 100.0f, (bool success) => {
                // handle success or failure
                if (success)
                {
                   // Game.Current.AddToast("Achievement Unlocked!");
                    UnlockedAchievements.Add(achieve_type);
                    SaveAchievement();
                }
            });
        }
    }


    public void SaveAchievement()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        try
        {
            file = File.Create(Application.persistentDataPath + "/acc1.gd");
            bf.Serialize(file, this);
        }
        finally
        {
            if (file != null) file.Close();
        }
    }


    public static void LoadAchievement()
    {
        string filename;
        filename = "/acc1.gd";
        if (File.Exists(Application.persistentDataPath + filename))
        {
            //     Debug.Log(Application.persistentDataPath + filename);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
                Achievement a = (Achievement)bf.Deserialize(file);
                current = a;               
                return;
            }
            finally
            {
                if (file != null) file.Close();
            }
        }
        current = new Achievement();
        return;
    }



}
