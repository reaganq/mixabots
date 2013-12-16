using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimationManager : MonoBehaviour {

    public ArmorOverrideAnimationController[] armorControllers = new ArmorOverrideAnimationController[5];
    public Animation animationTarget;
    public CharacterAnimationState myState = CharacterAnimationState.idle;

    void Awake()
    {
        armorControllers = new ArmorOverrideAnimationController[5];
    }
	// Use this for initialization
	void Start () {
        animationTarget.Play("Default_Idle");
	}
	
	// Update is called once per frame
	void Update () {
        //AnimationUpdate();
	}


    
    public void AnimateToIdle()
    {
        if(myState != CharacterAnimationState.idle)
        {
            animationTarget.CrossFade("Default_Idle");
            /*foreach(ArmorController armor in ArmorControllers)
            {
                if(armor != null && armor.hasIdleAnimationOverride)
                {
                    animationTarget.CrossFade(armor.IdleOverrideAnimationClip.name);
                }
            }*/
            myState = CharacterAnimationState.idle;
        }
    }
    
    public void AnimateToRunning()
    {
        if(myState != CharacterAnimationState.running)
        {
            animationTarget.CrossFade("Default_Run");
            foreach(ArmorOverrideAnimationController armor in armorControllers)
            {
                //Debug.Log("found armor");
                if(armor != null && armor.runningOverrideAnim != null)
                {
                    animationTarget.CrossFade(armor.runningOverrideAnim.clip.name);
                    Debug.Log("overriding running idle");
                }
            }
            myState = CharacterAnimationState.running;
        }
    }

    public void AddArmorcontroller(ArmorOverrideAnimationController controller, int index)
    {
        armorControllers[index] = controller;
    }
    
    /*public void UpdateUpperBodyMixingTransforms()
    {
        foreach(ArmorController armor in ArmorControllers)
        {
            if(armor != null)
                armor.UpdateMixingTransforms();
        }
    }*/
}



public enum CharacterAnimationState
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