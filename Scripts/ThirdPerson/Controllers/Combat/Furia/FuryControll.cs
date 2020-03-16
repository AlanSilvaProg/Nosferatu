using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class FuryControll : MonoBehaviour
{

    public static FuryControll Instance;

    public string inputToActive;
    public bool furyState;

    public UnityEvent Heightened_ENABLE = new UnityEvent();
    public UnityEvent Heightened_DISABLE = new UnityEvent();
    public UnityEvent FURY_ENABLE = new UnityEvent();
    public UnityEvent FURY_DISABLE = new UnityEvent();

    [HideInInspector] public bool highSense;

    float count;
    bool controll = false;

    InputMaster inputMaster;
    bool block;

    private void Awake()
    {
    
        Instance = this;

        FURY_ENABLE.AddListener(() => { Controller.Instance.addVeloFury = Controller.Instance.velocity; });
        FURY_ENABLE.AddListener(FuryON);
        Heightened_ENABLE.AddListener(HeightenedON);
        Heightened_ENABLE.AddListener(() => { highSense = true; });
        FURY_DISABLE.AddListener(() => { Controller.Instance.addVeloFury = 0; });
        FURY_DISABLE.AddListener(FuryOFF);
        Heightened_DISABLE.AddListener(HeightenedOFF);
        Heightened_DISABLE.AddListener(() => { highSense = false; });

        inputMaster = new InputMaster();

        inputMaster.PlayerControlls.Fury.performed += ctx =>
        {
            if (block) return; else block = true;

            if (highSense)
            {
                if (!furyState && PlayerInfo.Instance.fury > 20 && !controll)
                {
                    furyState = true;
                    EquippedControll.Instance.equip = 2;
                    FURY_ENABLE.Invoke();
                    controll = true;
                    return;
                }

                if (!furyState || PlayerInfo.Instance.fury <= 0)
                {
                    Heightened_DISABLE.Invoke();
                    EquippedControll.Instance.equip = 0;
                    controll = false;
                    return;
                }

                if (furyState || PlayerInfo.Instance.fury <= 0 && furyState)
                {
                    DisableAll();
                    return;
                }

            }
            if (!highSense)
            {
                Heightened_ENABLE.Invoke();
                return;
            }
        };

    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    void Update()
    {

        count = 0;

    }

    private void FixedUpdate()
    {
        block = false;
    }

    public void DisableAll()
    {
        furyState = false;
        FURY_DISABLE.Invoke();
        controll = false;
        Heightened_DISABLE.Invoke();
        EquippedControll.Instance.equip = 0;
    }

    void HeightenedON()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.HeightenedSenses(true);
        }
    }
    void HeightenedOFF()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.HeightenedSenses(false);
        }
    }

    void FuryON()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.FuryStart();
        }
    }
    void FuryOFF()
    {
        if (VFXHandler.Instance != null)
        {
            VFXHandler.Instance.FuryEnd();
        }
    }

}
