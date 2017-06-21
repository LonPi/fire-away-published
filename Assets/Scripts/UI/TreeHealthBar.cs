using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeHealthBar : MonoBehaviour
{

    public Image Healthbar;
    Tree tree;
    void Start()
    {
        tree = GameManager.instance._treeRef;
    }

    void Update()
    {
        float fillAmount = tree.hitPoints / tree.maxHitPoints;
        Healthbar.fillAmount = fillAmount;
    }


}
