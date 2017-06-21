using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour {

    [SerializeField]
    float
        meleeDamage,
        meleeRange,
        meleeCooldown,
        blinkRange,
        blinkCooldown,
        fireballDamage,
        fireballMoveSpeed,
        fireballTravelDistance,
        fireballCooldown,
        meteorInitialFallingHeight,
        meteorCastRange,
        meteorAreaOfDamage,
        meteorDamage,
        meteorCooldown;
    public GameObject FireballPrefab, MeteorPrefab;
    public MeleeSpell meleeSpell { get; private set; }
    public BlinkSpell blinkSpell { get; private set; }
    public FireballSpell fireballSpell { get; private set; }
    public MeteorSpell meteorSpell { get; private set; }
    Dictionary<string, SpellInfo> spellsData = new Dictionary<string, SpellInfo>();
    SpellInfo melee, blink, fireball, meteor;
    bool initialized = false;

    void Awake()
    {
        meleeSpell = new MeleeSpell(meleeDamage, meleeRange, meleeCooldown);
        blinkSpell = new BlinkSpell(blinkRange, blinkCooldown);
        fireballSpell = new FireballSpell(fireballDamage, fireballMoveSpeed, fireballTravelDistance, fireballCooldown, FireballPrefab);
        meteorSpell = new MeteorSpell(meteorInitialFallingHeight, meteorCastRange, meteorAreaOfDamage, meteorDamage, meteorCooldown, MeteorPrefab);
    }

    void Update()
    {
        meleeSpell.Update();
        blinkSpell.Update();
        fireballSpell.Update();
        meteorSpell.Update();
    }

    public Dictionary<string, SpellInfo> GetSpellsInfo()
    {
        if (!initialized)
        {
            melee = new SpellInfo();
            blink = new SpellInfo();
            fireball = new SpellInfo();
            meteor = new SpellInfo();
            spellsData.Add("melee", null);
            spellsData.Add("blink", null);
            spellsData.Add("fireball", null);
            spellsData.Add("meteor", null);
            initialized = true;
        }
        melee.cooldownTimer = meleeSpell.GetCooldownTimer();
        melee.cooldown = meleeCooldown;
        spellsData["melee"] = melee;

        blink.cooldownTimer = blinkSpell.GetCooldownTimer();
        blink.cooldown = blinkCooldown;
        spellsData["blink"] = blink;

        fireball.cooldownTimer = fireballSpell.GetCooldownTimer();
        fireball.cooldown = fireballCooldown;
        spellsData["fireball"] = fireball;

        meteor.cooldownTimer = meteorSpell.GetCooldownTimer();
        meteor.cooldown = meteorCooldown;
        spellsData["meteor"] = meteor;
        return spellsData;
    }

    public void OnLevelUp(int level)
    {
        meleeSpell.SetDamage(level * 0.01f);
        fireballSpell.SetDamage(level * 0.01f);
        meteorSpell.SetDamage(level * 0.01f);
    }

    public struct BlinkInfo
    {
        public const float SPELL_CAST_DELAY_AFTER_BLINK = 0.5f;
        public static bool castedBlinkPreviously;
        public static float castedBlinkTimestamp;

        public static void Reset()
        {
            castedBlinkPreviously = false;
            castedBlinkTimestamp = 0f;
        }
    }

    public class SpellInfo
    {
        public float cooldownTimer, cooldown;
        public SpellInfo() { }
        public void SetCooldownTimer(float cooldownTimer){ this.cooldownTimer = cooldownTimer;  }
        public void SetCooldown(float cooldown) { this.cooldown = cooldown; }
    }

    
}
