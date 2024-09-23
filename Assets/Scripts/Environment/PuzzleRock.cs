using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleRock : Interactable
{
    public CamState CamState = new CamState();
    [SerializeField] private List<PuzzleRockSlider> _sliders;

    private bool _inFocus;
    private bool _solved;
    private int _lastSetIndex;
    private int _indexAdd = 1;
    private bool _sliderReady = true;

    public override void Interact()
    {
        if (!_solved)
        {
            //GameManager.instance.StartInteractSession();
            Player.instance.ChangeState(PlayerStates.Interacting);
            UIPrompt.instance.ShowBackPrompt(true);
            CameraControl.MoveCam(CamState, 0.6f, () => { _inFocus = true; });
        }
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
}

[CustomEditor(typeof(PuzzleRock))]
public class PuzzleRockEditor : InteractableEditor
{
    private PuzzleRock PR;
    protected override void OnEnable()
    {
        base.OnEnable();
        PR = (PuzzleRock)target;
    }

    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();

        PR.CamState.pos = Handles.PositionHandle(PR.CamState.pos, Quaternion.Euler(PR.CamState.rot));
        PR.CamState.rot = Handles.RotationHandle(Quaternion.Euler(PR.CamState.rot), PR.CamState.pos).eulerAngles;
        Handles.DrawWireDisc(PR.CamState.pos, Vector3.up, 0.5f);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Align CamState To Cam"))
        {
            PR.CamState.pos = SceneView.lastActiveSceneView.camera.transform.position;
            PR.CamState.rot = SceneView.lastActiveSceneView.camera.transform.rotation.eulerAngles;
        }
    }
}