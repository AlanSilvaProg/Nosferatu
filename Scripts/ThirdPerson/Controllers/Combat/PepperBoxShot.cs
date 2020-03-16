using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperBoxShot : MonoBehaviour
{

    public Collider hitBox;

    private void Awake()
    {
        hitBox = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            hitBox.enabled = false;
            other.SendMessage("HitByShotgun", EquippedControll.Instance.guns[1].dmg);
        }
    }


}
