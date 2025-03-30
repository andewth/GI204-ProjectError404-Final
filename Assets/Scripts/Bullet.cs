using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage; // ดาเมจของกระสุน

    void Start()
    {
        // หา PlayerController และดึงค่า DamageWhenKill
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            damage = player.GetDamageWhenKill(); // ใช้ค่าดาเมจจาก PlayerController
        }
    }
}

