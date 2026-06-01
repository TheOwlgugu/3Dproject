using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosCenter: MonoBehaviour
{
    [Header("BASIC")]
    public Player player;
    public CameraScript CameraScript;
    private SphereCollider playerCollider;

    private void InitAll()
    {
        player = GetComponent<Player>();
        CameraScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
        playerCollider = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        InitAll();
        player_EwallCheck();
        player_FrontCheck();
    }

    private void player_EwallCheck()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, player.CheckRadius);
    }
    
    private void player_FrontCheck()
    {
        Vector3 CameraRight = CameraScript.transform.right;
        Vector3 CameraForward = CameraScript.transform.forward;
        Vector3 Move_Direction = (CameraRight * player.XInput + CameraForward * player.ZInput).normalized;
        Move_Direction = (Move_Direction + Vector3.down * 0.5f).normalized; 
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position , Move_Direction  * (playerCollider.radius + 0.1f));
    }
}
