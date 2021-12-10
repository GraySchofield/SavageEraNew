using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

using Soomla.Store;
using Soomla;

[System.Serializable]
public class Shop {

    public Shop()
    {
        all_products = new List<StackableItem>();
        product_index = new Dictionary<string, StackableItem>();
        EncryptionId = SystemInfo.deviceUniqueIdentifier.GetHashCode();
        ValidationTime = -1;
    }


    // singleton
    private static Shop current;
    public static Shop Current
    {
        get
        {
            return current;
        }
    }

    public int CoinCount
    {
        get
        {
			return StoreInventory.GetItemBalance(StoreInfo.Currencies[0].ItemId);
        }
    }

    //this encryption id will be unique for each device
    //to ensure that the saved data cannot be propagated
    public int EncryptionId
    {
        get;
        set;
    }


    //the time from 1970 ,1 ,1 
    public int ValidationTime
    {
        get;
        set;
    }


    //All the stuffs that the user has bought
    private List<StackableItem> all_products;
    private Dictionary<string, StackableItem> product_index;

    

    public List<StackableItem> AllProducts()
    {
        return all_products;
    }


    public void AddProduct(string type, int count)
    {
        StackableItem product = new StackableItem(type, count);
        if (product_index.ContainsKey(type))
        {
            product_index[type].Count += count;
        }
        else
        {
            all_products.Add(product);
            product_index.Add(type,product);
        }
        SaveShop();
    }

    public bool HasProduct(string type)
    {
        if (product_index.ContainsKey(type))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public StackableItem GetProduct(string type)
    {
        if (product_index.ContainsKey(type))
        {
            return product_index[type];
        }
        else
        {
            return null;
        }
    }


    public bool UseProduct(string type, int count)
    {
        if (!this.HasProduct(type))
        {
            return false;
        }
        else
        {
            StackableItem product = product_index[type];
            if(product.Count >= count)
            {
                product.Count -= count;
                if(product.Count == 0)
                {
                    product_index.Remove(type);
                    all_products.Remove(product);
                }
                SaveShop();
                return true;
            }
            else
            {
                //May be add a toast here;
                return false;
            }
        }
    }


    public void SaveShop()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = null;
        int t = Game.GetTime();
        PlayerPrefs.SetInt("s_t", t);
        ValidationTime = t;
        try
        {
            file = File.Create(Application.persistentDataPath + "/me_cha_nt.gd");
            bf.Serialize(file, this);
            PlayerPrefs.Save();
        }
        finally
        {
            if (file != null) file.Close();
        }
    }


    public bool Validate()
    {
        if (this.EncryptionId == SystemInfo.deviceUniqueIdentifier.GetHashCode()){
            if (PlayerPrefs.HasKey("s_t"))
            {
                if(PlayerPrefs.GetInt("s_t") == this.ValidationTime)
                {
                    return true;
                }
            }
        }

        return false;
    }


    public static void LoadShop()
    {
        string filename;
        filename = "/me_cha_nt.gd";
        if (File.Exists(Application.persistentDataPath + filename))
        {
       //     Debug.Log(Application.persistentDataPath + filename);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
                Shop s = (Shop)bf.Deserialize(file);
                //vallidate the unique device hash, and system time key
                if(s.Validate())
                {
                    //validated !
                    current = s;
                }
                else
                {
                    //if data not vallidated instiantiate a new shop
                    current = new Shop();
                }
                return;
            }
            finally
            {
                if (file != null) file.Close();
            }
        }
        current = new Shop();
        return;
    }




}
