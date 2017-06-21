using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressionText : MonoBehaviour
{

    Text levelProgressionText;


    void Start()
    {
        levelProgressionText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        levelProgressionText.text = GameManager.instance.currentExp.ToString() + "/" + GameManager.instance.expRequiredToCompleteLevel.ToString();
    }
}