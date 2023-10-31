using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    private ThirdPersonCharacter _tpc;
    public float MoveValMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        _tpc = GetComponent<ThirdPersonCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = PlayerInputs.instance.MoveVal();
        Debug.Log("move input: " + move);
        _tpc.Move(new Vector3(move.x * MoveValMultiplier, 0, move.y * MoveValMultiplier), false, false);
    }
}
