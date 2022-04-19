using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Crate : objectclass, IObject
{
    private Tributton[] options;
    private Vector3 adjustedPosition;

    private bool opened = false;

    [SerializeReference]
    public List<InventoryObject> cratespoils;

    public GameObject player;
    private Transform PullPosition;
    private Transform LeftHandle;
    private Transform RightHandle;

    public AudioSource SlidingSound;
    public AudioSource OpeningSound;

    private bool SlidSoundPaused;
    private bool OpenSoundPaused;

    private void Start()
    {
        if (!opened)
        {
            options = new Tributton[2];
            options[0] = new Tributton("open", () => { /*Debug.Log("crate opened");*/ open(); });
            options[1] = new Tributton("pull", () => { /*Debug.Log("pulling crate");*/ pullcrate(); });
        }
        else
        {
            options = new Tributton[1];
            options[0] = new Tributton("pull", () => { /*Debug.Log("pulling crate");*/ pullcrate(); });
        }
        adjustedPosition = new Vector3(transform.position.x +2, transform.position.y + 2, transform.position.z);


        PullPosition = transform.Find("pull position").GetComponent<Transform>();
        LeftHandle = transform.Find("left handle");
        RightHandle = transform.Find("right handle");

        PosEvent nametag = new PosEvent(transform.position, () => { GameManager.NameSetter("Crate", new Vector3(transform.position.x, transform.position.y + 3, transform.position.z)); }, () => { GameManager.NameDesetter("Crate"); });
        nametag.Tag = "crate";
        GameManager.InPosEvents.Add(nametag);

        RayManager.onstoppull += UpdateNametag;

        RayManager.onstartpull += startSound;
    }

    /*private void Update()
    {
        PlayerData.leftHandTarget.position = LeftHandle.position;
        PlayerData.leftHandTarget.localEulerAngles = LeftHandle.localEulerAngles;
        PlayerData.rightHandTarget.position = RightHandle.position;
        PlayerData.rightHandTarget.localEulerAngles = RightHandle.localEulerAngles;
    }*/

    private void UpdateNametag()
    {
        //find the one with the tag and update it
        for (int i = GameManager.InPosEvents.Count - 1; i >= 0; i--)  // iterate backwards
        {
            if (GameManager.InPosEvents[i].Tag == "crate")
            {
                GameManager.InPosEvents[i].Pos = transform.position;
                break;
            }
        }
        for (int i = GameManager.OutPosEvents.Count -1; i >= 0; i--)
        {
            if(GameManager.OutPosEvents[i].Tag == "crate")
            {
                GameManager.NameDesetter("Crate");
                GameManager.OutPosEvents[i].Pos = transform.position;
                GameManager.NameSetter("Crate", new Vector3(transform.position.x, transform.position.y + 3, transform.position.z));
                break;
            }
        }

        if(RayManager.pulling)
        {
            SetLeaveButton();
        }
        

        RayManager.onstoppull -= UpdateNametag;
        RayManager.onstartpull += AddUpdatenametag;
    }
    private void AddUpdatenametag()
    {
        RayManager.onstoppull += UpdateNametag;
        RayManager.onstartpull -= AddUpdatenametag;
    }

    private void SetLeaveButton()
    {
        options = new Tributton[1];
        options[0] = new Tributton("leave", () => {
            //Debug.Log("stopped pulling crate");
            StopPullCrate();
        });

        adjustedPosition = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
        setUI();
    }

    public void setUI()
    {
        GameManager.UISetter(options, adjustedPosition);   // need to use UISetter first, because it would remove any PosEvent with options Tag

        UIPosDerender();
    }


    public void open()
    {
        if (!opened)
        {
            //some animation before opening ui
            GameManager.UIDesetter(); // remove ui options so that you can see the animation
            GameObject a = transform.Find("crate cover").gameObject;
            OpeningSound.Play();
            a.transform.DOMove(a.transform.position + new Vector3(0, 0.2f, 0), 1f).OnComplete(() =>
            {
                a.transform.DOMove(a.transform.position + new Vector3(0, 0, 0.5f), 1f).OnComplete(() =>
                {
                    GameManager.OpenCrateUI(cratespoils);

                    options = new Tributton[1];
                    options[0] = new Tributton("pull", () => { /*Debug.Log("pulling crate");*/ pullcrate(); });

                    GameManager.UIDesetter();
                    setUI();
                });
            });

            opened = true;
        }
    }

    public bool PullPos;
    public void pullcrate()
    {
        GameManager.UIDesetter();
        if (!PullPos && !RayManager.pulling)
        {
            StartCoroutine(player.GetComponent<ThirdPersonCharacter>().GoDir(PullPosition, ()=> {
                //after aligning position and direction with crate
                PullPos = false;
                RayManager.pulling = true;
                StartCoroutine(placeHands(0, 0.35f));
            }));
            PullPos = true;
        }
        else
        {
            Debug.Log("Godir already running");
        }
        
        //RayManager.pulling = true;
    }

    public IEnumerator placeHands(float mintime, float maxtime)
    {
        /*Vector3 offset;
        offset = player.transform.position - transform.position;

        Vector3 newpos = player.transform.position - (-player.transform.forward * (offset.magnitude - 0.1f));
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + (Vector3.up * 0.3f), Vector3.down, out hitInfo, 3f))
        {
            newpos = new Vector3(newpos.x, hitInfo.point.y + off, newpos.z);
        }*/

        float time = mintime;
        while(time < maxtime)
        {
            PlayerData.leftHandTarget.position = Environment.interpolatevector(PlayerData.leftHandTarget.position,  LeftHandle.position, time, mintime, maxtime);
            PlayerData.leftHandTarget.localEulerAngles = Environment.interpolatevector(PlayerData.leftHandTarget.localEulerAngles, LeftHandle.localEulerAngles, time, mintime, maxtime);
            PlayerData.rightHandTarget.position = Environment.interpolatevector(PlayerData.rightHandTarget.position, RightHandle.position, time, mintime, maxtime);
            PlayerData.rightHandTarget.localEulerAngles = Environment.interpolatevector(PlayerData.rightHandTarget.localEulerAngles, RightHandle.localEulerAngles, time, mintime, maxtime);

            PlayerData.leftHandConstraint.weight = Environment.interpolate(mintime, maxtime, time, 0, 1);
            PlayerData.rightHandConstraint.weight = Environment.interpolate(mintime, maxtime, time, 0, 1);

            //transform.position = Environment.interpolatevector(transform.position, newpos, time, mintime, maxtime);

            time += Time.deltaTime;
            yield return null;
        }

        SetLeaveButton();
        player.GetComponent<NavMeshAgent>().SetDestination(player.transform.position); //experimental; to stop rotation after placing hands on the crate
        StartCoroutine(UpdateCrate());
        //transform.parent = player.transform;
    }

    public IEnumerator removeHands(float mintime, float maxtime)
    {
        float time = mintime;
        while(time < maxtime)
        {
            PlayerData.leftHandConstraint.weight = Environment.interpolate(mintime, maxtime, time, 1, 0);
            PlayerData.rightHandConstraint.weight = Environment.interpolate(mintime, maxtime, time, 1, 0);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void startSound()
    {
        SlidingSound.Play();
        RayManager.onstartpull -= startSound;
        RayManager.onstoppull += stopSound;
    }
    
    private void stopSound()
    {
        SlidingSound.Stop();
        RayManager.onstoppull -= stopSound;
        RayManager.onstartpull += startSound;
    }


    public float off = 4.2f;
    public IEnumerator UpdateCrate()
    {
        Vector3 offset;
        offset = player.transform.position - transform.position;
        float initcor = 0.2f; // 0.2 is an experimental correction value. used to correct crate jump at pull start
        while (RayManager.pulling)
        {
            Vector3 newpos;
            newpos = player.transform.position - (-player.transform.forward * (offset.magnitude-initcor));
            
            if(initcor > 0)
            {
                initcor -= (Time.deltaTime * 0.5f);
            }
            
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.3f), Vector3.down, out hitInfo, 3f))
            {
                newpos = new Vector3(newpos.x, hitInfo.point.y + off, newpos.z);
                Quaternion updir = Quaternion.FromToRotation(transform.up, hitInfo.normal);
                ///transform.localEulerAngles = new Vector3(updir.eulerAngles.x, transform.localEulerAngles.y, updir.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, updir, Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(-player.transform.right, transform.up);
                //Debug.Log(hitInfo.point);

                transform.position = newpos;

                PlayerData.leftHandTarget.position = LeftHandle.position;
                PlayerData.leftHandTarget.localEulerAngles = LeftHandle.localEulerAngles;
                PlayerData.rightHandTarget.position = RightHandle.position;
                PlayerData.rightHandTarget.localEulerAngles = RightHandle.localEulerAngles;
            }

            
            yield return null;
        }
    }

    private void StopPullCrate()
    {
        RayManager.pulling = false;
        StopCoroutine(UpdateCrate());
        StartCoroutine(removeHands(0, 0.5f));
        if (!opened)
        {
            options = new Tributton[2];
            options[0] = new Tributton("open", () => { /*Debug.Log("crate opened");*/ open(); });
            options[1] = new Tributton("pull", () => { /*Debug.Log("pulling crate");*/ pullcrate(); });
        }
        else
        {
            options = new Tributton[1];
            options[0] = new Tributton("pull", () => { /*Debug.Log("pulling crate");*/ pullcrate(); });
        }
        //UIPosDerender();
        adjustedPosition = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);
        setUI();

        if (SlidingSound.isPlaying)
        {
            SlidingSound.Stop();
        }
    }

    // pause the sound playing
    public void PauseSounds()
    {
        if (SlidingSound.isPlaying)
        {
            SlidingSound.Pause();
            SlidSoundPaused = true;
        }
        if (OpeningSound.isPlaying)
        {
            OpeningSound.Pause();
            OpenSoundPaused = true;
        }
    }

    // resume the paused sounds
    public void ResumeSounds()
    {
        if (SlidSoundPaused)
        {
            SlidingSound.UnPause();
            SlidSoundPaused = false;
        }
        if (OpenSoundPaused)
        {
            OpeningSound.UnPause();
            OpenSoundPaused = false;
        }
    }
}