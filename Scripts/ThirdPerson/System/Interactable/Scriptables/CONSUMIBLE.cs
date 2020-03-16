using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType { CONSUMIBLE, PICKABLE}

[System.Serializable]
public enum ConsType { LIFE, ENERGY }

[System.Serializable]
public class INFO_CONSUMIBLE
{
    public string itemName = "ITEM NAME";
    public ItemType itemType { get { return ItemType.CONSUMIBLE; } }
    public float value; // valor adicional 
    public ConsType RECUPERATION_TYPE; // Tipo de valor que será recuperado
    [Header("Modelo do item")]
    public GameObject Model;
}

[CreateAssetMenu(menuName = "ITEM/NEW_CONSUMIBLE")]
public class CONSUMIBLE : ScriptableObject
{

    public INFO_CONSUMIBLE[] DESCRIPTION = new INFO_CONSUMIBLE[1];
   
}
