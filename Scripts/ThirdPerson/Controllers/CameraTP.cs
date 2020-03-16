using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTP : MonoBehaviour
{

    public LayerMask collisionCam;
    public float camDistance = 10;
    public float sensibilidade = 2.0f; //Controla a sensibilidade do mouse

    public Transform anchorPos;
    public Transform aimPos;

    float mouseX = 0.0f; //Variáveis que controla a rotação do mouse
    float mouseY = 0.0f; //Variáveis que controla a rotação do mouse

    Transform pivot;
    bool collisionDetect;

    float distance;

    public delegate void StartGame();
    public StartGame onStartGame;
    public UnityEvent OnStartGame;

    //StartGame startGameCalls;

    private void Awake()
    {
        onStartGame += AdjustCamera;

        OnStartGame.AddListener(onStartGame.Invoke);
    }

    void Start()
    {
        OnStartGame.Invoke();
    }


    void Update()
    {
        if (!EquippedControll.Instance.aiming)
        {
            distance = Vector3.Distance(anchorPos.position, transform.position);
            Debug.DrawRay(anchorPos.position, (anchorPos.position - transform.position) * -1);
            mouseX += Input.GetAxis("Mouse X") * sensibilidade; // Incrementa o valor do eixo X e multiplica pela sensibilidade
            mouseY += Input.GetAxis("Mouse Y") * sensibilidade * -1; // Incrementa o valor do eixo X e multiplica pela sensibilidade

            mouseY = Nosferatu_Utility.instance.ClampAngleFPS(mouseY, -80f, 80f); // limitando rotação no eixo y em 80 graus

            Check();
            if (!collisionDetect && Vector3.Distance(transform.position, anchorPos.position) < camDistance && CheckBackCam() == false)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * 0.1f, 0.1f);
            }
        }
        else if(EquippedControll.Instance.guns[EquippedControll.Instance.equip].name == "Crossbow")
        {
            transform.position = aimPos.position;
        }
    }

    void LateUpdate()
    {
        if (!EquippedControll.Instance.aiming)
        {
            pivot.transform.eulerAngles = new Vector3(mouseY, mouseX, 0); //Executa a rotação da câmera de acordo com os eixos
            transform.LookAt(pivot.transform); // fazendo com que a camera olhe para o jogador
        }
    }

    bool CheckBackCam()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.back, out hit, 1, collisionCam);
    }

    //Função para ajusta a posição da camera em relação ao jogador
    void AdjustCamera()
    {
        transform.localPosition = Vector3.zero;
        Vector3 target; // Vector3 para guardar a posição que a camera deve ficar
        pivot = transform.parent; // acessando pivot da camera
        // calculando posição que a camera deve ir
        target = new Vector3(transform.position.x , pivot.transform.position.y + 0.5f, transform.position.z + (pivot.transform.forward.z + camDistance * -1));
        transform.position = target; // posicionando a camera 
    }

    void Check()
    {
        RaycastHit hit;
        if (Physics.Raycast(anchorPos.position, (anchorPos.position - transform.position) * -1, out hit, distance + 0.5f , collisionCam))
        {
            print("olhao bug aqui");
            transform.position = hit.point;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
        {
            collisionDetect = true;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * 0.1f, 0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collisionDetect = false;
    }

}
