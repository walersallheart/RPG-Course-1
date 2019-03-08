using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public CharStats[] playerStats;

	public bool gameMenuOpen, dialogActive, fadingBetweenAreas;

	public string[] itemsHeld;
	public int[] numberOfItems;
	public Item[] referenceItems;
		
	// Use this for initialization
	void Start () {
		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameMenuOpen || dialogActive || fadingBetweenAreas) {
			PlayerController.instance.canMove = false;
		} else {
			PlayerController.instance.canMove = true;
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
}
