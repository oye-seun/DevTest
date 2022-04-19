using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    //..................Position Event variables....................................................................................

    // list containing all the events triggered by position, needs to be accessible from anywhere
    public static List<PosEvent> InPosEvents = new List<PosEvent>();
    public static List<PosEvent> OutPosEvents = new List<PosEvent>();

    // reference to the player
    public GameObject playe;
    public static GameObject player;

    //dist to activate event
    public float EventDistance;

    //used in the calculation of nearness to event, created here to reduce garbage
    private Vector3 calcpos;
    private float len;
    //..............................................................................................................................



    //................UI name setter variables......................................................................................
    [SerializeField]
    public static Camera cam;
    public GameObject namecanvas;      //prefab for displaying name canvas
    private static CanvasObject[] CanvasPool = new CanvasObject[5];
    //..............................................................................................................................


    //................UI object options variables...................................................................................
    public GameObject optioncanvas;
    public GameObject uibutton;
    private static GameObject Ocanvas;
    private static GameObject[] options;
    //..............................................................................................................................


    //................navmesh agent.................................................................................................
    private ThirdPersonCharacter TPCagent;
    public Image SpeedIcon;
    //..............................................................................................................................


    //...............pause and play stuff...........................................................................................
    public GameObject playPanel;
    public static GameObject PlayPanel;

    public GameObject pausePanel;
    public static GameObject PausePanel;

    public GameObject gameoverPanel;
    public static GameObject GameoverPanel;
    //..............................................................................................................................


    //...............PlayerData stuff...............................................................................................
    public Text cointext;
    private static Text CoinText;
    //..............................................................................................................................


    //...............Crate open ui stuff............................................................................................
    public GameObject crateopencanvas;
    public GameObject crateitem;
    
    public static GameObject CrateOpenCanvas;
    public static GameObject CrateItem;

    private static GameObject COC;  // singleton of the Crate Open Canvas
    //..............................................................................................................................


    //...............UI Info stuff..................................................................................................
    public GameObject infopanel;
    private static GameObject InfoPanel;
    //..............................................................................................................................

    // Start is called before the first frame update
    void Start()
    {
        // assign the player
        player = playe;
        //Debug.Log(player.transform.position);

        //..........cerating posevents will be done in seperate script later...................................
        //Vector3 position = new Vector3(4.66f, 4f, 0);
        //InPosEvents.Add(new PosEvent(position, () => { NameSetter("Crate", new Vector3(position.x, position.y + 3, position.z)); }, () => { NameDesetter("Crate"); }));

        Vector3 position2 = new Vector3(-4.58f, 4.86f, 2.44f);
        InPosEvents.Add(new PosEvent(position2, () => { NameSetter("Vendor", new Vector3(position2.x, position2.y + 3, position2.z)); }, () => { NameDesetter("Vendor"); }));

        Vector3 position3 = new Vector3(1.43f, 3.05f, 5.37f);
        InPosEvents.Add(new PosEvent(position3, () => { NameSetter("Activator", new Vector3(position3.x, position3.y + 4, position3.z)); }, () => { NameDesetter("Activator"); }));
        //..............................................................................................

        // create several name canvasses on start
        for (int i = 0; i < CanvasPool.Length; i++)
        {
            GameObject canvas = Instantiate(namecanvas);  // Instantiate at start, minimize runtime garbage
            canvas.SetActive(false);
            CanvasPool[i] = new CanvasObject("canvas" + i, canvas);
        }


        //instantiate the options canvas
        Ocanvas = Instantiate(optioncanvas);
        Ocanvas.SetActive(false);
        options = new GameObject[5];
        for(int i = 0; i < 5; i++)
        {
            options[i] = Instantiate(uibutton, Ocanvas.transform.Find("panel").Find("grid"));
            options[i].SetActive(false);
        }


        // assign the camera
        cam = FindObjectOfType<Camera>();  // this probably has large execution time


        // assign the TPCagent
        TPCagent = player.GetComponent<ThirdPersonCharacter>();

        // assign the coin Text
        CoinText = cointext;      // don't want to search the scene anymore

        // assign crate open ui stuffs
        CrateItem = crateitem;
        CrateOpenCanvas = crateopencanvas;

        // assign the info panel
        InfoPanel = infopanel;

        //assign the panels
        PlayPanel = playPanel;
        PausePanel = pausePanel;
        GameoverPanel = gameoverPanel;
        
        load();
    }

    // Update is called once per frame
    void Update()
    {
        Poscheck();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PauseGame();
            //Debug.Log("on app focus");
        }
    }

 
    private void Poscheck()
    {
        //check the list of events waiting to happen;
        for (int i = InPosEvents.Count - 1; i >= 0; i--)  // iterate backwards
        {
            calcpos = player.transform.position - InPosEvents[i].Pos;      //get vector difference
            len = (calcpos.x * calcpos.x) + (calcpos.y * calcpos.y) + (calcpos.z * calcpos.z);    //calculate the square of the length
            if( len < (EventDistance * EventDistance))  //check if it is in range for event
            {
                InPosEvents[i].OnPos(); // Do the function
                OutPosEvents.Add(InPosEvents[i]);  // add the position event to the check out list
                InPosEvents.RemoveAt(i);  // remove the position event from the check in list
            }
        }

        //check the list of events waiting to go out of range
        for (int i = OutPosEvents.Count - 1; i >= 0; i--) // iterate backwards
        {
            calcpos = player.transform.position - OutPosEvents[i].Pos;      //get vector difference
            len = (calcpos.x * calcpos.x) + (calcpos.y * calcpos.y) + (calcpos.z * calcpos.z);    //calculate the square of the length
            if (len >= (EventDistance * EventDistance))  //check if it out of range for event
            {
                InPosEvents.Add(OutPosEvents[i]);  // add the position event to the check in list
                OutPosEvents[i].OutPos(); // Do the function
                OutPosEvents.RemoveAt(i);  // remove the position event from the check out list
            }
        }
    }

    public static void RemovePosEventByTag(string tag)
    {
        for (int i = InPosEvents.Count - 1; i >= 0; i--)  // iterate backwards
        {
            if (InPosEvents[i].Tag == tag)
            {
                InPosEvents.RemoveAt(i);
                return;
            }
        }
        for (int i = OutPosEvents.Count - 1; i >= 0; i--)  // iterate backwards
        {
            if (OutPosEvents[i].Tag == tag)
            {
                OutPosEvents.RemoveAt(i);
                return;
            }
        }
    }

    public static void NameSetter(string name, Vector3 pos)
    {
        // check to see unactive canvas pool
        foreach(CanvasObject canvas in CanvasPool)
        {
            if(canvas.canvasobject.activeSelf == false)  // canvas is not active and is ready to use
            {
                //set the name, so it can be found later
                canvas.Name = name;

                //use the canvas
                canvas.canvasobject.SetActive(true);
                canvas.canvasobject.transform.position = pos;
                canvas.canvasobject.transform.localEulerAngles = new Vector3(90 - cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
                canvas.canvasobject.transform.Find("Panel").Find("Text").GetComponent<Text>().text = name;
                float dist = (cam.transform.position - pos).magnitude;
                float a = 0.000379f;
                canvas.canvasobject.transform.localScale = new Vector3(0.005f  + (dist * a), 0.005f + (dist * a), 0.005f + (dist * a));
                return;
            }
        }
    }

    public static void NameDesetter(string name)
    {
        // check to see active canvas pool
        foreach (CanvasObject canvas in CanvasPool)
        {
            if (canvas.canvasobject.activeSelf == true && canvas.Name == name)  // canvas is active and ready to deactivate
            {
                // deactivate the canvas
                canvas.canvasobject.SetActive(false);
            }
        }
    }


    public static void UISetter(Tributton[] buttons, Vector3 pos, float scale = 1.3f)
    {
        if (buttons.Length > 5)
        {
            Debug.LogError("options more than ui array size");
            return;
        }

        RemovePosEventByTag("options");
        //......................................................................................


        //..............set UI in the desired location and rotation..............................
        Ocanvas.SetActive(true);
        Ocanvas.transform.position = pos;
        //.....................................................................................


        //.................hide all buttons...........................................
        for (int i = 0; i < 5; i++)
        {
            options[i].SetActive(false);
        }
        //...........................................................................

        //..................set rotation, best for orthogonal......................................
        //Ocanvas.transform.localEulerAngles = new Vector3(90 - cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
        //............................................................................................

        //..................set rotation, best for perspective.............................................
        Ocanvas.transform.LookAt(cam.transform.position);
        Ocanvas.transform.forward = -Ocanvas.transform.forward;
        Ocanvas.transform.localEulerAngles = new Vector3 (Ocanvas.transform.localEulerAngles.x,cam.transform.eulerAngles.y, Ocanvas.transform.localEulerAngles.z);
        //............................................................................................


        //..................scale the ui.............................................................
        float dist = (cam.transform.position - pos).magnitude;
        float a = 0.000379f;
        Ocanvas.transform.localScale = new Vector3((0.005f + (dist * a)) * scale, (0.005f + (dist * a)) * scale, (0.005f + (dist * a)) * scale);
        //...........................................................................................

        //modify the  buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            options[i].SetActive(true);
            int num = i;
            options[i].GetComponent<Button>().onClick.RemoveAllListeners();
            options[i].GetComponent<Button>().onClick.AddListener(() => { WaitUntilPos(pos, buttons[num].button); });
            options[i].transform.Find("Text").GetComponent<Text>().text = buttons[i].Name;
        }

        FindObjectOfType<AudioManager>().Play("uiclick");
    }


    public static void WaitUntilPos(Vector3 pos, Generalevent.PressButton action)
    {
        if (Vector3.Distance(player.transform.position,pos) < 5f)
        {
            action();
        }
        else
        {
            ShowInfo("move closer");
        }
    }

    public static void UIDesetter()
    {
        Ocanvas.SetActive(false);
    }

    public void IncreaseSpeed()
    {
        //float speed = agent.speed + 0.1f;
        float speed = TPCagent.normalspeed + 0.1f;
        speed = Mathf.Clamp(speed, 0.4f, 1.5f);
        TPCagent.normalspeed = speed;
        SpeedIcon.fillAmount = (speed - 0.4f) / 1.1f;
    }

    public void DecreaseSpeed()
    {
        float speed = TPCagent.normalspeed - 0.1f;
        speed = Mathf.Clamp(speed, 0.4f, 1.5f);
        TPCagent.normalspeed = speed;
        SpeedIcon.fillAmount = (speed - 0.4f) / 1.1f;
    }

    public void PauseGame()
    {
        PlayPanel.SetActive(false); //switch the panels
        PausePanel.SetActive(true);

        FindObjectOfType<AudioManager>().PauseAll();
        FindObjectOfType<Crate>().PauseSounds();
        FindObjectOfType<PortalRotate>().PauseSounds();

        Time.timeScale = 0;    // quickest way to pause unity
    }

    public void PlayGame()
    {
        Time.timeScale = 1;    // quickest way to pause unity

        FindObjectOfType<AudioManager>().ResumeAll();
        FindObjectOfType<Crate>().ResumeSounds();
        FindObjectOfType<PortalRotate>().ResumeSounds();

        PausePanel.SetActive(false);
        PlayPanel.SetActive(true);
    }

    public void load()
    {
        float speed = TPCagent.normalspeed;
        SpeedIcon.fillAmount = (speed - 0.4f) / 1.1f;

        //include script to load coin data here
        SetCoins(PlayerData.coins);
    }

    public static void SetCoins(int coin)
    {
        CoinText.text = coin.ToString();
    }

    public static void OpenCrateUI(List<InventoryObject> cratespoils)
    {
        PlayPanel.SetActive(false);
        COC = Instantiate(CrateOpenCanvas);    // instantiate the canvas
        COC.transform.Find("panel").Find("close").GetComponent<Button>().onClick.AddListener(() => { Destroy(COC); PlayPanel.SetActive(true); });

        //quick animation stuff
        COC.transform.Find("panel").GetComponent<RectTransform>().localScale = new Vector3(1, 0.05f, 1);
        COC.transform.Find("panel").GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() =>
        {
            renderitem(cratespoils.Count-1, cratespoils);
        });
    }

    //simple recursion algo for loading the crate items one by one
    private static void renderitem(int num, List<InventoryObject> cratespoils)
    {
        if(num < 0)
        {
            return;
        }
        GameObject A = Instantiate(CrateItem, COC.transform.Find("panel").Find("grid"), false);
        A.transform.Find("Image").GetComponent<Image>().sprite = cratespoils[num].Icon;
        A.transform.Find("name").GetComponent<Text>().text = cratespoils[num].Name;
        cratespoils[num].OnCollect();

        // animation stuff
        A.GetComponent<RectTransform>().localScale = new Vector3(1, 0.05f, 1);
        A.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() => { renderitem(num - 1, cratespoils); });
    }

    public static void ShowInfo(string info)
    {
        InfoPanel.transform.Find("text").GetComponent<Text>().text = info;
        InfoPanel.SetActive(true);

        //first set the alpha high
        InfoPanel.transform.GetComponent<Image>().CrossFadeAlpha(1f, 0.0f, false);
        InfoPanel.transform.Find("text").GetComponent<Text>().CrossFadeAlpha(1, 0.0f, false);

        //fade the alpha low
        InfoPanel.transform.GetComponent<Image>().CrossFadeAlpha(0, 3f, false);
        InfoPanel.transform.Find("text").GetComponent<Text>().CrossFadeAlpha(0, 2f, false);

        //InfoPanel.transform.GetComponent<Image>().DOFade
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
