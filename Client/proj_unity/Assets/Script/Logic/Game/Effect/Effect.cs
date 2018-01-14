using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    static int Count = 0;
    
    public Effect(short effectGdsId)
    {
        iD = ++Count;

    }

    public int iD
    {
        get;
        private set;
    }

    public void Destroy()
    {

    }
}
