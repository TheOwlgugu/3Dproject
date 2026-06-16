using UnityEngine;

public class DigitalMan : MonoBehaviour
{
    [Header("闲逛参数")]
    public float wanderRadius = 8f;           // 随机移动半径
    public float wanderInterval = 2f;         // 到达后停留时间（秒）
    public float moveSpeed = 2f;              // 移动速度
    public float rotateSpeed = 120f;          // 旋转速度（度/秒）
    public float stoppingDistance = 0.05f;    // 到达目标点的距离阈值（更小）
    public LayerMask groundLayer;             // 可走路的层

    private Vector3 targetPosition;
    private bool isRotating = true;
    private float idleTimer;
    private bool isIdle = true;

    private Vector3 lastTarget; // 用于 Gizmos

    public Animator anim;
    public bool shouldWalk;

    void Start()
    {
        shouldWalk = false;
        PickNewTarget();
    }

    void Update()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= wanderInterval)
            {
                isIdle = false;
                isRotating = true;
                PickNewTarget();
                idleTimer = 0f;
            }
            return;
        }

        if (isRotating)
        {
            Vector3 dir = targetPosition - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude < 0.01f)
            {
                isRotating = false;
                return;
            }
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRot) < 1f)
            {
                isRotating = false;
            }
        }
        else
        {
            // 使用 MoveTowards 确保精确到达，不会穿越
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 到达检查
            if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
            {
                // 精确对齐到目标点，防止微小偏移
                transform.position = targetPosition;
                isIdle = true;
            }
        }
        shouldWalk = !isIdle && !isRotating;
        anim.SetBool("walk", shouldWalk);
    }

    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 randomPoint = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

        RaycastHit hit;
        if (Physics.Raycast(randomPoint + Vector3.up * 20f, Vector3.down, out hit, 40f, groundLayer))
        {
            targetPosition = hit.point;
            lastTarget = targetPosition;
        }
        else
        {
            targetPosition = transform.position;
            lastTarget = transform.position;
            isIdle = true;
            isRotating = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);

        if (lastTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastTarget, 0.3f);
            Gizmos.DrawLine(transform.position, lastTarget);
        }
    }

    // 可选：按 F2 调试
    private void UpdateDebug()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log($"[DigitalMan] 状态: isIdle={isIdle}, isRotating={isRotating}, 目标点={targetPosition}, 距离={Vector3.Distance(transform.position, targetPosition)}");
        }
    }
}