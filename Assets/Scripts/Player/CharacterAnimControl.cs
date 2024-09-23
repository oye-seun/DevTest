using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterAnimControl : MonoBehaviour
{
    public static event Action<Vector3, Quaternion> AnimatorMove;
    public Animator m_Animator {  get; private set; }

    [SerializeField] LayerMask m_IkFootMask;
    [SerializeField] float m_ShoeDist;
    [SerializeField] float m_HipBoneToLeg;
    [SerializeField] float m_PosLerpSpeed = 2f;

    [SerializeField] Transform m_LShoeCollider;
    [SerializeField] Transform m_RShoeCollider;

    [SerializeField] bool m_ManualIk;
    [SerializeField] Transform m_LLegTarget;
    [SerializeField] Transform m_RLegTarget;
    [SerializeField, Range(0, 1)] float m_LLegWeight;
    [SerializeField, Range(0, 1)] float m_RLegWeight;

    public Transform RightHand { get { return m_Animator.GetBoneTransform(HumanBodyBones.RightHand); } }

    //ThirdPersonCharacter TPC;

    private void OnEnable()
    {
        CutsceneManager.CutsceneEnded += OnCutsceneCompleted;
    }

    private void OnDisable()
    {
        CutsceneManager.CutsceneEnded -= OnCutsceneCompleted;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        //TPC = transform.parent.GetComponent<ThirdPersonCharacter>();

        m_LShoeCollider.parent = m_Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        m_LShoeCollider.position = m_Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position + (Vector3.down * m_ShoeDist);
        m_LShoeCollider.LookAt(m_Animator.GetBoneTransform(HumanBodyBones.LeftToes).position);

        m_RShoeCollider.parent = m_Animator.GetBoneTransform(HumanBodyBones.RightFoot);
        m_RShoeCollider.position = m_Animator.GetBoneTransform(HumanBodyBones.RightFoot).position + (Vector3.down * m_ShoeDist);
        m_RShoeCollider.LookAt(m_Animator.GetBoneTransform(HumanBodyBones.RightToes).position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 MoveIKLegs(Animator anim, AvatarIKGoal goal, float shoeDist, LayerMask layerMask, float weight)
    {
        anim.SetIKPositionWeight(goal, weight);
        anim.SetIKRotationWeight(goal, weight);

        // Left Foot
        RaycastHit hit;
        Ray ray = new Ray(anim.GetIKPosition(goal) + Vector3.up, Vector3.down);
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * (shoeDist + 3f)), Color.red);
        if (Physics.Raycast(ray, out hit, shoeDist + 3f, layerMask))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += shoeDist;
            anim.SetIKPosition(goal, footPosition);
            anim.SetIKRotation(goal, Quaternion.LookRotation(transform.forward, hit.normal));
            //return hit.point;
        }
        return m_Animator.GetIKPosition(goal);

    }

    private void SetIKLegs(Animator anim, AvatarIKGoal goal, float weight, Transform trans)
    {
        anim.SetIKPositionWeight(goal, weight);
        anim.SetIKRotationWeight(goal, weight);

        anim.SetIKPosition(goal, trans.position);
        anim.SetIKRotation(goal, trans.rotation);
    }

    public void OnAnimatorMove()
    {
        //TPC.Move(m_Animator.deltaPosition);
        AnimatorMove?.Invoke(m_Animator.deltaPosition, m_Animator.deltaRotation);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!m_ManualIk)
        {
            float lweight = Mathf.Clamp01(m_Animator.GetFloat("LeftLegWeight") * 20);
            float rweight = Mathf.Clamp01(m_Animator.GetFloat("RightLegWeight") * 20);
            Vector3 Lpos = MoveIKLegs(m_Animator, AvatarIKGoal.LeftFoot, m_ShoeDist, m_IkFootMask, lweight);
            Vector3 Rpos = MoveIKLegs(m_Animator, AvatarIKGoal.RightFoot, m_ShoeDist, m_IkFootMask, rweight);

            //Debug.Log("Lpos: " + Lpos + " Rpos: " + Rpos);
            //m_LeftLegTarget.position = Lpos;

            float capsStartHeight = Mathf.Min(Lpos.y, Rpos.y);
            //capsStartHeight -= transform.position.y;
            //capsStartHeight /= transform.localScale.y;

            //capsStartHeight -= TPC.transform.position.y;
            //capsStartHeight /= TPC.transform.localScale.y;
            capsStartHeight -= transform.parent.position.y;
            capsStartHeight /= transform.parent.localScale.y;


            //Debug.Log("capsStartHeight: " + capsStartHeight);
            //m_CapsuleCenter = m_Capsule.center;
            //m_CapsuleCenter.y = capsStartHeight + (m_Capsule.height / 2);
            //m_Capsule.center = m_CapsuleCenter;

            //Vector3 hipPos = m_Animator.GetBoneTransform(HumanBodyBones.Hips).position;
            Vector3 hipPos = transform.localPosition;
            hipPos.y = capsStartHeight - m_ShoeDist;
            //Debug.Log("hipPos: " + hipPos.y);
            //m_Animator.GetBoneTransform(HumanBodyBones.Hips).position = hipPos;
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipPos, m_PosLerpSpeed * Time.deltaTime);

            m_LLegTarget.position = Lpos;
            m_RLegTarget.position = Rpos;   
        }
        else
        {
            SetIKLegs(m_Animator, AvatarIKGoal.LeftFoot, m_LLegWeight, m_LLegTarget);
            SetIKLegs(m_Animator, AvatarIKGoal.RightFoot, m_RLegWeight, m_RLegTarget);
        }
    }

    private void OnCutsceneCompleted()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}
