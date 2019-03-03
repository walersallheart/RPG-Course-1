using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIalogManager : MonoBehaviour {

	public Text dialogText;
	public Text nameText;
	public GameObject dialogBox;
	public GameObject nameBox;

	public string[] dialogLines;

	public int currentLine;

	// Use this for initialization
	void Start () {
		dialogText.text = dialogLines[0];
		nameText.text = "Gandalf";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
