using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour {

	public GameObject objectToActivate;

	public string questToCheck;

	public bool activeIfComplete;

	bool initialCheckDone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialCheckDone) { //this is in update so the Start() function in the GameManager has time to run
			initialCheckDone = true;
			CheckCompletion();
		}
	}

	public void CheckCompletion(){
		if (QuestManager.instance.CheckIfComplete(questToCheck)) {
			objectToActivate.SetActive(activeIfComplete);
		}
	}
}
