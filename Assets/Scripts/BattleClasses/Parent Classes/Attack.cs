using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Attack : Action
{
    // Start is called before the first frame update
    public string attack_name = "lorem";
    public int damage = 0;
    public string description = "ipsem";

    #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
    #else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
    #endif
    override abstract public void Do_Action(Combatant actor, List<Combatant> targets);
}
