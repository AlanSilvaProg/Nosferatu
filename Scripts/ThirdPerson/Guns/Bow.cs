using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bow : MonoBehaviour
{

    public GameObject arrowTemplate;
    public List<GameObject> poolList = new List<GameObject>();
    Dictionary<string, Queue<GameObject>> poolQueue;

    public UnityEvent ShotEvent = new UnityEvent();

    void Awake()
    {
        poolQueue = new Dictionary<string, Queue<GameObject>>(); // criando um dictionary novo vazio
        
        Queue<GameObject> queue = new Queue<GameObject>(); // fila para armazenar os objetos 
        for (int i = 0; i < 10; i++)
        {
            GameObject clone = Instantiate(arrowTemplate);
            clone.transform.SetParent(null);
            clone.SetActive(false); // desabilitando objeto para ficar inativo na cena
            queue.Enqueue(clone); // colocando na ultima posição da fila
        }

        poolQueue.Add("Flechas", queue); // adicionando nome do item da lista e a fila de objetos respectivos a ela

    }

    public void Shot()
    {

        GameObject objToSpawn = poolQueue["Flechas"].Dequeue(); //selecionando o primeiro objeto da fila
        
        if (objToSpawn.activeSelf)
        {
            objToSpawn.SetActive(false);
        }
        objToSpawn.transform.position = Camera.main.transform.position; // setando posição inicial
        objToSpawn.GetComponent<Arrows>().enabled = true;
        objToSpawn.SetActive(true); // ativando ele na cena

        poolQueue["Flechas"].Enqueue(objToSpawn); // retornando o objeto para o fim da fila

        ShotEvent.Invoke();

    }

}
