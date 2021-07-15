using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAttack4 : Attack
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Do_Action(Combatant actor, List<Combatant> targets)
    {
        Debug.Log(actor.combatant_name + "Is attacking ");
        Debug.Log(targets);
        Debug.Log("With " + attack_name);
        Debug.Log("With Base Damage:" + damage);

    }

    override public void Reset_Cameras(){
        return;
    }
    override public void Set_Camera_Order(int x){
        return;
    }
}
