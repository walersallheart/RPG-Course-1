﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public CharStats[] playerStats;
	public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;
	public string[] itemsHeld;
	public int[] numberOfItems;
	public Item[] referenceItems;
	public int currentGold;		
	// Use this for initialization
	void Start () {
		instance = this;

		DontDestroyOnLoad(gameObject);

		SortItems();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive) {
			PlayerController.instance.canMove = false;
		} else {
			PlayerController.instance.canMove = true;
		}

		if (Input.GetKeyDown(KeyCode.J)) {
			AddItem("Iron Armor");
			RemoveItem("Health Potion");
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			SaveData();
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			LoadData();
		}
	}

	public Item GetItemDetails(string itemToGrab){
		for (int i = 0; i<referenceItems.Length; i++){
			if (referenceItems[i].itemName == itemToGrab) {
				return referenceItems[i];
			}
		}

		return null;
	}

	public void SortItems(){
		bool itemAfterSpace = true;

		while(itemAfterSpace) {
			itemAfterSpace = false;

			for (int i = 0; i<itemsHeld.Length-1; i++){
				if (itemsHeld[i] == "") { //if this space is empty, move the object from the next spot into it
					itemsHeld[i] = itemsHeld[i + 1];
					itemsHeld[i + 1] = "";

					numberOfItems[i] = numberOfItems[i + 1];
					numberOfItems[i + 1] = 0;

					if (itemsHeld[i] !="") { //did we moved an item?
						itemAfterSpace = true;
					}
				}
			}
		}
	}

	public void AddItem(string itemToAdd) {
		int newItemPosition = 0;
		bool foundSpace = false;

		for (int i = 0; i<itemsHeld.Length; i++){
			if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd) {
				newItemPosition = i;
				i = itemsHeld.Length; //weird

				foundSpace = true;
			}
		}

		if (foundSpace) {
			bool itemExists = false;

			for (int i = 0; i<referenceItems.Length; i++){
				if (referenceItems[i].itemName == itemToAdd) {
					itemExists = true;
					i = referenceItems.Length;
				}
			}

			if (itemExists) {
				itemsHeld[newItemPosition] = itemToAdd;
				numberOfItems[newItemPosition]++;
			} else {
				Debug.LogError(itemToAdd + " Does Not Exist");
			}
		}

		GameMenu.instance.ShowItems();
	}

	public void RemoveItem(string itemToRemove){
		bool foundItem = false;
		int itemPosition = 0;

		for (int i = 0; i<itemsHeld.Length; i++){
			if (itemsHeld[i] == itemToRemove) {
				foundItem = true;
				itemPosition = i;

				i = itemsHeld.Length;
			}
		}

		if (foundItem) {
			numberOfItems[itemPosition]--;

			if (numberOfItems[itemPosition] <= 0) {
				itemsHeld[itemPosition] = "";
			}
		} else {
			Debug.LogError("Couldn't find " + itemToRemove);
		}

		GameMenu.instance.ShowItems();
	}

	public void SaveData(){
		PlayerPrefs.SetString("Current_Scene",SceneManager.GetActiveScene().name);
		PlayerPrefs.SetFloat("Player_Position_x",PlayerController.instance.transform.position.x);
		PlayerPrefs.SetFloat("Player_Position_y",PlayerController.instance.transform.position.y);
		PlayerPrefs.SetFloat("Player_Position_z",PlayerController.instance.transform.position.z);

		//save char info
		for (int i = 0; i<playerStats.Length; i++){
			if (playerStats[i].gameObject.activeInHierarchy) {
				PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active",1);
			} else {
				PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active",0);
			}

			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defense", playerStats[i].defense);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwn", playerStats[i].wpnPwr);
			PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
			PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
			PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);
		}

		//store inventory data
		for (int i = 0; i<itemsHeld.Length; i++){
			PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
			PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
		}
	}

	public void LoadData(){
		float x = PlayerPrefs.GetFloat("Player_Position_x");
		float y = PlayerPrefs.GetFloat("Player_Position_y");
		float z = PlayerPrefs.GetFloat("Player_Position_z");

		PlayerController.instance.transform.position = new Vector3(x, y, z);

		for (int i = 0; i<playerStats.Length; i++){
			if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0) {
				playerStats[i].gameObject.SetActive(false);
			} else {
				playerStats[i].gameObject.SetActive(true);
			}

			playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
			playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
			playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
			playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
			playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
			playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
			playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
			playerStats[i].defense = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defense");
			playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwn");
			playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
			playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
			playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
		}

		for (int i = 0; i<itemsHeld.Length; i++){
			itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
			numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
		}
	}
}
