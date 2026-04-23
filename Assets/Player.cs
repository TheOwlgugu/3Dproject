using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{

    public Rigidbody rb;

    [Header("MOVE")]
    private float Xinput;
    private float Zinput;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Speed = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Player_Move();
    }


    public void Player_Move()
    {
        Xinput = Input.GetAxis("Horizontal");
        Zinput = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(Xinput*Speed,rb.velocity.y, Zinput*Speed);

    }
}



