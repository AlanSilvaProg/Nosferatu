using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoreCatch : MonoBehaviour
{

    public int loreIndice = 0;
    public UnityEvent onCatch;
    public void CatchLore()
    {
        LoreControll.Instance.NewLore(loreIndice);
        onCatch.Invoke();
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            LoreControll.Instance.NewLore(loreIndice);
            onCatch.Invoke();
            Destroy(gameObject);
        }
    }

}
