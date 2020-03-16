using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Experimental.VFX;
using Cinemachine;
public class VFXHandler : MonoBehaviour
{
    public static VFXHandler Instance;

    private bool hst = true;
    [Header("Heightened Senses")]
    [SerializeField] private float heightTimer_MAX = 0.3f;
    [SerializeField] private PostProcessVolume heighetendPP;
   
    // Start is called before the first frame update

    [Header("Hit Effects")]
    [SerializeField] private GameObject SmallSparks;
    [SerializeField] private GameObject SmallSplinters;
    [SerializeField] private GameObject SmallBlood;
    [SerializeField] private GameObject AndreiHit;

    [Header("Trail Effects")]
    [SerializeField] private GameObject basicAttackTrail;

    [Header("Rain Controlls")]
    [SerializeField] private VisualEffect rain,mist;
    [SerializeField] private GameObject rainCPU;

    private List<GameObject> tempDecals = new List<GameObject>();
    [SerializeField] private int maxTempDecals = 400;

    private GameObject player;

    [Header("FuryEffects")]
    [SerializeField] private GameObject[] furyEffects;
    [SerializeField] private Animator andreinimator;


    [Header("Attacks")]
    [SerializeField] private GameObject ShotgunShotVFX;

    private ColorGrading PP_ColorGrade = null;
    [SerializeField] private PostProcessVolume PP_Volume;

    private float defaultSmoothness;


    [Header("Material Getter")]
    public TerrainData mTerrainData;
    private int alphamapWidth;
    private int alphamapHeight;

    private float[,,] mSplatmapData;
    private int mNumTextures;

    public enum FloorMaterials {
    Wet,
    Dry,
    Rock,
    Wood
    }



    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Shader.SetGlobalFloat("_GlobalSaturation", 0);

        PP_Volume.profile.TryGetSettings(out PP_ColorGrade);
        
        Adjust_Quality(QualitySettings.GetQualityLevel());

