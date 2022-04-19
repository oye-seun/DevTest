using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.EventSystems;

public class RayManager : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public ThirdPersonCharacter TPC; // where TPC means third person character
    public GameObject portalplatform;
    public static bool pulling;

    public static event Generalevent.PressButton onstoppull;
    public static event Generalevent.PressButton onstartpull;

    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CursorOverUI())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "ground" || hit.transform.tag == "floatable")
                {
                    agent.SetDestination(hit.point);
                    FindObjectOfType<Pointer>().Show(0.4f);
                    FindObjectOfType<Pointer>().SetPosiiton(hit.point, hit.normal);
                    //Debug.Log("Ray hit ground");
                }
                else if(hit.transform.tag == "object")
                {
                    IObject hitobject = hit.transform.GetComponent<IObject>();
                    hitobject.setUI();

                    agent.SetDestination(hit.transform.position);
                }
                else if(hit.transform.tag == "portal")
                {
                    agent.SetDestination(portalplatform.transform.position);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pulling = false;
            //Debug.Log("pulling changed to false");
        }

        if (pulling)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                GameManager.UIDesetter();
                TPC.MoveWithBack(agent.desiredVelocity);
                if(onstartpull != null)
                {
                    onstartpull();
                }
            }
            else
            {
                TPC.MoveWithBack(Vector3.zero);
                FindObjectOfType<Pointer>().Hide();
                if (onstoppull != null)
                {
                    onstoppull();
                }
            }
        }
        else
        {
            //Debug.Log("not pulling");
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                TPC.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                TPC.Move(Vector3.zero, false, false);
                FindObjectOfType<Pointer>().Hide();
            }
        }
        
    }

    private bool CursorOverUI()
    {
        // for when mouse is over ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        // for when touch is over ui
        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }

        return false;
    }
}
