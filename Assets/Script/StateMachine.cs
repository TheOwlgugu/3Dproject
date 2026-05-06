using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public enum MainState { player, camera, waring, drone, replay }
    [Header("Basic")]
    public MainState state;
    private Player player;


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
                if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("B_KEY"))
                {
                    state = MainState.player;
                    break;
                }

                break;
            
        }

    }
}