        GetTerrainProps();

        
    }

    // Update is called once per frame
    void Update()
    {
        //print(GetTerrainAtPosition(player.transform.position));
        //VFXHandler.FloorMaterials floorMat = VFXHandler.Instance.GetFloorMaterial();
        //print(floorMat);
        /*if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(HeightenedSenses(hst));
            hst = !hst;
        }*/
        if (rain == null) { return; }
        rain.SetFloat("_GlobalSaturation",Shader.GetGlobalFloat("_GlobalSaturation"));
        rain.SetVector3("_PlayerPos",player.transform.position);
        //mist.SetVector3("_PlayerPos", player.transform.position);
        Shader.SetGlobalVector("_PlayerPosition", player.transform.position);

        rainCPU.transform.position = new Vector3(player.transform.position.x,25,player.transform.position.z);
    }

    private IEnumerator HeightenedSensesCR(bool isStart)
    {
        float heightTimer = 0;
        float satValue = 0;
        
        while (heightTimer < heightTimer_MAX)
        {
            satValue = !isStart? (heightTimer_MAX-heightTimer)/heightTimer_MAX : heightTimer/heightTimer_MAX;
           // print("GS  " + Shader.GetGlobalFloat("_GlobalSaturation") + " Tim:" + heightTimer);
            Shader.SetGlobalFloat("_GlobalSaturation",satValue);
            heighetendPP.weight = satValue;
            heightTimer += Time.deltaTime;
            yield return null;
        }
        Shader.SetGlobalFloat("_GlobalSaturation", !isStart? 0:1);
        heighetendPP.weight = !isStart ? 0 : 1;
       // print("GS  " + Shader.GetGlobalFloat("_GlobalSaturation") + " Tim:" + heightTimer);
        yield return null;
    }

    public void HeightenedSenses(bool isStart)
    {
        StartCoroutine(HeightenedSensesCR(isStart));
    }

    public void SpawnBasicHitVFX(Collision col)
    {
        GameObject spawnFX = SmallSparks;
        if (col.gameObject.GetComponent<OBJ_MaterialType>() != null) {

            switch (col.gameObject.GetComponent<OBJ_MaterialType>().type) {
                case OBJ_MaterialType.ObjMaterial.wood:
                    spawnFX = SmallSplinters;
                    break;
                case OBJ_MaterialType.ObjMaterial.metal:
                    spawnFX = SmallSparks;
                    break;
                case OBJ_MaterialType.ObjMaterial.flesh:
                    spawnFX = SmallBlood;
                    break;
                case OBJ_MaterialType.ObjMaterial.noEffect:
                    spawnFX = null;
                    break;
            }
        }
        else
        {
            spawnFX = SmallSplinters;
        }

        Quaternion anglehit = Quaternion.Euler(col.GetContact(0).normal);

        Debug.DrawRay(col.GetContact(0).point,col.GetContact(0).normal,Color.red,100);
        if (spawnFX != null)
        {
            GameObject tempSys = Instantiate(spawnFX, col.GetContact(0).point, Quaternion.FromToRotation(Vector3.up, col.GetContact(0).normal));
        }
       

    }

    public void SpawnBasicMeeleeTrailVFX(Transform point)
    {
        Instantiate(basicAttackTrail, point);
    }

    public void SpawnBasicShotgunVFX(Transform point)
    {
        Instantiate(ShotgunShotVFX, point.transform.position,point.transform.rotation);
    }

    public void SpawnBasicBoltTrailVFX(Transform point)
    {

    }

    public void AddTemporaryDecal(GameObject decal)
    {
        if(tempDecals.Count >= maxTempDecals)
        {
            Destroy(tempDecals[0]);
            tempDecals.RemoveAt(0);
            tempDecals.Add(decal);
        }
        else
        {
            tempDecals.Add(decal);
        }
    }

    public void FuryStart()
    {
        andreinimator.SetTrigger("T_Wolf");
        foreach(GameObject o in furyEffects)
        {
            o.SetActive(true);
        }
    }

    public void FuryEnd()
    {
        andreinimator.SetTrigger("T_Wolf_Out");
        foreach (GameObject o in furyEffects)
        {
            o.SetActive(false);
        }
    }


    public void AndreiWasHit()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(new Vector3(1,1,1));
        Instantiate(AndreiHit,player.transform);
    }


    public void Adjust_Brightness(float value)
    {
        Mathf.Clamp(value, -1, 1);
        PP_ColorGrade.postExposure.value = Mathf.Lerp(-2, 2f, value);
    }

    public void Adjust_ScreenSize(Resolution resolution, bool fullscreen)
    {
        Screen.SetResolution(resolution.width,resolution.height, fullscreen);
    }

    public void Adjust_Quality(int quality)
    {
        QualitySettings.SetQualityLevel(quality,true);

        switch (quality)
        {
            case 0:
                maxTempDecals = 10;    
            break;
            case 1:
                maxTempDecals = 50;
            break;
            case 2:
                maxTempDecals = 100;
            break;
            case 3:
                maxTempDecals = 250;
            break;
        }
    }

    // --- //

    private void GetTerrainProps()
    {
        mTerrainData = Terrain.activeTerrain.terrainData;
        alphamapWidth = mTerrainData.alphamapWidth;
        alphamapHeight = mTerrainData.alphamapHeight;

        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 playerPos)
    {
        Vector3 vecRet = new Vector3();
        Terrain ter = Terrain.activeTerrain;
        Vector3 terPosition = ter.transform.position;
        vecRet.x = ((playerPos.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
        vecRet.z = ((playerPos.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
        return vecRet;
    }

    private int GetActiveTerrainTextureIdx(Vector3 pos)
    {
        Vector3 TerrainCord = ConvertToSplatMapCoordinate(pos);
        int ret = 0;
        float comp = 0f;
        for (int i = 0; i < mNumTextures; i++)
        {
            if (comp < mSplatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
                ret = i;
        }
        return ret;
    }

    public int GetTerrainAtPosition(Vector3 pos)
    {
        int terrainIdx = GetActiveTerrainTextureIdx(pos);
        return terrainIdx;
    }

    public VFXHandler.FloorMaterials GetFloorMaterial()
    {
        int idx = GetTerrainAtPosition(player.transform.position);

        switch (idx) {
            case 2: return VFXHandler.FloorMaterials.Wet;
            case 4: return VFXHandler.FloorMaterials.Wet;
            case 0: return VFXHandler.FloorMaterials.Dry;
            case 3: return VFXHandler.FloorMaterials.Dry;
            case 1: return VFXHandler.FloorMaterials.Rock;
            case 5: return VFXHandler.FloorMaterials.Rock;
            
        }
        return VFXHandler.FloorMaterials.Wet ;

        
    }

}
