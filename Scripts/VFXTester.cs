using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTester : MonoBehaviour
{
    [SerializeField] private VFXHandler vfxhandl;
    private Vector3 origin;
    private Quaternion originquat;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        originquat = transform.rotation;
        GetComponent<Rigidbody>().isKinematic=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
           
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        vfxhandl.SpawnBasicHitVFX(collision);
        Instantiate(gameObject, origin, originquat);
        Destroy(gameObject);
    }
}
