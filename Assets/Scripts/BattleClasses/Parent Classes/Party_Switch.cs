using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Switch : Action
{
    //because we're setting up to just queue actions,

    private void Awake() {
        action_name = "switch";
    } 
    override public void Do_Action(){
        is_active = true;
        var pc = GameObject.FindGameObjectWithTag("Party_Manager").GetComponent<Party_Controller>(); //this is fucked ok really fucked we shouldn't do this
        pc.Swap_Party_Order(active_idx,reserve_idx); //see it fucked up just like I said
        Exit_Action();
    }

    public void Exit_Action(){
        //return everything
        is_active = false;
        Reset_Cameras();
        if(next_action)
            next_action.Stage_Action();
        Die();
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

    void Die(){
        Destroy(this.gameObject);
    }
}
