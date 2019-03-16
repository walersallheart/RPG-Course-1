using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {

	public static BattleManager instance;

	private bool battleActive;

	public GameObject battleScene;

	public Transform[] playerPositions, enemyPositions;

	public BattleChar[] playerPrefabs, enemyPrefabs;

	public List<BattleChar> activeBattlers = new List<BattleChar>();

	public int currentTurn;
	public bool turnWaiting;

	public GameObject uiButtonsHolder;

	public BattleMove[] movesList;
	public GameObject enemyAttackEffect;

	public DamageNumber theDamageNumber;

	public Text[] playerName, playerHP, playerMP;

	public GameObject targetMenu;
	public BattleTargetButton[] targetButtons;

	public GameObject magicMenu;
	public BattleMagicSelect[] magicButtons;

	public BattleNotification battleNotice;

	public int chanceToFlee = 35;
	private bool fleeing;

	public string gameOverScene;

	public int rewardXP;
	public string[] rewardItems;


	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) {
			BattleStart(new string[] {"Eyeball", "Spider", "Skeleton"});
		}

		if (battleActive) {
			if (turnWaiting) {
				if (activeBattlers[currentTurn].isPlayer) {
					uiButtonsHolder.SetActive(true);
				} else {
					uiButtonsHolder.SetActive(false);

					StartCoroutine(EnemyMoveCo());
				}
			}

			if (Input.GetKeyDown(KeyCode.N)) {
				NextTurn();
			}
		}
	}

	public void BattleStart(string[] enemiesToSpawn){
		if (!battleActive) {
			battleActive = true;

			GameManager.instance.battleActive = true;

			transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);

			battleScene.SetActive(true);

			AudioManager.instance.PlayBGM(0);

			for (int i = 0; i < playerPositions.Length; i++){
				if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy){
					for (int j = 0; j<playerPrefabs.Length; j++){
						if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName){
							BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
							newPlayer.transform.parent = playerPositions[i];
							activeBattlers.Add(newPlayer);

							CharStats thePlayer = GameManager.instance.playerStats[i];
							activeBattlers[i].currentHP = thePlayer.currentHP;
							activeBattlers[i].maxHP = thePlayer.maxHP;
							activeBattlers[i].currentMP = thePlayer.currentMP;
							activeBattlers[i].maxMP = thePlayer.maxMP;
							activeBattlers[i].strength = thePlayer.strength;
							activeBattlers[i].defense = thePlayer.defense;
							activeBattlers[i].wpnPower = thePlayer.wpnPwr;
							activeBattlers[i].armrPower = thePlayer.armrPwr;
						}
					}
				}
			}

			for (int i = 0; i<enemiesToSpawn.Length; i++){
				if (enemiesToSpawn[i] != "") {
					for (int j = 0; j<enemyPrefabs.Length; j++){
						if (enemyPrefabs[j].charName == enemiesToSpawn[i]) {
							BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
							newEnemy.transform.parent = enemyPositions[i];
							activeBattlers.Add(newEnemy);
						}
					}
				}
			}

			turnWaiting = true;
			currentTurn = Random.Range(0, activeBattlers.Count);

			UpdateUIStats();
		}
	}

	public void NextTurn(){
		currentTurn++;
		
		if (currentTurn >= activeBattlers.Count) {
			currentTurn = 0;
		}

		turnWaiting = true;

		UpdateBattle();
	}

	public void UpdateBattle(){
		bool allEnemiesDead = true;
		bool allPlayersDead = true;

		for (int i = 0; i<activeBattlers.Count; i++){
			if (activeBattlers[i].currentHP < 0) {
				activeBattlers[i].currentHP = 0;
			}

			if (activeBattlers[i].currentHP == 0) {
				if (activeBattlers[i].isPlayer) {
					activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
				} else {
					Debug.Log("Kill " + activeBattlers[i].gameObject.name);
					activeBattlers[i].EnemyFade();
				}
			} else {
				if (activeBattlers[i].isPlayer) {
					allPlayersDead = false;
					activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
				} else {
					allEnemiesDead = false;
				}
			}

			UpdateUIStats();
		}

		if (allEnemiesDead || allPlayersDead) {
			if (allEnemiesDead) {
				StartCoroutine(EndBattleCo());
			} else {
				StartCoroutine(GameOverCo());
			}
		} else {
			while (activeBattlers[currentTurn].currentHP == 0) {
				currentTurn++;

				if (currentTurn >= activeBattlers.Count) {
					currentTurn = 0;
				}
			}
		}
	}

	public IEnumerator EnemyMoveCo(){
		turnWaiting = false;
		yield return new WaitForSeconds(1f);
		EnemyAttack();
		yield return new WaitForSeconds(1f);
		NextTurn();
	}

	public void EnemyAttack(){
		List<int> players = new List<int>();

		for (int i = 0; i<activeBattlers.Count; i++){
			if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0) { //make a list of valid targets
				players.Add(i);
			}
		}

		int selectedTarget = players[Random.Range(0,players.Count)];

		int selectAttack = Random.Range(0,activeBattlers[currentTurn].movesAvailable.Length);
		int movePower = 0;

		for (int i = 0; i<movesList.Length; i++) {
			if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack]) {
				Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
				movePower = movesList[i].movePower;
			}
		}

		Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

		DealDamage(selectedTarget, movePower);
	}

	public void DealDamage(int target, int movePower){
		float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
		float defPwr = activeBattlers[target].defense + activeBattlers[target].armrPower;

		float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
		int damageToGive = Mathf.RoundToInt(damageCalc);

		//Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " (" + damageToGive + ") damage to " + activeBattlers[target].charName);

		activeBattlers[target].currentHP -= damageToGive;

		Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

		UpdateUIStats();
	}

	public void UpdateUIStats(){
		for (int i = 0; i<playerName.Length; i++){
			if (activeBattlers.Count > i) {
				if (activeBattlers[i].isPlayer) {
					BattleChar playerData = activeBattlers[i];

					playerData.currentHP = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue);
					playerData.currentMP = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue);

					playerName[i].gameObject.SetActive(true);
					playerName[i].text = playerData.charName;
					playerHP[i].text = playerData.currentHP.ToString("n0") + "/" + playerData.maxHP.ToString("n0");
					playerMP[i].text = playerData.currentMP.ToString("n0") + "/" + playerData.maxMP.ToString("n0");
				} else {
					playerName[i].gameObject.SetActive(false);
				}
			} else {
				playerName[i].gameObject.SetActive(false);
			}
		}
	}

	public void PlayerAttack(string moveName, int selectedTarget){
		int movePower = 0;

		for (int i = 0; i<movesList.Length; i++) {
			if (movesList[i].moveName == moveName) {
				Instantiate(movesList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
				movePower = movesList[i].movePower;
			}
		}

		Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

		DealDamage(selectedTarget, movePower);

		uiButtonsHolder.SetActive(false); //prevent double clicking button

		NextTurn();

		targetMenu.SetActive(false);
	}

	public void OpenTargetMenu(string moveName) {
		targetMenu.SetActive(true);

		List<int> Enemies = new List<int>();

		for (int i = 0; i<activeBattlers.Count; i++){
			if (!activeBattlers[i].isPlayer) {
				Enemies.Add(i);
			}
		}

		for (int i = 0; i<targetButtons.Length; i++){
			if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0) {
				targetButtons[i].gameObject.SetActive(true);
				targetButtons[i].moveName = moveName;
				targetButtons[i].activeBattlerTarget = Enemies[i];
				targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
			} else {
				targetButtons[i].gameObject.SetActive(false);
			}
		}
	}

	public void OpenMagicMenu(){
		magicMenu.SetActive(true);

		for (int i = 0; i<magicButtons.Length; i++){
			if (activeBattlers[currentTurn].movesAvailable.Length > i) {
				magicButtons[i].gameObject.SetActive(true);
				magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
				magicButtons[i].nameText.text = magicButtons[i].spellName;

				for (int j = 0; j<movesList.Length; j++){
					if (movesList[j].moveName == magicButtons[i].spellName) {
						magicButtons[i].spellCost = movesList[j].moveCost;
						magicButtons[i].costText.text = magicButtons[i].spellCost.ToString("n0");
					}
				}
			} else {
				magicButtons[i].gameObject.SetActive(false);
			}
		}
	}

	public void Flee(){
		int fleeSuccess = Random.Range(0,100);

		if (fleeSuccess < chanceToFlee){
			//end battle
			battleActive = false;
			battleScene.SetActive(false);

			fleeing = true;

			StartCoroutine(EndBattleCo());
		} else {
			NextTurn();
			battleNotice.theText.text = "Couldn't escape";
			battleNotice.Activate();
		}
	}

	public IEnumerator EndBattleCo(){
		battleActive = false;
		uiButtonsHolder.SetActive(false);
		targetMenu.SetActive(false);
		magicMenu.SetActive(false);

		AudioManager.instance.PlayBGM(5);

		yield return new WaitForSeconds(.5f);

		UIFade.instance.FadeToBlack();

		yield return new WaitForSeconds(1.5f);

		for (int i = 0; i<activeBattlers.Count; i++){
			if (activeBattlers[i].isPlayer) {
				for (int j = 0; j<GameManager.instance.playerStats.Length; j++){
					if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName) {
						GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
						GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
					}
				}
			}

			Destroy(activeBattlers[i].gameObject);
		}

		UIFade.instance.FadeFromBlack();
		battleScene.SetActive(false);
		activeBattlers.Clear();
		currentTurn = 0;
		
		AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay); //restart level music
		
		if (fleeing) {
			GameManager.instance.battleActive = false;
			fleeing = false;
		} else {
			BattleRewards.instance.OpenRewardSceen(rewardXP, rewardItems);
		}
	}

	public IEnumerator GameOverCo(){
		battleActive = false;

		UIFade.instance.FadeToBlack();

		yield return new WaitForSeconds(1.5f);

		SceneManager.LoadScene(gameOverScene);

		battleScene.SetActive(false);
	}
}
