using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKSolution : MonoBehaviour
{

    public static IKSolution Instance;

    public LayerMask isGround;

    public Rig IKLeg;

    public Transform refFrontL;
    public Transform refBackL;
    public Transform refMidL;
    public Transform refFrontR;
    public Transform refBackR;
    public Transform refMidR;
    public GameObject targetL;
    public GameObject targetR;
    public GameObject targetSubFront;
    public GameObject targetSubBack;

    public bool left, right, atMid;

    public float stepTimer;

    bool controll = false;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {

        if(Controller.Instance.playerState == PlayerState.WALK && IKLeg.weight == 0 && !controll)
        {
            EnableIK();
        }
        else if (Controller.Instance.playerState != PlayerState.WALK && IKLeg.weight == 1 && controll)
        {
            DisableIK();
        }
        
        if (left && !right)
        {

            if (atMid)
            {
                if (Vector3.Distance(targetL.transform.position, refMidL.transform.position) < 0.1f)
                {
                    atMid = false;
                }
                else
                {
                    targetL.transform.position = Vector3.Lerp(targetL.transform.position, refMidL.transform.position, stepTimer * 2);
                }
            }
            else
            {
                GetFrontPositionLeft();
                targetL.transform.position = Vector3.Lerp(targetL.transform.position, targetSubFront.transform.position, stepTimer);
            }


            GetBackPositionRight();
            targetR.transform.position = Vector3.Lerp(targetR.transform.position, targetSubBack.transform.position, stepTimer);

            if (Vector3.Distance(targetSubFront.transform.position, targetL.transform.position) < 0.07f)
            {
                left = false;
                GetFrontPositionRight();
                atMid = true;
            }
        } 

        if(!left && right)
        {

            if (atMid)
            {
                if (Vector3.Distance(targetR.transform.position, refMidR.transform.position) < 0.1f)
                {
                    atMid = false;
                }
                else
                {
                    targetR.transform.position = Vector3.Lerp(targetR.transform.position, refMidR.transform.position, stepTimer * 2);
                }
            }
            else
            {
                GetFrontPositionRight();
                targetR.transform.position = Vector3.Lerp(targetR.transform.position, targetSubFront.transform.position, stepTimer);
            }

            GetBackPositionLeft();

            targetL.transform.position = Vector3.Lerp(targetL.transform.position, targetSubBack.transform.position, stepTimer);

            if (Vector3.Distance(targetSubFront.transform.position, targetR.transform.position) < 0.07f)
            {
                right = false;
                GetFrontPositionLeft();
                atMid = true;
            }

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GetFrontPositionLeft();
        }
       
    }

#region IKActions

    public void GetFrontPositionLeft()
    {
        Vector3 position = refFrontL.transform.position;
        Vector3 hitTarget;
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, 10, isGround);
        if (hits.Length != 0)
        {
            hitTarget = hits[0].point;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].point.y > hitTarget.y)
                {
                    hitTarget = hits[i].point;
                }
            }
            refMidL.transform.position = new Vector3(refMidL.transform.position.x, hitTarget.y + 0.2f, refMidL.transform.position.z);
            hitTarget.y += 0.09f;
            targetSubFront.transform.position = hitTarget;
            if (!left)
            {
                left = true;
            }
        }
    }

    public void GetBackPositionLeft()
    {
        Vector3 position = refBackL.transform.position;
        Vector3 hitTarget;
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, 10, isGround);
        if (hits.Length != 0)
        {
            hitTarget = hits[0].point;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].point.y > hitTarget.y)
                {
                    hitTarget = hits[i].point;
                }
            }
            hitTarget.y += 0.09f;
            targetSubBack.transform.position = hitTarget;
        }
    }

    public void GetFrontPositionRight()
    {
        Vector3 position = refFrontR.transform.position;
        Vector3 hitTarget;
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, 10, isGround);
        if (hits.Length != 0)
        {
            hitTarget = hits[0].point;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].point.y > hitTarget.y)
                {
                    hitTarget = hits[i].point;
                }
            }
            refMidR.transform.position = new Vector3(refMidR.transform.position.x, hitTarget.y + 0.2f, refMidR.transform.position.z);
            hitTarget.y += 0.09f;
            targetSubFront.transform.position = hitTarget;
            if (!right)
            {
                right = true;
            }
        }
    }

    public void GetBackPositionRight()
    {
        Vector3 position = refBackR.transform.position;
        Vector3 hitTarget;
        RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, 10, isGround);
        if (hits.Length != 0)
        {
            hitTarget = hits[0].point;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].point.y > hitTarget.y)
                {
                    hitTarget = hits[i].point;
                }
            }
            hitTarget.y += 0.09f;
            targetSubBack.transform.position = hitTarget;
        }
    }

    #endregion

    public void EnableIK()
    {

        controll = true;

        while (IKLeg.weight < 1f)
        {
            IKLeg.weight += Time.deltaTime;
        }

        GetFrontPositionLeft();

    }

    public void DisableIK()
    {

        controll = false;

        while (IKLeg.weight > 0f)
        {
            IKLeg.weight -= Time.deltaTime;
        }

        left = false;
        right = false;
        atMid = false;

    }

}
