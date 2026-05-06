using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("BASIC")]
    public Player player;
    [SerializeField]private StateMachine stateMachine;
    public float CameraSpeed;
    private float xInput, yInput, zInput;
    private float cameraPitch = 0f;
    private float cameraYaw = 0f;
    public float LookSpeed = 0.3f;  // 右摇杆旋转灵敏度

    void Start()
    {
        CameraSpeed = 15.0f;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        stateMachine = GameObject.FindWithTag("Player").GetComponent<StateMachine>();
    }

    void Update()
    {
        if(stateMachine.state == StateMachine.MainState.camera )
        {
            // 移动：水平方向跟随摄像机朝向
            CameraMove();
            // 升降：独立处理（世界Y轴）
            CameraMove_upanddown();
            // 旋转：右摇杆控制
            CameraRotate();
        }
    }

    // 水平移动（相对于摄像机当前朝向）
    public void CameraMove()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        // 使用摄像机的右方向和前方向构建移动方向
        Vector3 moveDirection = transform.right * xInput + transform.forward * zInput;
        moveDirection.y = 0;               // 确保不影响上下
        moveDirection.Normalize();          // 防止斜向更快

        Vector3 newPosition = transform.position + moveDirection * CameraSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    // 上下升降（世界Y轴，不受摄像机倾斜影响）
    private void CameraMove_upanddown()
    {
        if (player.UseController)
        {
            yInput = (Input.GetAxis("RT_AXIS") - Input.GetAxis("LT_AXIS"));
        }
        else
        {
            // 键盘控制：A键上升，Z键下降（保留你原来的按键）
            if (Input.GetKey(KeyCode.A))
                yInput = 1;
            else if (Input.GetKey(KeyCode.Z))
                yInput = -1;
            else
                yInput = 0;
        }

        // 只修改 Y 轴位置
        Vector3 upMove = Vector3.up * yInput * CameraSpeed * Time.deltaTime;
        transform.position += upMove;
    }

    // 右摇杆旋转视角
    private void CameraRotate()
    {
        float rightX = Input.GetAxis("RS_H");    // 右摇杆水平
        float rightY = Input.GetAxis("RS_V");    // 右摇杆垂直

        cameraYaw += rightX * LookSpeed;
        cameraPitch += rightY * LookSpeed * 0.5f;   // 垂直灵敏度略低

        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);

        transform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
    }
}