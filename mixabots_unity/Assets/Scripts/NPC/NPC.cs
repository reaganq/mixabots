using UnityEngine;
using System;
using System.Collections;

public class NPC: MonoBehaviour
{
	public RPGNPC character;
    public int ID;
    public bool Active = false;
    public Shop thisShop = null;

    void Start()
    {
		character = Storage.LoadById<RPGNPC>(ID, new RPGNPC());
    }
    
    public void OnTriggerEnter ( Collider other )
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("ShowNPC");
        }
    }
    
    public void OnTriggerExit ( Collider other )
    {
        if(Active && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("HideNPC");
            
        }
    }
    
    public IEnumerator ShowNPC()
    {
        yield return new WaitForEndOfFrame();
        Active = true;
        Player.Instance.ActiveNPC = this;
        //Player.Instance.ActiveNPCName = character.Name;
        if(character.ShopID > 0)
        {
            if(thisShop == null)
            {   
                thisShop = Storage.LoadById<Shop>(character.ShopID, new Shop());
                
            }
            thisShop.PopulateItems();
            Player.Instance.ActiveShop = thisShop;
        }
        GUIManager.Instance.DisplayNPC();
    }
    
    public IEnumerator HideNPC()
    {
        yield return new WaitForEndOfFrame();
        Player.Instance.ActiveNPC = null;
        if(Player.Instance.ActiveShop != null && character.ShopID == Player.Instance.ActiveShop.ID)
        {
            Player.Instance.ActiveShop = null;
            
        }
        
        GUIManager.Instance.HideNPC();
        
        Active = false;
    }

}
