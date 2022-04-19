using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 6f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.2f;

		Rigidbody m_Rigidbody;
		Animator m_Animator;
		public bool m_IsGrounded;
		public bool runjump;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;

		// my edit
		public NavMeshAgent agent;
		public float jumpingspeed;
		public float normalspeed;
		private GameObject virtualAvatar;
		public static bool onFloatable = true;

		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;

			virtualAvatar = new GameObject("virtual avatar");
			agent.updatePosition = false;
			CheckGroundStatus(normalspeed);
		}


		private void Update()
		{
			// I need to  manually update the Y axis
			agent.nextPosition = transform.position;
		}

		public void Move(Vector3 move, bool crouch, bool jump)
		{
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus(normalspeed);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();


			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}


			ScaleCapsuleForCrouching(crouch);
			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
			transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
			m_Animator.SetBool("pull", false);
			m_Capsule.center = new Vector3(0, standcapsuleheight.x, 0);
			m_Capsule.height = standcapsuleheight.y;
		}


		public IEnumerator GoDir(Transform target, Generalevent.PressButton del)
		{
			float dist = 10;
			while(Vector3.Distance(transform.position, target.position) > .5f)
			{
				agent.SetDestination(target.position);
				//Debug.Log("GoDir()");
				if (dist - Vector3.Distance(transform.position, target.position) == 0) // if the player has stopped moving towards the destination
				{
					break;
				}
				dist = Vector3.Distance(transform.position, target.position);
				yield return null;
			}
			StartCoroutine(AlignPlayer(target.right, 1, del));
		}

		public IEnumerator AlignPlayer(Vector3 dir, float time, Generalevent.PressButton del)
		{
			while(time > 0)
			{
				dir = dir.normalized;
				//float turn = Mathf.Atan2(dir.x, dir.z);
				Quaternion turner = Quaternion.LookRotation(dir, transform.up);
				float turn = (transform.localPosition.y - turner.eulerAngles.y) * Mathf.Deg2Rad;
				Move(Vector3.zero, false, false);
				//ApplyExtraTurnRotation();
				//Debug.Log("aligning");

				transform.rotation = Quaternion.Lerp(transform.rotation, turner, .2f);
				//Debug.Log(turn);
				time -= Time.deltaTime;
				yield return null;
				//Debug.Log("AlignPlayer()");
			}
			//Debug.Log("aligning done");

			del();
		}

		private void MoveAlign(float turnamount)
		{
			Vector3 move = Vector3.zero;

			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus(normalspeed);
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);

			m_TurnAmount = turnamount;

			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();

			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(false, false);
			}
			else
			{
				HandleAirborneMovement();
			}

			// send input and other state parameters to the animator
			UpdateAnimator(move);
			m_Animator.SetBool("pull", false);
		}

		private float ypos;
		private Vector2 squatcapsuleheight = new Vector2(0.8620808f, 1.820398f); //values for y centre and height respectively
		private Vector2 standcapsuleheight = new Vector2(0.9394705f, 1.665619f);
		public void MoveWithBack(Vector3 move)
		{
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus(normalspeed * 1.7f);
			ypos = transform.position.y;
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);

			Vector3 antimove = Quaternion.AngleAxis(180, Vector3.up) * move;

			m_TurnAmount = Mathf.Atan2(antimove.x, antimove.z);
			m_TurnAmount = Mathf.Clamp(m_TurnAmount, -0.1f, 0.1f);

			m_ForwardAmount = antimove.z;

			ApplyExtraTurnRotation();


			// send input and other state parameters to the animator
			UpdateAnimator(move);
			transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
			m_Animator.SetBool("pull", true);
			m_Capsule.center = new Vector3(0, squatcapsuleheight.x, 0);
			m_Capsule.height = squatcapsuleheight.y;
		}


		void TrigMethod()
		{
			Vector3 move;
			OffMeshLinkData data = agent.currentOffMeshLinkData;

			Vector3 target = data.endPos + (Vector3.up * 2);
			float r = Vector3.Angle(transform.forward, (data.endPos - data.startPos));

			Debug.Log("r: " + r);
			if (r < 13 && !runjump)
			{
				runjump = true;
				Debug.Log("runjump");
				m_Animator.SetBool("runjump", runjump);
			}
			else if (!runjump)
			{
				Debug.Log("aligning");


				move = (target - transform.position);

				if (move.magnitude > 1f) move.Normalize();
				//move = transform.InverseTransformDirection(move);
				move = Vector3.ProjectOnPlane(move, m_GroundNormal);
				m_TurnAmount = Mathf.Atan2(Mathf.Abs(move.x), Mathf.Abs(move.z)) * Mathf.Rad2Deg;

				// quadrant corrections
				if (move.x < 0 && move.z > 0) m_TurnAmount = 360 - m_TurnAmount;
				else if (move.x < 0 && move.z < 0) m_TurnAmount = 180 + m_TurnAmount;
				else if (move.x > 0 && move.z < 0) m_TurnAmount = 180 - m_TurnAmount;

				//m_ForwardAmount = move.z;

				Debug.Log("turn: " + m_TurnAmount);
				//ApplyExtraTurnRotation();
				Vector3 dir = transform.localEulerAngles;
				dir.y = m_TurnAmount;

				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, dir, 10 * Time.deltaTime);

				//UpdateAnimator2(move);
				return;
			}
			


			if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("jumping") && (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime - (int)m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime) > 0.9f)
			{
				Debug.Log("off link");
				runjump = false;
				m_Animator.SetBool("runjump", runjump);
				//Debug.Log("on link");
				agent.CompleteOffMeshLink();
			}
			return;
		}


		public void RotateMethod()
		{

			//virtualAvatar.SetActive(false);
			virtualAvatar.transform.position = transform.position;

			OffMeshLinkData data = agent.currentOffMeshLinkData;

			Vector3 target = data.endPos + (Vector3.up * 2); // if startpos is closer
			float r = Vector3.Angle(transform.forward, (data.endPos - transform.position));  // if startpos is closer


			Debug.Log("r: " + r);
			if (r < 13 && !runjump)
			{
				runjump = true;
				Debug.Log("runjump");
				m_Animator.SetBool("runjump", runjump);
			}
			else if (!runjump)
			{

				virtualAvatar.transform.LookAt(target);

				transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, virtualAvatar.transform.localEulerAngles.y, transform.localEulerAngles.z), 10 * Time.deltaTime);

				//m_Animator.SetFloat("Turn", virtualAvatar.transform.localEulerAngles.y, 0.1f, Time.deltaTime);
			}


			if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("jumping") && (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime - (int)m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime) > 0.9f)
			{
				Debug.Log("off link");
				runjump = false;
				m_Animator.SetBool("runjump", runjump);
				//Debug.Log("on link");
				agent.CompleteOffMeshLink();
			}
			return;
		}





		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
				//Debug.Log("handle ground movement ");
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}

		

		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}


		void CheckGroundStatus(float speed)
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.4f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.3f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
				agent.speed = speed;
				ypos = hitInfo.point.y;

				updateBaseOffset(hitInfo);
			}
			else
			{
				m_IsGrounded = true;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = true;
				//agent.speed = jumpingspeed;
			}
		}

		private void updateBaseOffset(RaycastHit hitinfo) //this is in order to move the chararter when on floating steps. dont want to bake navmesh
		{
			if(hitinfo.transform.tag == "floatable")
			{
				// landed on floatable object
				if (!onFloatable)
				{
					hitinfo.transform.GetComponent<floater>().loaded();
					onFloatable = true;
				}
				transform.position = hitinfo.point;
			}
		}
	}
}
