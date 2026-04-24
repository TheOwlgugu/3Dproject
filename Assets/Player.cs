using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{

    public Rigidbody rb;

    [Header("MOVE")]
    private float Xinput;
    private float Zinput;
    private float Yinput;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Speed = 20;
        Debug.Log("kaishi");
    }

    // Update is called once per frame
    void Update()
    {
        Player_Move();
        Player_Move_upanddown();
        Debug.Log(Xinput);
    }


    public void Player_Move()
    {
        Xinput = Input.GetAxis("Horizontal");
        Zinput = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(Xinput*Speed,0, Zinput*Speed);

    }


    public void Player_Move_upanddown()
    {
        if(Input.GetKey(KeyCode.A))
        {
            Yinput = 1;
        }
        else if(Input .GetKey (KeyCode.Z))
        {
            Yinput = -1;
        }
        else
        {
            Yinput = 0;
        }
        rb.velocity = new Vector3(Xinput * Speed, Yinput * Speed, Zinput * Speed);
    }
}



