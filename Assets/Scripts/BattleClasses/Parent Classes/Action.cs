using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
abstract public class Action : MonoBehaviour
{
    //have to have these since we need to keep track of what characters are being switched and undo them
    public int active_idx; //idx of the actor
    public int reserve_idx; //idx of target
    public int action_idx; //idx of the item or attack
    public bool is_active;

    abstract public void Do_Action(Combatant actor, List<Combatant> targets);

    abstract public void Reset_Cameras();

    //adds x number to cameras;
    abstract public void Set_Camera_Order(int x);
}
