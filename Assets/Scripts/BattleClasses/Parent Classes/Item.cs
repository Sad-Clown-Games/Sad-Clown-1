using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : Action
{
    public string item_name = "lorem";
    public int damage = 0;
    public string description = "ipsem";
    override abstract public void Do_Action(Combatant actor, List<Combatant> targets);
}
