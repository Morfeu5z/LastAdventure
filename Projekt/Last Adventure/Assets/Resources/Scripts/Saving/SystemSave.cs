using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    static string path_dir = Application.streamingAssetsPath + "/data";
    static string path_sav = Application.streamingAssetsPath + "/data/la_data_0.sav";

    public static void SaveGame(DataSave data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!Directory.Exists(path_dir)) Directory.CreateDirectory(path_dir);
        FileStream strem = new FileStream(path_sav, FileMode.Create);
        formatter.Serialize(strem, data);
        strem.Close();
    }

    public static DataSave LoadGame()
    {
        if (!Directory.Exists(path_dir)) Directory.CreateDirectory(path_dir);
        if (File.Exists(path_sav))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream strem = new FileStream(path_sav, FileMode.Open);
            DataSave data = formatter.Deserialize(strem) as DataSave;
            strem.Close();
            return data;
        }
        else
        {
            DataSave data = NewGame();
            return data;
        }
    }
    public static DataSave NewGame()
    {
        DataSave data = new DataSave(1, 0, 0, 1, "", "", "", "");
        SaveGame(data);
        return data;
    }
}


[System.Serializable]
public class DataSave
{
    public int scene;
    public float[] position;
    public int life;
    public string equipItems;
    public string activeItems;
    public string itemsToDestroy;
    public string wasActive;

    public DataSave(
        int sceneIndex = 1,
        float x = 0,
        float y = 0,
        int health = 1,
        string eqItems = "",
        string acItems = "",
        string dsItem = "",
        string usObject = ""
        )
    {
        scene = sceneIndex;
        position = new float[2];
        position[0] = x;
        position[1] = y;
        life = health;
        equipItems = eqItems;
        activeItems = acItems;
        itemsToDestroy = dsItem;
        wasActive = usObject;
    }
}
