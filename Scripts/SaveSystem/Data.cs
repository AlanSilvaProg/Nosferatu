using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data : MonoBehaviour
{

    public static Data Instance;

    [HideInInspector] public int bow_Ammo;
    [HideInInspector] public float life;
    [HideInInspector] public float fury;
    [HideInInspector] public Vector3 position;

    void Start()
    {
        Instance = this;

        if(CONTINUOUSDATA.Instance != null)
        {
            if (CONTINUOUSDATA.Instance.onLoadGame)
            {
                LoadGame();
            }
        }
    }

    public Data ( Data data)
    {
        bow_Ammo = data.bow_Ammo;
    }

    public void SaveGame()
    {
        SaveSystem.SaveData(this);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        Data loadData = SaveSystem.LoadData();
        if(loadData != null)
        {
            bow_Ammo = loadData.bow_Ammo;
            fury = loadData.fury;
            life = loadData.life;
            position = loadData.position;
        }
        else
        {
            return;
        }
    }

}
