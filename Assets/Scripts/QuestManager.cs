using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public string[] questMarkerNames;
	public bool[] questMarkersComplete;

	public static QuestManager instance;

	// Use this for initialization
	void Start () {
		instance = this;

		questMarkersComplete = new bool[questMarkerNames.Length];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log(CheckIfComplete("quest test"));
		}
	}

	public int GetQuestNumber(string questToFind) {
		for (int i = 0; i<questMarkerNames.Length; i++){
			if (questMarkerNames[i] == questToFind) {
				return i;
			}
		}

		Debug.LogError("Quest " + questToFind + " doest not exist");
		return 0;
	}

	public bool CheckIfComplete(string questToCheck) {
		if (GetQuestNumber(questToCheck) != 0) {
			return questMarkersComplete[GetQuestNumber(questToCheck)];
		}

		return false;
	}

	public void MarkQuestComplete(string questToMark){
		questMarkersComplete[GetQuestNumber(questToMark)] = true;
	}

	public void MarkQuestIncomplete(string questToMark) {
		questMarkersComplete[GetQuestNumber(questToMark)] = false;
	}
}
