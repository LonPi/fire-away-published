using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    float damage, areaOfDamage;
    float upwardVelocity = 20f;
    BoxCollider2D _boxCollider;
    const float damageInterval = 0.5f;
    Dictionary<int, float> damagedTargets;

    void Start ()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Friendly"));
        damagedTargets = new Dictionary<int, float>();
    }

    public void SetParams(float damage, float areaOfDamage)
    {
        this.damage = damage;
        this.areaOfDamage = areaOfDamage;
    }

    private void Update()
    {
        InflictDamage();
    }

    void InflictDamage()
    {
        Bounds bounds = _boxCollider.bounds;
        Vector2 raycastOrigin = bounds.center;
        float raycastDistance = areaOfDamage;
        Vector2 raycastDirection = Vector2.down;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastDistance, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            int instanceId = enemy.gameObject.GetInstanceID();
            if (enemy._isDead) continue;

            if (damagedTargets.ContainsKey(instanceId) && (Time.time - damagedTargets[instanceId] >= damageInterval))
            {
                enemy.TakeDamage(damage, upwardVelocity, enemy._moveDirection.x * -1);
                enemy.CreateCombatText(enemy.transform.position, damage.ToString());
                damagedTargets[instanceId] = Time.time;
                if (enemy._isDead)
                    damagedTargets.Remove(instanceId);
            }
            else if (!damagedTargets.ContainsKey(instanceId))
            {
                enemy.TakeDamage(damage, upwardVelocity, enemy._moveDirection.x * -1);
                enemy.CreateCombatText(enemy.transform.position, damage.ToString());
                damagedTargets.Add(instanceId, Time.time);
                if (enemy._isDead)
                    damagedTargets.Remove(instanceId);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            GameManager.instance._cameraShake.Shake(0.4f, 0.4f);
            Destroy(gameObject, 0.5f);
        }
    }
}
