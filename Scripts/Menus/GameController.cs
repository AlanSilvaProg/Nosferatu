using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public Animator HUD_Death;
    public Animator HUD_Bullets;
    public Animator HUD_Arrows;

    public TextMeshProUGUI dialogoText;
    public Image lifeBar;
    public Image furyBar;
    public Image fadeImage;
    public GameObject loadingIcon;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI arrowsText;

    public UnityEvent FullHp = new UnityEvent();
    public UnityEvent FullRage = new UnityEvent();

    bool loading;
    [HideInInspector]public bool fadein = false;
    [HideInInspector] public int arrows;
    [HideInInspector] public int bullets;

    string actualScene;
    [HideInInspector] public bool death;
    float timer;

    private void Awake()
    {
        Instance = this;
        fadein = false;
    }

    // Update is called once per frame
    void Update()
    {

        arrowsText.text = "" + arrows;
        bulletsText.text = "" + bullets;

        lifeBar.fillAmount = Mathf.Clamp(PlayerInfo.Instance.life / 100 , 0, 1);
        furyBar.fillAmount = Mathf.Clamp(PlayerInfo.Instance.fury / 100, 0, 1);

        if (fadeImage.color.a == 1)
        {
            loadingIcon.SetActive(true);
        }
        else
        {
            loadingIcon.SetActive(false);
        }

        if (fadein)
        {
            if(fadeImage.color.a < 1)
            {
                Color newColor = fadeImage.color;
                newColor.a += Time.deltaTime;
                newColor.a = newColor.a > 1 ? 1 : newColor.a;
                fadeImage.color = newColor; 
            }
        }
        else
        {
            if (fadeImage.color.a > 0)
            {
                Color newColor = fadeImage.color;
                newColor.a -= Time.deltaTime;
                newColor.a = newColor.a < 0 ? 0 : newColor.a;
                fadeImage.color = newColor;
            }
        }

    }

    public void Death()
    {
        if (!death)
        {
            Invoke("LoadNewScene", 4f);
            death = true;
        }
    }

    public void LoadNewScene()
    {
        actualScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(actualScene);
    }

}
