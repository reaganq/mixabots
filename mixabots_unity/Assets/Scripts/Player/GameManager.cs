using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 
    public static InputType inputType;
    public static EasyJoystick joystick;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();
        #if UNITY_EDITOR || UNITY_WEBPLAYER
            inputType = InputType.WASDInput;
        Debug.LogWarning("wasd");
            joystick.gameObject.SetActive(false);
        #endif
        
        #if UNITY_IPHONE 
            inputType = InputType.TouchInput;
        Debug.Log("wtf");
        #endif
        
        #if UNITY_ANDROID && !UNITY_EDITOR
            inputType = InputType.WASDInput;
        #endif
        
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum InputType
{
    TouchInput = 0,
    WASDInput = 1
}
