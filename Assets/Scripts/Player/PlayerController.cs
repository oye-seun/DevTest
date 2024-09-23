using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.tvOS;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    //private ThirdPersonCharacter _tpc;
    public float MoveValMultiplier = 1;

    [SerializeField] float _movingTurnSpeed = 360;
    [SerializeField] float _stationaryTurnSpeed = 180;
    [SerializeField] float _groundCheckDistance = 0.1f;
    [SerializeField] Animator _animator;

    [Header("Gameplay / Controller params")]
    [SerializeField] private float _gameplayTurnSpeed = 15f;
    [SerializeField] private float _animTurnDamping = 0.1f;
    [SerializeField] private float _wGameTurnSpeedRate = 2.5f;
    [Range(0, 1)] [SerializeField] private float _controllerThresh = 0.25f;
    private float _workingGameTurnSpeed;
    //public bool TransmitMotion = true;
    private PlayerControllerStates _state;

    public float StationaryTurnSpeed => _stationaryTurnSpeed;
    bool m_IsGrounded;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;


    private void OnEnable()
    {
        //GameManager.GameStateChanged += OnGameStateChanged;
        Player.OnPlayerStateChanged += OnPlayerStateChanged;
        CharacterAnimControl.AnimatorMove += HandleAnimMove;
        PlayerPositioner.Move += Move;
        PlayerPositioner.Turn += Turn;

        //CutsceneManager.CutsceneStarted += SetTransmitMotionFalse;
        //CutsceneManager.CutsceneEnded += SetTransmitMotionTrue;
        CutsceneManager.CutsceneStarted += SetStateOnCutsceneStarted;
        CutsceneManager.CutsceneEnded += SetStateOnCutsceneEnded;
        SimplePlayerPositioner.PositionerStarted += SetStateOnPositionerStarted;
        SimplePlayerPositioner.PositionerEnded += SetStateOnCutsceneEnded;
    }

    private void OnDisable()
    {
        //GameManager.GameStateChanged -= OnGameStateChanged;
        Player.OnPlayerStateChanged += OnPlayerStateChanged;
        CharacterAnimControl.AnimatorMove -= HandleAnimMove;
        PlayerPositioner.Move -= Move;
        PlayerPositioner.Turn -= Turn;

        //CutsceneManager.CutsceneStarted -= SetTransmitMotionFalse;
        //CutsceneManager.CutsceneEnded -= SetTransmitMotionTrue;
        CutsceneManager.CutsceneStarted -= SetStateOnCutsceneStarted;
        CutsceneManager.CutsceneEnded -= SetStateOnCutsceneEnded;
        SimplePlayerPositioner.PositionerStarted -= SetStateOnPositionerStarted;
        SimplePlayerPositioner.PositionerEnded -= SetStateOnCutsceneEnded;
    }

    // Start is called before the first frame update
    void Start()
    {
        //_tpc = GetComponent<ThirdPersonCharacter>();
        _animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.GameState == GameState.Playing) HandleInput(PlayerInputs.instance.MoveVal());

        switch (Player.instance.State)
        {
            case PlayerStates.PlayerControlled:
                HandleInput(PlayerInputs.instance.MoveVal());
                break;
            case PlayerStates.Interacting:
                HandleInput(Vector3.zero);
                break;
        }
        //else Move(Vector2.zero);
    }

    private void HandleInput(Vector2 move)
    {
        //if (move.magnitude < 0.2f) return;
        Quaternion rot = Quaternion.Euler(0, GameManager.instance.cam.transform.eulerAngles.y, 0);
        Vector3 relMove = (move.y * (rot * Vector3.forward)) + (move.x * (rot * Vector3.right));
        //Debug.Log("move input: " + move);
        ///*_tpc.*/Move(new Vector3(relMove.x * MoveValMultiplier, 0, relMove.z * MoveValMultiplier), false/*, false*/);
        
        ControllerMove(new Vector3(relMove.x, 0, relMove.z));
    }

    //private void ResetMoveOnInteract(GameState currentState, GameState nextState)
    //{
    //    if(nextState == GameState.Interacting) Move(Vector3.zero);
    //    Debug.Log("interacting");
    //}


    public void ControllerMove(Vector3 move)
    {
        if (move.magnitude >= _controllerThresh)
        {   
            if (Vector3.Dot(move.normalized, transform.forward) < 0f)
            {
                move = move.normalized;
                move = transform.InverseTransformDirection(move);
                CheckGroundStatus();
                move = Vector3.ProjectOnPlane(move, m_GroundNormal);
                float turnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
                _workingGameTurnSpeed = 0;
                _animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            }
            else
            {
                transform.forward = Vector3.Lerp(transform.forward, move.normalized, _workingGameTurnSpeed * Time.deltaTime);
                _animator.SetFloat("Turn", 0, _animTurnDamping, Time.deltaTime);
                _animator.SetFloat("Forward", move.magnitude * MoveValMultiplier, 0.1f, Time.deltaTime);
                _workingGameTurnSpeed = Mathf.Lerp(_workingGameTurnSpeed, _gameplayTurnSpeed, _wGameTurnSpeedRate * Time.deltaTime);
            }
        }
        else
        {
            _animator.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
            _animator.SetFloat("Turn", 0, _animTurnDamping, Time.deltaTime);
        }
        
    }

    public void Move(Vector3 move, /*bool crouch, bool jump,*/ bool back = false)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);

        if (!back)
        {
            m_ForwardAmount = Mathf.Clamp01(move.z);
            m_TurnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
        }
        else
        {
            m_ForwardAmount = Mathf.Clamp(move.z, -1, 0);
            m_TurnAmount = (move.x != 0 && move.z != 0) ? Mathf.Clamp(Mathf.Atan2(-move.x, -move.z), -1, 1) : 0;
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }


    public void Turn(Vector3 move, float speedFactor)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);

        m_TurnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1, 1);
        m_ForwardAmount = 0;
        _animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

        float turnSpeed = _stationaryTurnSpeed * speedFactor;
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        _animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        _animator.SetFloat("Turn", m_TurnAmount, /*0.1f*/ _animTurnDamping, Time.deltaTime);
        _animator.SetBool("OnGround", m_IsGrounded);
    }


    public void HandleAnimMove(Vector3 deltaPos, Quaternion deltaRot)
    {
        //Debug.Log("deltaPos: " + deltaPos);
        //if (!TransmitMotion)
        //{
        //    //Debug.Log("false transit otion");
        //    _animator.ApplyBuiltinRootMotion();
        //    //m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, Vector3.zero, Time.deltaTime * 7.5f);
        //    return;
        //}

        ////m_Animator.ApplyBuiltinRootMotion();
        //_animator.applyRootMotion = true;
        //transform.position += deltaPos;
        //transform.rotation *= deltaRot;


        switch (_state)
        {
            case PlayerControllerStates.cutscene:
                _animator.ApplyBuiltinRootMotion();
                break;
            case PlayerControllerStates.controlled:
                _animator.applyRootMotion = true;
                transform.position += deltaPos;
                transform.rotation *= deltaRot;
                break;
            case PlayerControllerStates.walking:
                // do nothing
                break;
        }

    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        #if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.4f), transform.position + (Vector3.up * 0.4f) + (Vector3.down * _groundCheckDistance));
        #endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.4f), Vector3.down, out hitInfo, _groundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            //m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            //m_Animator.applyRootMotion = false;
        }
    }

    //private void SetTransmitMotionTrue ()=> TransmitMotion = true;
    //private void SetTransmitMotionFalse ()=> TransmitMotion = false;

    private void SetStateOnCutsceneStarted( ) => _state = PlayerControllerStates.cutscene;
    private void SetStateOnCutsceneEnded( ) => _state = PlayerControllerStates.controlled;
    private void SetStateOnPositionerStarted( ) => _state = PlayerControllerStates.walking;

    private void OnPlayerStateChanged(PlayerStates state)
    {
        switch (state)
        {
            case /*GameState.Interacting:*/ PlayerStates.GameControlled:
                //_animator.SetBool("useComplexLocomotion", true);
                break;
            case /*GameState.Playing:*/ PlayerStates.PlayerControlled:
                //_animator.SetBool("useComplexLocomotion", false);
                break;
        }
    }


    public enum PlayerControllerStates { controlled, walking, cutscene}
}

