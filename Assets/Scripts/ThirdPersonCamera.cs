using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // ตัวละครที่กล้องจะติดตาม
    public float distance = 15.0f; // ระยะห่างจากตัวละคร
    public float height = 4.0f; // ความสูงของกล้อง
    public float rotationSpeed = 3.0f; // ความเร็วในการหมุน

    private float yaw = 0f; // มุมหมุนแนวนอน
    private float pitch = 10f; // มุมเงยก้ม

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("กล้องไม่มี Target! โปรดกำหนดตัวละครใน Inspector");
            return;
        }
        yaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // รับค่า Input การหมุนกล้องจากเมาส์
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f); // จำกัดมุมเงยก้ม

        // คำนวณตำแหน่งกล้องตามมุมที่กำหนด
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = new Vector3(0, height, -distance);
        Vector3 newPosition = target.position + rotation * offset;

        // อัปเดตตำแหน่งของกล้อง
        transform.position = newPosition;

        // ทำให้กล้องมองไปที่ตัวละคร
        transform.LookAt(target.position + Vector3.up * height * 0.5f);

        // หมุนตัวละครให้หันไปตามทิศทางของกล้อง
        target.rotation = Quaternion.Euler(0, yaw, 0);
    }
}
