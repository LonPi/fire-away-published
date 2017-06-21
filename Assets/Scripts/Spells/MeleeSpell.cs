using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSpell {

    float
        damage,
        range,
        cooldown,
        inputDelay,
        lastInputTime,
        timer;
    float upwardVelocity = 15f;

    public MeleeSpell(float damage, float range, float cooldown)
    {
        timer = 0f;
        this.damage = damage;
        this.range = range;
        this.cooldown = cooldown;
        this.inputDelay = 0.3f;
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

        Vector2 _spriteSize = player.spriteRenderer.bounds.size;
        Vector2 raycastDirection =  player.isFacingRight ? Vector2.right : Vector2.left;
        float raycastDistance = range;
        float raycastRadius = range;
        // circle cast downwards if player is jumping+melee
        raycastDirection = !player.Controller.collisionInfo.below ? Vector2.down : raycastDirection;
        if (raycastDirection == Vector2.down)
        {
            raycastDistance = 3f * player.transform.localScale.y;
        }
        Vector2 raycastOrigin = player.transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastRadius, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (enemy._isDead) continue;
            enemy.CreateCombatText(enemy.transform.position, damage.ToString());
            enemy.TakeDamage(damage, upwardVelocity, player.isFacingRight ? 1: -1);
            player.LifeSteal(damage / 10);
        }
        // cooldown active
        timer = cooldown;
        lastInputTime = Time.time;
        return true;
    }

    public void SetDamage(float scale)
    {
        this.damage = damage + damage * scale;
        this.damage = Mathf.Floor(damage);
    }
}
