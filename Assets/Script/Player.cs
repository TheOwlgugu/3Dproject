using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("BASIC")]
    public Rigidbody rb;
    public Camera_Script player_camera;
    public bool Use_Controller;
    public bool Use_Player;

    [Header("MOVE")]
    public float Speed;
    private float Xinput;
    private float Zinput;
    private float Yinput;

    // 用于存储水平方向的目标速度（不包含Y轴）
    private Vector3 horizontalVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Speed = 20;
        Use_Controller = true;
        Use_Player = true;

    }

    void Update()
    {
        Use_Switch();
        if (Use_Player)
        {
            Player_Move();            // 计算水平速度
            Player_Move_upanddown();  // 合并垂直速度并应用
        }
    }

    // 水平移动（相对于摄像机的方向）
    public void Player_Move()
    {
        
            Xinput = Input.GetAxis("Horizontal");
            Zinput = Input.GetAxis("Vertical");

            // 获取摄像机的右方向和前方向（忽略俯仰，只取水平）
            Vector3 camRight = player_camera.transform.right;
            Vector3 camForward = player_camera.transform.forward;
            camRight.y = 0f;
            camForward.y = 0f;
            camRight.Normalize();
            camForward.Normalize();

            // 计算移动方向并归一化（防止斜向更快）
            Vector3 moveDir = (camRight * Xinput + camForward * Zinput).normalized;
            horizontalVelocity = moveDir * Speed;
        
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

        if (Use_Controller)
        {
            Yinput = (Input.GetAxis("RT_AXIS") - Input.GetAxis("LT_AXIS"));
        }

        // 合并水平速度和垂直速度，一次性赋给 Rigidbody
        Vector3 finalVelocity = new Vector3(horizontalVelocity.x, Yinput * Speed, horizontalVelocity.z);
        rb.velocity = finalVelocity;
    }

    public void Use_Switch()
    {
        if (Input.GetButtonDown("Y_KEY") || Input.GetKeyDown(KeyCode.S))
        {
            Use_Player = !Use_Player;
            rb.velocity = Vector3.zero;
            horizontalVelocity = Vector3.zero;
        }
    }
}