using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell {
    float
        initialFallingHeight,
        castRange,
        areaOfDamage,
        damage,
        cooldown,
        timer,
        lastInputTime,
        inputDelay;
    GameObject MeteorPrefab;

    public MeteorSpell(float initialFallingHeight, float castRange, float areaOfDamage, float damage, float cooldown, GameObject MeteorPrefab)
    {
        this.initialFallingHeight = initialFallingHeight;
        this.castRange = castRange;
        this.areaOfDamage = areaOfDamage;
        this.damage = damage;
        this.cooldown = cooldown;
        this.MeteorPrefab = MeteorPrefab;
        this.inputDelay = 0.3f;
        this.timer = 0f;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public float GetCooldownTimer() { return timer; }

    public bool CanCast()
    {
        bool blinkDelayOver = true;
        if (SpellManager.BlinkInfo.castedBlinkPreviously)
            blinkDelayOver = false;
        // enforce delay in spell casting after blink
        if (!blinkDelayOver &&
            (Time.time - SpellManager.BlinkInfo.castedBlinkTimestamp >= SpellManager.BlinkInfo.SPELL_CAST_DELAY_AFTER_BLINK))
        {
            blinkDelayOver = true;
            SpellManager.BlinkInfo.Reset();
        }
        return timer <= 0f && (Time.time - lastInputTime >= inputDelay) && blinkDelayOver;
    }

    public bool Cast(Player player)
    {
        if (!CanCast())
            return false;
        float offset_x = player.isFacingRight ? castRange : -1 * castRange;
        Vector2 meteorStartPosition = new Vector2(player.transform.position.x + offset_x, initialFallingHeight);
        GameObject gameObj = GameObject.Instantiate(MeteorPrefab, meteorStartPosition, Quaternion.identity);
        gameObj.GetComponent<Meteor>().SetParams(damage, areaOfDamage);
        lastInputTime = Time.time;
        timer = cooldown;
        return true;
    }

    public void SetDamage(float scale)
    {
        this.damage = damage + damage * scale;
        this.damage = Mathf.Floor(damage);
    }
}
