using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BigIsland"); // โหลดซีน BigIsland
    }

    public void ExitGame()
    {
        Application.Quit(); // ออกจากเกม
        Debug.Log("Game is exiting..."); // แสดงข้อความใน Console (ใช้ได้แค่ใน Editor)
    }
}
