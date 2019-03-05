﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

	public Text dialogText;
	public Text nameText;
	public GameObject dialogBox;
	public GameObject nameBox;

	public string[] dialogLines;

	public int currentLine;

	bool justStarted;

	public static DialogManager instance;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogBox.activeInHierarchy) {
			if (Input.GetButtonUp("Fire1")) {
				if (!justStarted) {
					currentLine++;

					if (currentLine >= dialogLines.Length) {
						dialogBox.SetActive(false);
						PlayerController.instance.canMove = true;
					} else {
						CheckIfName();
						dialogText.text = dialogLines[currentLine];
					}
				}

				justStarted = false;
			}
		}
	}

	public void ShowDialog(string[] newLines){
		dialogLines = newLines;
		currentLine = 0;

		CheckIfName();

		dialogText.text = dialogLines[currentLine];

		dialogBox.SetActive(true);
		justStarted = true;

		PlayerController.instance.canMove = false;
	}

	public void CheckIfName(){
		if (dialogLines[currentLine].StartsWith("n-")) {
			nameText.text = dialogLines[currentLine].Substring(2);
			currentLine++;
		}
	}
}
