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

    [SerializeField] private PlayerPositioner.Waypoint _interactPoint;

    private bool _interacted;
    //private bool _animFinished;
    //private WalkTurn _walkNTurn = new WalkTurn();
    //private Waypoint _waypoint = new Waypoint();
    //private Transform _player;
    //private Transform _playerMain;
    //private Animator _playerAnim;
    //private float _moveMult;

    //private PlayerPositioner _playerPositioner;

    protected override void Start()
    {
        base.Start();
        //_playerMain = FindObjectOfType<ThirdPersonCharacter>().transform;
        //_player = FindObjectOfType<CharacterAnimControl>().transform;
        //_playerAnim = FindObjectOfType<CharacterAnimControl>().GetComponent<Animator>();
        //_moveMult = _playerMain.GetComponent<PlayerController>().MoveValMultiplier;

        //_playerPositioner = FindObjectOfType<PlayerPositioner>();

        //_playerPositioner = Player.instance.playerPositioner;
    }

    public override void Interact()
    {
        if (_interacted) return;
        Player.instance.ChangeState(PlayerStates.GameControlled);

        //Vector3 startDir = _actionPoint.Position - Player.instance.transform.position;
        //Player.instance.simplePlayerPositioner.MoveToPos(
        //    Player.instance.transform.position,
        //    /*startDir.normalized,*/ Player.instance.transform.forward,
        //    _actionPoint.Position,
        //    Quaternion.Euler(_actionPoint.Rotation) * Vector3.forward * Mathf.Lerp(_endFwdMagnitudeClose, _endFwdMagnitude, Mathf.InverseLerp(_endFwdMagMinStep, _endFwdMagMaxStep, startDir.magnitude)),
        //    0.4f,
        //    () => {
        //        Player.instance.ChangeState(PlayerStates.PlayerControlled);
        //        //CutsceneManager.instance.PlayClip(0, OnCompleteInteract);
        //    }
        //);


        Vector3 startDir = _interactPoint.Position - Player.instance.transform.position;
        float startDirMagnitude = startDir.magnitude;
        Player.instance.simplePlayerPositioner.MoveToPos(
            Player.instance.transform.position,
            Player.instance.transform.forward  * startDirMagnitude * 0.3f,
            _interactPoint.Position,
            _interactPoint.Forward * /*_interactPoint.FwdStrength*/ startDirMagnitude * 0.6f,
            startDirMagnitude * Player.instance.simplePlayerPositioner.TimePerDistConstant,
            () => {
                Player.instance.ChangeState(PlayerStates.Interacting);
                CutsceneManager.instance.PlayClip(0, OnCompleteInteract);
            }
        );
    }

    public void OnCompleteInteract()
    {
        //GameManager.instance.EndInteractSession();
        Player.instance.ChangeState(PlayerStates.PlayerControlled);
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


#if UNITY_EDITOR
[CustomEditor(typeof(Tree))]
public class TreeEditor : InteractableEditor
{
    private SerializedProperty _interactPoint;
    private bool _showInteractHandle;

    protected override void OnEnable()
    {
        base.OnEnable();
        _interactPoint = serializedObject.FindProperty("_interactPoint");
    }
    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();

        // Show interact point handle
        if(_showInteractHandle) ShowInteractHandle();
    }

    private void ShowInteractHandle()
    {
        Vector3 pos = _interactPoint.FindPropertyRelative("Position").vector3Value;
        Vector3 fwd = _interactPoint.FindPropertyRelative("Forward").vector3Value;
        Vector3 newPos = Vector3.zero;
        Vector3 newFwd = Vector3.zero;

        HandleHelper.PosFwdHandle_AxisUp(pos, fwd, out newPos, out newFwd);

        _interactPoint.FindPropertyRelative("Position").vector3Value = newPos;
        _interactPoint.FindPropertyRelative("Forward").vector3Value = newFwd;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(50);
        EditorGUILayout.BeginFoldoutHeaderGroup(false, new GUIContent("Editor tools"));
        _showInteractHandle = GUILayout.Toggle(_showInteractHandle, new GUIContent("Show Interact Point Handle"));
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif