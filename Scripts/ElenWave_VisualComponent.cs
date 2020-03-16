using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElenWave_VisualComponent : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Terrain");

        Physics.Raycast(transform.position + new Vector3(0, 20, 0), Vector3.down,out hit, 50, mask);

        transform.Translate(-Vector3.back *Time.deltaTime * 5, Space.Self);
        transform.position = new Vector3(transform.position.x,hit.point.y,transform.position.z);
    }
}
