using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleRock1 : Interactable
{
    public CamState CamState = new CamState();
    [SerializeField] private List<PuzzleRockSlider> _sliders;
    [SerializeField] private Vector3 _fruitPos;

    [Header("Interaction Point")]
    [SerializeField] private PlayerPositioner.Waypoint _interactPoint;
    //[SerializeField] private float _timePerDistanceConstant = 0.5f;

    private bool _inFocus;
    private bool _solved;
    private int _lastSetIndex;
    private int _indexAdd = 1;
    private bool _sliderReady = true;

    #if UNITY_EDITOR
    [HideInInspector] public bool _showInteractPointHandle;
    #endif

    public override void Interact()
    {
        if (!_solved /*&& Player.instance.playerData.inventory.ContainsKey("Energy Fruit")*/)
        {
            Vector3 startDir = _interactPoint.Position - Player.instance./*playerPositioner*/simplePlayerPositioner.transform.position;
            Player.instance./*playerPositioner*/simplePlayerPositioner./*MoveToPos*/MoveToPos(
            Player.instance./*playerPositioner*/transform.position,
            /*startDir.normalized*/ Player.instance.transform.forward * (startDir.magnitude/3),
            _interactPoint.Position,
            _interactPoint.Forward * /*_interactPoint.FwdStrength*/ (startDir.magnitude * 1.5f),
            /*0.7f*/ startDir.magnitude * Player.instance.simplePlayerPositioner.TimePerDistConstant,
            () =>
            {
                //CutsceneManager.instance.PlayClip(0, OnCompleteInteract);
                //Debug.Log("At interact point");
                //CutsceneManager.instance.PlayClip(2, () => { });
            });
        }
        //else if (!_solved)
        //{
        //    GameManager.instance.StartInteractSession();
        //    UIPrompt.instance.ShowBackPrompt(true);
        //    CameraControl.MoveCam(CamState, 0.6f, () => { _inFocus = true; });
        //}
    }

    private void LeaveInteractSession()
    {
        _inFocus = false;
        UIPrompt.instance.ShowPrompt(false);
        //GameManager.instance.EndInteractSession();
        Player.instance.ChangeState(PlayerStates.PlayerControlled);
        CameraControl.MoveCam(GameManager.instance.CamState, 0.6f, () => {});
    }

    private void Update()
    {
        if (!_inFocus) return;
        if(PlayerInputs.instance.BackKeyPressed()) LeaveInteractSession();

        // puzzle Logic
        Vector2 analogInput = PlayerInputs.instance.MoveVal();
        //analogInput.x *= -1;
        analogInput.y *= -1;
        if(analogInput.sqrMagnitude > 0.25f && _sliderReady)
        {
            int setIndexes = 0;
            foreach(PuzzleRockSlider slider in _sliders)
            {
                if(slider.set) setIndexes++;
            }

            float angle = Mathf.Atan2(analogInput.y, analogInput.x) * Mathf.Rad2Deg;
            angle = (angle < 0)? 360 + angle : angle;
            int index = Mathf.RoundToInt(angle/45);
            //Debug.Log("angle: " + angle + " index: " + index );
            index = index % _sliders.Count;

            if(index == (_lastSetIndex + _indexAdd)%_sliders.Count || setIndexes == 0)
            {
                _sliderReady = false;
                _sliders[index].Set(1, () => { _sliderReady = true; });
                _indexAdd = (_indexAdd + 1)%4;
                if (_indexAdd <= 0) _indexAdd = 1;
            }
            else
            {
                _sliderReady = false;
                foreach (PuzzleRockSlider slider in _sliders)
                {
                    slider.ReSet(1, () => { _sliderReady = true; });
                }
                _indexAdd = 1;
            }

            _lastSetIndex = index;
        }
    }

    public void PlaceFruit()
    {
        // get fruit
        Transform fruit = Player.instance.playerData.inventory["Energy Fruit"].gameObject.transform;
        Player.instance.playerData.inventory.Remove("Energy Fruit");

        // set to parent 
        fruit.parent = transform;
        //fruit.position = _fruitPos;

        // animate
        StartCoroutine(SprintTask.Run_c(0, 1, 3, (float t) =>
        {
            fruit.position = Vector3.Lerp(fruit.position, _fruitPos, t);

        },
        () =>
        {

        }));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PuzzleRock1))]
public class PuzzleRockEditor1 : InteractableEditor
{
    private PuzzleRock1 PR;
    private SerializedProperty _interactPoint;
    private SerializedProperty _showInteractPointHandle;

    protected override void OnEnable()
    {
        base.OnEnable();
        PR = (PuzzleRock1)target;
        _interactPoint = serializedObject.FindProperty("_interactPoint");
        _showInteractPointHandle = serializedObject.FindProperty("_showInteractPointHandle");
    }

    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();

        PR.CamState.pos = Handles.PositionHandle(PR.CamState.pos, Quaternion.Euler(PR.CamState.rot));
        PR.CamState.rot = Handles.RotationHandle(Quaternion.Euler(PR.CamState.rot), PR.CamState.pos).eulerAngles;
        Handles.DrawWireDisc(PR.CamState.pos, Vector3.up, 0.5f);

        ShowInteractHandles();
        serializedObject.ApplyModifiedProperties();
    }

    private void ShowInteractHandles()
    {
        if(!_showInteractPointHandle.boolValue) return;
        Vector3 pos = _interactPoint.FindPropertyRelative("Position").vector3Value;
        Vector3 fwd = _interactPoint.FindPropertyRelative("Forward").vector3Value;
        Vector3 newPos = Vector3.zero;
        Vector3 newFwd = Vector3.zero;

        HandleHelper.PosFwdHandle_AxisUp(pos, fwd, out newPos, out newFwd);

        _interactPoint.FindPropertyRelative("Position").vector3Value = newPos;
        _interactPoint.FindPropertyRelative("Forward").vector3Value = newFwd;

        // Debug
        Debug.DrawLine(newPos, newPos + newFwd, Color.magenta);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(50);
        EditorGUILayout.BeginFoldoutHeaderGroup(false, new GUIContent("Editor tools"));
        _showInteractPointHandle.boolValue = GUILayout.Toggle(_showInteractPointHandle.boolValue, new GUIContent("Show Interact Point Handle"));
        if (GUILayout.Button("Align CamState To Cam"))
        {
            PR.CamState.pos = SceneView.lastActiveSceneView.camera.transform.position;
            PR.CamState.rot = SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif