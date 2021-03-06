using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewards : MonoBehaviour {

	public static BattleRewards instance;

	public Text xpText, itemText;
	public GameObject rewardScreen;
	public string[] rewardItems;
	public int xpEarned;

	public bool markQuestComplete;
	public string questToMark;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Y)){
			OpenRewardSceen(54, new string[] {"Iron Sword", "Iron Armor"});
		}
	}

	public void OpenRewardSceen(int xp, string[] rewards){
		AudioManager.instance.PlayBGM(6);

		xpEarned = xp;
		rewardItems = rewards;

		xpText.text = "Everyone earned " + xpEarned.ToString("n0") + "xp!";
		itemText.text = "";

		for (int i = 0; i<rewardItems.Length; i++){
			itemText.text += rewardItems[i] + "\n";
		}

		rewardScreen.SetActive(true);
	}

	public void CloseRewardScreen(){
		for (int i = 0; i<GameManager.instance.playerStats.Length; i++){
			if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy) {
				GameManager.instance.playerStats[i].AddExp(xpEarned);
			}
		}

		for (int i = 0; i<rewardItems.Length; i++){
			GameManager.instance.AddItem(rewardItems[i]);
		}

		rewardScreen.SetActive(false);
		GameManager.instance.battleActive = false;

		if (markQuestComplete) {
			QuestManager.instance.MarkQuestComplete(questToMark);
		}
	}
}
