using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ArmorAnimation{

    public AnimationClip clip;
    public int animationLayer;
    public List<string> addMixingTransforms;
    public List<string> weightedAddMixingTransforms;
    public List<string> removeMixingTransforms;
    public List<string> weightedRemoveMixingTransforms;


}
