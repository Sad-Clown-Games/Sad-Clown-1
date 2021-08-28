using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
abstract public class Action : MonoBehaviour
{
    //have to have these since we need to keep track of what characters are being switched and undo them
    public string action_name= "lorem";
    public int damage = 0;
    public string description = "ipsem";

    public bool is_flipped = false;
    public int active_idx; //idx of the actor
    public int reserve_idx; //idx of target
    public int action_idx; //idx of the item or attack
    public bool is_active;
    public int speed = 1;
    public Action next_action;
    public Combatant cur_actor;
    public Battle_Controller controller;
    public List<Combatant> cur_targets;
    public GameObject damage_display;
    abstract public void Do_Action();

    abstract public void Reset_Cameras();

    //adds x number to cameras;
    abstract public void Set_Camera_Order(int x);

     abstract public void Stage_Action();

    public void Transfer_Actors(CombatAction ca){
        cur_actor = ca.actor;
        cur_targets = ca.targets;
    }
}
