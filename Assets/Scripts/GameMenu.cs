using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public GameObject theMenu;

	private CharStats[] playerStats;

	public Text[] nameText, hpText, mpText, lvlText, expText;
	public Slider[] expSlider;
	public Image[] charImage;
	public GameObject[] charStatHolder;

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
				UpdateMainStats();
				GameManager.instance.gameMenuOpen = true;
			}
		}
	}

	public void UpdateMainStats(){
		playerStats = GameManager.instance.playerStats;

		for(int i = 0; i<playerStats.Length; i++){
			if (playerStats[i].gameObject.activeInHierarchy) {
				charStatHolder[i].SetActive(true);
			} else {
				charStatHolder[i].SetActive(false);
			}
		}
	}
}
