using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shotgun : MonoBehaviour
{
    public static Shotgun Instance;
    bool enable;
    public Collider hitBox;

    public UnityEvent ShotEvent = new UnityEvent();
    public UnityEvent initShotEvent = new UnityEvent();

    private void Start()
    {
        Instance = this;
        initShotEvent.AddListener(() => { enable = true; });
        ShotEvent.AddListener(() => { enable = false; });
    }

    public void Shot()
    {
        hitBox.enabled = true;
        StartCoroutine("ShotBox");
        ShotEvent.Invoke();
    }

    public void Init()
    {
        initShotEvent.Invoke();
    }

    IEnumerator ShotBox()
    {
        yield return new WaitForSeconds(0.2f);
        hitBox.enabled = false;
    }

}
