using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour {

    Text killCountText;
    Player _playerRef;

	void Start ()
    {
        killCountText = GetComponent<Text>();
        _playerRef = GameManager.instance._playerRef;
	}
	
	void Update ()
    {
        killCountText.text = "Kill Count: " + _playerRef.killCount;
	}
}
