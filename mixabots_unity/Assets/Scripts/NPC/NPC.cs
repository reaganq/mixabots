using UnityEngine;
using System;
using System.Collections;

public class NPC
{
	public RPGNPC character;
    public int ID;

    void Start()
    {
		character = Storage.LoadById<RPGNPC>(ID, new RPGNPC());
    }

}
