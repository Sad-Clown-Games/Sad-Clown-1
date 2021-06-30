using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Controller : MonoBehaviour
{

    public int cur_cp_idx = 0;
    public int turn_count = 0;
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
        round_actions = new List<CombatAction>();
        cur_selected_player = player_combatants[cur_cp_idx];
        Start_Round();
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

    public CombatAction Init_CombatAction(Action action){
        cur_selected_action = new CombatAction();
        cur_selected_action.actor = cur_selected_player;
        cur_selected_action.action = action;
        //calculate attack speed from action
        cur_selected_action.speed = 0;
        return cur_selected_action;
    }

    public void NextPlayerCombatant(){
        cur_cp_idx++;
        cur_selected_player = player_combatants[cur_cp_idx];
        battle_menu.Start_Selecting(player_combatants[cur_cp_idx]);
    }

    public Player_Combatant GetPlayer_Combatant(){
        
        return player_combatants[cur_cp_idx];
    }


    //eyy we're real devs now
    public void Confirm_Action(List<Combatant> targets){
        cur_selected_action.targets = targets;
        Debug.Log(round_actions);
        round_actions.Add(cur_selected_action);
        if(cur_cp_idx < player_combatants.Count-1)
            NextPlayerCombatant();
        else{
            Calculate_Enemy_Attacks();
            Execute_Combat_Actions();
            round_actions.Clear();
        }
    }

    //pop it lock it sock it cock it
    public void Undo_Confirm_Action(){
        if(cur_cp_idx > 0){
            round_actions.RemoveAt(round_actions.Count-1);
            cur_cp_idx--;
            battle_menu.Start_Selecting(player_combatants[cur_cp_idx]);
        }

    }
    private void Calculate_Enemy_Attacks(){

    }
    private void Execute_Combat_Actions(){
        foreach (CombatAction action in round_actions)
        {
            action.action.Do_Action(action.actor, action.targets);
        }
        Start_Round();
    }

    private void Start_Round(){
        cur_cp_idx = 0;
        turn_count++;
        battle_menu.Start_Selecting(player_combatants[cur_cp_idx]);
    }
}
