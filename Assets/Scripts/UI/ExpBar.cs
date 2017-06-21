using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour {

    public Image PlayerExpBar;
    
    float fillAmount;

	void Start () {
        PlayerExpBar.fillAmount = 0f;
	}
	
	void Update () {
        float fillAmount = GameManager.instance.currentExp / GameManager.instance.expRequiredToCompleteLevel;
        PlayerExpBar.fillAmount = fillAmount;
	}
}
