using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : GOSingleton<SaveLoadManager>
{
    [System.Serializable] public class Data
    {
        public Data()
        {
            UserName = "You"; 
            Coin = 99999;
            WeaponCurrent = "Axe";
            IdPantMaterialCurrent = 0;
            //HeadCurrent = "Head1";
            WeaponOwners = new List<WeaponType>();
            PantOwners = new List<int>();
            HeadOwners = new List<string>();
            ShieldOwners = new List<string>();
            SetOwners = new List<string>();
            EquipOwners = new List<Equipment>();
            LevelID = 0;
        }
        public int IdPantMaterialCurrent { get; set; }
        public string WeaponCurrent { get; set; } 
        public string UserName { get; set; }
        public string HeadCurrent { get; set; }
        public string ShieldCurent { get; set; }
        public string SetCurrent { get; set; }
        public List<WeaponType> WeaponOwners { get; set; }
        public List<int> PantOwners { get; set; }
        public List<string> HeadOwners { get; set; }
        public List<string> ShieldOwners { get; set; }
        public List<string> SetOwners { get; set; }
        public List<Equipment> EquipOwners;
        public int Coin { get ; set; }

        public int LevelID { get; set; }
    }

     private string saveFileName = Constant.SAVE_FILE_NAME;
    private string saveFilePath; // ???ng d?n ??n file l?u d? li?u

    [SerializeField] private bool loadOnStart = true;
    private Data data;// Object dung de luu du lieu
    private BinaryFormatter formatter; //Object dung ma hoa data roi luu ra file duoi dang binary

    public Data Data1 { get => data; set => data = value; }

    public void OnInit()// Goi den khi ma nguoi choi vao game
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName); // Lay duong dan cua file minh save
        formatter = new BinaryFormatter();
        Debug.Log(saveFilePath);
        if (loadOnStart)
        {
            Load();
        }
        //UIManager.GetInstance().DisplayMainMenuPanel();
        Debug.Log(saveFileName);
        foreach(string x in data.HeadOwners)
        {
            Debug.Log(x);
        }
    }

    public void Load()
    {
        
        try
        {
            FileStream file = new FileStream(saveFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                data = (Data)formatter.Deserialize(file);
                //data = new Data();
                //if (data.Coin <1000)
                //{
                //    data.Coin = 9999;
                //}
                if (data.UserName.Trim() == "")
                {
                    data.UserName = "You";
                }
                if (data.WeaponCurrent == null)
                {
                    data.WeaponCurrent = "Axe";
                }
                if(data.IdPantMaterialCurrent <=0)
                {
                    data.IdPantMaterialCurrent = 0;
                }
                if(data.PantOwners == null)
                {
                    data.PantOwners = new List<int>();
                }
                if (data.HeadOwners == null)
                {
                    data.HeadOwners = new List<string>();
                }
                if (data.EquipOwners == null)
                {
                    data.EquipOwners = new List<Equipment>();
                }
                //if(data.HeadCurrent == null)
                //{
                //    data.HeadCurrent = "Head1";
                //}
                if (data.ShieldOwners == null)
                {
                    data.ShieldOwners = new List<string>();
                }
                if (data.SetOwners == null)
                {
                    data.SetOwners = new List<string>();
                }
                Debug.Log(data.Coin);
                Debug.Log(data.WeaponCurrent);
            }
            catch
            {
                Debug.Log("Cant Read Data");
                file.Close();
                Save();
            }
            file.Close();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            Save();
        }
    }

    public void Save()
    {
        if(data == null)
        {
            data = new Data();
        }
        try
        {
            FileStream file = new FileStream(saveFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, data);
            file.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
