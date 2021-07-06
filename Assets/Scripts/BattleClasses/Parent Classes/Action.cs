using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
abstract public class Action : MonoBehaviour
{
    //have to have these since we need to keep track of what characters are being switched and undo them
    public int active_idx;
    public int reserve_idx;
    abstract public void Do_Action(Combatant actor, List<Combatant> targets);
}
