using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private static Player instance;
    
    private Player() {}
    
    public static Player Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(Player)) as Player;
            return instance;
        }
    }
    
    public PlayerInformation Hero;
 //public GeneralPlayerSounds sounds;
 //public RPGScene scene;
 //public static bool isChangingScene;
 //public bool doEvents;
 
 //public static ContainerType Container;
 //public static RPGContainer rpgContainer;
 
 //public static float sellPriceModifier;
 
    public UsableItem currentItem;
    public int activeNPC;
    public string ActiveNPCName;
    public NPC ActiveNPC;
    public Shop ActiveShop;
    
    public GameObject avatarObject;
    public Avatar avatar;
 
 //public static GeneralData Data;
 
 //public Texture cursorImage;
 
 //public float effectTimer;
    //private float checkTimer;

    public bool IsLoading;
    //public BaseGameObject selectedObject;


    void Awake()
    {
        avatarObject = GameObject.FindGameObjectWithTag("Player");
        avatar = avatarObject.GetComponent<Avatar>();
        DontDestroyOnLoad(gameObject);
        Hero = new PlayerInformation();
        SaveLoad.content = new SavedContent();
        Hero.StartNewGame();
        //ChangeMinimap(false);
        //gameObject.AddComponent<GUIScale>();
    }
 
    void Start()
    {
     //cursorImage = (Texture2D)Resources.Load("Icon/Cross");
     //Data = new GeneralData();
        
        //spawnPoints = new List<SpawnPoint>();
        //sounds = new GeneralPlayerSounds();
     //LoadScene();
        
    }
 
}

public enum ContainerType
{
 Container = 1,
 CharacterBody = 2
}