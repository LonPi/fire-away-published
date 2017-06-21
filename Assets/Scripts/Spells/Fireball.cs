using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    float
        moveSpeed,
        damage,
        travelDistance;
    Player player;
    Vector2 _velocity;
    Vector2 targetPosition;
    BoxCollider2D _boxCollider;
    const float damageInterval = 0.5f;
    float upwardVelocity = 10f;
    Dictionary<int, float> damagedTargets;
    float _moveDirection;

    void Start () {
        player = GetComponentInParent<Player>();
        _velocity = new Vector2(moveSpeed, 0f);
        transform.parent = null;
        damagedTargets = new Dictionary<int, float>();
        _boxCollider = GetComponent<BoxCollider2D>();
        if (player.isFacingRight)
            targetPosition = new Vector2(transform.position.x + travelDistance, transform.position.y);
        else
            targetPosition = new Vector2(transform.position.x - travelDistance, transform.position.y);
        _moveDirection = (transform.position.x - targetPosition.x > 0) ? -1f : 1f;
    }

    void Update ()
    {
        Move();
        InflictDamage();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, _velocity.x * Time.deltaTime);
        if ((Vector2)transform.position == targetPosition)
            Destroy(gameObject);
    }

    public void SetParams(float moveSpeed, float damage, float travelDistance)
    {
        this.moveSpeed = moveSpeed;
        this.damage = damage;
        this.travelDistance = travelDistance;
    }

    void InflictDamage()
    {
        float raycastRadius, raycastDistance;
        Bounds bounds = _boxCollider.bounds;
        Vector2 raycastDirection = _velocity.x < 0 ? Vector2.left : Vector2.right;
        raycastDistance = raycastRadius = bounds.size.x / 2;
        Vector2 raycastOrigin = transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(raycastOrigin, raycastRadius, raycastDirection, raycastDistance, 1 << LayerMask.NameToLayer("Enemy"));

        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            int instanceId = enemy.gameObject.GetInstanceID();
            if (enemy._isDead) continue;

            if (damagedTargets.ContainsKey(instanceId) && (Time.time - damagedTargets[instanceId] >= damageInterval))
            {
                enemy.TakeDamage(damage, upwardVelocity, _moveDirection);
                enemy.CreateCombatText(enemy.transform.position, damage.ToString());
                damagedTargets[instanceId] = Time.time;
                if (enemy._isDead)
                    damagedTargets.Remove(instanceId);
            }
            else if (!damagedTargets.ContainsKey(instanceId))
            {
                enemy.TakeDamage(damage, upwardVelocity, _moveDirection);
                enemy.CreateCombatText(enemy.transform.position, damage.ToString());
                damagedTargets.Add(instanceId, Time.time);
                if (enemy._isDead)
                    damagedTargets.Remove(instanceId);
            }
        }
    }
}
