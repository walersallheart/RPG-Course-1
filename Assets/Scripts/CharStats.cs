using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour {

	public string charName;
	public string charClass;
	public int playerLevel=1;
	public int currentEXP = 0;

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
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
