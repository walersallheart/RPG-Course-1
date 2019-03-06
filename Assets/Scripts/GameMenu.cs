using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour {

	public GameObject theMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire2") || Input.GetKeyDown(KeyCode.M)) {
			if (theMenu.activeInHierarchy) {
				theMenu.SetActive(false);
				GameManager.instance.gameMenuOpen = false;
			} else {
				theMenu.SetActive(true);
				GameManager.instance.gameMenuOpen = true;
			}
		}
	}
}
