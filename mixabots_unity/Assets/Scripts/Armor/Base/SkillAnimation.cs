// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class SkillAnimation: ArmorAnimation
{
    public ArmorAnimation castAnimation;
    public ArmorAnimation followThroughAnimation;
    public float castTime;
    public float followThroughTime;
    public ArmorAttributeMultiplier[] armorAttributeMultipliers;
    public SkillEffectMultiplier[] skillEffectMultipliers;
}
