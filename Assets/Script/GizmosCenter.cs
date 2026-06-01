using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosCenter: MonoBehaviour
{
    public float rayLength = 5f;

    private void OnDrawGizmos()
    {
        // 从物体位置向前方绘制一条红色射线
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);

        // 或者用 DrawLine（也可以绘制带起止点的线）
        Vector3 endPoint = transform.position + transform.forward * rayLength;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
