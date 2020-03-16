using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_MaterialType : MonoBehaviour
{
    public enum ObjMaterial {
        wood = 0,
        stone = 1,
        metal = 2,
        flesh = 3,
        noEffect = 4
    }

    public ObjMaterial type = ObjMaterial.wood;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
