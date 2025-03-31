using System.Collections;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 1.2f;
    public float detectionRange = 15f; // ระยะที่ศัตรูเริ่มไล่ตามผู้เล่น
    public int maxHealth = 100;
    public float knockbackForce = 5f; // แรงกระแทกจากกระสุน
    public float knockbackDuration = 2f; // ระยะเวลาที่ศัตรูกระเด็น
    public float knockbackXModifier = 1.0f; // ค่าปรับเพิ่มในการกระเด็นในทิศทาง X

    private Transform player;
    private int currentHealth;
    private Rigidbody rb;
    private int damageFromBullet = 10; // กำหนดค่าดาเมจจากกระสุน

    private bool isKnockedBack = false; // เช็คว่าโดนกระแทกหรือไม่

    // มวลของศัตรู (ค่าตัวอย่าง)
    public float enemyMass = 50f;
    // มวลของกระสุน (ค่าตัวอย่าง)
    public float bulletMass = 1f;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>(); // ดึง Rigidbody จาก GameObject นี้

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || isKnockedBack) return; // หยุดการเคลื่อนที่ถ้ากระเด็น

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // ไล่ตามผู้เล่นเมื่ออยู่ในระยะ
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // หมุนให้หันหน้าหาผู้เล่น
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    public void TakeDamage(int damage, Vector3 bulletVelocity, float bulletMass)
    {
        Debug.Log($"Taking {damage} damage");
        currentHealth -= damage;

        if (rb != null)
        {
            // คำนวณโมเมนตัมของกระสุน
            Vector3 bulletMomentum = bulletMass * bulletVelocity;

            // คำนวณแรงกระแทก (Action = Reaction)
            Vector3 knockbackForce = bulletMomentum / Time.fixedDeltaTime;

            // ใช้แรงกระแทกเพื่อกระเด็น
            rb.AddForce(knockbackForce / enemyMass, ForceMode.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(StopMovementAndResumeAfterDelay());
    }


    // คำนวณแรงกระแทกจาก Universal Gravitation
    float CalculateGravitationalForce(float m1, float m2, float distance)
    {
        const float G = 6.67430e-11f; // ค่าคงที่ของแรงดึงดูดสากล
        return G * ((m1 * m2) / Mathf.Pow(distance, 2));
    }

    // Coroutine สำหรับหยุดการเคลื่อนที่
    private IEnumerator StopMovementAndResumeAfterDelay()
    {
        isKnockedBack = true; // ตั้งค่าเป็นกระเด็น

        // รอเวลา 2 วินาที
        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false; // หยุดการกระเด็นและเริ่มไล่ตามอีกครั้ง
    }

    void Die()
    {
        // แจ้งให้ PlayerController เพิ่ม Kill
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.AddKill();
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // ดึง Rigidbody ของกระสุน
            Rigidbody bulletRb = other.GetComponent<Rigidbody>();

            // ดึงค่าดาเมจจากกระสุน
            Bullet bullet = other.GetComponent<Bullet>();
            int damage = (bullet != null) ? bullet.damage : damageFromBullet; // ใช้ค่าดาเมจจาก Bullet ถ้ามี

            // ใช้ความเร็วและมวลของกระสุนเพื่อคำนวณแรงกระแทก
            Vector3 bulletVelocity = bulletRb != null ? bulletRb.linearVelocity : Vector3.zero;
            float bulletMass = bulletRb != null ? bulletRb.mass : 1f;

            // ส่งค่า damage และ bullet physics ไปยัง TakeDamage
            TakeDamage(damage, bulletVelocity, bulletMass);

            // ทำลายกระสุน
            Destroy(other.gameObject);
        }
    }

}

