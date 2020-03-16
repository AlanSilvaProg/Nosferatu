using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LampeaoControler : MonoBehaviour
{

    [SerializeField] private GameObject versaoQuebrada;
    [SerializeField] private GameObject spawnSecundario;
    public UnityEvent caiuNoChao = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Colidiu");
        if(collision.collider.gameObject.layer == 14)
        {
            print("Era terrno");
            Instantiate(versaoQuebrada, transform.position, transform.rotation);
            if (spawnSecundario != null)
            {
                Instantiate(spawnSecundario, transform.position, transform.rotation);
            }
            caiuNoChao.Invoke();
            Destroy(gameObject);
            
        }
    }
}
