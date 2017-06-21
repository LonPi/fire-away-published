using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class Enemy : MonoBehaviour {

    public float moveVelocity;
    public float hitPoints;
    public float damage;
    public bool _isDead { get; private set; }
    public float expGainPerKill;

    Transform _transform { get { return transform; } }
    Vector3 _localScale { get { return transform.localScale; } }
    Vector2 _targetPosition;
    public Vector2 _moveDirection { get; private set; }
    float lastAttackTime;
    float attackInterval = 0.5f;
    Animator animator;
    BoxCollider2D _boxCollider;
    EnemyController _controller;
    Canvas combatCanvas;
    Vector2 _velocity;
    float gravity = -20f;
    bool kinematicMotionEnabled;
    DefaultParams _defaultParams;
    float currentScale = 0.6f;

	void Start () {
        animator = GetComponent<Animator>();
        combatCanvas = GetComponentInChildren<Canvas>();
        _controller = GetComponent<EnemyController>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _targetPosition= GameManager.instance._treeRef.transform.position;
        _moveDirection = (_targetPosition.x - transform.position.x > 0 ? Vector2.right : Vector2.left);
        _isDead = false;
        kinematicMotionEnabled = false;
        _velocity = new Vector2(moveVelocity * _moveDirection.x, 0f);
        _defaultParams = new DefaultParams(moveVelocity, hitPoints, damage);
    }
	
	void Update () {
        if (_isDead)
        {
            ApplyGravity();
            return;
        }

        if (kinematicMotionEnabled)
        {
            ApplyMotion();
        }
        else MoveTowardsTarget();

        if ((Time.time - lastAttackTime >= attackInterval))
            InflictDamage();
        
    }

    void MoveTowardsTarget()
    {
        if (_controller.collisionInfo.below)
        {
            _velocity.y = 0f;
        }

        // X movement
        Flip();
        // if has reached the target position, change target to nearby location
        if (transform.position.x == _targetPosition.x && _targetPosition.x == GameManager.instance._treeRef.transform.position.x)
        {
            float randomDirX = Random.Range(-1.99f, 1.99f);
            float randomDistX = Random.Range(1.2f, 2.2f);
            _targetPosition = new Vector2(transform.position.x + (randomDirX * randomDistX), transform.position.y);
        }
        // otherwise move to target position
        else if (transform.position.x == _targetPosition.x && _targetPosition.x != GameManager.instance._treeRef.transform.position.x)
            _targetPosition = new Vector2(GameManager.instance._treeRef.transform.position.x, transform.position.y);
        _moveDirection = (_targetPosition.x - transform.position.x > 0 ? Vector2.right : Vector2.left);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_targetPosition.x, transform.position.y), moveVelocity * Time.deltaTime);

        // Y movement (gravity)
        ApplyGravity();
    }

    void ApplyMotion()
    {
        ApplyGravity();
        transform.Translate(new Vector2(_velocity.x*Time.deltaTime, 0f));

        if (_controller.collisionInfo.below)
            kinematicMotionEnabled = false;
    }

    void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        float deltaMovementY = _velocity.y * Time.deltaTime;
        _controller.MoveYAxis(ref deltaMovementY);
    }

    public void TakeDamage(float _damage, float upwardVelocity, float directionFrom)
    {
        hitPoints -= _damage;
        if (!kinematicMotionEnabled)
        {
            _velocity.y = upwardVelocity;
            if (Mathf.Sign(directionFrom) != Mathf.Sign(_velocity.x))
                _velocity.x *= -1;
            kinematicMotionEnabled = true;
        }

        if (hitPoints <= 0 && !_isDead)
        {
            _isDead = true;
            animator.SetTrigger("dead");
            GameManager.instance._playerRef.IncrementKillCount();
            GameManager.instance.IncrementExp(expGainPerKill);
            StartCoroutine(_ReturnToPool());
        }
    }

    public void SetParams(int level, Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;

        if (_defaultParams == null)
            _defaultParams = new DefaultParams(moveVelocity, hitPoints, damage);

        _defaultParams.Reset(this);

        this.damage += this.damage * (float)level / 10;
        this.hitPoints += this.hitPoints * (float)level / 10;
        this.transform.localScale = new Vector3(currentScale + (float)level / 50, currentScale + (float)level / 50, 0);

        //Debug.Log(gameObject.name + "  level: " + level + " hp: " + hitPoints + " damage: " + damage);
    }

    void InflictDamage()
    {
        float raycastDistance, raycastRadius;
        Bounds bounds = _boxCollider.bounds;
        raycastDistance = raycastRadius = bounds.size.x / 2;
        RaycastHit2D hit = Physics2D.CircleCast(_transform.position, raycastRadius, _moveDirection, raycastDistance, 1 << LayerMask.NameToLayer("Friendly"));
        lastAttackTime = Time.time;
        if (hit)
        {
            Player player = hit.collider.gameObject.GetComponent<Player>();
            Tree tree = hit.collider.gameObject.GetComponent<Tree>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            if (tree != null)
            {
                tree.TakeDamage(damage);
            }
        }
    }

    void Flip()
    {
        // sprite is facing +x direction by default
        if (_moveDirection == Vector2.right && _localScale.x < 0 || _moveDirection == Vector2.left && _localScale.x > 0)
        {
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public void CreateCombatText(Vector2 position, string fillText)
    {
        combatCanvas.transform.GetChild(0).GetComponent<CombatText>().Activate(fillText);
    }

    IEnumerator _ReturnToPool()
    {
        yield return new WaitForSeconds(2);
        PoolManager.instance.ReturnObjectToPool(gameObject);
    }

    class DefaultParams
    {
        float
            moveVelocity,
            hitPoints,
            damage;

        public DefaultParams(float moveVelocity, float hitPoints, float damage)
        {
            this.moveVelocity = moveVelocity;
            this.hitPoints = hitPoints;
            this.damage = damage;
        }

        public void Reset(Enemy enemy)
        {
            enemy.damage = this.damage;
            enemy.moveVelocity = this.moveVelocity;
            enemy.hitPoints = this.hitPoints;
            enemy._isDead = false;
            enemy._targetPosition = GameManager.instance._treeRef.transform.position;
            enemy._moveDirection = (enemy._targetPosition.x - enemy.transform.position.x > 0 ? Vector2.right : Vector2.left);
            enemy._isDead = false;
            enemy.kinematicMotionEnabled = false;
            enemy._velocity = new Vector2(enemy.moveVelocity * enemy._moveDirection.x, 0f);
        }
    }
}
