using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell {
    float
        damage,
        moveSpeed,
        travelDistance,
        cooldown,
        timer,
        inputDelay,
        lastInputTime;
    GameObject FireballPrefab;

   public FireballSpell(float damage, float moveSpeed, float travelDistance, float cooldown, GameObject FireballPrefab)
    {
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.travelDistance = travelDistance;
        this.cooldown = cooldown;
        this.FireballPrefab = FireballPrefab;
        timer = 0f;
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

        Vector2 spriteSize = player.spriteRenderer.bounds.size;
        float offset_x = player.isFacingRight ? spriteSize.y / 2 : -1 * spriteSize.y / 2;
        Vector2 spawnPosition = new Vector2(player.transform.position.x + offset_x, player.transform.position.y);
        GameObject gameObj = GameObject.Instantiate(FireballPrefab, spawnPosition, Quaternion.identity, player.transform);
        gameObj.GetComponent<Fireball>().SetParams(moveSpeed, damage, travelDistance);
        // cooldown active
        timer = cooldown;
        // record last input time
        lastInputTime = Time.time;
        return true;
    }

    public void SetDamage(float scale)
    {
        this.damage = damage + damage * scale;
        this.damage = Mathf.Floor(damage);
    }
}
