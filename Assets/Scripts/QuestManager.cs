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
		
	}
}
