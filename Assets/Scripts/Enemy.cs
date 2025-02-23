using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    protected float moveSpeed;

    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int maxHealth = 3;

    public GameObject hitVFXPrefab;
    public Transform hitVFXSpawnPoint;

    protected int currentHealth;
    protected Transform player;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected bool isAttacking = false;
    protected bool isDead = false;
    protected bool isTakingHit = false;

    protected virtual void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);

        if (hitVFXPrefab == null)
            Debug.LogWarning("EnemyAI: hitVFXPrefab is NOT assigned!");

        if (hitVFXSpawnPoint == null)
            Debug.LogWarning("EnemyAI: hitVFXSpawnPoint is NOT assigned!");
    }

    protected virtual void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isAttacking && !isTakingHit)
        {
            if (distanceToPlayer > attackRange)
                ChasePlayer();
            else
                AttackPlayer();
        }

        anim.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    protected virtual void ChasePlayer()
    {
        if (isAttacking || isDead || isTakingHit) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        anim.SetBool("isRunning", true);
        anim.SetBool("isAttacking", false);

        GetComponent<SpriteRenderer>().flipX = direction.x < 0;
    }

    protected virtual void AttackPlayer()
    {
        if (isDead || isTakingHit) return;

        isAttacking = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", true);

        int attackType = Random.Range(0, 2);
        anim.SetInteger("AttackType", attackType);

        Invoke(nameof(DelayedHit), 1f);
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    protected virtual void DelayedHit()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("⚡ Attack HIT the Player!");
            FindObjectOfType<PlayerHealth>().TakeDamage(1); // ✅ Apply damage
        }
        else
        {
            Debug.Log("❌ Attack MISSED!");
        }
    }

    protected virtual void ResetAttack()
    {
        if (isDead) return;
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public virtual void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        Debug.Log($"⚠️ Enemy took damage! Health: {currentHealth}");

        anim.SetTrigger("TakeHit");
        isTakingHit = true;

        SpawnHitVFX();

        float takeHitDuration = anim.GetCurrentAnimatorStateInfo(0).length;

        if (currentHealth <= 0)
            Invoke(nameof(Die), takeHitDuration);
        else
            Invoke(nameof(ResetAfterHit), takeHitDuration - 0.1f);
    }

    protected virtual void SpawnHitVFX()
    {
        if (hitVFXPrefab != null && hitVFXSpawnPoint != null)
        {
            GameObject vfx = Instantiate(hitVFXPrefab, hitVFXSpawnPoint.position, Quaternion.identity);
            Animator vfxAnimator = vfx.GetComponent<Animator>();

            int randomVFXIndex = Random.Range(1, 12);
            vfxAnimator.Play("HitVFX" + randomVFXIndex, 0, 0);

            vfx.transform.localScale = new Vector3(8f, 8f, 1f);

            Destroy(vfx, 0.5f);
        }
    }

    protected virtual void ResetAfterHit()
    {
        if (isDead) return;
        isTakingHit = false;
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic; // Prevents movement while keeping animations

        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.ResetTrigger("TakeHit");

        anim.SetTrigger("Die");

        // ✅ UI Update
        FindObjectOfType<UI>().IncreaseKillCount();
        FindObjectOfType<UI>().SetEnemiesLeft(FindObjectOfType<WaveSpawner>().GetRemainingEnemies());

        // ✅ Wait for animation length before destroying (Without IEnumerator)
        float deathAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;
        Invoke(nameof(DisableEnemy), deathAnimationLength);
    }

    // Disable enemy (but let animation finish)
    protected virtual void DisableEnemy()
    {
        GetComponent<Collider2D>().enabled = false; // Prevent collisions
        rb.bodyType = RigidbodyType2D.Static; // Ensure no movement issues

        Invoke(nameof(DestroyEnemy), 1f); // Delayed destruction for visibility
    }

    protected virtual void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
