using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ArmorAttribute {

    public ArmorAttributeName attributeName;
    public float attributeValue;
}

public enum ArmorAttributeName
{
    damage,
    cooldown,
    reloadTime,
    ammoClipSize,
    limitedUse,
    comboDmgMultiplier,
    finalComboDmgMultiplier
}