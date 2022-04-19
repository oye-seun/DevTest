using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script contains several classes needed in the game */
public class Generalevent
{
    public delegate void PressButton();
}

public class Tributton : Generalevent
{
    private string name;                      //text to be displayed on button
    private PressButton onpressed;            //what to do when button pressed

    //............name getter and setter.................................
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    //...................................................................


    //............onpressed getter and setter............................
    public PressButton button
    {
        get { return onpressed; }
        set { onpressed = value; }
    }
    //...................................................................


    //...........constructor.............................................
    public Tributton(string S, PressButton P)
    {
        Name = S;
        button = P;
    }
    //...................................................................
}

/// <summary>
/// this class stores informmation for events needed to be triggered by position
/// </summary>
public class PosEvent : Generalevent
{
    private Vector3 pos;
    private PressButton onPos;
    private PressButton outPos;
    private string tag;

    //.............position getter and setter............................
    public Vector3 Pos
    {
        get { return pos; }
        set { pos = value; }
    }
    //...................................................................


    //..............onpos getter and setter..............................
    public PressButton OnPos
    {
        get { return onPos; }
        set { onPos = value; }
    }
    //...................................................................


    //..............outpos getter and setter.............................
    public PressButton OutPos
    {
        get { return outPos; }
        set { outPos = value; }
    }
    //...................................................................

    
    //..............tag getter and setter...............................
    public string Tag
    {
        get { return tag; }
        set { tag = value; }
    }
    //...................................................................


    //...............constructor.........................................
    public PosEvent(Vector3 position, PressButton enter, PressButton exit)
    {
        Pos = position;
        OnPos = enter;
        OutPos = exit;
    }
}


public class CanvasObject
{
    private string name;
    private GameObject canvas;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public GameObject canvasobject
    {
        get { return canvas; }
        set { if (value.GetComponent<Canvas>() != null) { canvas = value; }
              else { Debug.LogError("object does not contain canvas"); } }
    }

    public CanvasObject(string nam, GameObject C)
    {
        Name = nam;
        canvas = C;
    }
}

public interface IObject
{
    void setUI();
}



[RequireComponent(typeof(ObjectTagSetter))]
public class objectclass : MonoBehaviour
{
    public void UIPosDerender()
    {
        PosEvent derender = new PosEvent(transform.position, () => { /* do nothing */}, () => {

            //for when it goes out of range
            GameManager.UIDesetter();
            for (int i = GameManager.InPosEvents.Count - 1; i >= 0; i--)  // iterate backwards
            {
                if (GameManager.InPosEvents[i].Pos == transform.position)
                {
                    GameManager.InPosEvents.RemoveAt(i);
                    return;
                }
            }
        });
        derender.Tag = "options";

        GameManager.InPosEvents.Add(derender);
    }
}
