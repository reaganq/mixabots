using UnityEngine;
using System.Collections;

public class ThirdPersonCamera: MonoBehaviour {
 
    public Transform targetTransform = null;
    public Transform pivotTransform = null;
    private Vector3 offsetPosition;
    
    public float rotationSpeed = 50;
    public EasyJoystick joystick = null;
    
    private Transform _myTransform;
    
    private Vector2 outputAngleVector;
    private float outputAngle = 0.0f;
    
    private Vector2 camRotation;
    
	// Use this for initialization
	void Start () {
	    _myTransform = transform;
        camRotation = new Vector2(0,0);
        outputAngleVector = new Vector2(0,0);
        offsetPosition = pivotTransform.position;
        joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();
	}
    
    /*void OnEnable(){
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }
 
    void OnDisable(){
        EasyJoystick.On_JoystickMove -= On_JoystickMove ;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }
     
    void OnDestroy(){
        EasyJoystick.On_JoystickMove -= On_JoystickMove;    
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }
	
    void On_JoystickMove( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
        {
            //move.joystickAxis
            outputAngleVector = move.joystickAxis;
        }
    }
    
    void On_JoystickMoveEnd( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
            outputAngleVector = new Vector2(0,0);
    }*/
    
	// Update is called once per frame
	void LateUpdate () {
        
        outputAngle = Mathf.Atan2(joystick.JoystickAxis.x, joystick.JoystickAxis.y);
        
        
        pivotTransform.position = targetTransform.position;
        camRotation = new Vector2(Mathf.Sin(outputAngle), Mathf.Cos(outputAngle));
        camRotation.x *= rotationSpeed;
        camRotation *= Time.deltaTime;
        
        //Debug.Log(camRotation.x);
        
        pivotTransform.Rotate(0, camRotation.x, 0, Space.World );
        
        
        /*var camRotation = new Vector2(Mathf.Sin(joystickCircle.outputAngleRad) , Mathf.Cos(joystickCircle.outputAngleRad));
 camRotation.x *= rotationSpeed.x;
 camRotation.y *= rotationSpeed.y;
 camRotation *= Time.deltaTime;
 
 // Rotate around the character horizontally in world, but use local space
 // for vertical rotation
 cameraPivot.Rotate( 0, camRotation.x, 0, Space.World );
 cameraPivot.Rotate( camRotation.y * -5, 0, 0 );*/
        
    }

}
