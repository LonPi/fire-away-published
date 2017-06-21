using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    Text timerText;
    float time;

	void Start () {
        timerText = GetComponent<Text>();
        time = 0f;
	}
	
	void Update () {
        time += Time.deltaTime;
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);

        //update the label value
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
