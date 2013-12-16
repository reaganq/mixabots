using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorOverrideAnimationController : MonoBehaviour {

    public ArmorAnimation idleOverrideAnim;
    public ArmorAnimation walkingOverrideAnim;
    public ArmorAnimation runningOverrideAnim;

    public Animation animationTarget;
    public Avatar avatar;

    public void TransferAnimations(Animation target, Avatar character)
    {
        animationTarget = target;
        avatar = character;

        if(runningOverrideAnim != null)
        {
            animationTarget.AddClip(runningOverrideAnim.clip, runningOverrideAnim.clip.name);
            animationTarget[runningOverrideAnim.clip.name].layer = runningOverrideAnim.animationLayer;
            StartCoroutine(MixingTransforms( runningOverrideAnim.addMixingTransforms, runningOverrideAnim.removeMixingTransforms, runningOverrideAnim.clip));
        }

    }

    public IEnumerator MixingTransforms(List<string> bonelist, List<string> removelist, AnimationClip clip)
    {
        yield return null;
        
        if(bonelist.Count>0)
        {
            foreach(string bonename in bonelist)
            {
                if(bonename.IndexOf("Recursive") != -1)
                {
                    string newname = bonename.Replace("Recursive", string.Empty);
                    //Debug.Log(newname);
                    animationTarget[clip.name].AddMixingTransform(GetBone(newname), true);
                }
                else
                {
                    animationTarget[clip.name].AddMixingTransform(GetBone(bonename), false);
                    //Debug.Log("added mixing transform");
                    //yield return null;
                }
            }
        }
        
        if(removelist.Count>0)
        {
            foreach(string bonename in removelist)
            {
                animationTarget[clip.name].RemoveMixingTransform(GetBone(bonename));
                //Debug.Log("remove mixing transform: "+bonename);
                //yield return null;
            }
        }
        
    }

    public Transform GetBone(string bonename)
    {
        Transform[] kids = avatar.transform.GetComponentsInChildren<Transform>();
        //Debug.Log(kids.Length);
        for (int i = 0; i < kids.Length; i++) {
            if(kids[i].name == bonename)
            {
                Debug.Log(kids[i].name);
                return kids[i];
            }
        }
        return null;
    }
}
