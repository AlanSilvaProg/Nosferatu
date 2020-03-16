using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRef : MonoBehaviour
{

    public GameObject T_PoseREF;
    
    void Update()
    {
        if(T_PoseREF != null)
        {
            transform.position = T_PoseREF.transform.position;
            transform.rotation = T_PoseREF.transform.rotation;
        }
        else
        {
            Debug.LogError("T_Pose Reference is not found !!! OBS: Insert the t pose reference on Mesh Ref Script !!");
        }
    }
}
