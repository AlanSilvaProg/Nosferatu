using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallOnProximity : MonoBehaviour
{
    [TextArea]
    public string[] dialogues;

    public UnityEvent TriggerDialogue;

    bool trigged;

    // Start is called before the first frame update
    void Start()
    {

        TriggerDialogue.AddListener(() => {

            trigged = true;

            DialogueManager.Instance.dialogos.Add(dialogues[0]);
            for (int i = 0; i < dialogues.Length; i++){
                DialogueManager.Instance.dialogos.Add(dialogues[i]);
            }

            if(!DialogueManager.Instance.showDialogue)
                DialogueManager.Instance.NewDialogue();
        });

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !trigged)
        {
            TriggerDialogue.Invoke();
        }
            
    }

}
