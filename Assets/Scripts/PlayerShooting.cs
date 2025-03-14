using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject gun;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Animator shootVFXAnimator;
    public GameObject shootVFX;

    public float bulletSpeed = 15f; // ✅ Restored original bullet speed
    public float gunShowTime = 0.2f;
    public float recoilAmount = 0.2f;
    public float recoilSpeed = 0.1f;
    public float bobSpeed = 6f;
    public float bobAmount = 0.04f;
    public float shootDirectionLockTime = 0.10f; // ✅ Shortened to 0.10 seconds

    private SpriteRenderer gunSpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private Vector3 originalGunPosition;
    private bool isRecoiling = false;
    private PlayerMovement playerMovement;

    private void Start()
    {
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalGunPosition = gun.transform.localPosition;
        playerMovement = GetComponent<PlayerMovement>();
        gun.SetActive(true);
    }

    private void Update()
    {
        AimGunAtMouse();

        // ✅ SHOOT when left mouse button is clicked
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

        // ✅ Flip gun based on mouse position
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

        // ✅ Flip player based on shooting direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool facingRight = mousePos.x > transform.position.x;

        // ✅ Lock facing for at least 0.10 seconds
        playerMovement.LockFacingDirection(shootDirectionLockTime, facingRight);

        // ✅ Create Bullet (RESTORED ORIGINAL BEHAVIOR)
        Vector2 shootDirection = (mousePos - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator RecoilAnimation()
    {
        isRecoiling = true;

        Vector3 recoilPosition = originalGunPosition + gun.transform.right * -recoilAmount;
        float elapsedTime = 0f;

        while (elapsedTime < recoilSpeed)
        {
            gun.transform.localPosition = Vector3.Lerp(originalGunPosition, recoilPosition, elapsedTime / recoilSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < recoilSpeed)
        {
            gun.transform.localPosition = Vector3.Lerp(recoilPosition, originalGunPosition, elapsedTime / recoilSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isRecoiling = false;
    }

    void BobGun()
    {
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        gun.transform.localPosition = originalGunPosition + new Vector3(0, bobOffset, 0);
    }
}
