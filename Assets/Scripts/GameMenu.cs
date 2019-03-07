using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public GameObject theMenu;
	public GameObject[] windows;

	private CharStats[] playerStats;

	public Text[] nameText, hpText, mpText, lvlText, expText;
	public Slider[] expSlider;
	public Image[] charImage;
	public GameObject[] charStatHolder;
	public GameObject[] statusButtons;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire2") || Input.GetKeyDown(KeyCode.M)) {
			if (theMenu.activeInHierarchy) {
				CloseMenu();
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

				nameText[i].text = playerStats[i].charName;
				hpText[i].text = "HP:" + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
				mpText[i].text = "MP:" + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
				lvlText[i].text = "LVL:" + playerStats[i].playerLevel;
				expText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
				expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
				expSlider[i].value = playerStats[i].currentEXP;
				charImage[i].sprite = playerStats[i].charImage;
				
			} else {
				charStatHolder[i].SetActive(false);
			}
		}
	}

	public void ToggleWindows(int windowNumber) {
		UpdateMainStats();

		for (int i=0; i<windows.Length; i++){
			if (i == windowNumber) {
				windows[i].SetActive(!windows[i].activeInHierarchy);
			} else {
				windows[i].SetActive(false);
			}
		}
	}
	
	public void CloseMenu(){
		for (int i=0; i<windows.Length; i++){
			windows[i].SetActive(false);
		}

		theMenu.SetActive(false);

		GameManager.instance.gameMenuOpen = false;
	}

	public void OpenStatus(){
		UpdateMainStats();

		for (int i = 0; i<statusButtons.Length; i++){
			statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
			statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
		}
	}
}
