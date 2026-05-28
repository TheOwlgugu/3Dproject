using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public enum MainState { player, camera, warning, drone, replay, walk }
    [Header("BASIC")]
    public MainState state;
    private Player player;
    private CameraScript maincamera;
    public TextMeshProUGUI uiText;   // UI 文本（Canvas 下的）
    public Rigidbody rb;
    public bool UseEwall;           //是否检测电子围墙



    void Start()
    {
        state = MainState.player;
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
        maincamera  = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
        uiText = GameObject.FindWithTag("e_wall_UI").GetComponentInChildren<TextMeshProUGUI>();
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
                    Debug.Log("进入摄像机模式");
                    break;
                }

                if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("B_KEY"))
                {
                    state = MainState.drone;
                    player.rb.velocity = Vector3.zero;
                    maincamera.CameraSetXrotation();
                    maincamera.CameraPositionSave();
                    Debug.Log("进入无人机模式");
                    break;
                }

                if(Input.GetButtonDown("X_KEY")&&player.RoadCheck())
                {
                    state = MainState.walk;
                    player.rb.velocity = Vector3.zero;
                    rb.useGravity = true;
                    break;

                }
                else if (Input.GetButtonDown("X_KEY"))
                {
                    Debug.Log("进入walk模式失败！");
                    break;
                }
                    break;

            case MainState.camera :
                if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Y_KEY"))
                {
                    state = MainState.player;
                    Debug.Log("进入默认模式");
                    break;
                }
               break;

            case MainState.drone :
                EwallCheck();
                if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("B_KEY"))
                {
                    state = MainState.player;
                    maincamera.CameraPositionLoading();//摄像机自动返回保存位置
                    Debug.Log("进入默认模式");
                    break;
                }
                if (player.WarningSingnal())
                {
                    state = MainState.warning;
                    player.rb.velocity = Vector3.zero;
                    Debug.Log("警告！已停止飞行操作,按下回航键返回");
                }
                break;
             
            case MainState.warning :
                if (player.WarningDown)
                {
                    state = MainState.drone;
                    player.WarningDown = false;
                }

                break;

            case MainState.walk :
                if (Input.GetButtonDown("X_KEY"))
                {
                    state = MainState.player;
                    player.rb.velocity = Vector3.zero;
                }


                break;
        }

    }

    public void EwallCheck()
    {
        // 每帧检测是否靠近空气墙
        bool isNearAirWall =player.CheckNearEWall();
        if (isNearAirWall && UseEwall)
        {
            uiText.enabled = true;
            //Debug.Log("靠近电子围墙了！");
        }
        else
        {
            uiText.enabled = false;
        }
    }

}
