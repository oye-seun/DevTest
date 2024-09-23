using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioner : MonoBehaviour
{
    [System.Serializable]
    public struct Waypoint
    {
        public Vector3 Position;
        public Vector3 Forward;
        public float FwdStrength;
    }

    public static event Action<Vector3, bool> Move;
    public static event Action<Vector3, float> Turn;


    [SerializeField] private float _maxFollowDist = 1f;
    [SerializeField] private float _updateSpeed = 0.5f;
    [SerializeField] private float _zeroTimeAfterComplete = 0.2f;
    [SerializeField] private float _slowMovementStartDist = 0.5f;

    private Spline _spline;
    private float _moveProgress;
    private Action _onComplete;
    //private ThirdPersonCharacter _character;
    private bool _useBack;

    //private Vector3 _dir;
    private bool _turning;


    void Start()
    {
        _spline = new Spline();
        //_character = GetComponent<ThirdPersonCharacter>();
        _moveProgress = 1.1f + _zeroTimeAfterComplete;
    }

    public void MoveToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float curveStart, Action onComplete)
    {
        if (_moveProgress >= 1.1f + _zeroTimeAfterComplete && !_turning)
        {
            _onComplete = onComplete;
            _spline.Update(start, startDir, end, endDir);
            _useBack = false;
            _moveProgress = curveStart;
            StartCoroutine("RunMoveToPos");
        }
    }

    public void MoveWithBackToPos(Vector3 start, Vector3 startDir, Vector3 end, Vector3 endDir, float curveStart, Action onComplete)
    {
        if (_moveProgress >= 1.1f + _zeroTimeAfterComplete && !_turning)
        {
            _onComplete = onComplete;
            _spline.Update(start, startDir, end, endDir);
            _useBack = true;
            _moveProgress = curveStart;
            StartCoroutine("RunMoveToPos");
        }
    }

    private IEnumerator RunMoveToPos()
    {
        while (_moveProgress < 1.1f + _zeroTimeAfterComplete)
        {
            Vector3 newPos = _spline.GetPosAt(Mathf.Clamp01( _moveProgress)); newPos.y = transform.position.y;
            Vector3 dir = newPos - transform.position;


            Vector3 endPos = _spline.End; endPos.y = transform.position.y;
            Vector3 distToEnd = transform.position - endPos;
            if (_moveProgress < 1.1f)
            {
                if(!_useBack) /*_character.Move*/Move?.Invoke(dir.normalized * Player.instance.playerController.MoveValMultiplier * Mathf.Clamp(Mathf.InverseLerp(0, _slowMovementStartDist, distToEnd.magnitude), 0.1f, 1), /*false,*/ false);
                else /*_character.Move*/Move?.Invoke(dir.normalized * Player.instance.playerController.MoveValMultiplier * Mathf.Clamp(Mathf.InverseLerp(0, _slowMovementStartDist, distToEnd.magnitude), 0.1f, 1), /*false, false,*/ true);
            }
            else
            {
                if(!_useBack)/* _character.Move*/Move?.Invoke(Vector3.zero, /*false,*/ false);
                else /*_character.Move*/Move?.Invoke(Vector3.zero, /*false, false,*/ true);
            }

            _moveProgress += Time.deltaTime * _updateSpeed * Mathf.InverseLerp(_maxFollowDist, 0, dir.magnitude);

            yield return null;
        }
        _onComplete();
    }


    public void TurnToPos(Vector3 dir, Action onComplete, float speedFactor)
    {
        if (!_turning && _moveProgress >= 1.1f + _zeroTimeAfterComplete)
        {
            //_dir = dir;
            //_onComplete = onComplete;
            StartCoroutine(RunTurnToDir(dir, onComplete, speedFactor));
        }
    }


    private IEnumerator RunTurnToDir(Vector3 dir, Action onComplete, float speedFactor)
    {
        _turning = true;
        while(Vector3.Dot(transform.forward, dir) < 0.95f)
        {
            /*_character.Turn*/Turn?.Invoke(dir, speedFactor);
            yield return null;
        }

        _turning = false;
        onComplete();
        Debug.Log("turn finished");
    }

    private void OnDrawGizmos()
    {
        if (_spline == null) return;
        if (_moveProgress >= 1.1f + _zeroTimeAfterComplete) return;

        Vector3 movPos = _spline.GetPosAt(Mathf.Clamp01( _moveProgress)); movPos.y = transform.position.y + 1;
        Gizmos.DrawCube(movPos, Vector3.one * 0.3f);

        //..........show line.................................................
        for (int i = 0; i < 19; i++)
        {
            Vector3 start = _spline.GetPosAt((float)i / 20); start.y = transform.position.y + 1;
            Vector3 end = _spline.GetPosAt((float)(i + 1) / 20); end.y = transform.position.y + 1;
            Debug.DrawLine(start, end, Color.cyan);
        }
        //....................................................................
    }
}
