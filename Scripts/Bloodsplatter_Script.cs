using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodsplatter_Script : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodSys;
    [SerializeField] private GameObject bloodDecal;
    
    List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
      
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {

        ParticlePhysicsExtensions.GetCollisionEvents(bloodSys, other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
        Quaternion anglehit = Quaternion.FromToRotation(Vector3.up, collisionEvents[i].normal);


            GameObject tempSys = Instantiate(bloodDecal, collisionEvents[i].intersection, anglehit);
            tempSys.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
;            VFXHandler.Instance.AddTemporaryDecal(tempSys);
        }

    }
}
