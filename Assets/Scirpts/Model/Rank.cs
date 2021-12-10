using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

[System.Serializable]
public class Rank  {
    public Rank()
    {
        DayCountNormal = 0;
        DayCountSurvival = 0;
        EvilCountNormal = 0;
        EvilCountSurvival = 0;
        DyingDayCountNormal = 0;
        DyingDayCountSurvival = 0;
        EncryptionId = SystemInfo.deviceUniqueIdentifier.GetHashCode();
        SaveRank();
    }

    // singleton
    private static Rank current;
    public static Rank Current
    {
        get
        {
            return current;
        }
    }

    public int DayCountSurvival
    {
        get;
        set;
    }
	
    public int EvilCountSurvival
    {
        get;
        set;
    }


    public int DayCountNormal
    {
        get;
        set;
    }

    public int EvilCountNormal
    {
        get;
        set;
    }


    public int DyingDayCountNormal
    {
        get;
        set;
    }


    public int DyingDayCountSurvival
    {
        get;
        set;
    }


    public int EncryptionId
    {
        get;
        set;
    }

    public void UpdateDayCount(int day_count, GameMode mode)
    {
        if(mode == GameMode.Normal)
        {
            if(day_count > DayCountNormal)
            {
                DayCountNormal = day_count;
                this.SaveRank();
            }
        }
        else if (mode == GameMode.Survial)
        {
            if(day_count > DayCountSurvival)
            {
                DayCountSurvival = day_count;
                this.SaveRank();
            }
        }
    }

    public void UpdateDyingDayCount(int day_count, GameMode mode)
    {
        if (mode == GameMode.Normal)
        {
            if (day_count > DyingDayCountNormal)
            {
                DyingDayCountNormal = day_count;
                this.SaveRank();
            }
        }
        else if (mode == GameMode.Survial)
        {
            if (day_count > DyingDayCountSurvival)
            {
                DyingDayCountSurvival = day_count;
                this.SaveRank();
            }
        }
    }



    public void UpdateEvil(int lvl, GameMode mode)
    {
        if(mode == GameMode.Normal)
        {
            if(lvl > EvilCountNormal)
            {
                EvilCountNormal = lvl;
                this.SaveRank();
            }

        }else if (mode == GameMode.Survial)
        {
            if(lvl > EvilCountSurvival)
            {
                EvilCountSurvival = lvl;
                this.SaveRank();
            }

        }
    }

    public void SaveRank()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        try
        {
            file = File.Create(Application.persistentDataPath + "/r_k_2.gd");
            bf.Serialize(file, this);
        }
        finally
        {
            if (file != null) file.Close();
        }
    }


    public static void LoadRank()
    {
        string filename;
        filename = "/r_k_2.gd";
        if (File.Exists(Application.persistentDataPath + filename))
        {
            //     Debug.Log(Application.persistentDataPath + filename);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
                Rank r = (Rank)bf.Deserialize(file);
                //vallidate the unique device hash
                if (r.EncryptionId == SystemInfo.deviceUniqueIdentifier.GetHashCode())
                {
                    //validated !
                    current = r;
                }
                else
                {
                    //if data not vallidated instiantiate a new shop
                    current = new Rank();
                }
                return;
            }
            finally
            {
                if (file != null) file.Close();
            }
        }
        current = new Rank();
        return;
    }




}
