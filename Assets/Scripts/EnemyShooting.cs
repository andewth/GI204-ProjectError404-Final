using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // กระสุน
    public Transform firePoint; // จุดยิงกระสุน
    public float bulletSpeed = 15f; // ความเร็วกระสุน
    public float fireRate = 1f; // อัตราการยิง (วินาที)
    public float detectRange = 10f; // ระยะตรวจจับ
    public float shootHeightOffset = 1.5f; // ความสูงในการยิง (เล็งให้โดนตัวผู้เล่น)

    private Transform player; 
    private float nextFireTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange) // ถ้าอยู่ในระยะตรวจจับ
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); // หันไปหา Player
            TryShoot();
        }
    }

    void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector3 shootDirection = (player.position - firePoint.position).normalized; // คำนวณทิศทางการยิง
        shootDirection.y += shootHeightOffset / detectRange; // ปรับมุมให้ยิงโดนตัวผู้เล่น

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = shootDirection * bulletSpeed;
        }

        Destroy(bullet, 3f); // ทำลายกระสุนเมื่อผ่านไป 3 วินาที
    }
}

