using UnityEngine;

public class SimpleWallGizmo : MonoBehaviour
{
    [Header("手动输入碰撞箱大小（世界单位）")]
    public float length;   // X轴长度
    public float height;  // Y轴高度
    public float thickness; // Z轴厚度
    private BoxCollider boxCollider;

    public Color gizmoColor = Color.green;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }  

    private void Update()
    {
        length = boxCollider.size.x;
        height = boxCollider.size.y;
        thickness = boxCollider.size.z;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 size = new Vector3(length, height, thickness);
        Gizmos.DrawWireCube(transform.position, size);
    }
}