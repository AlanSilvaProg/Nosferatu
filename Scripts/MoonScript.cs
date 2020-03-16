using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : MonoBehaviour
{
    private Material moonMat;
    // Start is called before the first frame update
    void Start()
    {
        moonMat = GetComponent<Renderer>().material;
        moonMat.SetVector("_MoonPosition", transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoonPosition()
    {
        moonMat.SetVector("_MoonPosition", transform.position);
    }
}
