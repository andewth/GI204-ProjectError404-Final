using System.Collections;
using UnityEngine;


public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab ของกระสุน
    public Transform firePoint; // จุดที่กระสุนเกิด
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f; // เวลาหน่วงระหว่างการยิง
    public float chargeTime = 3f; // เวลาที่ต้องกดค้างเพื่อยิงโปรเจกไทล์
    public float bulletRange = 100f; // ระยะยิงของ Raycast
    public float bulletDamage = 10f; // ดาเมจที่ทำได้

    private float nextFireTime = 0f;
    private float holdStartTime = 0f;
    private bool isCharging = false;

    public AudioSource attackAudio;
    public AudioClip attackFx;

    private Animator animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            holdStartTime = Time.time;
            isCharging = true;
        }

        if (Input.GetMouseButton(0)) // ตรวจจับการกดค้าง
        {
            float holdDuration = Time.time - holdStartTime;

            if (holdDuration >= chargeTime)
            {
                if (Time.time >= nextFireTime) // ยิงรัวเมื่อกดค้างเกิน 3 วินาที
                {
                    ShootProjectile();
                    nextFireTime = Time.time + fireRate;
                }
            }
            else
            {
                if (Time.time >= nextFireTime) // ยิงรัวเมื่อกดค้างปกติ
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isCharging = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Shoot()
    {
        attackAudio.PlayOneShot(attackFx, 1);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 shootDirection = Camera.main.transform.forward;
            shootDirection.Normalize();

            // ป้องกันไม่ให้แกน Y ต่ำเกินไป
            if (shootDirection.y < 0.12f)
            {
                shootDirection.y = 0.12f;
            }

            rb.linearVelocity = shootDirection * bulletSpeed;
        }

        Destroy(bullet, 2f); // ทำลายกระสุนหลังจาก 2 วินาที

        // รีเซ็ตอนิเมชั่นหลังจากยิงเสร็จ
        StartCoroutine(ResetAttackAnimation());
    }

    void ShootProjectile()
    {
        attackAudio.PlayOneShot(attackFx, 1);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 shootDirection = Camera.main.transform.forward;
            shootDirection.Normalize();

            // ป้องกันไม่ให้แกน Y ต่ำเกินไป
            if (shootDirection.y < 0.12f)
            {
                shootDirection.y = 0.12f;
            }

            rb.linearVelocity = shootDirection * bulletSpeed * 2; // โปรเจกไทล์มีความเร็ว 2 เท่า
        }

        RaycastHit hit;
        // ใช้ Raycast เพื่อตรวจจับการชนของกระสุน
        if (Physics.Raycast(firePoint.position, bullet.transform.forward, out hit, bulletRange))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // ถ้ากระสุนชนกับศัตรู ให้ลด HP ของศัตรู
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
        }

        Destroy(bullet, 3f); // ทำลายกระสุนหลังจาก 3 วินาที

        // รีเซ็ตอนิเมชั่นหลังจากยิงเสร็จ
        StartCoroutine(ResetAttackAnimation());
    }

    // คอร์รูทีนสำหรับรีเซ็ตอนิเมชั่น
    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.2f); // รอให้เล่นอนิเมชั่นสักครู่
        animator.SetBool("attack", false);
    }

}




