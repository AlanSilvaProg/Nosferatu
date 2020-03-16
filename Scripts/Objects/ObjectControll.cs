using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectControll : MonoBehaviour
{

    public float life = 50;

    public UnityEvent receivedHit = new UnityEvent();
    public UnityEvent onDestroy = new UnityEvent();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            hitByArrow(25);
        }
    }

    public void hitByArrow(float dmg)
    {
        life -= dmg;
        print("Hitou");
        if (life < 0)
        {
            onDestroy.Invoke();
        }
        else
        {
            receivedHit.Invoke();
        }
    }

    public void hitByShotgun(float dmg)
    {
        life -= dmg;
        if (life < 0)
        {
            onDestroy.Invoke();
        }
        else
        {
            receivedHit.Invoke();
        }
    }

    public void HittedSW(float dmg)
    {
        life -= dmg;
        if (life < 0)
        {
            onDestroy.Invoke();
        }
        else
        {
            receivedHit.Invoke();
        }
    }

}
