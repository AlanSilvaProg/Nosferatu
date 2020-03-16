using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class INFO_PICKABLE
{
    public string itemName = "ITEM NAME"; // nome do item
    public ItemType itemType { get { return ItemType.PICKABLE; } }
    [Header("Pode ser interagido mais de uma vez")]
    public bool moreThanOne; // sempre que for true este item poderá ser interagido mais de uma vez
    [Header("Modelo do item")]
    public GameObject Model;
}

[CreateAssetMenu(menuName = "ITEM/NEW_PICKABLE")]
public class PICKABLE : ScriptableObject
{

    public INFO_PICKABLE[] DESCRIPTION = new INFO_PICKABLE[1];

}