using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    ParabolaEdit
}


[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
    public Animator anim;

    IEnumerator Start()
    {
        anim = GetComponent<Animator>();

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink)
            {
                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine(NormalSpeed(agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine(Parabola(agent, 0.7f, 0.5f));
                else if (method == OffMeshLinkMoveMethod.ParabolaEdit)
                    yield return StartCoroutine(ParabolaEdit(agent, 0.3f, 0.9f));
                agent.CompleteOffMeshLink();
                anim.applyRootMotion = true;
                anim.SetBool("runjump", false);
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
    IEnumerator ParabolaEdit(NavMeshAgent agent, float height, float duration)
    {

        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.gameObject.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        Vector3 disp = endPos - transform.position;
        Quaternion dir = Quaternion.LookRotation(disp);

        Debug.Log("A");
        float normalizedTime = -0.4f;
        while (normalizedTime < 1.0f)
        {
            if(normalizedTime < 0.0f)
            {
                //Debug.Log("turning");
                transform.rotation = Quaternion.Lerp(transform.rotation, dir, 10 * Time.deltaTime);
                anim.SetBool("prejump", true);
            }
            else
            {
                //Debug.Log("jumping");
                transform.rotation = Quaternion.Lerp(transform.rotation, dir, 10 * Time.deltaTime);
                ThirdPersonCharacterModified.onFloatable = false;
                
                anim.applyRootMotion = false;
                anim.SetBool("runjump", true);
                anim.SetBool("prejump", false);
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            }
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        ThirdPersonCharacterModified.onFloatable = false;
        anim.SetBool("runjump", false);
    }
}
