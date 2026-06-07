using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Basic")]
    public Rigidbody cube;
    [Header("Move")]
    public float Speed=10f;
    public float horizontalInput;
    public float verticalInput;
    public float updownInput;
    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out cube))
        Debug.LogError("Move 脚本需要挂载的物体拥有 Rigidbody 组件！");
        cube.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Z))
        {
            updownInput = 1f;
        }
        else if(Input.GetKey(KeyCode.X))
        {
            updownInput = -1f;
        }
        else
        {
            updownInput = 0f;
        }
    }
     void FixedUpdate()
    {
        if (cube.isKinematic)
            return;
        Vector3 horizontalDir = new Vector3(horizontalInput, 0, verticalInput);
        if (horizontalDir.magnitude > 0.1f)
            horizontalDir.Normalize();
        Vector3 newVel = cube.velocity;
        newVel.x = horizontalDir.x * Speed;
        newVel.z = horizontalDir.z * Speed;
        if (Mathf.Abs(updownInput) > 0.01f)
            newVel.y = updownInput * Speed;

        cube.velocity = newVel;
    }
}
