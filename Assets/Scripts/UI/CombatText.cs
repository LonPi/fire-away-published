using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

    public float moveSpeed;
    public float lifeSpan;
    Text text;
    string fillText;
    Enemy parent;
    float aliveTimer;
    RectTransform rect;

    void Start () {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        parent = GetComponentInParent<Enemy>();
	}
	
    public void Activate(string _fillText)
    {
        text.text = _fillText;
        text.enabled = true;
        aliveTimer = 0f;
    }

	void Update () {
        aliveTimer += Time.deltaTime;

        if (aliveTimer >= lifeSpan)
        {
            text.enabled = false;
        }
        Flip();
	}

    void Flip()
    {
        
        if (parent.transform.localScale.x < 0 && rect.localScale.x > 0 || parent.transform.localScale.x > 0 && rect.localScale.x < 0)
        {
            Vector3 theScale = rect.localScale;
            theScale.x *= -1;
            rect.localScale = theScale;
        }
    }

}
