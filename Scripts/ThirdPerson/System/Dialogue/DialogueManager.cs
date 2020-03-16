using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance;

    public float timeToShow;
    public float timerToDestroy;

    Text ui_Dialogue;

    [HideInInspector]public string currentStepText;
    [HideInInspector] public string textToShow;

    [HideInInspector] public int currentIndice;
    float timer;

    TextMeshProUGUI currentDialogueBox;

    [HideInInspector] public bool showDialogue = false;
    [HideInInspector] public bool endingDialogue;

    [HideInInspector] public List<string> dialogos = new List<string>();

    public UnityEvent StartDialogue;
    public UnityEvent EndingDialogue;

    [Header("Configurações do RPG Talk")]
    public GameObject textUiObj;
    public GameObject textName;
    public GameObject enable_Start;
    public Transform choice_Parent;

    [HideInInspector] public bool onTalk;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDialogueBox = GameController.Instance.dialogoText;

        EndingDialogue.AddListener(() =>
        {
            showDialogue = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
        ;
        
            if (showDialogue)
            {
            if (GameObject.FindObjectOfType<RPGTalk>() == null)
            {
                GameController.Instance.dialogoText.text = currentStepText;
                
                timer += Time.deltaTime;
                if (timer >= timeToShow)
                {
                    if (currentIndice < textToShow.Length)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            currentStepText = textToShow;
                            currentIndice = textToShow.Length;
                        }
                        else
                        {
                            currentStepText += textToShow[currentIndice];
                            currentIndice++;
                        }
                        timer = 0;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E) || timer >= timerToDestroy)
                        {
                            if (dialogos.Count == 0)
                            {
                                EndingDialogue.Invoke();
                            }
                            else
                            {
                                CallNewDialogue();
                            }
                        }
                    }
                }
            }

        }

    }

    public void CallNewDialogue()
    {
        GameController.Instance.dialogoText.text = "";
        textToShow = dialogos[0];
        dialogos.RemoveAt(0);
        showDialogue = true;
        timer = 0;
        currentIndice = 0;
        currentStepText = "";
    }

    public void NewDialogue(string text = "")
    {
        if(text != "")
            dialogos.Add(text);
        if (!showDialogue)
        {
            StartDialogue.Invoke();
            CallNewDialogue();
        }
    }

}
