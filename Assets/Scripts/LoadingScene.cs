using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {

	public float waitToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (waitToLoad > 0f) {
			waitToLoad -= Time.deltaTime;

			if (waitToLoad <= 0f){
				SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
				GameManager.instance.LoadData();
				QuestManager.instance.LoadQuestData();
			}
		}
	}
}
