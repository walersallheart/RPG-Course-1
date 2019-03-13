﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			currentTurn = 0;
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
				
			} else {
				if (activeBattlers[i].isPlayer) {
					allPlayersDead = false;
				} else {
					allEnemiesDead = false;
				}
			}
		}

		if (allEnemiesDead || allPlayersDead) {
			if (allEnemiesDead) {
				//todo end battle in victory
			} else {
				//todo end battle in failure
			}

			battleScene.SetActive(false);
			GameManager.instance.battleActive = false;
			battleActive = false;
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

		//activeBattlers[selectedTarget].currentHP -= 30;

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

		Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " (" + damageToGive + ") damage to " + activeBattlers[target].charName);

		activeBattlers[target].currentHP -= damageToGive;
	}
}
