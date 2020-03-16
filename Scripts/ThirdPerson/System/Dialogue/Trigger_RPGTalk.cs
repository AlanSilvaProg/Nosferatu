using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Trigger_RPGTalk : MonoBehaviour
{

    public GameObject talkPrefab;
    public bool needInteraction;

    public UnityEvent OnTalkStart;
    GameObject talkObj;

    private void Start()
    {
        OnTalkStart.AddListener(() =>
        {
            GameObject talk;
            DialogueManager.Instance.onTalk = true;
            talk = Instantiate(talkPrefab,transform);
            
            talkObj = talk;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        });
    }

    private void Update()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        Gamepad controller = InputSystem.GetDevice<Gamepad>();

        if (controller != null && controller.crossButton.wasPressedThisFrame && DialogueManager.Instance.onTalk)
        {
            talkObj.GetComponent<RPGTalk>().PlayNext();
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!needInteraction && other.tag == "Player")
        {
            OnTalkStart.Invoke();
            //Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        Gamepad controller = InputSystem.GetDevice<Gamepad>();

        if(needInteraction && other.tag == "Player")
        {
            if (controller != null && controller.crossButton.wasPressedThisFrame || kb.fKey.wasPressedThisFrame)
            {
                OnTalkStart.Invoke();
               // Destroy(gameObject);
            }
        }
    }

}
