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
    public List<Player_Combatant> active_player_combatants; //pure data combatants
    public List<GameObject> pawns; //combatants on the field
    public List<GameObject> reserve_pawns; //combatants on the field
    public List<Player_Combatant> reserve_player_combatants; //pure data combatants in the reserve
    public List<Enemy_Combatant> enemy_combatants;
    public Player_Combatant cur_selected_player;
    public Battle_Menu battle_menu;
    public GameObject pawn_prefab;
    public Party_Switch party_switch;
    public float party_spacing;

    public List<Action> executing_actions;
    public GameObject action_base;
    public CombatAction cur_executing_combat_action;
    public Action cur_executing_action;
    public bool is_executing_actions;
    public int cur_executing_action_index;

    // Start is called before the first frame update
    void Start()
    {
        round_actions = new List<CombatAction>();
        Start_Round();
    }

    // Update is called once per frame
    void Update()
    {
        if(is_executing_actions){
            if(!cur_executing_combat_action.is_started){
                //attach the action to a new gameobject
                cur_executing_action = executing_actions[cur_executing_action_index];
                //activate it?? idk this is fuking bonkers at this point
                cur_executing_action.is_active = true;
                cur_executing_combat_action.is_started = true;
                cur_executing_action.Do_Action(cur_executing_combat_action.actor,cur_executing_combat_action.targets);
            }
            else if(!cur_executing_action.is_active){ //if the class is null
                cur_executing_action_index++;
                if(cur_executing_action_index >= round_actions.Count){
                    is_executing_actions = false;
                    round_actions.Clear();
                    executing_actions.Clear();
                    Start_Round();
                }
                else
                    cur_executing_combat_action = round_actions[cur_executing_action_index];
            }
        }
    }

    public List<Combatant> Get_All_Combatants_Player_First(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(active_player_combatants);
        combatants.AddRange(enemy_combatants);
        return combatants;
    }

    public List<Combatant> Get_All_Combatants_Enemy_First(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(enemy_combatants);
        combatants.AddRange(active_player_combatants);
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
        cur_selected_player = active_player_combatants[cur_cp_idx];
        battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
    }

    public Player_Combatant GetPlayer_Combatant(){
        
        return active_player_combatants[cur_cp_idx];
    }


    //eyy we're real devs now
    public void Confirm_Action(List<Combatant> targets,int index){
        cur_selected_action.targets = targets;
        cur_selected_action.action.action_idx = index;
        Debug.Log(round_actions);
        round_actions.Add(cur_selected_action);
        if(cur_cp_idx < active_player_combatants.Count-1)
            NextPlayerCombatant();
        else{
            Calculate_Enemy_Attacks();
            Execute_Combat_Actions();
        }
    }

    public void Switch_Combatant(int idx){
        //create a new party switch and queue it
        cur_selected_action = new CombatAction();
        cur_selected_action.actor = cur_selected_player;
        party_switch = new Party_Switch();
        party_switch.active_idx = cur_cp_idx;
        party_switch.reserve_idx = idx;
        Debug.Log("Swapping "+ cur_cp_idx + " and " + idx);
        cur_selected_action.action = party_switch;
        round_actions.Add(cur_selected_action); 
        Game_Manager.Instance.Flip_Switch_Flag(party_switch.reserve_idx);
        if(cur_cp_idx < active_player_combatants.Count-1)
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
            var last_action = round_actions[round_actions.Count-1];
            if(last_action.action.GetType().Name == "Party_Switch"){
                Game_Manager.Instance.Flip_Switch_Flag(last_action.action.reserve_idx);
            }
            round_actions.RemoveAt(round_actions.Count-1);
            cur_cp_idx--;
            battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
        }

    }
    private void Calculate_Enemy_Attacks(){

    }
    private void Execute_Combat_Actions(){
        int cam_order = 99;
        foreach(CombatAction ca in round_actions){
            var cur = (Instantiate(Game_Manager.Instance.action_registry.Get_Attack_By_Name(ca.Get_Action_Name())));
            //cur.Set_Camera_Order(cam_order);
            executing_actions.Add(cur);
            cam_order -= 1;
        }
        is_executing_actions = true;
        cur_executing_action_index = 0;
        cur_executing_combat_action = round_actions[cur_executing_action_index];
    }

    public void Update_Party_Data(){
        List<Player_Combatant> party = Game_Manager.Instance.Get_Combat_Party_By_Order();
        active_player_combatants = party.GetRange(0,4);
        reserve_player_combatants = party.GetRange(4,party.Count-4);
    }

    private void Start_Round(){
        cur_cp_idx = 0;
        turn_count++;
        Update_Party_Data(); 
        Set_Active_Pawns();
        Set_Reserve_Pawns();
        cur_selected_player = active_player_combatants[cur_cp_idx];
        battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
    }

    private void Set_Active_Pawns(){
        foreach(GameObject a in pawns){
            Destroy(a);
        }
        pawns.Clear();
        foreach(Player_Combatant a in active_player_combatants){
            var cur = Instantiate(pawn_prefab);
            pawns.Add(cur);
            a.pawn = cur;
        }
        for(int i = 0; i < pawns.Count; i++){
            Vector3 offset = active_player_combatants[i].offset;
            offset.x = (i*2.15f) - 3.5f;
            pawns[i].transform.position = offset;
        }
    }

    private void Set_Reserve_Pawns(){
        foreach(GameObject a in reserve_pawns){
            Destroy(a);
        }
        reserve_pawns.Clear();
        foreach(Player_Combatant a in reserve_player_combatants){
            var cur = Instantiate(pawn_prefab);
            reserve_pawns.Add(cur);
            a.pawn = cur;
        }
        for(int i = 0; i < reserve_pawns.Count; i++){
            Vector3 offset = reserve_player_combatants[i].offset;
            offset.x = -((i*2.15f) - 3.5f);
            offset.z -= 4;
            reserve_pawns[i].transform.position = offset;
        }
    }


}
