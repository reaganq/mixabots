using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
 
    private InputType inputType;
    public CharacterController character = null;
    private Transform _myTransform;
    public CharacterMotor motor = null;
    
    //touch joy stick 
    public EasyJoystick joystick = null;
    
    public Transform cameraTransform = null;
    public Vector3 inputDir;
    
    public Vector2 outputAngleVector;
    public float outputAngle = 0.0f;
    public float speed = 5;
    public float outputForce = 0.0f;
    
    //wasd controls
    
    void Awake()
    {

        
        _myTransform = transform;
        character = gameObject.GetComponent<CharacterController>();
        motor = gameObject.GetComponent<CharacterMotor>();
        
        //outputAngleVector = new Vector2(0,0);
        cameraTransform = Camera.main.gameObject.transform;
        
        
    }
	// Use this for initialization
	void Start () {
	    inputType = GameManager.inputType;
        
        if(inputType == InputType.WASDInput)
        {
        }
        
        else if(inputType == InputType.TouchInput)
        {
            joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
        if(inputType == InputType.WASDInput)
        {
            /*bool dirPressed = true;
            
            float inputX=Input.GetAxis("Horizontal");
            float inputY=Input.GetAxis("Vertical");
            
            if(inputX!=0f||inputY!=0f){
                dirPressed=true;
                
                inputDir=new Vector3(inputX,0f,inputY).normalized;
                motor.SetRotAngle(_myTransform.position+inputDir);
                Debug.Log(inputDir);
            }
            
            if(inputX == 0f && inputY == 0)
            {
                inputDir = Vector3.zero;
                Debug.Log("inputs are zero");
            }*/
            
            //inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //inputDir = _myTransform.TransformDirection(inputDir);
            
            var direction = Vector3.zero;
     var forward = Quaternion.AngleAxis(-90,Vector3.up) * cameraTransform.right;
     
     if(Input.GetKey(KeyCode.W))
            {
     direction += forward;
                //Debug.Log("W");
            }
     if(Input.GetKey(KeyCode.S))
     direction -= forward;
     if(Input.GetKey(KeyCode.A))
     direction -= cameraTransform.right;
     if(Input.GetKey(KeyCode.D))
     direction += cameraTransform.right;
            
                direction.Normalize();
     motor.Move(direction);
        }
        
        
        if(inputType == InputType.TouchInput)
        {
    	    outputAngle = Mathf.Atan2(joystick.JoystickAxis.x, joystick.JoystickAxis.y);
            
            //Debug.Log(outputAngle +" angle");
            //var movement = cameraTransform.TransformDirection( Vector3( Mathf.Sin(joystickCircle.outputAngleRad) , 0, Mathf.Cos(joystickCircle.outputAngleRad) ) );
            Vector3 movement = cameraTransform.TransformDirection( new Vector3(Mathf.Sin(outputAngle), 0, Mathf.Cos(outputAngle)) );
     
            outputForce = Vector2.Distance(joystick.JoystickAxis, Vector2.zero);
            // We only want the camera-space horizontal direction
            movement.y = 0;
            movement.Normalize(); // Adjust magnitude after ignoring vertical movement
            
            // Let's use the largest component of the joystick position for the speed.
            movement *= speed * outputForce;
            /*movement *= speed * moveJoystick.outputPower;
            
            // Check for jump
            //if ( !character.isGrounded )
            {   
             // Apply gravity to our velocity to diminish it over time
             velocity.y += Physics.gravity.y * Time.deltaTime;
             
             // Adjust additional movement while in-air
             movement.x *= inAirMultiplier;
             movement.z *= inAirMultiplier;
            }*/
            
            //movement += velocity;
            movement += Physics.gravity;
            movement *= Time.deltaTime;
            
            // Actually move the character
            character.Move( movement );
            
            //Debug.Log(character.velocity);
            //Vector3 movement = new Vector3(0,0,0);]
            FaceMovementDirection();
        }
    }
    
    void FaceMovementDirection()
    {    
        Vector3 horizontalVelocity = character.velocity;
        horizontalVelocity.y = 0; // Ignore vertical movement
     
     // If moving significantly in a new direction, point that character in that direction
        if ( horizontalVelocity.magnitude > 0.1 )
            _myTransform.forward = horizontalVelocity.normalized;
         //{
         //var newDir = Vector3.RotateTowards
         //}
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
            outputAngleVector = move.joystickAxis;
            outputForce = Vector2.Distance(move.joystickAxis, Vector2.zero);
            Debug.Log(outputForce + " outputforce");
            Debug.Log(move.joystickValue);
        }
    }
    
    void On_JoystickMoveEnd( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
        {
            outputAngleVector = new Vector2(0,0);
            outputForce = 0;
        }
    }*/
}

