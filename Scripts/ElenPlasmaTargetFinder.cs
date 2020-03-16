using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[ExecuteInEditMode]
public class ElenPlasmaTargetFinder : MonoBehaviour
{
    private VisualEffect vfxPlasma;
    [SerializeField] private float range = 5;

    private float timer = 0;
    [SerializeField] private float refreshRate = 2;
    // Start is called before the first frame update

    private void Reset()
    {
        vfxPlasma = gameObject.GetComponent<VisualEffect>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= refreshRate)
        {
            timer = 0;

            RaycastHit hit;

            Physics.Raycast(transform.position, Random.insideUnitSphere,out hit,range);

            if (hit.collider != null)
            {
                vfxPlasma.SetVector3("Target", hit.point);
            }

        }
    }
}
