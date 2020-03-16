using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo Instance;

    public float life;
    public float fury;

    public UnityEvent DeathEvent = new UnityEvent();

    public float velocityFuryIncrease;
    public float velocityFuryDecrease;

    [HideInInspector] public bool imHidden = false;
    [HideInInspector] public bool imScreaming = false;
    public Transform toEnemyCheckView;
    float timerToScreaming = 0;

    bool fullRage, fullHp;
    

    private void Awake()
    {

        fullRage = true;
        fullHp = true;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DeathEvent.AddListener(() => 
        {
            if (CONTINUOUSDATA.Instance != null) Destroy(CONTINUOUSDATA.Instance.gameObject);
        }
        );
    }

    // Update is called once per frame
    void Update()
    {

        if (life <= 0 && !GameController.Instance.death)
        {
            return;
        }

        if (Controller.Instance.playerState == PlayerState.ATTACK)
        {
            imScreaming = true;
        }
        else
        {
            if (Controller.Instance.playerState == PlayerState.WALK)
            {
                if (Controller.Instance.running)
                {
                    imScreaming = true;
                }
                else
                {
                    if (timerToScreaming >= 3)
                    {
                        imScreaming = true;
                    }
                    timerToScreaming += Time.deltaTime;
                }
            }
            else
            {
                timerToScreaming = 0;
                imScreaming = false;
            }
        }

        if (Cheats.Instance != null)
        {
            if (Cheats.Instance.lifeCheat)
            {
                life = 100;
            }
            if (Cheats.Instance.furyCheat)
            {
                fury = 100;
            }
        }

        if (fury < 100 && fullRage) fullRage = false;
        if (life < 100 && fullHp) fullHp = false;

        if (fury < 100 && !FuryControll.Instance.highSense && life > 35)
        {
            fury += velocityFuryIncrease * Time.deltaTime;

            if (!fullRage && fury >= 100)
            {
                fullRage = true;
                GameController.Instance.FullRage.Invoke();
            }

        }
        else if (FuryControll.Instance.highSense && fury > 0)
        {
            fury -= velocityFuryDecrease * Time.deltaTime;
        }

        if(fury <= 0 && FuryControll.Instance.highSense)
        {
            FuryControll.Instance.DisableAll();
        }

        if (!fullHp && life >= 100)
        {
            fullHp = true;
            GameController.Instance.FullHp.Invoke();
        }

       

    }

    public void DeathCall()
    {
        DeathEvent.Invoke();
    }

    public void Hitted(float dmg, bool KnockBack = false)
    {
        if (life <= 0) return;

        if (Controller.Instance.onDash)
        {
            return;
        }

        life -= dmg;
        if(life <= 0)
        {
            Controller.Instance.playerAnimator.SetTrigger("Death");
        }
        if(VFXHandler.Instance != null)
            VFXHandler.Instance.AndreiWasHit();
    }

}
