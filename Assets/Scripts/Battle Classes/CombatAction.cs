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
}
