using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActivationDevice : objectclass, IObject
{
    private Tributton[] options = new Tributton[1];
    private Vector3 adjustedPosition;
    
    private Tributton[] options2 = new Tributton[2];
    private Vector3 adjustedPosition2;

    public Transform ball;
    private Vector3 initialHeight;

    public Camera cam;
    public Vector3 inspectpos;
    public Vector3 inspectrot;


    public CamState campos0;
    public CamState campos1;
    public CamState campos2;

    public Vector3 originalpos;
    public Vector3 originalrot;

    private Generalevent.PressButton inspectDel;

    public GameObject centrePeice;
    public GameObject missingPiece;

    // Start is called before the first frame update
    void Start()
    {
        options[0] = new Tributton("inspect", () => { Inspect(); });
        adjustedPosition = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z);

        options2[0] = new Tributton("Back", () => { /*Debug.Log("go back");*/ back(); });
        options2[1] = new Tributton("rotate", () => { /*Debug.Log("rotate");*/ RotateCentre(); });
        //adjustedPosition2 = new Vector3(transform.position.x + 1f, transform.position.y + 0.4f, transform.position.z+1);
        adjustedPosition2 = new Vector3(2.542f, 4.994f, 5.169f);

        initialHeight = ball.position;

        inspectDel += AnimateBall;

        // inspect state
        campos2 = new CamState(75, inspectpos, inspectrot);

        //the state after passing the puzzle
        campos1 = new CamState(63, new Vector3(8.610293f, 14.92599f, -2.714752f), new Vector3(35.512f, 341.829f, -0.117f));

        //the state at the begining
        campos0 = new CamState(63, new Vector3(6.590869f, 14.17019f, -8.2f), new Vector3(51.946f, 325.896f, -0.002f));
}

    // Update is called once per frame
    void Update()
    {
        if (inspectDel != null)
        {
            inspectDel();
        }
    }

    public void setUI()
    {
        GameManager.UISetter(options, adjustedPosition);   // need to use UISetter first, because it would remove any PosEvent with options Tag

        UIPosDerender();
    }

    public void FixedUpdate()
    {
        
    }


    public void Inspect()
    {
        GameManager.UIDesetter();
        CameraControl.MoveCam(campos2, 0.5f, () => {
            GameManager.UISetter(options2, adjustedPosition2, 0.3f);
            this.GetComponent<Collider>().enabled = false;  // quick way to avoid being clicked on when in inspect state
        });

        if(inspectDel != null)
        {
            inspectDel -= AnimateBall;
        }
    }


    public void back()
    {
        GameManager.UIDesetter();
        CameraControl.MoveCam(campos0, 0.5f, () => {
            Bcd = blendCountdown; inspectDel += Blendball;
            this.GetComponent<Collider>().enabled = true;
        });
    }

    public void AnimateBall()
    {
        ball.position = new Vector3(initialHeight.x, initialHeight.y + (0.5f * Mathf.Cos(Time.time * 2)), initialHeight.z);
    }

    public float blendCountdown = 1f;
    private float Bcd;

    // function to blend the stationary ball to its cosine movement, to avoid it jumping
    private void Blendball()
    {
        ball.position = Vector3.Lerp(ball.position, new Vector3(initialHeight.x, initialHeight.y + (0.5f * Mathf.Cos(Time.time * 2)), initialHeight.z), 0.2f);
        if(Bcd <= 0)
        {
            // remove blendball from loop
            inspectDel += AnimateBall;
            inspectDel -= Blendball;
        }
        else
        {
            Bcd -= Time.deltaTime;
        }
    }


    private bool rotating;    // to know if the piece is still rotating
    private bool placedPiece;    // to know if the missing piece has been placed

    //function for rotating the centre piece
    public void RotateCentre()
    {
        if(rotating == true)
        {
            return;
        }

        // loop through inventory
        if (PlayerData.inventory.Count == 0)
        {
            //Debug.Log("nothing in inventory");
            GameManager.ShowInfo("find missing piece first");
            return;
        }

        foreach (InventoryObject item in PlayerData.inventory)
        {
            if (item.Name == "broken key piece")
            {
                //proceed
                //Debug.Log("player has missing piece");
                rotating = true;
                if(placedPiece == false)
                {
                    Vector3 Mpos = missingPiece.transform.position;  //get the position of the missing piece
                    missingPiece.transform.Translate(missingPiece.transform.up * 5f);  //shift the position up
                    missingPiece.SetActive(true);
                    missingPiece.transform.DOMove(Mpos, 1f).OnComplete(() => { actualrotate(); });
                    placedPiece = true;
                }
                else
                {
                    actualrotate();
                }
            }
            else
            {
                //Debug.Log("player does not have missing piece");
                GameManager.ShowInfo("first find missing piece first");
                return;
            }
        }
    }

    // coed behind the actual rotation
    private void actualrotate()
    {
        rotating = true;
        Vector3 initialpos;
        //Transform virtualTransform;

        Vector3 initialangle;
        Vector3 targetangle;

        initialpos = centrePeice.transform.position;
        initialangle = centrePeice.transform.localEulerAngles;

        centrePeice.transform.RotateAround(centrePeice.transform.position, centrePeice.transform.up, 60); //set the desired rotation
        targetangle = centrePeice.transform.localEulerAngles;  //get the desired rotation
        centrePeice.transform.localEulerAngles = initialangle; // reset the rotation

        // produce sound
        FindObjectOfType<AudioManager>().Play("whoosh1");
        centrePeice.transform.DOMove((centrePeice.transform.position + (centrePeice.transform.up * 0.15f)), 0.2f).OnComplete(() => // move piece up
        {
            FindObjectOfType<AudioManager>().Play("whoosh2");
            centrePeice.transform.DOLocalRotate(targetangle, 0.5f).OnComplete(() => //rotate the piece
            {
                FindObjectOfType<AudioManager>().Play("whoosh1");
                centrePeice.transform.DOMove(initialpos, 0.2f).OnComplete(() => { 
                    rotating = false;
                    if (CheckPuzzle()) 
                    { 
                        GameManager.UIDesetter();
                        //cam.DOShakePosition(dur, stren, vibrato, randomnes, true);
                        CameraControl.MoveCam(campos1, 1f, () => { loadnewblocks(); });
                        ball.DOMove(new Vector3(2.03f, 5.214f, 5.26f), 2f);
                    }
                }); // move the piece down
            });
        });
    }

    private bool CheckPuzzle()
    {
        //Debug.Log("A: " + centrePeice.transform.localEulerAngles.y);
        if (centrePeice.transform.localEulerAngles.y >= 355.631f && centrePeice.transform.localEulerAngles.y <= 357.631f)
        {
            //Debug.Log("angle match");
            return true;
        }
        else
        {
            //Debug.Log("angle mismatch");
            return false;
        }
    }

    public PortalRotate portal;
    public GameObject[] floatingsteps;

    private Vector3 activatecampos0 = new Vector3(-0.29f, 3.29f, -1.99f);
    private Vector3 activatecampos1 = new Vector3(-2.55f, 3.29f, 6.98f);

    private void loadnewblocks()
    {
        for(int i = 0; i < floatingsteps.Length; i++)
        {
            GameObject f = floatingsteps[i];
            f.SetActive(true);
            Vector3 initpos = f.transform.position;
            f.transform.Translate(f.transform.up * -50f);
            f.transform.DOMove(initpos, 1.5f + i).OnComplete(() => {
                floater g = f.GetComponent<floater>();
                g.enabled = true;
                StartCoroutine(g.emitparticle(10, 1.5f));
            });
        }

        StartCoroutine(portal.StartPortal(0, 6));

        // change the camera if the character goes to activatecampos1 position
        setcam();
    }

    private void setcam()
    {
        GameManager.RemovePosEventByTag("setcam");
        if (cam.transform.position == campos1.pos)
        {
            PosEvent A = new PosEvent(activatecampos0, () => { CameraControl.MoveCam(campos0, 1f, () => { setcam(); }); }, () => { });
            A.Tag = "setcam";
            GameManager.InPosEvents.Add(A);
        }
        else
        {
            PosEvent A = new PosEvent(activatecampos1, () => { CameraControl.MoveCam(campos1, 1f, () => { setcam(); }); }, () => { });
            A.Tag = "setcam";
            GameManager.InPosEvents.Add(A);
        }
    }

    public void SimpleRotateCentre()
    {
        centrePeice.transform.RotateAround(centrePeice.transform.position, centrePeice.transform.up, 60);
    }
}
