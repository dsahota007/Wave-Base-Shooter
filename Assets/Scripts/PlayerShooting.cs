using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject gun;  // Gun GameObject
    public GameObject bulletPrefab;  // Bullet Prefab
    public Transform firePoint;  // FirePoint (where bullets spawn)
    public Animator shootVFXAnimator; // VFX Animator on separate object
    public GameObject shootVFX; // VFX Object

    public float bulletSpeed = 15f;
    public float gunShowTime = 0.2f;
    public float recoilAmount = 0.2f;
    public float recoilSpeed = 0.1f;
    public float bobSpeed = 6f;
    public float bobAmount = 0.04f;

    private SpriteRenderer gunSpriteRenderer;
    private Vector3 originalGunPosition;
    private bool isRecoiling = false;

    private void Start()
    {
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
        originalGunPosition = gun.transform.localPosition;
        gun.SetActive(true); // Gun is always visible
    }

    private void Update()
    {
        AimGunAtMouse();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (!isRecoiling)
        {
            BobGun();
        }
    }

    void AimGunAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        gun.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 🔥 Mirror gun if facing left
        if (mousePos.x < transform.position.x)
        {
            gunSpriteRenderer.flipY = true;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, 0);
        }
        else
        {
            gunSpriteRenderer.flipY = false;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, 0);
        }
    }

    void Shoot()
    {
        if (isRecoiling) return;

        StartCoroutine(RecoilAnimation());

        // 🔥 Create a new VFX instance every time
        GameObject newVFX = Instantiate(shootVFX, firePoint.position, Quaternion.identity);
        newVFX.SetActive(true);
        newVFX.transform.localScale = new Vector3(2f, 2f, 1f);

        // 🔥 Offset the VFX slightly in random positions
        Vector3 vfxOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0);
        newVFX.transform.position += vfxOffset;

        // 🔥 Randomize ShootVFX Animation
        int randomShoot = Random.Range(1, 5);
        Animator vfxAnimator = newVFX.GetComponent<Animator>();
        vfxAnimator.SetInteger("ShootVariant", randomShoot);

        // 🔥 Destroy the VFX object after animation completes
        Destroy(newVFX, 0.5f);

        // 🔥 Get mouse position for bullet direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // 🔥 Calculate direction to mouse
        Vector2 shootDirection = (mousePos - firePoint.position).normalized;

        // 🔥 Spawn Bullet & Set Direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        // 🔥 Rotate Bullet to Face Mouse Direction
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator RecoilAnimation()
    {
        isRecoiling = true;

        // 🔥 Move gun backward
        Vector3 recoilPosition = originalGunPosition + gun.transform.right * -recoilAmount;
        float elapsedTime = 0f;

        while (elapsedTime < recoilSpeed)
        {
            gun.transform.localPosition = Vector3.Lerp(originalGunPosition, recoilPosition, (elapsedTime / recoilSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 🔥 Return gun to original position
        elapsedTime = 0f;
        while (elapsedTime < recoilSpeed)
        {
            gun.transform.localPosition = Vector3.Lerp(recoilPosition, originalGunPosition, (elapsedTime / recoilSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isRecoiling = false;
    }

    void BobGun()
    {
        // 🔥 Make the gun bob up and down using Sin wave
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        gun.transform.localPosition = originalGunPosition + new Vector3(0, bobOffset, 0);
    }
}
