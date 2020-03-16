using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtomLore : MonoBehaviour
{

    public Lore myContent;
    public TMP_Text buttomText;
    public TMP_Text title, pageOne, pageTwo;

    public void SetInformation(Lore loreInfo)
    {
        myContent = loreInfo;
        buttomText.text = myContent.Title;
    }

    public void OnClickEvent()
    {
        buttomText.text = "" + myContent.Title;
        title.text = "" + myContent.Title;
        pageOne.text = "" + myContent.ContentPageOne;
        pageTwo.text = "" + myContent.ContentPageTwo;
    }

}
