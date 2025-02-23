using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;  // Bullet disappears after 2 seconds

    private void Start()
    {
        Destroy(gameObject, lifetime);  // Auto-destroy bullet after time
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);  // ✅ Debug what bullet hits

        // 🔥 Destroy bullet if it hits an enemy
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit an ENEMY!");
            collision.GetComponent<EnemyAI>().TakeDamage(); // Damage enemy
            //Destroy(gameObject);
        }

        // 🔥 Destroy bullet if it hits ground or any other specific tag
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Bullet hit the GROUND!");
            Destroy(gameObject);
        }
    }
}