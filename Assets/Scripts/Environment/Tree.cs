using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tree : Interactable
{
    [SerializeField] private Waypoint _actionPoint;
    [SerializeField] private float _endFwdMagMinStep = 1;
    [SerializeField] private float _endFwdMagMaxStep = 3;
    [SerializeField] private float _endFwdMagnitude = 10;
    [SerializeField] private float _endFwdMagnitudeClose = 1;

    private bool _interacted;
    //private bool _animFinished;
    //private WalkTurn _walkNTurn = new WalkTurn();
    //private Waypoint _waypoint = new Waypoint();
    //private Transform _player;
    //private Transform _playerMain;
    //private Animator _playerAnim;
    //private float _moveMult;

    private PlayerPositioner _playerPositioner;

    protected override void Start()
    {
        base.Start();
        //_playerMain = FindObjectOfType<ThirdPersonCharacter>().transform;
        //_player = FindObjectOfType<CharacterAnimControl>().transform;
        //_playerAnim = FindObjectOfType<CharacterAnimControl>().GetComponent<Animator>();
        //_moveMult = _playerMain.GetComponent<PlayerController>().MoveValMultiplier;

        _playerPositioner = FindObjectOfType<PlayerPositioner>();
    }

    public override void Interact()
    {
        if (_interacted) return;
        GameManager.instance.StartInteractSession();
        //if(!_interacted && !_animFinished) _interacted = true;

        Vector3 startDir = _actionPoint.Position - _playerPositioner.transform.position;

        _playerPositioner.MoveToPos(
            _playerPositioner.transform.position,
            startDir.normalized, 
            _actionPoint.Position, 
            Quaternion.Euler(_actionPoint.Rotation) * Vector3.forward * Mathf.Lerp(_endFwdMagnitudeClose, _endFwdMagnitude, Mathf.InverseLerp(_endFwdMagMinStep, _endFwdMagMaxStep, startDir.magnitude)), 
            0.5f,
            () => {
                //GameManager.instance.StartCutscene();
                CutsceneManager.instance.PlayClip(0, OnCompleteInteract); }
        );
    }

    public void OnCompleteInteract()
    {
        GameManager.instance.EndInteractSession();
        _interacted = true;
    }

    //private void Update()
    //{
    //    if (!_animFinished && _interacted)
    //    {
    //        bool reached = _walkNTurn.Move(_playerMain, _playerAnim, _moveMult, _actionPoint, "Forward", "Turn");
    //        if (reached)
    //        {
    //            _interacted = false;
    //            GameManager.instance.EndInteractSession();
    //        }
    //    }
    //}
}


[CustomEditor(typeof(Tree))]
public class TreeEditor : InteractableEditor
{
    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}