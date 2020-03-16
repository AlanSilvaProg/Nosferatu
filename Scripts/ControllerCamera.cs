using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerCamera : MonoBehaviour
{
    private CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();   
    }

    // Update is called once per frame
    void Update()
    {

        

       /* if (Input.GetJoystickNames()[1] != null)
        {
            cam.m_XAxis.m_InputAxisName = "HorizontalCam";
            cam.m_YAxis.m_InputAxisName = "VerticalCam";
        }
        else
        {*/
            cam.m_XAxis.m_InputAxisName = "Mouse X";
            cam.m_YAxis.m_InputAxisName = "Mouse Y";
        
       // cam.m_XAxis.Value = Input.GetAxis("HorizontalCam");
       // cam.m_YAxis.Value = Input.GetAxis("VerticalCam");
    }
}
