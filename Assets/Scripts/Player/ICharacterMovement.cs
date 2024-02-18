using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacterMovement
{
	bool Move(Transform transform, Waypoint currentWaypoint, Waypoint newWaypoint, string fwdString, string turnString);
}

public class Teleport : ICharacterMovement
{
	private float _elapsedDuration;
	public bool Move(Transform transform, Waypoint currentWaypoint, Waypoint newWaypoint, string fwdString = "", string turnString = "")
	{
		bool reachedWaypoint = false;
		// use duration to wait
		_elapsedDuration += Time.deltaTime;
		if (_elapsedDuration >= currentWaypoint.Duration)
		{
			TeleportToWaypoint(transform, newWaypoint);
			_elapsedDuration = 0;
			reachedWaypoint = true;
		}

		return reachedWaypoint;
	}

	private void TeleportToWaypoint(Transform transform, Waypoint waypoint)
	{
		transform.position = waypoint.Position;
		transform.rotation = Quaternion.Euler(waypoint.Rotation);
	}
}

public class Lerp : ICharacterMovement
{
	private float _elapsedDuration;
	public bool Move(Transform transform, Waypoint currentWaypoint, Waypoint newWaypoint, string fwdString = "", string turnString =  "") 
	{
		bool reachedWaypoint = false;
		// use duration to wait
		_elapsedDuration += Time.deltaTime;
		float percMoved = _elapsedDuration/currentWaypoint.Duration;
		transform.position = Vector3.Lerp(transform.position, newWaypoint.Position, percMoved);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newWaypoint.Rotation), percMoved);

		if (_elapsedDuration >= currentWaypoint.Duration)
		{
			_elapsedDuration = 0;
			reachedWaypoint = true;
		}

		return reachedWaypoint;
	}
}

public class Walk /*: ICharacterMovement*/
{
	private Animator _animator;
	private float _walk;
	private float _turn;
	private const float _walkDampConstant = 2;
	private const float _turnDampConstant = 3;
	private const float _exitDistance = 1;

	public void Setup(Animator animator, string fwdString, string turnString)
	{
		_animator = animator;
        _turn = _animator.GetFloat(turnString);
        _walk = _animator.GetFloat(fwdString);
    }
	public bool Move(Transform transform, Waypoint newWaypoint, string fwdString, string turnString) 
	{
		bool reachedWaypoint = false;

		// Setup
		if (_animator == null)
		{
			_animator = transform.GetComponent<Animator>();
			_turn = _animator.GetFloat(turnString);
			_walk = _animator.GetFloat(fwdString);
        }

        Vector3 dir = (newWaypoint.Position - transform.position).normalized;
		//dir = transform.InverseTransformDirection(dir);
		dir = Vector3.ProjectOnPlane(dir, transform.up);
		float turn = Mathf.Atan2(dir.x, dir.z);
		_turn = Mathf.Lerp(_turn, turn, Time.deltaTime * _turnDampConstant);
		_turn = Mathf.Clamp(_turn, -1, 1);
		_walk = Mathf.Lerp(_walk, dir.z, Time.deltaTime * _walkDampConstant);

		Debug.Log("walk: " + _walk + " turn: " + turn);
		
		_animator.SetFloat(turnString, _turn);
		_animator.SetFloat(fwdString,  Mathf.Clamp01(_walk));

        transform.Rotate(0, _turn * 90 * Time.deltaTime, 0); // swap 90 with character turn speed

        // check if transform is within the exit distance
        if (Vector3.SqrMagnitude(transform.position - newWaypoint.Position) < _exitDistance)
			reachedWaypoint = true;

		return reachedWaypoint;
	}
}

//public class Idle : ICharacterMovement
//{
//	private Animator _animator;
//	private Walk _walkMovementType;
//	private float _elapsedDuration;
//	private float _walk;
//	private float _turn;
//	private const float _turnAngleScaling = 0.1f;
//	private const float _walkStopTime = 1.5f;
//	private const float _turnSpeed = 3f;

