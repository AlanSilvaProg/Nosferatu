using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{

    [Header("Dano que o inimigo receberá enquanto estiver em chamas, de tempo em tempo")]
    public float dmgInOnFire;
    [Header("Dano instantaneo ao estar próximo ao centro da explosão")]
    public float dmgOnExplosion;
    [Header("Duração das chamas no inimigo")]
    public float onFireDuration;
    [Header("Delay entre os danos continuos ao estar em chamas")]
    public float delayPerDmg;
    float count;
    bool hitted;

    // Update is called once per frame
    void Update()
    {
        if (hitted)
        {
            count += Time.deltaTime;
            if(count >= delayPerDmg)
            {
                hitted = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (!hitted)
            {
                hitted = true;
                other.GetComponent<Nosferatu_Basic>().hitByFire(dmgInOnFire, delayPerDmg, onFireDuration);
            }
        }   
    }

}
