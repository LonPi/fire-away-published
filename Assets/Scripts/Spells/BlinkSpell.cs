using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkSpell {
    float
        range,
        cooldown,
        timer,
        inputDelay,
        lastInputTime;

    public BlinkSpell(float range, float cooldown)
    {
        timer = 0f;
        lastInputTime = 0f;
        this.range = range;
        this.cooldown = cooldown;
        inputDelay = 0.3f;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f) timer = 0f;
    }

    public float GetCooldownTimer() { return timer; }

    public bool CanCast()
    {
        return timer <= 0f && (Time.time - lastInputTime >= inputDelay);
    }

    public bool Cast(Player player, Vector2 direction)
    {
        if (!CanCast())
        {
            return false;
        }
        Vector2 deltaMovement = range * direction;
        player.Controller.Move(ref deltaMovement);
        player.IndicateBlink();
        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        // record blink state
        SpellManager.BlinkInfo.castedBlinkPreviously = true;
        SpellManager.BlinkInfo.castedBlinkTimestamp = Time.time;
        return true;
    }


}
