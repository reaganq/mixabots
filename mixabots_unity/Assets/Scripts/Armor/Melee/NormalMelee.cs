using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalMelee : ArmorSkill {


    /***** set up in inspector *****/
    public AttackAnimation[] attackAnimations;
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

    #region skill state

    #endregion

    #region setup and unequip
    public override void Initialise(Animation target, Transform character, CharacterActionManager actionManager)
    {
        base.Initialise(target,character, actionManager);
        TransferAnimations();
        Debug.Log("override");
    }

    public void TransferAnimations()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            StartCoroutine(TransferAnimation(attackAnimations[i]));
        }
    }

    public override void UnEquip()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            RemoveAnimation(attackAnimations[i].clip);
        }
    }
    #endregion

	// Update is called once per frame
	void Update () {
	    /*if(refreshTime > 0)
        {
            refreshTime -= Time.deltaTime;
        }*/

        if(armorState == ArmorState.onUse)
        {
            if(detectionMethod == TargetDetectionMethod.weaponRange)
            {
                //sphereoverlap
            }
        }
	}

    public override IEnumerator UseSkill()
    {
        yield return null;
        Debug.Log("use skill");
        if(attackAnimations.Length == 0)
        {
            Debug.Log("no attack animations");
            yield break;
        }

        armorState = ArmorState.casting;

        int i = Random.Range(0, attackAnimations.Length);
        Debug.Log("i = "+i);
        characterAnimation[attackAnimations[i].clip.name].time = 0;
        characterAnimation.CrossFade(attackAnimations[i].clip.name, 0.05f);

        float totalTime = attackAnimations[i].clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

       /* Debug.Log(totalTime);
        Debug.Log(castTime);
        Debug.Log(attackduration);
        Debug.Log(followThroughTime);*/

        yield return new WaitForSeconds(castTime);
        armorState = ArmorState.onUse;
        _skillActive = true;

        yield return new WaitForSeconds(attackduration);
        armorState = ArmorState.recoiling;
        _skillActive = false;

        yield return new WaitForSeconds(followThroughTime*0.3f);
        characterAnimation.Blend(attackAnimations[i].clip.name, 0, followThroughTime*0.7f);

        yield return new WaitForSeconds(followThroughTime * 0.7f);

        Reset();

    }

    public void Reset()
    {
        armorState = ArmorState.ready;
    }
}
