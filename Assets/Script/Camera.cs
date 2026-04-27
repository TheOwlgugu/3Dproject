using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    [Header("BASIC")]
    public Player player;
    public float Camera_Speed;
    private float Xinput, Yinput, Zinput;
    private float CameraPitch = 0f;
    private float CameraYaw = 0f;
    public float LookSpeed = 0.3f;  // 右摇杆旋转灵敏度

    void Start()
    {
        Camera_Speed = 15.0f;
    }

    void Update()
    {
        if (!player.Use_Player)
        {
            // 移动：水平方向跟随摄像机朝向
            Camera_Move();
            // 升降：独立处理（世界Y轴）
            Camera_Move_upanddown();
            // 旋转：右摇杆控制
            Camera_Rotate();
        }
    }

    // 水平移动（相对于摄像机当前朝向）
    public void Camera_Move()
    {
        Xinput = Input.GetAxis("Horizontal");
        Zinput = Input.GetAxis("Vertical");

        // 使用摄像机的右方向和前方向构建移动方向
        Vector3 moveDirection = transform.right * Xinput + transform.forward * Zinput;
        moveDirection.y = 0;               // 确保不影响上下
        moveDirection.Normalize();          // 防止斜向更快

        Vector3 newPosition = transform.position + moveDirection * Camera_Speed * Time.deltaTime;
        transform.position = newPosition;
    }

    // 上下升降（世界Y轴，不受摄像机倾斜影响）
    private void Camera_Move_upanddown()
    {
        if (player.Use_Controller)
        {
            Yinput = (Input.GetAxis("RT_AXIS") - Input.GetAxis("LT_AXIS"));
        }
        else
        {
            // 键盘控制：A键上升，Z键下降（保留你原来的按键）
            if (Input.GetKey(KeyCode.A))
                Yinput = 1;
            else if (Input.GetKey(KeyCode.Z))
                Yinput = -1;
            else
                Yinput = 0;
        }

        // 只修改 Y 轴位置
        Vector3 upMove = Vector3.up * Yinput * Camera_Speed * Time.deltaTime;
        transform.position += upMove;
    }

    // 右摇杆旋转视角
    private void Camera_Rotate()
    {
        float rightX = Input.GetAxis("RS_H");    // 右摇杆水平
        float rightY = Input.GetAxis("RS_V");    // 右摇杆垂直

        CameraYaw += rightX * LookSpeed;
        CameraPitch += rightY * LookSpeed * 0.5f;   // 垂直灵敏度略低

        CameraPitch = Mathf.Clamp(CameraPitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(CameraPitch, CameraYaw, 0);
    }
}