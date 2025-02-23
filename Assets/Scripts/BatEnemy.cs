using UnityEngine;

public class BatEnemy : EnemyAI
{

    protected override void Start()
    {
        base.Start(); // Call the parent class Start()

        // Ensure BatEnemy has its own layer
        gameObject.layer = LayerMask.NameToLayer("BatEnemy");

        // Ignore collisions with Ground, Enemy, and Player
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BatEnemy"), LayerMask.NameToLayer("Ground"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BatEnemy"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("BatEnemy"), LayerMask.NameToLayer("Player"), true);

    }

    // Override ChasePlayer for full 2D movement.
    protected override void ChasePlayer()
    {
        if (isAttacking || isDead || isTakingHit) return;

        Vector2 direction = (player.position - transform.position).normalized;
        // Full 2D movement: use the entire direction vector.
        rb.velocity = direction * moveSpeed;

        anim.SetBool("isRunning", true);
        anim.SetBool("isAttacking", false);

        // Flip sprite based on horizontal component.
        GetComponent<SpriteRenderer>().flipX = direction.x < 0;
    }

    // Override AttackPlayer so that the bat continues moving while attacking.
    protected override void AttackPlayer()
    {
        if (isDead || isTakingHit) return;

        isAttacking = true;
        // Do NOT zero the velocity—the bat keeps moving while attacking.
        anim.SetBool("isAttacking", true);
        anim.SetBool("isRunning", true);  // Keep the running (movement) animation active

        int attackType = Random.Range(0, 2);
        anim.SetInteger("AttackType", attackType);

        Invoke(nameof(DelayedHit), 1f);
        Invoke(nameof(ResetAttack), attackCooldown);
    }
}