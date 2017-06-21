using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayCanvas : MonoBehaviour {

    OnScreenMessage tryAgainText;
    OnScreenMessage levelUpText;
    Text highScoreText;
    GameObject highScoreScreen;
    GameObject timer;

    void Start ()
    {
        tryAgainText = GameObject.Find("TryAgain").GetComponentInChildren<OnScreenMessage>();
        levelUpText = GameObject.Find("LevelUp").GetComponentInChildren<OnScreenMessage>();
        highScoreScreen = GameObject.Find("ScoreScreen");
        timer = transform.Find("Timer").gameObject;
        highScoreScreen.SetActive(false);
	}

    public void OnDisplayHighScore(string name)
    {
        // disable timer
        timer.SetActive(false);
        StartCoroutine(_OnDisplayHighScore(name));
    }

    public void OnLevelUp()
    {
        levelUpText.ShowText();
    }

    IEnumerator _OnDisplayHighScore(string name)
    {
        tryAgainText.ShowText();
        yield return new WaitForSeconds(tryAgainText.lifeSpan);
        highScoreText = highScoreScreen.GetComponentInChildren<Text>();
        highScoreText.text =
            (name == "Player" ? "You got killed by slimes...\n" : "The Spirit Tree is dead...\n") +
            "Max kills: " + GameManager.instance.highScore.maxKills.ToString() + "\n" +
            "Max level: " + GameManager.instance.highScore.maxLevel.ToString() + "\n";
        highScoreScreen.SetActive(true);
    }

    public void PressRestartLevel()
    {
        GameManager.instance.ReloadLevel();
    }
}
