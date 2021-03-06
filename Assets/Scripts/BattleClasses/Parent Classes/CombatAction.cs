using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CombatAction 
{
    public Action action;
    public List<Combatant> targets;
    public Combatant actor;
    public float speed;
    public GameObject base_object;
    public bool is_started;
    public void Do_Action(){
        is_started = true;
        action.is_active = true;
        action.GetComponent<Action>().Do_Action();
    }
    public string Get_Action_Name(){
        return action.name;
    }
}
