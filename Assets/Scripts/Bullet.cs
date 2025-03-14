using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; 

    private void Start()
    {
        Destroy(gameObject, lifetime);  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet hit: " + collision.gameObject.name);  

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit an ENEMY!");
            collision.GetComponent<EnemyAI>().TakeDamage(); 
            //Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Bullet hit the GROUND!");
            Destroy(gameObject);
        }
    }
}