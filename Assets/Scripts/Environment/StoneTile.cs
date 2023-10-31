using UnityEngine;
using DG.Tweening;

public class StoneTile : MonoBehaviour
{
    [SerializeField] private float _topY;
    [SerializeField] private float _bottomY;
    [SerializeField] private float _lerpSpeed;

    [SerializeField] private Transform _connector;
    [SerializeField] private float _cncTopY;
    [SerializeField] private float _cncBottomY;


    private bool _pressed;
    private Vector3 _targetPos;
    private Vector3 _cncTargetPos;

    private bool Pressed {
        get { return _pressed; }
        set { 
            _pressed = value;
            if (_pressed)
            {
                _targetPos = new Vector3(transform.localPosition.x, _bottomY, transform.localPosition.z);
                _cncTargetPos = new Vector3(_connector.localPosition.x, _cncTopY, _connector.localPosition.z);
            }
            else
            {
                _targetPos = new Vector3(transform.localPosition.x, _topY, transform.localPosition.z);
                _cncTargetPos = new Vector3(_connector.localPosition.x, _cncBottomY, _connector.localPosition.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pressed = true;
        transform.DOLocalMove(_targetPos, _lerpSpeed);
        _connector.DOLocalMove(_cncTargetPos, _lerpSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        Pressed = false;
        transform.DOLocalMove(_targetPos, _lerpSpeed);
        _connector.DOLocalMove(_cncTargetPos, _lerpSpeed);
    }

    private void Start()
    {
        Pressed = false;
    }
}
