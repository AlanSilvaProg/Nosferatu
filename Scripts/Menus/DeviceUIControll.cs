using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceUIControll : MonoBehaviour
{

    public GameObject[] allPs4, allPc, allXbox;
    bool enableAll;

    private void Start()
    {

    }

    void Update()
    {
        allPs4 = GameObject.FindGameObjectsWithTag("PS4");
        allPc = GameObject.FindGameObjectsWithTag("PC");
        allXbox = GameObject.FindGameObjectsWithTag("XBOX");
        if (Gamepad.current == null && !enableAll)
        {
            for(int i = 0; i < allPs4.Length; i++)
            {
                allPs4[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < allPc.Length; i++)
            {
                allPc[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < allXbox.Length; i++)
            {
                allXbox[i].gameObject.SetActive(false);
            }
            enableAll = true;
        }
        else
        {
            for (int i = 0; i < allPs4.Length; i++)
            {
                allPs4[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < allPc.Length; i++)
            {
                allPc[i].gameObject.SetActive(true);
            }
            for (int i = 0; i < allXbox.Length; i++)
            {
                allXbox[i].gameObject.SetActive(false);
            }
            enableAll = false;
        }
    }
}