//	public bool Move(Transform transform, Waypoint currentWaypoint, Waypoint newWaypoint, string fwdString, string turnString)
//	{
//		bool reachedWaypoint = false;

//		// setup
//		if (_animator == null)
//		{ 
//			_animator = transform.GetComponent<Animator>();
//			_turn = _animator.GetFloat(turnString);
//			_walk = _animator.GetFloat(fwdString);
//			_walkMovementType = new Walk();
//		}

//		// quick way to rotate using the shortest path
//		float turn = currentWaypoint.Rotation.y - transform.rotation.eulerAngles.y;
//		if (turn > 180 || turn < -180)
//			turn = -turn;
//		turn *= _turnAngleScaling;
//		turn = Mathf.Clamp(turn, -1, 1);

//		_walk = Mathf.Lerp(_walk, 0, _elapsedDuration/_walkStopTime);
//		_turn = Mathf.Lerp(_turn, turn, Time.deltaTime * _turnSpeed);

//		_animator.SetFloat(fwdString, Mathf.Clamp01(_walk));
//		_animator.SetFloat(turnString, _turn);

//		// use duration to wait
//		_elapsedDuration += Time.deltaTime;
//		if (_elapsedDuration >= currentWaypoint.Duration)
//		{
//			// when Idling is done, walk to the next Waypoint
//			reachedWaypoint = _walkMovementType.Move(transform, currentWaypoint, newWaypoint, fwdString, turnString);
//			if(reachedWaypoint)
//				_elapsedDuration = 0;
//		}

//		return reachedWaypoint;
//	}
//}


//public class WalkNTurn: ICharacterMovement
//{
//	private Animator _animator;
//	private Walk _walkMovementType;
//	private float _elapsedDuration;
//	private float _walk;
//	private float _turn;
//	private const float _turnAngleScaling = 0.05f;
//	private const float _walkStopTime = 1.5f;
//	private const float _exitVal = 0.1f;
//	private const float _turnSpeed = 3f;
//	private bool _reachedWaypoint;
//	public bool Move(Transform transform, Waypoint currentWaypoint, Waypoint newWaypoint, string fwdString, string turnString)
//	{
//		bool reachedOrientation = false;

//		// setup
//		if (_animator == null)
//		{
//			_animator = transform.GetComponent<Animator>();
//			_turn = _animator.GetFloat(turnString);
//			_walk = _animator.GetFloat(fwdString);
//			_walkMovementType = new Walk();
//		}

//		if (!_reachedWaypoint)
//			_reachedWaypoint = _walkMovementType.Move(transform, currentWaypoint, newWaypoint, fwdString, turnString);
//		else
//		{
//			// quick way to rotate using the shortest path
//			float turn = newWaypoint.Rotation.y - transform.rotation.eulerAngles.y;
//			if (turn > 180 || turn < -180)
//				turn = -turn;
//			turn *= _turnAngleScaling;
//			turn = Mathf.Clamp(turn, -1, 1);

//			_walk = Mathf.Lerp(_walk, 0, _elapsedDuration / _walkStopTime);
//			_turn = Mathf.Lerp(_turn, turn, Time.deltaTime * _turnSpeed);

//			_animator.SetFloat(fwdString, Mathf.Clamp01(_walk));
//			_animator.SetFloat(turnString, _turn);

//			_elapsedDuration += Time.deltaTime;

//			if (turn < _exitVal && turn > -_exitVal)
//			{
//				reachedOrientation = true;
//				_animator.SetFloat(fwdString, 0);
//				_animator.SetFloat(turnString, 0);
//				transform.rotation = Quaternion.Euler(newWaypoint.Rotation);
//			}
//		}

//		return reachedOrientation;
//	}
//}

public enum CharacterMovementType
{
	Teleport,
	Lerp,
	Walk,
	Idle
}


