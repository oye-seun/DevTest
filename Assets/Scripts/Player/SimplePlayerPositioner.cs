using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerPositioner : MonoBehaviour
{
    public static event Action PositionerStarted;
    public static event Action PositionerEnded;
    public static Action<Vector3, bool> Move;
    public static Action<Vector3, float> Turn;
    public float TimePerDistConstant => _timePerDistanceConstant;


    [SerializeField] private float _updateSpeed = 0.5f;
    [SerializeField] private float _legSpeed = 0.1f;
    [SerializeField] private float _backLegSpeed = 0.1f;
    [SerializeField] private float _lspeed = 40;
    [SerializeField] private float _turnSpeed = 1;
    [SerializeField] private float _fwdLerpSpeed = 1;
    [SerializeField] private bool _useVel;
    [SerializeField] private float _timePerDistanceConstant = 0.15f;

    [Tooltip("If the start and end positions are less than this distance apart, walking is skipped, if angle < _angleDistanceTrigger")]
    [SerializeField] private float _posDistanceTrigger;
    [Tooltip("If the start and end Directions are less than this value apart, walking is skipped, if dist < _posDistanceTrigger")]
    [SerializeField] private float _angleDistanceTrigger;
    //private Spline spline;
    //private float _moveProgress;
    //private Action onComplete;
    //private ThirdPersonCharacter _character;
    private bool _useBack;

    private Vector3 _endDir;

    //private Vector3 _dir;
    //private bool _turning;`
    private PlayerPositionerState _state;

    void Start()
    {
        //spline = new Spline();
    }


    public void MoveToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float duration, Action onComplete, bool useBack = false)
    {
        if (useBack) MoveWithBackToPos(start, startDir, end, endDir, duration, onComplete);
        else MoveToPos(start, startDir, end, endDir, duration, onComplete);

    }

    private void MoveToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float duration, Action onComplete)
    {
        if(CheckPosAndFwd(start, startDir, end, endDir))
        {
            onComplete?.Invoke();
            return;
        }

        if (_state == PlayerPositionerState.idle)
        {
            _endDir = endDir;
            //this.onComplete = onComplete;
            Spline spline = new Spline();
            spline.Update(start, startDir, end, endDir, 0, duration);
            //_useBack = false;
            //_moveProgress = curveStart;
            StartCoroutine(RunMoveToPos(spline, duration, onComplete));
        }
    }

    //public void MoveWithBackToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float curveStart, Action onComplete)
    //{
    //    if (_moveProgress >= 1.1f + _zeroTimeAfterComplete && !_turning)
    //    {
    //        _onComplete = onComplete;
    //        _spline.Update(start, startDir, end, endDir);
    //        _useBack = true;
    //        _moveProgress = curveStart;
    //        StartCoroutine("RunMoveToPos");
    //    }
    //}

    private IEnumerator RunMoveToPos(Spline spline, float dur, Action onComplete)
    {
        _state = PlayerPositionerState.moving;
        PositionerStarted?.Invoke();
        float t = 0;
        //float angle = transform.rotation.eulerAngles.y;
        //Vector3 pos = _spline.Start;

        Player.instance.characterAnimControl.m_Animator.SetBool("simpleLocomotion", true);
        while (t <= dur /*+ 0.1f*/)
        {
            //Vector3 newPos = spline.GetPosAt(Mathf.Clamp01(t)); newPos.y = transform.position.y;
            //Vector3 newDir = spline.GetDirAt(t);

            //// handling Walking
            //if(t <= dur)
            //{
            //    if (!_useVel) Player.instance.characterAnimControl.m_Animator.SetFloat("Forward", Vector3.Distance(transform.position, newPos) * _lspeed);
            //    else Player.instance.characterAnimControl.m_Animator.SetFloat("Forward", newDir.magnitude * _legSpeed);

            //    transform.position = newPos;
            //}

            //// handling Turning
            //float turnAngle = Vector3.SignedAngle(transform.forward.normalized, newDir.normalized, Vector3.up);
            //turnAngle = Mathf.Clamp(turnAngle * _turnSpeed, -1, 1); 
            //Player.instance.characterAnimControl.m_Animator.SetFloat("Turn", turnAngle);

            //transform.forward = Vector3.Slerp(transform.forward, newDir.normalized, _fwdLerpSpeed);

            //// Hanling progression
            //float fwdAlign = Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, newDir.normalized));
            //fwdAlign = Mathf.Pow(fwdAlign, 2);
            //t += Time.deltaTime * _updateSpeed * fwdAlign * newDir.magnitude;


            //# if UNITY_EDITOR
            ////..........show line.................................................
            //for (int i = 0; i < 19; i++)
            //{
            //    Vector3 start = spline.GetPosAt((float)i * dur / 20); start.y = transform.position.y + 1;
            //    Vector3 end = spline.GetPosAt((float)(i + 1) * dur/ 20); end.y = transform.position.y + 1;
            //    //Debug.DrawLine(start, end, Color.cyan);
            //    Debug.DrawLine(start, end, Color.Lerp(Color.blue, Color.red, spline.GetDirAt((float)i * dur / 20).magnitude/10));

            //    //Vector3 dir = spline.GetDirAt((float)i * dur / 20);
            //    //Debug.DrawLine(start, start + (dir.normalized * 0.1f), Color.red);
            //}

            //Vector3 start2 = spline.GetPosAt(dur); start2.y = transform.position.y + 1;
            //Vector3 zerodir = spline.GetDirAt(dur).normalized;
            //Debug.DrawLine(start2, start2 + (1.5f * zerodir), Color.yellow);
            ////Debug.DrawLine(start2, start2 + _endDir, Color.green);
            ////....................................................................
            //#endif

            t = MoveFunction(spline, t);
            yield return null;
        }
        MoveFunction(spline, dur);


        PositionerEnded?.Invoke();
        onComplete();
        Player.instance.characterAnimControl.m_Animator.SetBool("simpleLocomotion", false);
        _state = PlayerPositionerState.idle;
    }


    private float MoveFunction(Spline spline, float t)
    {
        Vector3 newPos = spline.GetPosAt(t); newPos.y = transform.position.y;
        Vector3 newDir = spline.GetDirAt(t);

        if (!_useVel) Player.instance.characterAnimControl.m_Animator.SetFloat("Forward", Vector3.Distance(transform.position, newPos) * _lspeed);
        else Player.instance.characterAnimControl.m_Animator.SetFloat("Forward", newDir.magnitude * _legSpeed);
        transform.position = newPos;

        // handling Turning
        float turnAngle = Vector3.SignedAngle(transform.forward.normalized, newDir.normalized, Vector3.up);
        turnAngle = Mathf.Clamp(turnAngle * _turnSpeed, -1, 1);
        Player.instance.characterAnimControl.m_Animator.SetFloat("Turn", turnAngle);

        transform.forward = Vector3.Slerp(transform.forward, newDir.normalized, _fwdLerpSpeed);

        // Hanling progression
        float fwdAlign = Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, newDir.normalized));
        fwdAlign = Mathf.Pow(fwdAlign, 2);
        return t += Time.deltaTime * _updateSpeed * fwdAlign * newDir.magnitude;
    }







    private void MoveWithBackToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float duration, Action onComplete)
    {
        if (_state == PlayerPositionerState.idle)
        {
            _endDir = endDir;
            Spline spline = new Spline();
            spline.Update(start, startDir, end, endDir, 0, -duration);
            StartCoroutine(RunMoveWithBackToPos(spline, -duration, onComplete));
        }
    }

    private IEnumerator RunMoveWithBackToPos(Spline spline, float dur, Action onComplete)
    {
        _state = PlayerPositionerState.moving;
        PositionerStarted?.Invoke();
        Player.instance.characterAnimControl.m_Animator.SetBool("simpleLocomotion", true);

        float t = 0;
        while (t >= dur)
        {
            t = MoveWithBackFunction(spline, t);
            yield return null;
        }
        MoveWithBackFunction(spline, dur);


        PositionerEnded?.Invoke();
        onComplete();
        Player.instance.characterAnimControl.m_Animator.SetBool("simpleLocomotion", false);
        _state = PlayerPositionerState.idle;
    }

    private float MoveWithBackFunction(Spline spline, float t)
    {
        Vector3 newPos = spline.GetPosAt(t); newPos.y = transform.position.y;
        Vector3 newDir = spline.GetDirAt(t);

        // handling walking
        Player.instance.characterAnimControl.m_Animator.SetFloat("Forward", -newDir.magnitude * _backLegSpeed);
        transform.position = newPos;

        // handling Turning
        float turnAngle = Vector3.SignedAngle(transform.forward.normalized, newDir.normalized, Vector3.up);
        turnAngle = Mathf.Clamp(turnAngle * _turnSpeed, -1, 1);
        Player.instance.characterAnimControl.m_Animator.SetFloat("Turn", turnAngle);

        transform.forward = Vector3.Slerp(transform.forward, newDir.normalized, _fwdLerpSpeed);

        // Hanling progression
        float fwdAlign = Mathf.Clamp01(Vector3.Dot(transform.forward.normalized, newDir.normalized));
        fwdAlign = Mathf.Pow(fwdAlign, 2);
        return t -= Time.deltaTime * _updateSpeed * fwdAlign * newDir.magnitude;
    }

    private bool CheckPosAndFwd(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir)
    {
        Vector3 disp = start - end;
        disp.y = 0;
        startDir.y = 0;
        endDir.y = 0;
        return (disp.magnitude < _posDistanceTrigger && Vector3.Angle(startDir, endDir) < _angleDistanceTrigger);
    }

    //public void TurnToPos(Vector3 dir, Action onComplete, float speedFactor)
    //{
    //    if (!_turning && _moveProgress >= 1.1f + _zeroTimeAfterComplete)
    //    {
    //        //_dir = dir;
    //        //_onComplete = onComplete;
    //        StartCoroutine(RunTurnToDir(dir, onComplete, speedFactor));
    //    }
    //}


    //private IEnumerator RunTurnToDir(Vector3 dir, Action onComplete, float speedFactor)
    //{
    //    _turning = true;
    //    while (Vector3.Dot(transform.forward, dir) < 0.95f)
    //    {
    //        /*_character.Turn*/
    //        Turn?.Invoke(dir, speedFactor);
    //        yield return null;
    //    }

    //    _turning = false;
    //    onComplete();
    //    Debug.Log("turn finished");
    //}

    //private void OnDrawGizmos()
    //{
    //    if (_spline == null) return;
    //    if (_moveProgress >= 1.1f + _zeroTimeAfterComplete) return;

    //    Vector3 movPos = _spline.GetPosAt(Mathf.Clamp01(_moveProgress)); movPos.y = transform.position.y + 1;
    //    Gizmos.DrawCube(movPos, Vector3.one * 0.3f);

    //    //..........show line.................................................
    //    for (int i = 0; i < 19; i++)
    //    {
    //        Vector3 start = _spline.GetPosAt((float)i / 20); start.y = transform.position.y + 1;
    //        Vector3 end = _spline.GetPosAt((float)(i + 1) / 20); end.y = transform.position.y + 1;
    //        Debug.DrawLine(start, end, Color.cyan);
    //    }
    //    //....................................................................
    //}

    private enum PlayerPositionerState { idle, moving, turning}
}
