using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attack : Action
{
    // Start is called before the first frame update
    public string attack_name = "lorem";
    public int damage = 0;
    public string description = "ipsem";


    override abstract public void Do_Action();
}
