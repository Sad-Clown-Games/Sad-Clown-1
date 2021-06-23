using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour
{
    public Action action;
    public List<Combatant> targets;
    public Combatant actor;
    public float speed;
}
