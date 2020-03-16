using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONTINUOUSDATA : MonoBehaviour
{

    public static CONTINUOUSDATA Instance;
    [HideInInspector] public bool onLoadGame;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
