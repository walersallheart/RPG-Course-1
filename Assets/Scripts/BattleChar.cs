using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour {

	public bool isPlayer;
	public string[] movesAvailable;

	public string charName;

	public int currentHP, maxHP, currentMP, maxMP, strength, defense, wpnPower, armrPower;
	public bool hasDied;

	public SpriteRenderer theSprite;
	public Sprite deadSprite,aliveSprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
