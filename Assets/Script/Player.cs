using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("BASIC")]
    public Rigidbody rb;
    public CameraScript PlayerCamera;
    public bool UseController;
    public bool UsePlayer;
    private StateMachine stateMachine;

    [Header("MOVE")]
    public float Speed;
    private float Xinput,Zinput,Yinput;    
    private Vector3 HorizontalVelocity;// 用于存储水平方向的目标速度（不包含Y轴）
    private Vector3 FinalVelocity; //最终速度  

    [Header("CAMERA")]
    public Vector3 CameraRight, CameraForward;

    

    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody>();
        Speed = 20;
        UseController = true;
        UsePlayer = true;

    }

    void Update()
    {
        if (stateMachine.state == StateMachine.MainState.player)
        {
            Player_Move();            // 计算水平速度
            Player_Move_upanddown();  // 合并垂直速度
            FinalSpeed();
        }
    }

    // 水平移动（相对于摄像机的方向）
    public void Player_Move()
    {
        
            Xinput = Input.GetAxis("Horizontal");
            Zinput = Input.GetAxis("Vertical");

            // 获取摄像机的右方向和前方向（忽略俯仰，只取水平）
            CameraRight = PlayerCamera.transform.right;
            CameraForward = PlayerCamera.transform.forward;
            CameraRight.y = 0f;
            CameraForward.y = 0f;
            CameraRight.Normalize();
            CameraForward.Normalize();

            // 计算移动方向并归一化（防止斜向更快）
            Vector3 Move_Direction = (CameraRight * Xinput + CameraForward * Zinput).normalized;
            HorizontalVelocity = Move_Direction * Speed;
    }

    public void Player_Move_upanddown()
    {
        // 上下输入（手柄扳机或键盘 A/Z）
        if (Input.GetKey(KeyCode.A))
        {
            Yinput = 1;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            Yinput = -1;
        }
        else
        {
            Yinput = 0;
        }

        if (UseController)
        {
            Yinput = (Input.GetAxis("RT_AXIS") - Input.GetAxis("LT_AXIS"));
        }

        
    }

    public void FinalSpeed()
    {
        // 合并水平速度和垂直速度，一次性赋给 Rigidbody
        FinalVelocity = new Vector3(HorizontalVelocity.x, Yinput * Speed, HorizontalVelocity.z);
        rb.velocity = FinalVelocity;
    }

    
}