[System.Serializable]
public class Waypoint
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private CharacterMovementType _movementType;
    [SerializeField] private float _duration;

    public Waypoint(Vector3 position, Vector3 rotation, float duration, CharacterMovementType movementType)
    {
        _position = position;
        _rotation = rotation;
        _duration = duration;
        _movementType = movementType;
    }

    public Waypoint()
    {
        // basic constructor
    }

    public Vector3 Position => _position;

    public Vector3 Rotation => _rotation;

    public float Duration => _duration;

    public CharacterMovementType MovementType => _movementType;
}



public class WalkTurn
{
	private ThirdPersonCharacter _character;
    private Animator _animator;
    //private Walk _walkMovementType;
    private float _elapsedDuration;
    private float _walk;
    private float _turn;
    private const float _turnAngleScaling = 0.05f;
    private const float _walkStopTime = 1.5f;
    private const float _exitVal = 0.1f;
    private const float _turnSpeed = 3f;
    private const float _exitDistance = 0.5f;
    //private bool _reachedWaypoint;
    public bool Move(Transform transform, Animator animator, float moveMultiplier, Waypoint newWaypoint, string fwdString, string turnString)
    {
        bool reachedOrientation = false;

        // setup
        if (_character == null)
        {
			_character = transform.GetComponent<ThirdPersonCharacter>();
            _animator = animator;
            _turn = _animator.GetFloat(turnString);
            _walk = _animator.GetFloat(fwdString);
            //_walkMovementType = new Walk();
        }

		Vector3 disp = newWaypoint.Position - transform.position;
        //Debug.Log("sqr mag" + disp.sqrMagnitude);
        if (Vector3.SqrMagnitude(disp) > _exitDistance)
		{
            _character.Move(disp.normalized * moveMultiplier, false, false);
		}
        else
        {
			// quick way to rotate using the shortest path
			float turn = newWaypoint.Rotation.y - transform.rotation.eulerAngles.y;
			if (turn > 180 || turn < -180)
				turn = -turn;
			turn *= _turnAngleScaling;
			turn = Mathf.Clamp(turn, -1, 1);

			_walk = Mathf.Lerp(_walk, 0, _elapsedDuration / _walkStopTime);
			_turn = Mathf.Lerp(_turn, turn, Time.deltaTime * _turnSpeed);


			//Debug.Log("turn: " + _turn);

			_animator.SetFloat(fwdString, Mathf.Clamp01(_walk));
			_animator.SetFloat(turnString, _turn);

			//_elapsedDuration += Time.deltaTime;

			// help the character turn faster (this is in addition to root rotation in the animation)
			transform.Rotate(0, _turn * _character.StationaryTurnSpeed * Time.deltaTime, 0);
			//transform.Rotate(transform.up, _turn * _character.StationaryTurnSpeed * Time.deltaTime);

			//float preRotY = transform.rotation.eulerAngles.y;
			//Vector3 preRotFwd = transform.forward;
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newWaypoint.Rotation), 0.01f * _character.StationaryTurnSpeed);
			////float turnn = (transform.rotation.eulerAngles.y - preRotY) * 10;
			//float turnn = Vector3.SignedAngle(preRotFwd, transform.forward, transform.up);
			//Debug.Log("turnn: " + turnn);
			//_animator.SetFloat(turnString, turnn);

			if (turn < _exitVal && turn > -_exitVal)
			{
				reachedOrientation = true;
				_animator.SetFloat(fwdString, 0);
				_animator.SetFloat(turnString, 0);
				transform.rotation = Quaternion.Euler(newWaypoint.Rotation);
			}

			//if (turnn < 0.01f && turnn > -0.01f)
			//{
			//             reachedOrientation = true;
			//	_animator.SetFloat(fwdString, 0);
			//	_animator.SetFloat(turnString, 0);
			//	transform.rotation = Quaternion.Euler(newWaypoint.Rotation);
			//}
		}

        return reachedOrientation;
    }
}