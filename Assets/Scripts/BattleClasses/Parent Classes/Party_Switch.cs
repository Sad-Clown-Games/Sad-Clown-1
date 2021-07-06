using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Switch : Action
{
    // Start is called before the first frame update
    public string attack_name = "lorem";
    public int damage = 0;
    public string description = "ipsem";
    //because we're setting up to just queue actions, 
    override public void Do_Action(Combatant actor, List<Combatant> targets){
        var gm = Game_Manager.Instance;
        gm.Swap_Party_Order(active_idx,reserve_idx);
    }


}
