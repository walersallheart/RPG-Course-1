﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public static Shop instance;
	public GameObject shopMenu;
	public GameObject buyMenu;
	public GameObject sellMenu;
	public Text goldText;
	public string[] itemsForSale;

	public ItemButton[] buyItemButtons;
	public ItemButton[] sellItemButtons;

	public Item selectedItem;
	public Text buyItemName, buyItemDescription, buyItemValue;
	public Text sellItemName, sellItemDescription, sellItemValue;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy){
			OpenShop();
		}
	}

	public void OpenShop(){
		shopMenu.SetActive(true);
		GameManager.instance.shopActive = true;
		goldText.text = GameManager.instance.currentGold.ToString("n0") + "g";
		OpenBuyMenu();
		
	}

	public void CloseShop(){
		shopMenu.SetActive(false);
		GameManager.instance.shopActive = false;
	}

	public void OpenBuyMenu(){
		buyMenu.SetActive(true);
		sellMenu.SetActive(false);

		buyItemButtons[0].Press(); //select the first item by default

		for (int i = 0; i<buyItemButtons.Length; i++){
			buyItemButtons[i].buttonValue = i;

			if(itemsForSale[i] != "") {
				buyItemButtons[i].buttonImage.gameObject.SetActive(true);
				buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
				buyItemButtons[i].amountText.text = "";
			} else {
				buyItemButtons[i].buttonImage.gameObject.SetActive(false);
				buyItemButtons[i].amountText.text = "";
			}
		}
	}

	public void OpenSellMenu(){
		buyMenu.SetActive(false);
		sellMenu.SetActive(true);

		GameManager.instance.SortItems();

		sellItemButtons[0].Press(); //select the first item by default

		for (int i = 0; i<sellItemButtons.Length; i++){
			sellItemButtons[i].buttonValue = i;

			if(GameManager.instance.itemsHeld[i] != "") {
				sellItemButtons[i].buttonImage.gameObject.SetActive(true);
				sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
				sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString("n0");
			} else {
				sellItemButtons[i].buttonImage.gameObject.SetActive(false);
				sellItemButtons[i].amountText.text = "";
			}
		}
	}

	public void SelectBuyItem(Item buyItem){
		selectedItem = buyItem;
		buyItemName.text = selectedItem.itemName;
		buyItemDescription.text = selectedItem.description;
		buyItemValue.text = "Value: " + selectedItem.value.ToString("n0") + "g";
	}

	public void SelectSellItem(Item sellItem){
		selectedItem = sellItem;
		sellItemName.text = selectedItem.itemName;
		sellItemDescription.text = selectedItem.description;
		sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString("n0") + "g";
	}
}
