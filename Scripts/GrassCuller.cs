using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrassCuller : MonoBehaviour
{

    public GameObject[] grasses;
    public float distanceToPlayer = 10;
    public GameObject player;

    private float refreshratemax = 1;
    private float refresh = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        refresh += Time.deltaTime;
        if(refresh > refreshratemax)
        {
            refresh = 0;
            foreach (GameObject o in grasses)
            {
                if (Vector3.Distance(player.transform.position,o.transform.position) > distanceToPlayer)
                {
                    o.SetActive(false);
                }
                else
                {
                    o.SetActive(true);
                }
            }
        }
    }

    private void Reset()
    {
        grasses = new GameObject[transform.childCount];
        int a = 0;
        foreach (Transform child in transform)
        {
            grasses[a++] = child.gameObject;
        }
        
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
