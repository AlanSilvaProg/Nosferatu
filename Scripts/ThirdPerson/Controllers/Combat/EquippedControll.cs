using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public enum GunType { LONGSW, SHORTSW, SHOOTER, NOWEAPON}

[System.Serializable]
public class GunsInfo
{
    public string name = "unarmed";
    public GunType gunType = GunType.NOWEAPON;
    public GameObject gun;
    public float dmg = 25;
    public float delayPerShot;
    [Tooltip("if this gun is shooter")]public int bullets;
}

public class EquippedControll : MonoBehaviour
{
    public static EquippedControll Instance;

    public GunsInfo[] guns;
    public int equip = 0;
    public Animator playerAnimator;
    public static bool onAttack;
    [HideInInspector] public bool aiming;

    public UnityEvent AIM_ON = new UnityEvent();
    public UnityEvent AIM_OFF = new UnityEvent();
    [Header("Quando Equipa a Shotgun")]
    public UnityEvent SwitchToShotgun = new UnityEvent();
    [Header("Evento para quando a shotgun está sem bala")]
    public UnityEvent SwitchToCrossBow = new UnityEvent();
    [Header("Evento para quando a shotgun está sem bala")]
    public UnityEvent NONBULLETS = new UnityEvent();
    [Header("Evento para quanto não há mais flechas")]
    public UnityEvent NONARROWS = new UnityEvent();

    float timer;
    PlayerState oldState;
    bool ikActive;

    int hits;

    InputMaster inputMaster;
    bool block;

    Gamepad controller;

    private void Awake()
    {

        Instance = this;

        inputMaster = new InputMaster();
        inputMaster.PlayerControlls.AimInput.performed += act => 
        {
            if (block) return;
            block = true;

            if (!aiming)
                aiming = Controller.Instance.playerState != PlayerState.ATTACK;
            else
                aiming = false;
        };

        inputMaster.PlayerControlls.CombatInputs.performed += act =>
        {
            if (block) return;
            block = true;

            if (aiming && Input.GetMouseButtonDown(0))
            {
                if (timer > guns[2].delayPerShot)
                {
                    timer = 0;
                    guns[2].gun.GetComponent<Bow>().Shot();
                }
                return;
            }

            if (!onAttack && Controller.Instance.playerState != PlayerState.FALL)
            {
                if (!FuryControll.Instance.furyState)
                {
                    if (hits == 0)
                    {
                        playerAnimator.SetTrigger("Attack");
                        playerAnimator.ResetTrigger("Shot");
                    }                    
                }
                else
                {
                    if (hits == 0 && FuryControll.Instance.furyState)
                    {
                        playerAnimator.SetTrigger("Assassinate");
                        playerAnimator.ResetTrigger("Shot");
                    }
                }

            }
        };

        inputMaster.PlayerControlls.ShootInputs.performed += act =>
        {
            if (block) return;
            block = true;

            if (!aiming)
            {
                timer = 0;
                Shot();
                return;
            }

            if (aiming && !Input.GetMouseButtonDown(1))
            {
                if (timer > guns[2].delayPerShot)
                {
                    timer = 0;
                    guns[2].gun.GetComponent<Bow>().Shot();
                }
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

    private void Update()
    {

        if (Cheats.Instance != null)
        {
            guns[1].bullets = 999;
            guns[2].bullets = 999;
            
        }

        GameController.Instance.arrows = guns[2].bullets;
        GameController.Instance.bullets = guns[1].bullets;

        timer += Time.deltaTime;

        if (aiming && !ikActive)
        {
            AIM_ON.Invoke();
            ikActive = true;
        }
        else if (!aiming && ikActive)
        {
            AIM_OFF.Invoke();
            ikActive = false;
        }

    }

    private void FixedUpdate()
    {
        block = false;
    }

    void Shot()
    {
        playerAnimator.SetTrigger("Shot");
    }

    public void StateAttack(bool update = false)
    {
        if (update)
        {
            hits++;
            oldState = Controller.Instance.playerState != PlayerState.ATTACK ? Controller.Instance.playerState : oldState;
            Controller.Instance.playerState = PlayerState.ATTACK;
            onAttack = true;
        }
        else
        {
            hits--;
            onAttack = false;
            Controller.Instance.playerState = oldState;
        }
    }

    public void StateAssassinate(bool update = false)
    {
        if (update)
        {
            hits++;
            oldState = Controller.Instance.playerState != PlayerState.ATTACK ? Controller.Instance.playerState : oldState;
            Controller.Instance.playerState = PlayerState.ATTACK;
            onAttack = true;
        }
        else
        {
            hits--;
            onAttack = false;
            Controller.Instance.playerState = oldState;
        }
    }

}
