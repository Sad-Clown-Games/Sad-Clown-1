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

    public override void Do_Action()
    {
        Debug.Log(cur_actor.combatant_name + "Is attacking ");
        Debug.Log(cur_targets);
        Debug.Log("With " + action_name);
        Debug.Log("With Base Damage:" + damage);

    }

        public override void Stage_Action()
    {
        
    }

    override public void Reset_Cameras(){
        return;
    }
    override public void Set_Camera_Order(int x){
        return;
    }
}
