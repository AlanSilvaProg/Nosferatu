using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NOS_HighlightScript : MonoBehaviour
{
    private GameObject highlightObj;
    [SerializeField] private bool isAnimated = false;
    private ParticleSystem partSys;

    private enum highlightType {
        Collectable,
        Interactable,
        Important
    }

    [SerializeField] private highlightType highType;

    // Start is called before the first frame update
    void Start()
    {
        if (!isAnimated)
        {
            if (highType == highlightType.Collectable)
            {

                highlightObj = new GameObject();

                highlightObj.transform.parent = transform;

                MeshFilter meshfilter = highlightObj.AddComponent<MeshFilter>();
                meshfilter.mesh = gameObject.GetComponent<MeshFilter>().mesh;

                MeshRenderer meshRend = highlightObj.AddComponent<MeshRenderer>();
                meshRend.material = (Material)Resources.Load("CollectableHighlight_Mat");

                highlightObj.transform.position = transform.position;
                highlightObj.transform.rotation = transform.rotation;
                highlightObj.transform.localScale = Vector3.one;

                
            }
            else if (highType == highlightType.Interactable)
            {

                highlightObj = new GameObject();

                highlightObj.transform.parent = transform;

                MeshFilter meshfilter = highlightObj.AddComponent<MeshFilter>();
                meshfilter.mesh = gameObject.GetComponent<MeshFilter>().mesh;

                MeshRenderer meshRend = highlightObj.AddComponent<MeshRenderer>();
                meshRend.material = (Material)Resources.Load("InteractableHighlight_Mat");

                highlightObj.transform.position = transform.position;
                highlightObj.transform.rotation = transform.rotation;
                highlightObj.transform.localScale = Vector3.one;
            }else
            {
                highlightObj = new GameObject();

                highlightObj.transform.parent = transform;

                MeshFilter meshfilter = highlightObj.AddComponent<MeshFilter>();
                meshfilter.mesh = gameObject.GetComponent<MeshFilter>().mesh;

                MeshRenderer meshRend = highlightObj.AddComponent<MeshRenderer>();
                meshRend.material = (Material)Resources.Load("Highlight_Mat");

                highlightObj.transform.position = transform.position;
                highlightObj.transform.rotation = transform.rotation;
                highlightObj.transform.localScale = transform.localScale;
            }
            
        }
        else
        {
            GameObject sysObj = Instantiate((GameObject)Resources.Load("Highlight_System"),transform);
            partSys = sysObj.GetComponent<ParticleSystem>();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimated)
        {
            if (Shader.GetGlobalFloat("_GlobalSaturation") == 0 && !partSys.isPaused)
            {
                partSys.Clear();
                partSys.Pause();
            }
            else if (Shader.GetGlobalFloat("_GlobalSaturation") > 0)
            {
                partSys.Play();
            }
        }
    }

    public void OnKill()
    {
        Destroy(highlightObj);
    }
}
