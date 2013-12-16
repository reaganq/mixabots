using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : MonoBehaviour {

	// Use this for initialization
    public CharacterMotor animationController;
    public Animation animationTarget;
    public CharacterAnimationManager animManager;
    
    public Transform PelvisBone;
    public Transform BodyRoot;
    public Transform Spine2Bone;
    public Transform ArmLRoot;
    public Transform ArmRRoot;
    public Transform HeadRoot;
    public Transform LegsRoot;
    
    public List<GameObject> HeadObjects;
    public List<GameObject> BodyObjects;
    public List<GameObject> ArmLObjects;
    public List<GameObject> ArmRObjects;
    public List<GameObject> LegObjects;

    private string LShoulderRootName = "bones:L_Shoulder";
    private string RShoulderRootName = "bones:R_Shoulder";
    private string LegsRootName = "bones:LegsRoot";
    private Transform _myTransform;

	void Awake () {
        animationController = GetComponent<CharacterMotor>();
        animManager = GetComponent<CharacterAnimationManager>();
        animationTarget = PelvisBone.GetComponent<Animation>();
	}
    
    void Start()
    {
        _myTransform = transform;

    }
    
    void Update()
    {
    }

    public void EquipBodyPart(string objectpath, EquipmentSlots slot)
    {
        switch(slot)
        {
            case EquipmentSlots.Head:
            SpawnHead(objectpath);
            Debug.Log("changing head");
                break;
            case EquipmentSlots.Body:
            SpawnBody(objectpath);
            Debug.Log("changing body");
                break;
            case EquipmentSlots.ArmL:
            SpawnArmL(objectpath);
            Debug.Log("changing armL");
                break;
            case EquipmentSlots.ArmR:
            SpawnArmR(objectpath);
            Debug.Log("changing armr");
                break;
            case EquipmentSlots.Legs:
            SpawnLegs(objectpath);
            Debug.Log("changing legs");
            break;
        }
    }

    public void SpawnHead(string objectpath)
    {
        if(HeadObjects.Count > 0)
        {
            for (int i = 0; i < HeadObjects.Count; i++) {
                Destroy(HeadObjects[i]); 
            }
            HeadObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = HeadRoot;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.Euler(-90, 90, 0);
        HeadObjects.Add(temp);
    }
   
#region SpawnBody
    public void SpawnBody(string objectpath)
    {        
        if(BodyObjects.Count > 0)
        {
            for (int i = 0; i < BodyObjects.Count; i++) {
                Destroy(BodyObjects[i]);
            }
            BodyObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = BodyRoot;
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localPosition = Vector3.zero;
        

        SkinnedMeshRenderer[] BonedObjects = temp.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach( SkinnedMeshRenderer smr in BonedObjects )
            ProcessBonedObject( smr );
        
        Destroy(temp);
        
    }
    
    private void ProcessBonedObject( SkinnedMeshRenderer ThisRenderer )
    {       
     // Create the SubObject
        GameObject bodyMesh = new GameObject( ThisRenderer.gameObject.name );    
        bodyMesh.transform.parent = transform;
        bodyMesh.transform.localRotation = Quaternion.identity;
        bodyMesh.transform.localPosition = Vector3.zero;
        
        // Add the renderer
        SkinnedMeshRenderer NewRenderer = bodyMesh.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
        
        // Assemble Bone Structure  
        Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];
        
        Debug.Log("bone number" + ThisRenderer.bones.Length);
    
        /*foreach(Transform bone in ThisRenderer.bones)
    {
        Debug.Log(bone.ToString());
        
    }*/
        // As clips are using bones by their names, we find them that way.
        for( int i = 0; i < ThisRenderer.bones.Length; i++ )
        {
            Debug.Log(ThisRenderer.bones[i].name);
            MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
        }
    //
        NewRenderer.bones = MyBones;    
        NewRenderer.sharedMesh = ThisRenderer.sharedMesh;   
        NewRenderer.materials = ThisRenderer.materials; 
        BodyObjects.Add(bodyMesh);
    }

    // Recursive search of the child by name.
    private Transform FindChildByName( string ThisName, Transform ThisGObj )    
    {   
        Transform ReturnObj;
       
        // If the name match, we're return it
        if( ThisGObj.name == ThisName ) 
            return ThisGObj.transform;
        
        // Else, we go continue the search horizontaly and verticaly
        foreach( Transform child in ThisGObj )  
        {   
            ReturnObj = FindChildByName( ThisName, child );
        
            if( ReturnObj != null ) 
                return ReturnObj;   
        }
        
        return null;    
    }
#endregion
    
    public void SpawnArmL(string objectpath)
    {
        if(ArmLObjects.Count > 0)
        {
            for (int i = 0; i < ArmLObjects.Count; i++) {
                Destroy(ArmLObjects[i]);
            }
            ArmLObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
        temp.transform.parent = ArmLRoot;
        for (int i = 0; i < temp.transform.childCount; i++) {
            if(temp.transform.GetChild(i).name == LShoulderRootName)
            {
                ArmLObjects.Add(temp.transform.GetChild(i).gameObject);
                PositionArmL(temp.transform.GetChild(i));
            }
        }
        ArmorOverrideAnimationController armLcontroller = temp.GetComponent<ArmorOverrideAnimationController>();
        if(armLcontroller != null)
        {
            armLcontroller.TransferAnimations(animationTarget, this);


            animManager.AddArmorcontroller(armLcontroller,2);
            Debug.Log("transfer animation");
        }
        ArmLObjects.Add(temp);
        //ArmorController armLcontroller = ArmL.GetComponent<ArmorController>();

        /*if(armLcontroller != null)
        {
            armLcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[2] = armLcontroller;
        }*/
        
        //animationController.UpdateAnimation();
    }
    
    public void PositionArmL(Transform obj)
    {
        obj.parent = ArmLRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmLRoot.parent;
    }
    
    public void SpawnArmR(string objectpath)
    {
        if(ArmRObjects.Count > 0)
        {
            for (int i = 0; i < ArmRObjects.Count; i++) {
                Destroy(ArmRObjects[i]);
            }
            ArmRObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
        temp.transform.parent = ArmRRoot;
        for (int i = 0; i < temp.transform.childCount; i++) {
            if(temp.transform.GetChild(i).name == RShoulderRootName)
            {
                ArmRObjects.Add(temp.transform.GetChild(i).gameObject);
                PositionArmR(temp.transform.GetChild(i));
            }
        }
        
        ArmRObjects.Add(temp);
        /*if(armRcontroller != null)
        {
            armRcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[3] = armRcontroller;
            
        }*/

        //animationController.UpdateAnimation();
    }
    
    public void PositionArmR(Transform obj)
    {
        obj.parent = ArmRRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmRRoot.parent;
    }
    
    public void SpawnLegs(string objectpath)
    {
        if(LegObjects.Count > 0)
        {
            for (int i = 0; i < LegObjects.Count ; i++) {
                Destroy(LegObjects[i]);
            }
            LegObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = LegsRoot;
        
        for (int i = 0; i < temp.transform.childCount ; i++) {
            if(temp.transform.GetChild(i).name == LegsRootName)
            {
                LegObjects.Add(temp.transform.GetChild(i).gameObject);
                PositionLegs(temp.transform.GetChild(i));
            }
        }
        LegObjects.Add(temp);
        
        //PositionLegs(boneObj);
        //Destroy(temp);
        //animationController.UpdateAnimation();
    }
    
    public void PositionLegs(Transform obj)
    {
        obj.parent = LegsRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = LegsRoot.parent;
    }
}
