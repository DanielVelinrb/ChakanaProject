using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{

    public bool walking;
    public bool interact;
    public bool interacting;
    public bool lookingRight;
    public bool jumping;
    public bool recoilingX;
    public bool recoilingY;
    public bool casting;
    public bool castReleased;
    public bool onBench;
    public bool atBench;
    public bool atNPC;
    public bool usingNPC;
    //public bool 

    public float x= -0.28f;
    public float y=0;
    public int firstRunFlag = 1;
    public int flipFlag = 0;

}
