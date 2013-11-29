using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : MonoBehaviour {

	// Use this for initialization
    public CharacterMotor animationController;
    public Animation animationTarget;
    
    public Transform HipBone;
    public Transform BodyRoot;
    public Transform Spine2Bone;
    public Transform ArmLRoot;
    public Transform ArmRRoot;
    public Transform HeadRoot;
    
    public GameObject Head;
    public GameObject Body;
    public GameObject ArmL;
    public GameObject ArmR;
    public List<GameObject> Legs;
    
    
	void Awake () {
	    //HipBone = transform.FindChild("Hips");
        animationController = GetComponent<CharacterMotor>();
        animationTarget = HipBone.GetComponent<Animation>();
	}
    
    void Update()
        
    {
    }
	
    public void SpawnHead(string objectpath)
    {
        if(Head != null)
            Destroy(Head);
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        Head = temp.transform.Find("Head").gameObject;
        PositionHead();
        
        Destroy(temp);
    }
    
    public void PositionHead()
    {
        Head.transform.parent = HeadRoot;
        Head.transform.localPosition = Vector3.zero;
        Head.transform.localRotation = Quaternion.identity;
        Head.transform.parent = HeadRoot.parent;
    }
    
    public void SpawnBody(string objectpath)
    {
        if(ArmL != null)
            ArmL.transform.parent = HipBone;
        if(ArmR != null)
            ArmR.transform.parent = HipBone;
        if(Head != null)
            Head.transform.parent = HipBone;
        
        if(Body != null)
            Destroy(Body);
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        //temp.transform.localRotation = Quaternion.identity;
        temp.transform.parent = BodyRoot;
        //Debug.Log("here");
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.identity;
        
        Body = temp.transform.Find("Spine").gameObject;
        Body.transform.parent = HipBone;
        
        Destroy(temp);
        
        UpdateBones();
        
        if(ArmL != null )
            PositionArmL();
        if(ArmR != null)
            PositionArmR();
        if(Head != null)
            PositionHead();
        
        animationController.UpdateUpperBodyMixingTransforms();
        animationController.UpdateAnimation();
    }
    
    public void SpawnArmL(string objectpath)
    {
        if(ArmL != null)
            Destroy(ArmL);
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        ArmL = temp.transform.Find("LeftArm").gameObject;
        ArmorController armLcontroller = ArmL.GetComponent<ArmorController>();
        
        PositionArmL();
        
        if(armLcontroller != null)
        {
            armLcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[2] = armLcontroller;
        }
        
        Destroy(temp);
        
        animationController.UpdateAnimation();
    }
    
    public void PositionArmL()
    {
        ArmL.transform.parent = ArmLRoot;
        ArmL.transform.localPosition = Vector3.zero;
        ArmL.transform.localRotation = Quaternion.identity;
        ArmL.transform.parent = ArmLRoot.parent;
    }
    
    public void SpawnArmR(string objectpath)
    {
        if(ArmR != null)
        {
            Destroy(ArmR);
            if(animationController.ArmorControllers[3] != null && animationController.ArmorControllers[3].hasIdleAnimationOverride)
            {
                animationTarget.Stop(animationController.ArmorControllers[3].IdleOverrideAnimationClip.name);
                animationController.ArmorControllers[3] = null;
                Debug.Log("taking out armorcontroller");
            }
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        ArmR = temp.transform.Find("RightArm").gameObject;
        ArmorController armRcontroller = ArmR.GetComponent<ArmorController>();
        PositionArmR();
        
        if(armRcontroller != null)
        {
            armRcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[3] = armRcontroller;
            
        }

        Destroy(temp);
        animationController.UpdateAnimation();
    }
    
    public void PositionArmR()
    {
        ArmR.transform.parent = ArmRRoot;
        ArmR.transform.localPosition = Vector3.zero;
        ArmR.transform.localRotation = Quaternion.identity;
        
        ArmR.transform.parent = ArmRRoot.parent;
    }
    
    public void SpawnLegs(string objectpath)
    {
        if(Legs.Count > 0)
        {
            foreach(GameObject leg in Legs)
            {
                Destroy(leg);
                
            }
            Legs.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        //Debug.Log("got here");
        temp.transform.parent = HipBone;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.identity;
        //int index = 0;
        
        //Debug.Log(temp.transform.childCount);
        for (int i = 0; i < temp.transform.childCount ; i++) {
            //index ++;
            //Debug.Log(index);
            //Debug.Log(temp.transform.GetChild(i).name);
            //temp.transform.GetChild(i).parent = HipBone;
            //child.parent = HipBone;
            Legs.Add(temp.transform.GetChild(i).gameObject);
        }
        
        foreach(GameObject obj in Legs)
        {
            obj.transform.parent = HipBone;
        }
        
        Destroy(temp);
        animationController.UpdateAnimation();
        
    }
    
    public void UpdateBones()
    {
        if(Body != null)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform item in allChildren) {
                if(item.name == "armLRoot")
                    ArmLRoot = item;
                if(item.name == "armRRoot")
                    ArmRRoot = item;
                if(item.name == "headRoot")
                    HeadRoot = item;
                if(item.name == "Spine2")
                    Spine2Bone = item;
            }
        }
    }
    
    public void AnalyseNewPart()
    {
    }
}
