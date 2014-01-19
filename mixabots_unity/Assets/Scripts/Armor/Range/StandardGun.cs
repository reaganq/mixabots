using UnityEngine;
using System.Collections;

public class StandardGun : ArmorSkill {

    public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;
    public GameObject bulletPrefab;
    public bool canHitMultipleTargets;

    public int currentAmmoCount = 0;
    public int shotsFired = 0;

	// Use this for initialization
    #region setup and unequip
    public override void Initialise(Animation target, Transform character, CharacterActionManager actionManager, Collider masterCollider)
    {
        base.Initialise(target,character, actionManager, masterCollider);
        TransferAnimations();
        currentAmmoCount = maxAmmoCount;
    }

    public void TransferAnimations()
    {
        StartCoroutine(TransferAnimation(castAnimation));
        StartCoroutine(TransferAnimation(durationAnimation));
        StartCoroutine(TransferAnimation(recoilAnimation));
        StartCoroutine(TransferAnimation(reloadAnimation));
        StartCoroutine(TransferAnimation(followThroughAnimation));
    }

    public override void UnEquip()
    {
        RemoveArmorAnimation(castAnimation);
        RemoveArmorAnimation(durationAnimation);
        RemoveArmorAnimation(recoilAnimation);
        RemoveArmorAnimation(reloadAnimation);
        RemoveArmorAnimation(followThroughAnimation);
    }
    #endregion

    public void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if(cooldownTimer <= 0 && isReloading)
        {
            isReloading = false;
        }

        if(fireSpeedTimer > 0)
        {
            fireSpeedTimer -= Time.deltaTime;
        }
    }

    public override IEnumerator PressDown()
    {
        armorState = ArmorState.casting;
        characterAnimation.CrossFade(castAnimation.clip.name, 0.05f);
        yield return new WaitForSeconds(castAnimation.clip.length);

        characterAnimation.CrossFade(durationAnimation.clip.name);
        _skillActive = true;

        while(_skillActive)
        {
            if(fireSpeedTimer <= 0 && !isReloading)
            {
                armorState = ArmorState.onUse;
                FireOneShot();
            }
        }

    }

    public override IEnumerator PressUp()
    {
        if(armorState == ArmorState.casting)
        {
            yield return new WaitForEndOfFrame();
        }

        if(armorState == ArmorState.onUse && shotsFired == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        if(isReloading)
        {
            yield return new WaitForEndOfFrame();
        }

        _skillActive = false;
        armorState = ArmorState.followThrough;
        characterAnimation.CrossFade(followThroughAnimation.clip.name);
        yield return new WaitForSeconds(followThroughAnimation.clip.length);

        Reset();

    }

    public void Reset()
    {
        characterAnimation[castAnimation.clip.name].time = 0;
        characterAnimation[durationAnimation.clip.name].time = 0;
        characterAnimation[recoilAnimation.clip.name].time = 0;
        characterAnimation[reloadAnimation.clip.name].time = 0;
        characterAnimation[followThroughAnimation.clip.name].time = 0;
        armorState = ArmorState.ready;

    }

    public IEnumerator FireOneShot()
    {
        //fire bullet
        characterAnimation.Play(recoilAnimation.clip.name);
        currentAmmoCount --;
        fireSpeedTimer = fireSpeed;
        yield return new WaitForSeconds(recoilAnimation.clip.length);
        shotsFired ++;
        if(currentAmmoCount <= 0)
        {
            Reload();
        }
        Debug.Log("fired a shot, " + "shortsfired: "+ shotsFired + "ammo: " + currentAmmoCount);
    }

    public void Reload()
    {
        cooldownTimer = cooldown;
        isReloading = true;
        armorState = ArmorState.onCooldown;
        characterAnimation.CrossFade(reloadAnimation.clip.name);
        currentAmmoCount = maxAmmoCount;
    }
}
