using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arrows : MonoBehaviour
{

    public float speed;
    public float soundDistance = 15;
    bool onFire;
    float count;
    Vector3 target;

    public Transform backArrow;

    public UnityEvent onEnable = new UnityEvent();
    public UnityEvent onHitOrganic = new UnityEvent();
    public UnityEvent onHitInorganic = new UnityEvent();

    private void OnEnable()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.SpawnBasicBoltTrailVFX(backArrow);
        }

        transform.SetParent(null);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            target = hit.point;
        }
        onFire = true;
        count = 0;
        transform.LookAt(target);

        onEnable.Invoke();

    }
    // Update is called once per frame
    void Update()
    {

        if (onFire)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "MainCamera" && other.tag != "Respawn")
        {
            if (onFire && other.tag == "Enemy")
            {
                other.SendMessage("HitByArrow", EquippedControll.Instance.guns[EquippedControll.Instance.equip].dmg);
                onFire = false;
                transform.SetParent(other.transform);
                onHitOrganic.Invoke();
                gameObject.SetActive(false);
            }
            else if(other.name == "Hurtbox")
            {
                onFire = false;
                other.SendMessage("TurnOFF");
                onHitInorganic.Invoke();
                gameObject.SetActive(false);
            }
            else if (other.tag == "Default")
            {
                onFire = false;
                Collider[] cols= Physics.OverlapSphere(transform.position, soundDistance);
                foreach(Collider col in cols)
                {
                    if(col.tag == "Enemy")
                    {
                        if (!EnemyGeneral.Instance.playerOnView)
                        {
                            col.GetComponent<Nosferatu_Basic>().SetDistraction(transform.position);
                        }
                    }
                }
                transform.SetParent(other.gameObject.transform);
                onHitInorganic.Invoke();
                this.enabled = false;
            }
        }
    }
}
