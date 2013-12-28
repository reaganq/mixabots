using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorSkill : MonoBehaviour {

    public bool canUse;
    public bool isLimitedUse;

    public InputTrigger inputTrigger;

    /*** attribute ***/

    public float cooldown;
    public float maxAmmoCount;
    public float fireSpeed;
    public float reloadTime;
    public float damage;

    public ArmorAttribute[] armorAttributes;
    public SkillEffect[] skillEffects;
    public List<SkillEffect> onUseSkillEffects;
    public List<SkillEffect> onHitSkillEffects;
    public List<SkillEffect> onReceiveHitSkillEffects;

    public Transform characterTransform;
    public Animation characterAnimation;
    public CharacterActionManager characterManager;
    public ArmorState armorState;

    public virtual void Initialise(Animation target, Transform character, CharacterActionManager actionManager)
    {
        characterAnimation = target;
        characterTransform = character;
        characterManager = actionManager;
        Debug.Log("base initialise");
    }

    public virtual IEnumerator UseSkill()
    {
        Debug.Log("using skill");
        yield return null;
    }

    public virtual IEnumerator StartSkill()
    {
        yield return null;
    }

    public virtual IEnumerator EndSkill()
    {
        yield return null;
    }

    public void ApplyOnHitSkillEffects()
    {
    }

    public void ApplyOnReceiveHitSkillEffects()
    {
    }

    public void ApplyOnUseSkillEffects()
    {
        for (int i = 0; i < onUseSkillEffects.Count; i++) {
            switch(onUseSkillEffects[i].effectType)
            {
            case SkillEffectCategory.speed:
                if(onUseSkillEffects[i].effectTarget == TargetType.self)
                {
                    characterManager.motor.runSpeed *= onUseSkillEffects[i].effectValue;
                    characterManager.motor.AnimationUpdate();
                }
                Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
        Debug.Log("onuseskilleffects");
    }

    public void RemoveOnUseSkillEffects()
    {
        for (int i = 0; i < onUseSkillEffects.Count; i++) {
            switch(onUseSkillEffects[i].effectType)
            {
            case SkillEffectCategory.speed:
                if(onUseSkillEffects[i].effectTarget == TargetType.self && onUseSkillEffects[i].durationFormat == SkillEffectFormat.useDuration)
                {
                    characterManager.motor.runSpeed /= onUseSkillEffects[i].effectValue;
                    characterManager.motor.AnimationUpdate();
                }
                Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
    }

    public void InitializeAttributesStats(ArmorAttribute[] attributes, SkillEffect[] effects)
    {
        //characterTransform = Player.Instance.avatarObject.transform;
        skillEffects = effects;
        armorAttributes = attributes;
    }

    public void InitializeAttributesStats()
    {
        for (int i = 0; i < armorAttributes.Length ; i++) {
            if(armorAttributes[i].attributeName == ArmorAttributeName.cooldown)
                cooldown = armorAttributes[i].attributeValue;
            if(armorAttributes[i].attributeName == ArmorAttributeName.damage)
                damage = armorAttributes[i].attributeValue;
        }
    }

    public void PopulateSkillEffects()
    {
        for (int i = 0; i < skillEffects.Length ; i++) {
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onUse)
                onUseSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onHit)
                onHitSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onReceiveHit)
                onReceiveHitSkillEffects.Add(skillEffects[i]);
        }
    }

    public float GetAttributeValue(ArmorAttributeName name)
    {
        for (int i = 0; i < armorAttributes.Length; i++) {
            if(armorAttributes[i].attributeName == name)
                return armorAttributes[i].attributeValue;
        }
        return 0;
    }

    #region Animation Setup
   
    public IEnumerator TransferAnimation(ArmorAnimation anim)
    {
        characterAnimation.AddClip(anim.clip, anim.clip.name);
        characterAnimation[anim.clip.name].layer = anim.animationLayer;
        //StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
        yield return null;
        
        if(anim.addMixingTransforms.Count>0)
        {
            for (int i = 0; i < anim.addMixingTransforms.Count; i++) {
                characterAnimation[anim.clip.name].AddMixingTransform(GetBone(anim.addMixingTransforms[i]), false);
            }
        }

        if(anim.removeMixingTransforms.Count>0)
        {
            for (int i = 0; i < anim.removeMixingTransforms.Count; i++) 
            {
                characterAnimation[anim.clip.name].RemoveMixingTransform(GetBone(anim.removeMixingTransforms[i]));
            }
        }
    }

    public virtual void UnEquip()
    {

    }

    public void RemoveAnimation(AnimationClip clip)
    {
        characterAnimation.RemoveClip(clip.name);
    }

    /*public IEnumerator MixingTransforms(List<string> bonelist, List<string> removelist, AnimationClip clip)
    {
        yield return null;
        
        if(bonelist.Count>0)
        {
            for (int i = 0; i < bonelist.Count; i++) {
                animationTarget[clip.name].AddMixingTransform(GetBone(bonename), false);
            }
        }
        
        if(removelist.Count>0)
        {
            foreach(string bonename in removelist)
            {
                animationTarget[clip.name].RemoveMixingTransform(GetBone(bonename));
            }
        }
        
    }*/
    
    public Transform GetBone(string bonename)
    {
        Transform[] kids = characterTransform.GetComponentsInChildren<Transform>();
        //Debug.Log(kids.Length);
        for (int i = 0; i < kids.Length; i++) {
            if(kids[i].name == bonename)
            {
                //Debug.Log(kids[i].name);
                return kids[i];
            }
        }
        return null;
    }

    #endregion
}

public enum ArmorState
{
    ready,
    casting,
    onUse,
    recoiling,
    onCooldown,
    reloading
}

public enum TargetDetectionMethod
{
    directionalRange,
    customColliderHit,
    weaponRange,
    characterRange,
    projectileCollider,
    notNeeded
}

public enum InputTrigger
{
    OnPressUp,
    OnPressDown,
    OnClick
}

