using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.tvOS;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoSingleton<PlayerController>
{
    private ThirdPersonCharacter _tpc;
    public float MoveValMultiplier = 1;


    private void OnEnable()
    {
        //GameManager.GameStateChanged += ResetMoveOnInteract;
    }

    private void OnDisable()
    {
        //GameManager.GameStateChanged -= ResetMoveOnInteract;
    }

    // Start is called before the first frame update
    void Start()
    {
        _tpc = GetComponent<ThirdPersonCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GameState == GameState.Playing) Move(PlayerInputs.instance.MoveVal());
        //else Move(Vector2.zero);
    }

    private void Move(Vector2 move)
    { 
        Quaternion rot = Quaternion.Euler(0, GameManager.instance.cam.transform.eulerAngles.y, 0);
        Vector3 relMove = (move.y * (rot * Vector3.forward)) + (move.x * (rot * Vector3.right));
        //Debug.Log("move input: " + move);
        _tpc.Move(new Vector3(relMove.x * MoveValMultiplier, 0, relMove.z * MoveValMultiplier), false, false);
    }

    //private void ResetMoveOnInteract(GameState currentState, GameState nextState)
    //{
    //    if(nextState == GameState.Interacting) Move(Vector3.zero);
    //    Debug.Log("interacting");
    //}
}
