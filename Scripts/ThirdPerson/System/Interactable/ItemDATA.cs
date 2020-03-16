using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDATA : MonoBehaviour
{

    public static ItemDATA instance;
    public PICKABLE pickables;
    public CONSUMIBLE consumibles;

    private void Awake()
    {
        if(ItemDATA.instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

}
