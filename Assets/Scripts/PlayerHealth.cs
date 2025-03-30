using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI deathText;

    public int maxHealth = 100; // เลือดสูงสุด
    private int currentHealth;

    public Image healthBarImage; // ลิงก์กับ UI Image ที่ใช้เป็นหลอดเลือด (ต้องลากใส่ใน Inspector)
    
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        deathText.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันค่าติดลบ
        UpdateHealthUI();

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarImage != null)
        {
            // คำนวณค่า fillAmount ของ healthBarImage
            healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);
        // สามารถเพิ่มแอนิเมชันตาย หรือรีสตาร์ทเกมได้ที่นี่

        deathText.gameObject.SetActive(true); // แสดงข้อความ "คุณตาย"
        
        // ซ่อนตัวละครหรือทำให้ควบคุมไม่ได้
        gameObject.SetActive(false);
        RespawnToLobbyAsync();
    }

    async Task RespawnToLobbyAsync()
    {
        await Task.Delay(2000);
        SceneManager.LoadScene("Lobby"); // เปลี่ยนไปซีน Lobby
        Cursor.lockState = CursorLockMode.None; // ปลดล็อคเมาส์
        Cursor.visible = true;
    }
}
