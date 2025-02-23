using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject gun;  // Gun GameObject
    public GameObject bulletPrefab;  // Bullet Prefab
    public Transform firePoint;  // FirePoint (where bullets spawn)
    public Animator shootVFXAnimator; // VFX Animator on separate object
    public GameObject shootVFX; // VFX Object

    public float bulletSpeed = 15f;
    public float gunShowTime = 0.2f;

    private void Update()
    {
        AimGunAtMouse();

        if (Input.GetMouseButtonDown(0)) // Left-click to shoot
        {
            Shoot();
        }
    }

    void AimGunAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        // Show gun
        gun.SetActive(true);

        // 🔥 Create a new VFX instance every time
        GameObject newVFX = Instantiate(shootVFX, firePoint.position, Quaternion.identity);
        newVFX.SetActive(true);
        newVFX.transform.localScale = new Vector3(2f, 2f, 1f); // Increase size

        // 🔥 Move the VFX slightly in random positions
        Vector3 vfxOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0);
        newVFX.transform.position += vfxOffset;

        // Get the Animator of the newly created VFX
        Animator vfxAnimator = newVFX.GetComponent<Animator>();

        // 🔥 Randomize ShootVFX Animation
        int randomShoot = Random.Range(1, 5);
        vfxAnimator.SetInteger("ShootVariant", randomShoot);
        Debug.Log("Playing ShootVFX: " + randomShoot);

        // 🔥 Destroy the VFX object after animation completes
        Destroy(newVFX, 0.5f); // Adjust timing based on animation length

        // 🔥 Get mouse position for bullet direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure it's in 2D space

        // 🔥 Calculate direction to mouse
        Vector2 shootDirection = (mousePos - firePoint.position).normalized;

        // 🔥 Spawn Bullet & Set Direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        // Rotate Bullet to Face Mouse Direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Hide gun after a short delay
        Invoke(nameof(HideGun), gunShowTime);
    }


    void HideGun()
    {
        gun.SetActive(false);
    }

    void HideVFX()
    {
        shootVFX.SetActive(false);
    }
}
