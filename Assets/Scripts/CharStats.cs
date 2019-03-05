﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

	public string charName;
	public string charClass;
	public int playerLevel=1;
	public int currentEXP = 0;
	public int[] expToNextLevel;
	public int maxLevel = 100;
	public int baseEXP = 1000;

	public int currentHP;
	public int maxHP=100;
	public int currentMP;
	public int maxMP=30;
	public int strength;
	public int defense;
	public int wpnPwr;
	public int armrPwr;
	public string equippedWpn;
	public string equippedArmr;
	public Sprite charImage;

	// Use this for initialization
	void Start () {
		expToNextLevel = new int[maxLevel];

		expToNextLevel[1] = baseEXP;

		for (int i = 2; i<expToNextLevel.Length; i++){
			expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
