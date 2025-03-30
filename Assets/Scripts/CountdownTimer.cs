using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks; // ใช้ TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // ลิงก์กับ TextMeshPro สำหรับแสดงเวลาถอยหลัง

    public TextMeshProUGUI timeOutTxt;
    private float timeRemaining = 90f; // ตั้งเวลาเริ่มต้นเป็น 90 วินาที (1 นาที 30 วินาที)
    private bool timerRunning = false;

    void Start()
    {
        // แสดงเวลาเริ่มต้นและซ่อนข้อความเมื่อตัวนับเวลาถอยหลังเริ่ม
        UpdateCountdownText();
        timerRunning = true;
        timeOutTxt.gameObject.SetActive(false);
    }


    void TimeOut()
    {
        timeOutTxt.gameObject.SetActive(true); // แสดงข้อความ "คุณตาย"
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

    void Update()
    {
        if (timerRunning)
        {
            // ลดเวลาทีละเฟรม
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                countdownText.text = "TIME'S UP!"; // แสดงข้อความเมื่อเวลาหมด
                Debug.Log("Time's Up!");
                TimeOut();
            }

            UpdateCountdownText(); // อัพเดต UI ทุกเฟรม
        }
    }

    void UpdateCountdownText()
    {
        // คำนวณเวลาที่เหลือในรูปแบบนาที:วินาที
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // อัพเดตข้อความใน UI
    }
}
