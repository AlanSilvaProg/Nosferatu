using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueTPFX_Handler : MonoBehaviour
{
    [SerializeField] private GameObject[] statueObjs;
    [SerializeField] private GameObject glowingOrb;

    [SerializeField] private float timeToTP;
    [SerializeField] private float TPduration;

    [SerializeField] private GameObject StatueTPStart_System;
    [SerializeField] private GameObject StatueTPStart_EndSystem;
    [SerializeField] private GameObject StatueTPEnd;

    [Header("DEBUG")]
    public bool Debug_goaway = false;
    public bool Debug_comeback = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Dissapearing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug_goaway)
        {
            Debug_goaway = !Debug_goaway;
            StartCoroutine(Dissapearing());
        }else if (Debug_comeback)
        {
            Debug_comeback = !Debug_comeback;
            StartCoroutine(Appearing());
        }

    }

    public void GoAway()
    {
        StartCoroutine(Dissapearing());
    }

    public void ComeBack()
    {
        StartCoroutine(Appearing());
    }

    IEnumerator Dissapearing()
    {
        StatueTPStart_System.SetActive(true);
        glowingOrb.SetActive(false);
        float timer = 0;
        while (timer < timeToTP)
        {
            timer += Time.deltaTime;

            int a = 0;
            foreach (GameObject obj in statueObjs)
            {
                statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_DarknessTint", Mathf.InverseLerp(0,timeToTP,timer));
                a++;
            }

            yield return new WaitForFixedUpdate();
        }

        timer = 0;
        StatueTPStart_EndSystem.SetActive(true);
        while (timer < TPduration)
        {
            timer += Time.deltaTime;
            int a = 0;

            foreach(GameObject obj in statueObjs)
            {
                statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", Mathf.InverseLerp(0, TPduration, timer));
                a++;
            }

            yield return new WaitForFixedUpdate();
        }

    }

    IEnumerator Appearing()
    {
        StatueTPEnd.SetActive(true);
        int a = 0;
        foreach (GameObject obj in statueObjs)
        {
            statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", 1);
            a++;
        }
        a = 0;
        foreach (GameObject obj in statueObjs)
        {
            statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_DarknessTint", 1);
            a++;
        }

        float timer = 0;
        while (timer < TPduration)
        {
            timer += Time.deltaTime;
            a = 0;

            foreach (GameObject obj in statueObjs)
            {
                statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_Dissolve", 1-(Mathf.InverseLerp(0, TPduration, timer)));
                a++;
            }

            yield return new WaitForFixedUpdate();
        }
        timer = 0;
        while (timer < timeToTP)
        {
            timer += Time.deltaTime;

            a = 0;
            foreach (GameObject obj in statueObjs)
            {
                statueObjs[a].GetComponent<MeshRenderer>().material.SetFloat("_DarknessTint", 1-( Mathf.InverseLerp(0, timeToTP, timer)));
                a++;
            }

            yield return new WaitForFixedUpdate();
        }
        glowingOrb.SetActive(true);

        yield return new WaitForFixedUpdate();
    }
}
