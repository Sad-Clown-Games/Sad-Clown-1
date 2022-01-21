using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using NodeCanvas.DialogueTrees;

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
    
    public Cinemachine.CinemachineVirtualCamera party_cam;
    public bool won = false;
    public bool lost = false;
    public Vector3 party_location;
    public BattleUIController UIController;
    public Party_Controller party_controller;
    public bool battle_started = false;
    public List<Item> battle_end_items;
    public int battle_end_exp;
    List<Player_Combatant> battle_end_leveled; 
    public bool dialogue_started;
    public bool done_listing_items;
    public bool done_listing_levels;
    public bool set_won_flags;
    private int end_item_idx = 0;
    private int end_level_idx = 0;
    public DialogueTreeController win_dialogue_controller;
    public DialogueTreeController level_dialogue_controller;
    public DialogueTreeController item_dialogue_controller;
    public DialogueTreeController lose_dialogue_controller;
    public NodeCanvas.Framework.Blackboard win_blackboard;
    public NodeCanvas.Framework.Blackboard level_blackboard;
    public NodeCanvas.Framework.Blackboard item_blackboard;
    public NodeCanvas.Framework.Blackboard lose_blackboard;

    // Start is called before the first frame update
    void Start()
    {
        round_actions = new List<CombatAction>();
        battle_started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Game_Manager.Instance.finished_loading && !battle_started){
            battle_started = true;
            Start_Round();
        }
        if(is_executing_actions){
            //Check if the party won or lose
            if(Is_Party_Dead()){
                End_Battle_Lose();
            }
            if(Is_Enemies_Dead()){
                End_Battle_Win();
            }
            if(!cur_executing_combat_action.is_started && is_executing_actions){ //starting new action
                //startup actions
                cur_executing_action = executing_actions[cur_executing_action_index];
                //get our action from 
                cur_executing_action.is_active = true;
                cur_executing_combat_action.is_started = true;
                cur_executing_combat_action.action.controller = this;
                if(cur_executing_action_index == 0){
                    cur_executing_action.Stage_Action();
                }
                cur_executing_action.Do_Action();
            }
            else if(is_executing_actions && !cur_executing_action.is_active){ //if action already done
                cur_executing_action_index++;
                if(cur_executing_action_index >= round_actions.Count){ 
                    //clear everything and start the next round
                    is_executing_actions = false;
                    round_actions.Clear();
                    executing_actions.Clear();
                    Start_Round();
                }
                else //do the next action
                    cur_executing_combat_action = round_actions[cur_executing_action_index];
            }
        }
        if(set_won_flags){
            End_Dialogue();
        }
    }

    public List<Combatant> Get_All_Combatants_Player_First(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(active_player_combatants);
        combatants.AddRange(enemy_combatants);
        return combatants;
    }

    public List<Combatant> Get_All_Player_Combatants(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(active_player_combatants);
        combatants.AddRange(reserve_player_combatants);
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
        cur_selected_action.speed = cur_selected_player.stats.spd + action.speed;
        return cur_selected_action;
    }

    public void NextPlayerCombatant(){
        cur_cp_idx++;
        cur_selected_player = active_player_combatants[cur_cp_idx];
        battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
    }

    public List<Combatant> Get_All_Enemy_Combatants(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(enemy_combatants);
        return combatants;
    }

    public List<Combatant> Get_All_Active_Player_Combatants(){
        List<Combatant> combatants = new List<Combatant>();
        combatants.AddRange(active_player_combatants);
        return combatants;
    }

    public Player_Combatant GetPlayer_Combatant(){
        return active_player_combatants[cur_cp_idx];
    }

    public void Confirm_Action(List<Combatant> targets,int index){
        cur_selected_action.targets = targets;
        cur_selected_action.action.action_idx = index;
        round_actions.Add(cur_selected_action);
        if(cur_cp_idx < active_player_combatants.Count-1)
            NextPlayerCombatant();
        else{
            battle_menu.menus_active = false; //disable input to the battle_menu
            Calculate_Enemy_Attacks();
            Execute_Combat_Actions();
        }
    }

    public void Switch_Combatant(int idx){
        //create a new party switch and queue it
        cur_selected_action = new CombatAction();
        cur_selected_action.actor = cur_selected_player;
        party_switch = (Instantiate(Game_Manager.Instance.action_registry.Get_Switch()));
        party_switch.active_idx = cur_cp_idx;
        party_switch.reserve_idx = idx;
        Debug.Log("Swapping "+ cur_cp_idx + " and " + idx);
        cur_selected_action.action = party_switch;
        cur_selected_action.speed = cur_selected_player.stats.spd + party_switch.speed;
        round_actions.Add(cur_selected_action); 
        party_controller.Flip_Switch_Flag(party_switch.reserve_idx);
        if(cur_cp_idx < active_player_combatants.Count-1)
            NextPlayerCombatant();
        else{
            Calculate_Enemy_Attacks();
            Execute_Combat_Actions();
            //round_actions.Clear();
        }
    }

    //pop it lock it sock it cock it
    public void Undo_Confirm_Action(){
        if(cur_cp_idx > 0){
            var last_action = round_actions[round_actions.Count-1];
            if(last_action.action.GetType().Name == "Party_Switch"){
                party_controller.Flip_Switch_Flag(last_action.action.reserve_idx);
            }
            round_actions.RemoveAt(round_actions.Count-1);
            cur_cp_idx--;
            battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
        }
    }
    
    private void Calculate_Enemy_Attacks(){
        List<Combatant> enemies = Get_All_Enemy_Combatants();
        foreach(Enemy_Combatant enemy in enemies){
            Debug.Log(Get_All_Active_Player_Combatants());
            CombatAction action = enemy.Run_AI(Get_All_Active_Player_Combatants(),Get_All_Enemy_Combatants());
            round_actions.Add(action);
        }
    }
    private void Execute_Combat_Actions(){ 
        //we have combat actions that store the targets and actor and speed
        //we need to pass the targets and actors into the action
        Action last = null; 
        Action cur = null;
        //sort round_actions by speed;
        round_actions = round_actions.OrderByDescending(a=>a.speed).ToList(); 
        foreach(CombatAction ca in round_actions){
            Debug.Log(ca.action.name);
            if(ca.action.action_name is "switch"){
                cur = ca.action;
            }
            else{
                //create the action's object and cameras
                cur = (Instantiate(Game_Manager.Instance.action_registry.Get_Attack_By_Name(ca.Get_Action_Name())));
                //set up "next action" so that the camera can be activated at the right time
                //pass actors in
                cur.Transfer_Actors(ca);
            }
            if(last){
                last.next_action = cur; //set next action
            }
            executing_actions.Add(cur);
            last = cur;

        }
        //this is really stupid and I might have been drunk when I coded this
        is_executing_actions = true;
        cur_executing_action_index = 0; //start executing actions at 0, which is going at Update();
        cur_executing_combat_action = round_actions[cur_executing_action_index];
    }

    public void Update_Party_Data(){
        List<Player_Combatant> party = party_controller.Get_Combat_Party_By_Order();
        int party_count = party.Count;
        if(party_count < 4){
            active_player_combatants = party.GetRange(0,party_count);
        }
        else{
            active_player_combatants = party.GetRange(0,4);
            reserve_player_combatants = party.GetRange(4,party.Count-4); //start index at 4 and then count 0-4 up?? this method is fucked.
        }
    }

    private void Start_Round(){
        cur_cp_idx = 0;
        turn_count++;
        Update_Party_Data(); 
        Set_Active_Pawns();
        Set_Reserve_Pawns();
        cur_selected_player = active_player_combatants[cur_cp_idx];
        battle_menu.menus_active = true;
        battle_menu.Start_Selecting(active_player_combatants[cur_cp_idx]);
    }

    private void Set_Active_Pawns(){
        //killing and remaking pawns will let us switch characters easier. 
        foreach(GameObject a in pawns){
            Destroy(a);
        }
        pawns.Clear();
        foreach(Player_Combatant a in active_player_combatants){
            var cur = a.Initialize_Pawn();
            pawns.Add(cur);
            a.pawn = cur;
        }
        for(int i = 0; i < pawns.Count; i++){
            Vector3 offset = active_player_combatants[i].offset + party_location;
            offset.x = (i*2.15f) - pawns.Count;
            pawns[i].transform.position = offset;
            UIController.bannerController.GetPartyMemberByIdx(i).Fill_Combatant_Info(active_player_combatants[i]);
        }
    }

    private void Set_Reserve_Pawns(){
        foreach(GameObject a in reserve_pawns){
            Destroy(a);
        }
        reserve_pawns.Clear();
        foreach(Player_Combatant a in reserve_player_combatants){
            var cur = Instantiate(a.pawn_prefab);
            reserve_pawns.Add(cur);
            a.pawn = cur;
        }
        for(int i = 0; i < reserve_pawns.Count; i++){
            Vector3 offset = reserve_player_combatants[i].offset + party_location;
            offset.x = -((i*2.15f) - 3.5f);
            offset.z -= 3;
            reserve_pawns[i].transform.position = offset;
        }
    }

    public bool Is_Party_Dead(){
        foreach(Combatant c in active_player_combatants){
            if(c.is_dead == false)
                return false;
        }
        return true;
    }

    public bool Is_Enemies_Dead(){
        foreach(Combatant c in enemy_combatants){
            if(c.is_dead == false)
                return false;
        }
        return true;
    }

    private void End_Battle_Lose(){
        is_executing_actions = false;
        battle_menu.Reset_Menu_Cameras(); //set cameras to 0
        party_cam.Priority = 99;
        lost = true; //accept input in update based on lost

    }
    private void End_Battle_Win(){
        is_executing_actions = false;
        battle_menu.Reset_Menu_Cameras(); //set cameras to 0
        party_cam.Priority = 99;
        won = true; //accept input in update based on won
        set_won_flags = true; 
        //calculate drops
        foreach(Enemy_Combatant e in enemy_combatants){
            battle_end_exp += e.Get_Exp();
            battle_end_items.AddRange(e.Calculate_Drop());
        }
        //calculate exp
        battle_end_leveled = Dispense_EXP(battle_end_exp);
    }

    //return a list of characters that leveled up
    public List<Player_Combatant> Dispense_EXP(int exp){
        List<Player_Combatant> leveled = new List<Player_Combatant>();
        foreach(Player_Combatant p in Get_All_Player_Combatants()){
            int cur_level = p.GetLevel();
            int new_level = p.AddEXP(exp);
            if(new_level == cur_level){
                leveled.Add(p);
            }   
        }
        return leveled;
    }

    public void End_Dialogue(){
        if(!dialogue_started){
            win_dialogue_controller.StartDialogue();
            dialogue_started = true;
        }
        if(dialogue_started && !win_dialogue_controller.isRunning && !item_dialogue_controller.isRunning){
            if(end_item_idx < battle_end_items.Count){
                    item_blackboard.SetVariableValue("item",battle_end_items[end_item_idx].action_name);
                    item_dialogue_controller.StartDialogue();
                    end_item_idx++;
            }
        }
        if(end_item_idx >= battle_end_items.Count && !item_dialogue_controller.isRunning){
            done_listing_items = true;
        }

        if(dialogue_started && !win_dialogue_controller.isRunning && done_listing_items && !level_dialogue_controller.isRunning){
            if(end_level_idx < battle_end_leveled.Count){
                level_blackboard.SetVariableValue("level",battle_end_leveled[end_level_idx].GetLevel());
                level_blackboard.SetVariableValue("character",battle_end_leveled[end_level_idx].GetEXP());
                level_dialogue_controller.StartDialogue();
                end_level_idx++;
            }
        }
        if(dialogue_started && done_listing_items 
        && !level_dialogue_controller.isRunning
        && !item_dialogue_controller.isRunning
        && !win_dialogue_controller.isRunning){
           StartCoroutine(ReturnToWorld());
        }
    }

    float transition_time = 1f;
    public Animator transition;
    IEnumerator ReturnToWorld(){   
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transition_time); //why don't I do this more lol
        SceneTransferHelper.GoToWorldSceneFromBattleScene();
    }

    public void ShowAttackText(string text){
        UIController.Raise_Text(text);
    }
    
    public void HideAttackText(){
        UIController.Hide_Text();
    }
}
