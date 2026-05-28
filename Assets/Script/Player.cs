using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static Player;

public class Player : MonoBehaviour
{
    [Header("BASIC")]
    public Rigidbody rb;
    public CameraScript PlayerCamera;
    public bool UsePlayer;
    private StateMachine stateMachine;

    [Header("MOVE")]
    public float Speed;
    private float Xinput,Zinput,Yinput;    
    private Vector3 HorizontalVelocity;// 用于存储水平方向的目标速度（不包含Y轴）
    private Vector3 FinalVelocity; //最终速度  

    private Vector3 CameraRight, CameraForward;


    [Header("E_WALL")]
    public LayerMask eWallLayer;        // 在 Inspector 中勾选 e_Wall 层
    private float checkRadius;      // 检测半径
    private float warningRadius;

    public struct EwallCheckMes
    {
        public float dis;
        public Vector3 dir;

        public  EwallCheckMes(float dis,Vector3 dir)
        {
            this.dis = dis;
            this.dir = dir;
        }

    }
    public EwallCheckMes ewallCheckMes;

    //AUTOBACK
    private bool startAutoBack;
    public bool WarningDown;

    [Header("WALK")]
    private SphereCollider playerCollider;
    public LayerMask RoadLayer;


    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
        Speed = 30f;
        UsePlayer = true;
        checkRadius = 10f;
        warningRadius = 1.5f;
        ewallCheckMes = new EwallCheckMes();
        startAutoBack = false;
        WarningDown = false;
    }

    void Update()
    {
        if (stateMachine.state == StateMachine.MainState.player)
        {
            Player_Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));            // 计算水平速度
            Player_Move_upanddown(
                (Input.GetKey(KeyCode.A) || Input.GetButton("RB_KEY")),
                (Input.GetKey(KeyCode.Z) || Input.GetButton("LB_KEY"))
                );
            FinalSpeed();
           
        }

        if(stateMachine.state == StateMachine.MainState.drone)
        {
            Player_Move(Input.GetAxis("RS_H"), -Input.GetAxis("RS_V"));            // 计算水平速度
            Player_Move_upanddown((Input.GetAxis("Vertical") > 0) ,(Input.GetAxis("Vertical") < 0));
            FinalSpeed();
            PrepareWarning();
        }

        if (stateMachine.state == StateMachine.MainState.walk)
        {
            Player_Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Yinput = -0.5f;
            FinalSpeed();
        }

        if (stateMachine.state == StateMachine.MainState.warning)
        {
            DroneAutoBack();
        }       
    }

    // 水平移动（相对于摄像机的方向）
    private void Player_Move(float xinput,float zinput)
    {  
            Xinput = xinput;
            Zinput = zinput;
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

     private void Player_Move_upanddown(bool up,bool down)
    {
        // 上下输入
        if ( up ){
            Yinput = 1;
        }else if ( down ){
            Yinput = -1;
        }else{
            Yinput = 0;
        }    
    }

    private void FinalSpeed()
    {
        // 合并水平速度和垂直速度，一次性赋给 Rigidbody
        FinalVelocity = new Vector3(HorizontalVelocity.x, Yinput * Speed, HorizontalVelocity.z);
        rb.velocity = FinalVelocity;
    }

    public bool CheckNearEWall()//用于检测无人机是否接近电子围墙
    {
        // 在玩家位置周围进行球形检测，只检测 airWallLayer 指定的层
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, eWallLayer);
        return hitColliders.Length > 0;
    }


    public bool WarningSingnal()
    {
        if(CheckNearEWall() && (ewallCheckMes.dis <= warningRadius))
        {
            return true;
        }
        return false;
    }

    private void PrepareWarning()
    {
        if (CheckNearEWall())
        {
            ewallCheckMes = GetMinWallDisDir();
        }
    }

    private EwallCheckMes GetMinWallDisDir()
    {
        
        EwallCheckMes ewallCheckMes = new EwallCheckMes();
        float minDist = float.MaxValue;
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, 50f, eWallLayer))
            {
                if (hit.distance < minDist)
                {
                    minDist = hit.distance;
                    ewallCheckMes.dir = dir;
                }
            }
        }
        ewallCheckMes.dis = (minDist == float.MaxValue ? -1f : minDist);
        return ewallCheckMes;
    }

    private void DroneAutoBack()
    {
        if (Input.GetButtonDown("L3_KEY"))
        {
            startAutoBack = true;
            
        }
        if (startAutoBack && Physics.Raycast(transform.position, ewallCheckMes.dir, checkRadius, eWallLayer))
        {
            PlayerCamera.transform.position = transform.position;//摄像机跟随
            rb.velocity = -ewallCheckMes.dir;
        }
        else if(startAutoBack)
        {
            WarningDown = true;
            rb.velocity = Vector3.zero;
            startAutoBack = false;
        }
    }

    public bool RoadCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerCollider.radius+0.1f, RoadLayer))
        { 
            return true;
        }
            return false;
    }

    // 可选：在 Scene 视图中可视化检测范围（调试用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }


}