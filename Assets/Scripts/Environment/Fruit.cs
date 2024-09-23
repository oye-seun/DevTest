using DG.Tweening;
using UnityEngine;

public class Fruit : Interactable
{
    [SerializeField] private float _pickingOffset;
    [SerializeField] private float _fwdPickingRadius;
    [SerializeField] private float _turnPickingRadius;
    [SerializeField] private Vector3 _handHoldPosition;
    [SerializeField] private FruitScriptableObject _fruitSO;
    private bool _interacted;
    private PlayerPositioner _playerPositioner;

    public void FruitDrop()
    {
        transform.parent = null;
    }

    protected override void Start()
    {
        base.Start();
        _playerPositioner = FindObjectOfType<PlayerPositioner>();
    }

    public override void Interact()
    {
        //throw new System.NotImplementedException();
        if (_interacted) return;
        //GameManager.instance.StartInteractSession();
        Player.instance.ChangeState(PlayerStates.GameControlled);

        ////if(!_interacted && !_animFinished) _interacted = true;

        //Vector3 startDir = GetDisp();

        //if (startDir.magnitude > _fwdPickingRadius)  // further enough
        //{
        //    _playerPositioner.MoveToPos(
        //        _playerPositioner.transform.position,
        //        startDir.normalized,
        //        transform.position + (-startDir.normalized * _pickingOffset),
        //        (-startDir.normalized * _pickingOffset),
        //        0.5f,
        //        () =>
        //        {
        //            //GameManager.instance.StartCutscene();
        //            CutsceneManager.instance.PlayClip(1, OnCompleteInteract);
        //            //OnCompleteInteract();
        //        }
        //    );
        //}
        //else if(startDir.magnitude > _turnPickingRadius)
        //{
        //    _playerPositioner.TurnToPos(startDir.normalized, () => { CutsceneManager.instance.PlayClip(1, OnCompleteInteract); }, 0.7f);
        //}
        //else
        //{
        //    Debug.Log("dist: " + startDir.magnitude);
        //    _playerPositioner.MoveWithBackToPos(
        //        _playerPositioner.transform.position,
        //        -_playerPositioner.transform.forward,
        //        transform.position + (-_playerPositioner.transform.forward * 1.2f),
        //        (startDir.normalized * _pickingOffset),
        //        0.5f,
        //        () =>
        //        {
        //            //GameManager.instance.StartCutscene();
        //            //CutsceneManager.instance.PlayClip(1, OnCompleteInteract);
        //            _playerPositioner.TurnToPos(GetDisp().normalized, () => { CutsceneManager.instance.PlayClip(1, OnCompleteInteract); }, 0.7f);
        //            //OnCompleteInteract();
        //        }
        //    );
        //}

        Vector3 startDir = Player.instance.transform.position - transform.position;
        startDir.y = 0;
        bool useBack = (startDir.magnitude < _pickingOffset);
        Vector3 pickPos = transform.position + ((useBack) ? -(startDir.normalized * _pickingOffset) : (startDir.normalized * _pickingOffset));
        Vector3 moveDisp = Player.instance.transform.position - pickPos;
        moveDisp.y = 0;
        float moveDist = moveDisp.magnitude;

        Player.instance.simplePlayerPositioner.MoveToPos(
            Player.instance.transform.position,
            Player.instance.transform.forward * moveDist * 0.4f,
            /*transform.position + (startDir.normalized * _pickingOffset)*/ pickPos,
            moveDist * 0.3f * ((useBack) ? startDir.normalized: -startDir.normalized),
            /*0.2f*/ moveDist * Player.instance.simplePlayerPositioner.TimePerDistConstant * 1.5f,
            () =>
            {
                //GameManager.instance.StartCutscene();
                Player.instance.ChangeState(PlayerStates.Interacting);
                CutsceneManager.instance.PlayClip(1, OnCompleteInteract);
                //OnCompleteInteract();
            },
            useBack
        );
    }

    private Vector3 GetDisp() => transform.position - _playerPositioner.transform.position;

    public void OnCompleteInteract()
    {
        //GameManager.instance.EndInteractSession();
        Player.instance.ChangeState(PlayerStates.PlayerControlled);
        //_interacted = true;
    }

    public void AttachToHand()
    {
        transform.parent = Player.instance.characterAnimControl.RightHand;
        transform.DOLocalMove(_handHoldPosition, 0.5f).OnComplete(() => { this.enabled = false; });
        _fruitSO.gameObject = this.gameObject;
        Player.instance.playerData.inventory.Add(_fruitSO.Name, _fruitSO);
    }
}
