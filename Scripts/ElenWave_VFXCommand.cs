using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElenWave_VFXCommand : MonoBehaviour
{
    private float currentRadius = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRadius += Time.deltaTime;
    }
}
