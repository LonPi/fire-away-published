using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour {

    Text levelText;
    
	void Start () {
        levelText = GetComponent<Text>();
	}
	
	void Update () {
        levelText.text = "Level " + GameManager.instance.currentLevel.ToString();	
	}
}
