using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalMelee : ArmorSkill {


    /***** set up in inspector *****/
    public SkillAnimation[] attackAnimations;
    public TargetDetectionMethod detectionMethod;
    public Transform weaponLocation;
    public GameObject weaponCollider;
    public float detectionRange;
    //public float collisionRange;
    //can hit multiple enemies or heal multiple allies?
    public bool canHitMultipleTargets;
    /***************/

    #region skill state

    #endregion

    #region setup and unequip
    public override void Initialise(Animation target, Transform character, CharacterActionManager actionManager, Collider masterCollider)
    {
        base.Initialise(target,character, actionManager, masterCollider);
        TransferAnimations();
        if(weaponCollider != null)
        {
            TriggerCollider tc = weaponCollider.GetComponent<TriggerCollider>();
            if(tc != null)
            {
                tc.ownerCollider = characterCollider;
                tc.IgnoreCollision();
            }
            weaponCollider.SetActive(false);
        }
    }

    public void TransferAnimations()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            TransferSkillAnimation(attackAnimations[i]);
        }
    }

    public override void UnEquip()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            RemoveSkillAnimation(attackAnimations[i]);
        }
    }
    #endregion

	// Update is called once per frame
	void Update () {

	}

    public override IEnumerator PressUp()
    {
        yield return null;
        //Debug.Log("use skill");
        if(attackAnimations.Length == 0)
        {
            Debug.Log("no attack animations");
            yield break;
        }

        armorState = ArmorState.casting;

        int i = Random.Range(0, attackAnimations.Length);
        //Debug.Log("i = "+i);
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
        if(weaponCollider != null)
        {
            weaponCollider.SetActive(true);
        }
        _skillActive = true;

        yield return new WaitForSeconds(attackduration);
        armorState = ArmorState.followThrough;
        _skillActive = false;

        if(weaponCollider != null)
        {
            weaponCollider.SetActive(false);
        }

        yield return new WaitForSeconds(followThroughTime*0.3f);
        characterAnimation.Blend(attackAnimations[i].clip.name, 0, followThroughTime*0.7f);

        yield return new WaitForSeconds(followThroughTime * 0.7f);

        Reset();

    }

    public override void HitEnemy(CharacterStatus target)
    {
        //package up all attack data, damage + status effects etc
        //target.receiveHit(data);
        target.DealDamage(50);
        Debug.Log("hitenemy");
    }

    public override void HitAlly(CharacterStatus target)
    {
        //package up all attack data, damage + status effects etc
        //target.receiveHit(data)
    }

    public void Reset()
    {
        armorState = ArmorState.ready;
        HitEnemies.Clear();
    }
}
