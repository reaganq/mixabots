using UnityEngine;
using System.Collections;

public class RobotAnimationController : MonoBehaviour {
 
    public ArmorController[] ArmorControllers = new ArmorController[5];
    
    public CharacterController controller;
    public Animation animationTarget;
    public Avatar avatar;
    public animationState myState;
    public ThirdPersonJoystickController joystickController;
    
    
    public float maxForwardSpeed;
    private Vector3 characterVelocity;
    private Vector3 horizontalVelocity;
    private float speed;
    
	// Use this for initialization
    void OnEnable()
    {
        AttackButtonMessage.onSingleClick += onClick;
        AttackButtonMessage.onPress += onPress;
        AttackButtonMessage.onRelease += onRelease;
        
    }
    
    void OnDisable()
    {
        AttackButtonMessage.onSingleClick -= onClick;
        AttackButtonMessage.onPress -= onPress;
        AttackButtonMessage.onRelease -= onRelease;
    }
    
    public void onClick(int index)
    {
        ArmorControllers[index].ButtonClicked();
    }
    
    public void onPress(int index)
    {
        ArmorControllers[index].ButtonPressed();
    }
    
    public void onRelease(int index)
    {
        ArmorControllers[index].ButtonReleased();
    }
    
	void Start () {
        
	    controller=GetComponent<CharacterController>();
        avatar = GetComponent<Avatar>();
        joystickController = GetComponent<ThirdPersonJoystickController>();
        myState = animationState.idle;
        animationTarget.Play("idle");
        //UpdateAnimation(); 
        
	}
	
	// Update is called once per frame
	void Update () {
        
        characterVelocity = controller.velocity;
	    horizontalVelocity = characterVelocity;
        horizontalVelocity.y = 0;
        speed = horizontalVelocity.magnitude;
        
        float t = 0.0f;
        
        if(speed > 0)
        {
            //Debug.Log(speed);
            t = Mathf.Clamp( Mathf.Abs( speed / maxForwardSpeed ), 0, maxForwardSpeed );
            animationTarget["run"].speed = Mathf.Lerp( 0.7f, 1.5f, t);
            AnimateToRunning();
        }
        else
        {
            AnimateToIdle();
        }
	}
    
    public void AnimateToIdle()
    {
        if(myState != animationState.idle)
        {
            animationTarget.CrossFade("idle");
            foreach(ArmorController armor in ArmorControllers)
            {
                //Debug.Log("found armor");
                if(armor != null && armor.hasIdleAnimationOverride)
                {
                    animationTarget.CrossFade(armor.IdleOverrideAnimationClip.name);
                Debug.Log("overriding idle");
                        }
            }
            myState = animationState.idle;
        }
    }
    
    public void AnimateToRunning()
    {
        if(myState != animationState.running)
        {
            animationTarget.CrossFade("run");
            foreach(ArmorController armor in ArmorControllers)
            {
                //Debug.Log("found armor");
                if(armor != null && armor.hasRunningOverride)
                {
                    animationTarget.Blend(armor.RunningOverrideAnimationClip.name);
                Debug.Log("overriding idle");
                }
            }
            myState = animationState.running;
        }
        
        
    }
    
    public void UpdateAnimation()
    {
        if(myState == animationState.idle)
        {
            animationTarget.CrossFade("idle");
            foreach(ArmorController armor in ArmorControllers)
            {
                //Debug.Log("found armor");
                if(armor != null && armor.hasIdleAnimationOverride)
                {
                    animationTarget.Play(armor.IdleOverrideAnimationClip.name);
                Debug.Log("overriding idle");
                }
            }
        }
    }
    
    public void UpdateUpperBodyMixingTransforms()
    {
        foreach(ArmorController armor in ArmorControllers)
        {
            if(armor != null)
                armor.UpdateMixingTransforms();
        }
    }
    
    
}

public enum animationState
{
    idle,
    running,
    armLpreattack,
    armLattack,
    armLrecoil,
    armLpostattack,
    armRpreattack,
    armRattack,
    armRrecoil,
    armRpostattack,
    specialattack,
    stunned,
    frozen,
    victory,
    death
}

// idle
// run
// armLpreattack
//armLattack
//armLrecoil
//armLpostattack

// armRpreattack
//armRattack
//armRrecoil
//armRpostattack

//specialattack

//stunned
//frozen
//victory
//death