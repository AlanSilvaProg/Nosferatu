using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    
    public static void SaveData(Data toSave)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Data data = new Data(toSave);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static Data LoadData()
    {

        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Nothing to Load");
            return null;
        }

    }

}
