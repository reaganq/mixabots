﻿using UnityEngine;
using System.Collections;

public class NPCGUIController : MonoBehaviour {
 
    public UILabel textLabel = null;
    public GameObject enterShopButton = null;
    public GameObject confirmButton = null;
    
	// Use this for initialization
	// Update is called once per frame
	public void Enable()
    {
        textLabel.text = Player.Instance.ActiveNPC.character.Name;
        if(Player.Instance.ActiveShop != null)
        {
            enterShopButton.SetActive(true);
        }
        else
        {
            enterShopButton.SetActive(false);
        }
        confirmButton.SetActive(true);
    }
    
    public void OnConfirmButtonPressed()
    {
        GUIManager.Instance.HideNPC();
    }
    
    public void OnEnterShopButton()
    {
        GUIManager.Instance.DisplayShop();
    }
}
