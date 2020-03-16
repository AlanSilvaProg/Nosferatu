using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeleeAttacks : MonoBehaviour
{

    public UnityEvent axeCut;
    public Transform axeCenter;
    public Transform shotPoint;

    public void EndDeath()
    {
        PlayerInfo.Instance.DeathCall();
    }

    public void Init()
    {
        Controller.Instance.ruivando = true;
    }

    public void End()
    {
        Controller.Instance.ruivando = false;
    }

    public void StartAssassinateAnimation()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.SpawnBasicMeeleeTrailVFX(axeCenter.transform);
        }
        EquippedControll.Instance.StateAssassinate(true);
    }

    public void OnAssassinate()
    {
        EquippedControll.Instance.guns[0].gun.GetComponent<BoxCollider>().enabled = true;
    }
    public void EndAssassinate()
    {
        EquippedControll.Instance.guns[0].gun.GetComponent<BoxCollider>().enabled = false;
        EquippedControll.Instance.StateAssassinate();
    }

    public void Shotting()
    {
        Shotgun.Instance.SendMessage("Shot");
        if (VFXHandler.Instance != null)
            VFXHandler.Instance.SpawnBasicShotgunVFX(shotPoint.transform);
    }

    public void InitShotting()
    {
        Shotgun.Instance.SendMessage("Init");
    }

    public void StartAttackAnimation()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.SpawnBasicMeeleeTrailVFX(axeCenter.transform);
        }
        EquippedControll.Instance.StateAttack(true);
    }
    public void OnAttack()
    {
        axeCut.Invoke();
        
        EquippedControll.Instance.guns[0].gun.GetComponent<BoxCollider>().enabled = true;
    }
    public void EndAttack()
    {
        EquippedControll.Instance.guns[0].gun.GetComponent<BoxCollider>().enabled = false;
        EquippedControll.Instance.StateAttack();
    }
}
