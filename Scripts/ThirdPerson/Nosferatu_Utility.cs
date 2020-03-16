using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nosferatu_Utility: MonoBehaviour 
{

    public static Nosferatu_Utility instance;

    private void Awake()
    {
        instance = this;
        LockMouse();
    }


    //travando o mouse na tela
    public void LockMouse()
    {
        Cursor.visible = false; //Oculta o cursor do mouse
        Cursor.lockState = CursorLockMode.Locked; //Trava o cursor do centro
    }

    public float ClampAngleFPS(float angulo, float min, float max)
    {
        if (angulo < -360)
        {
            angulo += 360;
        }
        if (angulo > 360)
        {
            angulo -= 360;
        }
        return Mathf.Clamp(angulo, min, max);
    }
    

}
