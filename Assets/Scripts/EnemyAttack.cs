using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 2f; // ระยะโจมตี
    public float attackRate = 1.5f; // ความถี่ในการโจมตี (วินาที)
    public int attackDamage = 10; // ดาเมจของศัตรู

    private Transform player;
    private float nextAttackTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack()
    {
        Debug.Log("Enemy Attacks Player!"); // ล็อกเมื่อโจมตี (เปลี่ยนเป็นแอนิเมชันหรือทำดาเมจ)

        // ตรวจสอบว่ามี Player อยู่ในระยะ
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage); // ลด HP ของผู้เล่น
                }
            }
        }
    }

    // แสดงระยะโจมตีใน Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

