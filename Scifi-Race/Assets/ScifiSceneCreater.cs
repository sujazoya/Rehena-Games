using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelCreater
{
    public ConstantSection menuSection;
    public ConstantSection startSection;
    public RandomSection section1;
    public RandomSection section2;
    public ConstantSection section3;
    public RandomSection section4;
    public RandomSection section5;
    public ConstantSection section6;

    public AudioClip[] musicLoop;
    public AudioClip[] menuLoop;    

}

[System.Serializable]
public class Blocks
{
    public string name;
    public GameObject[] blocks;
    public Material skyboxMat;
    public float blockSize;
    public Color32 levelSkyColor;

}

public class ScifiSceneCreater : MonoBehaviour
{

    [SerializeField]
    LevelCreater levelItems;

    [SerializeField] Blocks[] blocks;
    public SkyboxRotator skyboxRotator;
    public SoundManager soundManager;
    public static ScifiSceneCreater instance;
    public LevelManager levelManager;
    int index;
   //[SerializeField] HeightFogGlobal heightFogGlobal;
   // [SerializeField] HeightFogOverride heightFogOverride;
    public static int SceneType
    {
        get { return PlayerPrefs.GetInt("SceneType", 0); }
        set { PlayerPrefs.SetInt("SceneType", value); }
    }
    private void Awake()
    {
        instance = this;
        CreateLevel(SceneType);
        index = Random.Range(0, 1);
        soundManager.music = new AudioClip[2];

        soundManager.music[0] = levelItems.musicLoop[index];
        soundManager.music[1] = levelItems.menuLoop[index];
    }
    // Start is called before the first frame update
    void Start()
    {
        //heightFogGlobal = FindObjectOfType<HeightFogGlobal>();
        //heightFogOverride = FindObjectOfType<HeightFogOverride>();

    }
    void CreateLevel(int levelIndex)
    {
        levelManager.kBlockSize = blocks[levelIndex].blockSize;
        levelItems.menuSection.block = blocks[levelIndex].blocks[0];
        levelItems.startSection.block = blocks[levelIndex].blocks[0];
        levelItems.section1.blocks = new GameObject[1];
        levelItems.section1.blocks[0]= blocks[levelIndex].blocks[1];
        levelItems.section2.blocks = new GameObject[2];
        levelItems.section2.blocks[0] = blocks[levelIndex].blocks[2];
        levelItems.section2.blocks[1] = blocks[levelIndex].blocks[3];
        levelItems.section3.block = blocks[levelIndex].blocks[4];

        levelItems.section4.blocks = new GameObject[1];
        levelItems.section4.blocks[0] = blocks[levelIndex].blocks[3];

        levelItems.section5.blocks = new GameObject[3];
        levelItems.section5.blocks[0] = blocks[levelIndex].blocks[2];
        levelItems.section5.blocks[1] = blocks[levelIndex].blocks[1];
        levelItems.section5.blocks[2] = blocks[levelIndex].blocks[3];
        
        levelItems.section6.block = blocks[levelIndex].blocks[4];
        skyboxRotator.skyboxMaterial= blocks[levelIndex].skyboxMat;
        RenderSettings.skybox= blocks[levelIndex].skyboxMat;
        Color newColor = blocks[levelIndex].levelSkyColor;
        newColor.a = 0.2f;
        RenderSettings.fogColor = newColor;
        //heightFogGlobal.fogColor = newColor;
        //heightFogOverride.fogColor = newColor;
        //heightFogGlobal.directionalColor = newColor;
        //heightFogOverride.directionalColor = newColor;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
