using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Controller : MonoBehaviour
{

    public int cur_cp_idx = 0;
    //Each combatant adds one action for their turn.
    public List<CombatAction> round_actions;
    public CombatAction cur_selected_action;
    public List<Player_Combatant> player_combatants;
    public List<Enemy_Combatant> enemy_combatants;
    public Player_Combatant cur_selected_player;
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

    public List<Combatant> Get_All_Combatants_Player_First(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(player_combatants);
        combatants.AddRange(enemy_combatants);
        return combatants;
    }

    public List<Combatant> Get_All_Combatants_Enemy_First(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(enemy_combatants);
        combatants.AddRange(player_combatants);
        return combatants;
    }

    public CombatAction Init_CombatAction(){
        cur_selected_action = new CombatAction();
        cur_selected_action.actor = cur_selected_player;
        cur_selected_action.speed = 0;
        return cur_selected_action;
    }

    public void Confirm_Action(){
        round_actions.Add(cur_selected_action);
    }
}
