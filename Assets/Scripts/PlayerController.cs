using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = 9.81f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    public Transform cameraTransform;

    public int killCount = 0;  // จำนวน Kill
    public int deathCount = 0; // จำนวน Death

    public Text kdaText; // UI สำหรับแสดงค่า KDA
    public Text powerUpText; // UI สำหรับแสดงสถานะ PowerUp

    public int DamageWhenKill = 5;

    private Animator animator;

    // ตัวแปรสำหรับ PowerUp
    private bool hasPowerUp = false;
    private float powerUpDuration = 5f; // ระยะเวลาของ PowerUp


    public ParticleSystem runParticle;

    public AudioSource playerAudio;
    public AudioClip jumpFx;


    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        UpdateKDA();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float speed = isRunning ? runSpeed : walkSpeed;

        animator.SetBool("run", isRunning);
        
        // ตรวจสอบว่าผู้เล่นกำลังเดินหรือไม่
        bool isWalking = (moveX != 0 || moveZ != 0) && !isRunning;
        animator.SetBool("walk", isWalking);

        // ถ้ามี PowerUp จะเพิ่มความเร็ว
        if (hasPowerUp)
        {
            speed *= 1.5f;
            powerUpDuration -= Time.deltaTime;

            if (powerUpDuration <= 0)
            {
                hasPowerUp = false;
            }
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 move = (right * moveX + forward * moveZ).normalized;
        moveDirection.x = move.x * speed;
        moveDirection.z = move.z * speed;

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
                animator.SetBool("jump", true);
                playerAudio.PlayOneShot(jumpFx, 1);
            }
            else
            {
                moveDirection.y = -1f;
                animator.SetBool("jump", false);
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }


        // ตรวจสอบการกดปุ่ม ESC
        if (Input.GetKeyDown(KeyCode.Escape)) // ปุ่ม ESC
        {
            SceneManager.LoadScene("Lobby"); // เปลี่ยนไปซีน Lobby
            Cursor.lockState = CursorLockMode.None; // ปลดล็อคเมาส์
            Cursor.visible = true;
        }
    }


    // ฟังก์ชันเพิ่ม Kill
    public void AddKill()
    {
        killCount++;
        UpdateKDA();
    }

    // ฟังก์ชันเพิ่ม Death
    public void AddDeath()
    {
        deathCount++;
        UpdateKDA();
    }

    // อัปเดต UI KDA
    void UpdateKDA()
    {
        if (kdaText != null)
        {
            kdaText.text = $"KDA: {killCount} / {deathCount}";
        }

        Debug.Log($"KDA: {killCount} / {deathCount}");
        DamageWhenKill += 3;
    }

    // ฟังก์ชันสำหรับการดึงค่า DamageWhenKill
    public int GetDamageWhenKill()
    {
        return DamageWhenKill;
    }

    // ตรวจจับ PowerUp และทำลายมัน
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("เก็บ PowerUp!");
            Destroy(other.gameObject); // ทำลาย PowerUp
            hasPowerUp = true; // ตั้งค่า PowerUp เป็น true
            powerUpDuration = 5f; // ตั้งเวลาของ PowerUp ใหม่
        }
    }
}
