using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Combatant : Combatant
{

    //ok so the way this is going to work is that we will use the attacks list for if the player is scanning the character for attacks/beastiary
    //and we just keep track of the index of the attack we want to do
    //enemies will really only have under 10 attacks, and for the most part 2-3.

    //
    abstract public CombatAction Run_AI(List<Combatant> player_party, List<Combatant> enemy_party );

}
