using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Placeholder: Enemy_Combatant
{
    // Start is called before the first frame update

    public List<Combatant> debug_party;
    public List<Combatant> debug_enemy;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    override public CombatAction Run_AI(List<Combatant> player_party, List<Combatant> enemy_party ){
        CombatAction ca = new CombatAction();
        debug_enemy = enemy_party;
        debug_party = player_party;
        ca.targets = new List<Combatant>();
        ca.targets.Add(player_party[0]); //get random party member
        ca.action = attacks[0];
        ca.actor = this;
        ca.speed = stats.spd + ca.action.speed;
        //ca.action.is_flipped = true;
        return ca;
    }
}
