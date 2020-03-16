using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTalk : MonoBehaviour
{
    public UnityEvent EndTalkEvent = new UnityEvent();

    public bool isOutSide = false;


    public void CallEndTalk()
    {
        EndTalkEvent.Invoke();
        //print(GetComponent<RPGTalk>().isPlaying);
    }

    public void Update()
    {
        if (!isOutSide)
        {
            //print(GetComponent<RPGTalk>().isPlaying);
            if (GetComponent<RPGTalk>().isPlaying == false)
            {
                CallEndTalk();
                SendMessageUpwards("CallEndTalk");
                Destroy(gameObject);
            }
        }
    }

}
