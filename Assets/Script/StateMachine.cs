using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public enum MainState { player, camera, waring, drone, replay }
    [Header("BASIC")]
    public MainState state;
    private Player player;

    [Header("E_WALL")]
    public LayerMask eWallLayer;        // 在 Inspector 中勾选 e_Wall 层
    public float checkRadius = 3f;      // 检测半径
    public bool isNearAirWall = false;    // 是否靠近空气墙

    void Start()
    {
        state = MainState.player;
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
       
        switch (state)
        {
            case MainState.player :
                if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Y_KEY"))
                {
                    state = MainState.camera;
                    player.rb.velocity = Vector3.zero;
                    break;
                }

                if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("B_KEY"))
                {
                    state = MainState.drone;
                    player.rb.velocity = Vector3.zero;
                }
                break;

            case MainState.camera :
                if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Y_KEY"))
                {
                    state = MainState.player;
                    break;
                }
               break;

            case MainState.drone :
                EwallCheck();
                if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("B_KEY"))
                {
                    state = MainState.player;
                    break;
                }

                break;
            
        }

    }

    public void EwallCheck()
    {
        // 每帧检测是否靠近空气墙
        isNearAirWall = CheckNearAirWall();
        if (isNearAirWall)
        {
            // 靠近空气墙时的处理，例如减速、播放提示等
            Debug.Log("靠近电子围墙了！");
        }
    }

    public bool CheckNearAirWall()
    {
        // 在玩家位置周围进行球形检测，只检测 airWallLayer 指定的层
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, eWallLayer);
        return hitColliders.Length > 0;
    }

    // 可选：在 Scene 视图中可视化检测范围（调试用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

}
