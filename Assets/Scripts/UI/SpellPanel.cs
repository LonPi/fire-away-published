using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPanel : MonoBehaviour
{
    public Image
        meleeIcon,
        blinkIcon,
        fireballIcon,
        meteorIcon;

    SpellManager spellManager;

    void Start()
    {
        spellManager = GameManager.instance._playerRef.GetComponent<SpellManager>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        Dictionary<string, SpellManager.SpellInfo> data = spellManager.GetSpellsInfo();
        meleeIcon.fillAmount = data["melee"].cooldownTimer / data["melee"].cooldown;
        blinkIcon.fillAmount = data["blink"].cooldownTimer / data["blink"].cooldown;
        fireballIcon.fillAmount = data["fireball"].cooldownTimer / data["fireball"].cooldown;
        meteorIcon.fillAmount = data["meteor"].cooldownTimer / data["meteor"].cooldown;
    }


}
