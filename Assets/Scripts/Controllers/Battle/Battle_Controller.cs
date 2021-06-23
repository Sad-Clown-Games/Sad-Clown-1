using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Controller : MonoBehaviour
{

    public int cur_cp_idx = 0;
    //Each combatant adds one action for their turn.
    public List<CombatAction> round_actions;
    public List<Player_Combatant> player_combatants;
    public Battle_Menu battle_menu;


    // Start is called before the first frame update
    void Start()
    {
        battle_menu.Start_Selecting(player_combatants[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
