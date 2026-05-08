using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("BASIC")]
    public Player player;
    public bool AutoSaving;
    public float CameraSpeed;
    private StateMachine stateMachine;
    private float xInput, yInput, zInput;
    public float LookSpeed = 0.3f;  // 右摇杆旋转灵敏度
    public LocationMessage LocationMes;

    public struct LocationMessage
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public LocationMessage(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }
    }

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
            CameraRotate(Input.GetAxis("RS_H"), Input.GetAxis("RS_V"));
        }

        if(stateMachine.state == StateMachine.MainState.drone)
        {
            
            transform.position = player.transform.position;
            CameraRotate(Input.GetAxis("Horizontal"), 0);
        }
    }

    // 水平移动（相对于摄像机当前朝向）
    private void CameraMove()
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

        // 键盘控制：A键上升，Z键下降（保留你原来的按键）
        if (Input.GetKey(KeyCode.A) || Input.GetButton("RB_KEY"))
        {
            yInput = 1;
        }
        else if (Input.GetKey(KeyCode.Z) || Input.GetButton("LB_KEY"))
        {
            yInput = -1;
        }
        else
        {
            yInput = 0;
        }
        // 只修改 Y 轴位置
        Vector3 upMove = Vector3.up * yInput * CameraSpeed * Time.deltaTime;
        transform.position += upMove;
    }

    // 右摇杆旋转视角
    private void CameraRotate(float rightX, float rightY)
    {
        // 计算本次的旋转增量（度）
        float yawDelta = rightX * LookSpeed;      // 水平偏航
        float pitchDelta = rightY * LookSpeed * 0.5f; // 垂直俯仰

        // 应用旋转
        transform.Rotate(0, yawDelta, 0, Space.World);        // 绕世界 Y 轴旋转
        transform.Rotate(pitchDelta, 0, 0, Space.Self);      // 绕自身 X 轴旋转（注意正负号）

        // 限制俯仰角（防止翻转）
        Vector3 angles = transform.eulerAngles;
        // 将角度转换到 -180~180 范围，方便 clamping
        float pitch = angles.x;
        if (pitch > 180) pitch -= 360;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        angles.x = pitch;
        transform.eulerAngles = angles;
    }

    public void CameraPositionSave()
    {
        if (AutoSaving)
        {
            LocationMes = new LocationMessage(transform.position, transform.rotation);
            Debug.Log("相机位置信息已保存");
        }
    }

    public void CameraPositionLoading()
    {
        if(AutoSaving)
        {
            Debug.Log("载入相机位置");
            transform.position = LocationMes.Position;
            transform.rotation = LocationMes.Rotation;
        }
    }

    public void CameraSetXrotation()
    {
        Debug.Log("SetX=0");
        // 获取当前欧拉角，但只修改 X 分量，然后重新生成四元数
        Vector3 angles = transform.eulerAngles;
        angles.x = 0;
        transform.rotation = Quaternion.Euler(angles);
    }
}