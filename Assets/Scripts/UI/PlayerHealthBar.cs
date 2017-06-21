using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{

    public Image Healthbar;
    Player player;
    void Start()
    {
        player = GameManager.instance._playerRef;
    }

    void Update()
    {
        float fillAmount = player.hitPoints / player.maxHitPoints;
        Healthbar.fillAmount = fillAmount;
    }


}
