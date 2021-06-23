using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttack1 : Attack
{
    // Start is called before the first frame update
    new public string attack_name = "DummyAttack1";
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Do_Action(List<GameObject> targets)
    {
        Debug.Log("Attack with " + attack_name);
        Debug.Log("With" + targets);
    }
}
