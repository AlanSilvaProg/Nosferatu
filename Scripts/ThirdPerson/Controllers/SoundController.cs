using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioSource source;
    public AudioClip[] passosPedra;

    int i = 0;

    public void PassosPedra()
    {
        if (i >= passosPedra.Length)
        {
            i = 0;
        }
        if (Controller.Instance.crouch)
        {
            source.PlayOneShot(passosPedra[i], 0.1f);
        }
        else
        {
            source.PlayOneShot(passosPedra[i], 0.3f);
        }
        i++; 
    }

}
