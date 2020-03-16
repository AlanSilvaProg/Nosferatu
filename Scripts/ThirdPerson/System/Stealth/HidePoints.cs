using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePoints : MonoBehaviour
{

    public bool needToCrouch = true;
    public bool playerIsHere = false;

    [Header("Quantidade de vezes que o jogador pode se esconder até ser encontrado")]
    [Tooltip("Quando o jogador se esconder essa quantidade de vezes, o inimigo irá encontra-lo ao chegar perto")]
    public int quantitysToVunerable = 3;

    [Header("Tempo para aumentar + 1 possibilidade de esconder sem ser visto ")]
    [Tooltip("se o jogador passar essa quantidade de tempo sem se esconder nesse ponto ele reduzirá em 1 a quantidade " +
        "total que ele ja se escondeu aqui, permitindo que se esconda denovo sem ser visto")]
    public float timeToForget = 20;

    [HideInInspector] public int quantitysHidded = 0;

    float count;

    private void Update()
    {
        if (!playerIsHere && quantitysHidded > 0)
        {
            count += Time.deltaTime;
            if(count >= timeToForget)
            {
                quantitysHidded--;
                count = 0;
            }
        }
        else if(playerIsHere && quantitysHidded >= quantitysToVunerable)
        {
            EnemyGeneral.Instance.foundInHide = true;
        }

        if(!PlayerInfo.Instance.imHidden)
        {
            EnemyGeneral.Instance.foundInHide = false;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIsHere = true;
            if (needToCrouch)
            {
                if (Controller.Instance != null && PlayerInfo.Instance != null)
                {
                    if (Controller.Instance.crouch)
                    {
                        if(!PlayerInfo.Instance.imHidden)
                            quantitysHidded ++;
                        PlayerInfo.Instance.imHidden = true;
                    }
                    else
                    {
                        PlayerInfo.Instance.imHidden = false;
                    }
                }
                else
                {
                    Debug.LogError("Player script not found - PlayerInfo or Controller");
                    return;
                }
            }
            else
            {
                if (PlayerInfo.Instance != null)
                {
                    if (!PlayerInfo.Instance.imHidden)
                        quantitysHidded++;
                    PlayerInfo.Instance.imHidden = true;
                }
                else
                {
                    Debug.LogError("Player script not found - PlayerInfo or Controller");
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerIsHere = false;
            PlayerInfo.Instance.imHidden = false;
        }
    }

}
