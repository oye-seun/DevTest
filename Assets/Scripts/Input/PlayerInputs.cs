using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoSingleton<PlayerInputs>
{
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _backAction;
    // Start is called before the first frame update
    void Start()
    {
        _moveAction = _playerInput.actions["move"];
        _interactAction = _playerInput.actions["interact"];
        _backAction = _playerInput.actions["back"];
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public Vector2 MoveVal()
    {
        return _moveAction.ReadValue<Vector2>();
    }

    public bool InteractKeyPressed()
    {
        return _interactAction.IsPressed();
    }

    public bool BackKeyPressed()
    {
        return _backAction.IsPressed();
    }
}
