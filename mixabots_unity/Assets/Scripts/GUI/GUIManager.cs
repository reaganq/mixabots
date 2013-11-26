using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
 
    private static GUIManager instance;
    
    private GUIManager() {}
    
    public static GUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(GUIManager)) as GUIManager;
            return instance;
        }
    }
    
    public Transform UICameraRoot = null;
    
    public bool CanShowInventory = true;
    public bool CanShowShop = true;

    public GameObject ArmoryGUI = null;
    public GameObject MainGUI = null;
    public GameObject ShopGUI = null;
    public GameObject Joystick = null;
    public GameObject OpenInventoryButton = null;
    
    public bool IsInventoryDisplayed = false;
    public bool IsShopDisplayed = false;
    public bool IsMainGUIDisplayed = false;
    
    
    public Camera Inventory3DCamera = null;
    
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    public void Start()
    {
        //DisplayMainGUI();
        UICameraRoot = GameObject.FindGameObjectWithTag("UICamera").transform;
        
        HideInventory();
    }
    
    public void OnLevelWasLoaded(int level)
    {
        if(level == 0)
            CanShowInventory = true;
        else if (level == 1)
            CanShowInventory = false;
        
        DisplayMainGUI();
    }
    
    public void DisplayMainGUI()
    {
        if(!IsMainGUIDisplayed)
        {
            Joystick.SetActive(true);
            MainGUI.SetActive(true);
            if(!CanShowInventory)
                OpenInventoryButton.SetActive(false);
            IsMainGUIDisplayed = true;
        }
    }
    
    public void HideMainGUI()
    {
        if(IsMainGUIDisplayed)
        {
            MainGUI.SetActive(false);
            Joystick.SetActive(false);
            IsMainGUIDisplayed = false;
        }
    }
    
    public void DisplayInventory ()
    {
        if(!IsInventoryDisplayed && CanShowInventory)
        {
            HideMainGUI();
            ArmoryGUI.SetActive(true);
            IsInventoryDisplayed = true;
            Inventory3DCamera.enabled = true;
            ArmoryGUI.SendMessage("Enable");
        }
    }
    
    public void HideInventory ()
    {
        if(IsInventoryDisplayed)
        {
            ArmoryGUI.SetActive(false);
            IsInventoryDisplayed = false;
            //Debug.Log("hiding inventory");
            DisplayMainGUI();
            Inventory3DCamera.enabled = false;
            
        }
    }
    
    public void DisplayShop ()
    {
        if(!IsShopDisplayed && CanShowShop)
        {
            HideMainGUI();
            ShopGUI.SetActive(true);
            IsShopDisplayed = true;
            ShopGUI.SendMessage("Enable");
                
        }
    }
    
    public void HideShop()
    {
        if(IsShopDisplayed)
        {
            ShopGUI.SetActive(false);
            IsShopDisplayed = false;
            DisplayMainGUI();
        }
    }
}
