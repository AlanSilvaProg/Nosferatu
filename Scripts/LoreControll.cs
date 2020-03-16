using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Lore
{
    public string Title = "Title Here";
    public string ContentPageOne = "Content Page one Here";
    public string ContentPageTwo = "Content Page Two Here";
    public int Indice = 0;
}

public class LoreControll : MonoBehaviour
{

    public static LoreControll Instance;

    public GameObject buttomTemplate;
    public GameObject buttomFather;

    [HideInInspector] public List<GameObject> allButtonsSpawned;

    public Lore[] allLores;
    public List<Lore> unlockedLores;
 
    private void Awake()
    {

        
        Instance = this;

        var jsonTextFile = Resources.LoadAll<TextAsset>("Lore/");
        allLores = new Lore[jsonTextFile.Length];
        for (int i = 0; i < jsonTextFile.Length; i++)
        {
            allLores[i] = JsonUtility.FromJson<Lore>(jsonTextFile[i].text);
        }

        buttomTemplate.SetActive(false);

        
    }

    

    public void NewLore(int indice)
    {
        unlockedLores.Add(allLores[indice]);
        for (int i = 0; i < allLores.Length; i++)
        {
            if (i != 0 && allLores[i].Indice == indice)
            {
                GameObject clone = Instantiate(buttomTemplate, buttomFather.transform);
                Vector3 pos = clone.transform.position;
                clone.transform.position = pos;
                clone.GetComponent<ButtomLore>().SetInformation(allLores[i]);
                clone.SetActive(true);
                allButtonsSpawned.Add(clone);
                return;
            }
            else if (allLores[i].Indice == indice)
            {
                GameObject clone = Instantiate(buttomTemplate, buttomFather.transform);
                Vector3 pos = clone.transform.position;
                clone.transform.position = pos;
                clone.GetComponent<ButtomLore>().SetInformation(allLores[i]);
                clone.SetActive(true);
                allButtonsSpawned.Add(clone);
                return;
            }
        }
        
    }
    
}
