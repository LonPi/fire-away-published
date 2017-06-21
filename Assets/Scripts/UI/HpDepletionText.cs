using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpDepletionText : MonoBehaviour {

    Text hpDepletionText;
    Player _playerRef;
    Tree _treeRef;

    void Start()
    {
        hpDepletionText = GetComponent<Text>();
        if (gameObject.transform.parent.name == "PlayerHealthbar")
            _playerRef = GameManager.instance._playerRef;
        if (gameObject.transform.parent.name == "TreeHealthbar")
            _treeRef = GameManager.instance._treeRef;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerRef != null)
        {
            hpDepletionText.text = ((int)_playerRef.hitPoints).ToString() + "/" + ((int)_playerRef.maxHitPoints).ToString();
        }

        if (_treeRef != null)
        {
            hpDepletionText.text = ((int)_treeRef.hitPoints).ToString() + "/" + ((int)_treeRef.maxHitPoints).ToString();
        }
    }
}
