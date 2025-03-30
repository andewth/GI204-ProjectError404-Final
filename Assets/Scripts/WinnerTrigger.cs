using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class WinnerTrigger : MonoBehaviour
{
    public TextMeshProUGUI winnerTxt;

    void Start()
    {
        winnerTxt.gameObject.SetActive(false);
    }

    void Winner()
    {
        Debug.Log("Player Winner!");
        winnerTxt.gameObject.SetActive(true);
        
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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Winner();
        }
    }

}
