using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCol : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision other)
    {
        if (VFXHandler.Instance != null)
        {
            if (EquippedControll.Instance.equip == 0 || EquippedControll.Instance.equip == 2)
            {
                VFXHandler.Instance.SpawnBasicHitVFX(other);
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            if (EquippedControll.Instance.equip == 0 || EquippedControll.Instance.equip == 2)
            {
                other.gameObject.SendMessage("HittedSW", EquippedControll.Instance.guns[0].dmg);
            }
            else
            {
                if (FuryControll.Instance.furyState || Controller.Instance.crouch)
                {
                    other.gameObject.SendMessage("HittedSW", 100);
                }
                else
                {
                    other.gameObject.SendMessage("HittedSW", EquippedControll.Instance.guns[0].dmg);
                }
            }
            
            GetComponent<BoxCollider>().enabled = false;

            if (FuryControll.Instance.furyState)
            {
                FuryControll.Instance.furyState = false;
                FuryControll.Instance.FURY_DISABLE.Invoke();
            }
        }
    }

}
