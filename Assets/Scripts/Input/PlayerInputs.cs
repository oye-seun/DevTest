using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoSingleton<PlayerInputs>
{
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _moveAction;
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = _playerInput.actions["move"];
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public Vector2 MoveVal()
    {
        return _moveAction.ReadValue<Vector2>();
    }
}
