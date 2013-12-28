using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongDurationMelee : ArmorSkill {

    /***** set up in inspector *****/
    public ArmorAnimation backSwingAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation followThroughAnimation;
    public TargetDetectionMethod detectionMethod;
    public Transform weaponLocation;
    public float detectionRange;
    //public float collisionRange;
    //can hit multiple enemies or heal multiple allies?
    public bool canHitMultipleTargets;
    
    public List<Transform> HitEnemies;
    public List<Transform> HitAllies;
    
    /***************/
    
    public bool _skillActive = false;

    #region setup and unequip
    public override void Initialise(Animation target, Transform character, CharacterActionManager actionManager)
    {
        base.Initialise(target,character, actionManager);
        TransferAnimations();
        Debug.Log("override");
    }
    
    public void TransferAnimations()
    {
        if(backSwingAnimation != null)
            StartCoroutine(TransferAnimation(backSwingAnimation));
        if(durationAnimation != null)
            StartCoroutine(TransferAnimation(durationAnimation));
        if(followThroughAnimation != null)
            StartCoroutine(TransferAnimation(followThroughAnimation));
    }
    
    public override void UnEquip()
    {
        if(backSwingAnimation != null)
            RemoveAnimation(backSwingAnimation.clip);
        if(durationAnimation != null)
            RemoveAnimation(durationAnimation.clip);
        if(followThroughAnimation != null)
            RemoveAnimation(followThroughAnimation.clip);
    }
    #endregion
    
    // Update is called once per frame
    
    public override IEnumerator StartSkill()
    {
        yield return null;
        Debug.Log("start skill");

        armorState = ArmorState.casting;

        characterAnimation[backSwingAnimation.clip.name].time = 0;
        characterAnimation[backSwingAnimation.clip.name].speed = 1;
        characterAnimation[durationAnimation.clip.name].time = 0;
        characterAnimation[followThroughAnimation.clip.name].time = 0;

        characterAnimation.Play(backSwingAnimation.clip.name);
        yield return new WaitForSeconds(backSwingAnimation.clip.length);
  
        armorState = ArmorState.onUse;
        _skillActive = true;

        characterAnimation.Play(durationAnimation.clip.name);
        //yield return new WaitForSeconds(attackduration);

        Debug.Log("end of start action");
        ApplyOnUseSkillEffects();

        
    }

    public override IEnumerator EndSkill()
    {
        if(_skillActive)
        {
            Debug.Log("wtf");
            _skillActive = false;
            RemoveOnUseSkillEffects();


        }

        float t = characterAnimation[backSwingAnimation.clip.name].normalizedTime;

        Debug.Log("t: " + t);
        float blendTime = 1;

        if(t > 0)
        {
            characterAnimation[followThroughAnimation.clip.name].normalizedTime = (1-t);
            blendTime = followThroughAnimation.clip.length * t;
        }
        else
            blendTime = followThroughAnimation.clip.length;

        if(characterAnimation.IsPlaying(backSwingAnimation.clip.name))
           characterAnimation[backSwingAnimation.clip.name].speed = 0;

        characterAnimation.CrossFade(followThroughAnimation.clip.name);
        armorState = ArmorState.recoiling;

        //yield return new WaitForSeconds(followThroughAnimation.clip.length*0.3f);

        characterAnimation.Blend(followThroughAnimation.clip.name, 0, blendTime);
        
        yield return new WaitForSeconds(blendTime*0.9f);
        //Debug.Log("time pased: "+ (Time.realtimeSinceStartup - time));
        //Debug.Log("finish");
        
        Reset();

    }
    
    public void Reset()
    {
        armorState = ArmorState.ready;
    }
}
