using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
Battle Menu
User chooses options, each option is tagged with it's respective action
User should be able to go back to previous character
If there are remaining characters that havn't acted then we go to the next one in the user's
preferred turn order.
*/
public class Battle_Menu : MonoBehaviour
{
    public ActionOptionController action_options;
    public Skill_Option_Controller skill_options;
    //public ItemOptionController cur_options;
    public Target_Select_Controller target_options;
    public SpecialTargetActionController special_action_options;
    public Item_Option_Controller item_options;
    public Party_Switch_Controller swap_options;
    public int cur_option_idx;
    public int cur_action_idx;
    public int last_target_idx;
    public bool is_action_select_menu;
    public bool is_skill_select_menu;
    public bool is_target_select_menu;
    public bool is_specialaction_select_menu;
    public bool is_item_select_menu;
    public bool is_selecting_target;
    public bool is_switching_party;
    public GameObject root_menu;
    public GameObject action_select_menu;
    public GameObject skill_select_menu;
    public GameObject target_select_menu;
    public GameObject item_select_menu;
    public GameObject cur_selection_menu;
    public GameObject swap_select_menu;
    public GameObject specialaction_select_menu;

    public Battle_Controller battle_controller;

    public Cinemachine.CinemachineVirtualCamera action_select_camera;
    public Cinemachine.CinemachineVirtualCamera skill_select_camera;
    public Cinemachine.CinemachineVirtualCamera item_select_camera;
    public Cinemachine.CinemachineVirtualCamera target_select_camera;
    public Cinemachine.CinemachineVirtualCamera swap_select_camera;

    public Player_Combatant cur_combatant;
    public List<Combatant> current_targets = new List<Combatant>();

    private bool holding_horiz;
    private bool holding_vert;
    private string return_menu; //keep track of the menu we're returning to when we target
    public bool menus_active;


    // Start is called before the first frame update
    void Start()
    {
        target_options.Hide_Arrow();
        swap_options.Hide_Arrow();
        //target_options.Hide_SpecialSelect();
    }

    // Update is called once per frame
    void Update()
    {
        //if we do it during update then it'll be easier to position for chars
        if(cur_combatant)
            transform.position = cur_combatant.Get_Menu_Position();
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        bool input_select = Input.GetKeyDown("x");
        bool input_cancel = Input.GetKeyDown("c");
        if(menus_active){
            //easy af to add an assload of menus if you pleases
            if(is_action_select_menu){
                if(CinemachineCore.Instance.IsLive(action_select_camera))
                    Action_Select_Menu(horiz,vert,input_select,input_cancel);
            }
            else if(is_item_select_menu){
                if(CinemachineCore.Instance.IsLive(item_select_camera))
                    Item_Select_Menu(horiz,vert,input_select,input_cancel);
            }
            else if(is_skill_select_menu){
                if(CinemachineCore.Instance.IsLive(skill_select_camera))
                    Skill_Select_Menu(horiz,vert,input_select,input_cancel);
            }
            else if(is_specialaction_select_menu){
                SpecialAction_Select_Menu(horiz,vert,input_select,input_cancel);
            }
            else if(is_target_select_menu){
                if(CinemachineCore.Instance.IsLive(target_select_camera))
                    Target_Select_Menu(horiz,vert,input_select,input_cancel);
            }
            else if(is_switching_party){
                if(CinemachineCore.Instance.IsLive(swap_select_camera))
                    Swap_Select_Menu(horiz,vert,input_select,input_cancel);
            }
        }

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }

    //greys out options based on the inputted actor
    public void Grey_Out_Options(Player_Combatant actor){
        action_options.options[2].UnGray();
        action_options.options[3].UnGray();
        action_options.options[4].UnGray();
        if(actor.attacks.Count < 1)
            action_options.options[2].Gray_Out();
        if(Game_Manager.Instance.player_data.items.Count < 1)
            action_options.options[3].Gray_Out();
        if(battle_controller.reserve_player_combatants.Count < 1)
            action_options.options[4].Gray_Out();
    }

    public void Action_Select_Menu(float horiz, float vert, bool select, bool cancel){
        int prev_option_index = cur_option_idx;
        Grey_Out_Options(cur_combatant);
        if(vert < 0 && cur_option_idx < action_options.options.Count-1){
            if(!holding_vert){
                cur_option_idx += 1;
                holding_vert = true;
                bool skip_option = false;
                //ok now we make sure that we're not selecting a gray option
                do
                {
                    skip_option = false;
                    switch (action_options.options[cur_option_idx].option_name)
                        {
                            case "Defend":
                                //maybe we disallow it under a status effect
                                break;
                            case "Mental":
                                //no skills
                                if(cur_combatant.attacks.Count < 1){
                                    cur_option_idx += 1;
                                    skip_option = true;
                                }
                                break;
                            case "Items":
                                //no items
                                if(Game_Manager.Instance.player_data.items.Count < 1){
                                    cur_option_idx += 1;
                                    skip_option = true;
                                }
                                break;
                            case "Switch":
                                if(battle_controller.reserve_player_combatants.Count < 1){
                                    cur_option_idx += 1;
                                    skip_option = true;
                                }
                                //gray out if we have nobody to switch to
                                break;
                            case "Run":
                                //maybe we gray out run at some point
                                break;
                            default:
                                break;
                        }
                } while (skip_option);

            }
        }
        if(vert > 0 && cur_option_idx > 0){
            if(!holding_vert){
                cur_option_idx -= 1;
                holding_vert = true;
                bool skip_option;
                do{
                    skip_option = false;
                    switch (action_options.options[cur_option_idx].option_name)
                    {
                        case "Defend":
                            //maybe we disallow it under a status effect
                            break;
                        case "Mental":
                            //no skills
                            if(cur_combatant.attacks.Count < 1){
                                cur_option_idx -= 1;
                                skip_option = true;
                            }
                            break;
                        case "Items":
                            //no items
                            if(Game_Manager.Instance.player_data.items.Count < 1){
                                cur_option_idx -= 1;
                                skip_option = true;
                            }
                            break;
                        case "Switch":
                                if(battle_controller.reserve_player_combatants.Count < 1){
                                cur_option_idx -= 1;
                                skip_option = true;
                            }
                            //gray out if we have nobody to switch to
                            break;
                        case "Run":
                            //maybe we gray out run at some point
                            break;
                        default:
                            break;
                    }
                } while (skip_option);
            }
        }
        action_options.options[cur_option_idx].hovered = true;
        if(cur_option_idx != prev_option_index)
        action_options.options[prev_option_index].hovered = false;
        if(select){
            switch (action_options.options[cur_option_idx].option_name)
            {
                case "Attack":
                    //base attack, straight to targets'
                    Action_To_Target();
                    break;
                case "Defend":
                    //straight to creating a combat action\
                    Action_To_Defend();
                    break;
                case "Mental":
                    Action_To_Skills();
                    break;
                case "Items":
                    Action_To_Items();
                    break;
                case "Switch":
                    //go to switching menus
                    Action_To_Switch();
                    break;
                case "Run":
                    Action_To_Run();
                    break;
                default:
                    Debug.Log("Invalid Name!");
                    break;
            }
            
        }
        if(cancel){
            battle_controller.Undo_Confirm_Action();
        }
    }

    public void Item_Select_Menu(float horiz, float vert, bool select, bool cancel){
        int prev_option_index = cur_option_idx;
            if(!holding_vert){
                if(vert > 0){
                    if(item_options.options[cur_option_idx].up_adjacent 
                    && item_options.options[cur_option_idx].up_adjacent.activeSelf){
                        cur_option_idx -= 2;
                        
                    }
                    else{
                        if(item_options.Goto_Page(item_options.cur_page-1)){
                            if(cur_option_idx%2 == 1)
                                cur_option_idx = 7;
                            else
                                cur_option_idx = 6;
                        }
                    }
                    holding_vert = true;
                }
                if(vert < 0){
                    if(item_options.options[cur_option_idx].down_adjacent
                    && item_options.options[cur_option_idx].down_adjacent.activeSelf){
                        cur_option_idx += 2;
                        
                    }
                    else{
                        if(item_options.Goto_Page(item_options.cur_page+1)){
                            if(cur_option_idx%2 == 0)
                                cur_option_idx = 0;
                            else if(item_options.cur_options < 2)
                                cur_option_idx = 0;
                            else
                                cur_option_idx = 1;
                        }
                    }
                    holding_vert = true;
                }
                
            }
            if(!holding_horiz){
                if(horiz > 0){
                    if(item_options.options[cur_option_idx].right_adjacent
                    && item_options.options[cur_option_idx].right_adjacent.activeSelf){
                        cur_option_idx += 1;
                        holding_horiz = true;
                    }
                    else{
                        holding_horiz = true;
                        if(item_options.Goto_Page(item_options.cur_page+1)){
                            if(item_options.cur_options-1 >= cur_option_idx-1)
                                cur_option_idx -= 1;
                            else if(item_options.cur_options%2.0 == 1)  
                                cur_option_idx = item_options.cur_options-1;
                            else
                                cur_option_idx = item_options.cur_options-2;
                        }
                    }
                }
                if(horiz < 0){
                    if(item_options.options[cur_option_idx].left_adjacent
                    && item_options.options[cur_option_idx].left_adjacent.activeSelf){
                        cur_option_idx -= 1;
                        
                    }
                    else{
                        if(item_options.Goto_Page(item_options.cur_page-1)){
                            cur_option_idx += 1;
                        }
                    }
                    holding_horiz = true;
                }
                
            }
        item_options.options[cur_option_idx].hovered = true;
        if(cur_option_idx != prev_option_index)
            item_options.options[prev_option_index].hovered = false;
        if(cancel){
            Items_To_Action();
        }
        if(select){
            Items_To_Target();
        }
    }

    public void Skill_Select_Menu(float horiz, float vert, bool select, bool cancel){
        int prev_option_index = cur_option_idx;
        if(!holding_vert){
            if(vert > 0){
                if(skill_options.options[cur_option_idx].up_adjacent 
                && skill_options.options[cur_option_idx].up_adjacent.activeSelf){
                    cur_option_idx -= 2;
                }
                else{
                    //if going to previous page 
                    if(skill_options.Goto_Page(skill_options.cur_page-1)){
                        if(cur_option_idx%2 == 1)
                            cur_option_idx = 7;
                        else
                            cur_option_idx = 6;
                    }
                }
                holding_vert = true;
            }
            
            if(vert < 0){
                if(skill_options.options[cur_option_idx].down_adjacent
                && skill_options.options[cur_option_idx].down_adjacent.activeSelf){
                    cur_option_idx += 2;
                }
                else{
                    if(skill_options.Goto_Page(skill_options.cur_page+1)){
                        if(cur_option_idx%2 == 0)
                            cur_option_idx = 0;
                        else if(skill_options.cur_options < 2)
                            cur_option_idx = 0;
                        else
                            cur_option_idx = 1;
                    }
                }
            holding_vert = true;
            }
        }
        if(!holding_horiz){
            if(horiz > 0){
                if(skill_options.options[cur_option_idx].right_adjacent
                && skill_options.options[cur_option_idx].right_adjacent.activeSelf){
                    cur_option_idx += 1;
                    holding_horiz = true;
                }
                else{
                    
                    holding_horiz = true;
                    //turning the page
                    if(skill_options.Goto_Page(skill_options.cur_page+1)){
                        //if there are enough options to get the corresponding option
                        if(skill_options.cur_options-1 >= cur_option_idx-1)
                            cur_option_idx -= 1;
                        else if(skill_options.cur_options%2.0 == 1)  
                            cur_option_idx = skill_options.cur_options-1;
                        else
                            cur_option_idx = skill_options.cur_options-2;
                    }
                }
            }
            if(horiz < 0){
                if(skill_options.options[cur_option_idx].left_adjacent
                && skill_options.options[cur_option_idx].left_adjacent.activeSelf){
                    cur_option_idx -= 1;
                }
                else{
                    
                    if(skill_options.Goto_Page(skill_options.cur_page-1)){
                        cur_option_idx += 1;
                    }
                }
                holding_horiz = true;
            }
            
        }
        skill_options.options[cur_option_idx].hovered = true;
        if(cur_option_idx != prev_option_index)
            skill_options.options[prev_option_index].hovered = false;
        if(cancel){
            Skills_To_Action();
        }
        if(select){
            Skills_To_Target();
        }
    }

    public void Target_Select_Menu(float horiz, float vert, bool select, bool cancel){
         if(!holding_horiz){
                if(horiz > 0 && cur_option_idx < target_options.options.Count-1){
                    cur_option_idx += 1;
                    holding_horiz = true;
                }
                if(horiz < 0 && cur_option_idx > 0){
                    cur_option_idx -= 1;
                    holding_horiz = true;
                }
         }
        target_options.Set_Target(target_options.options[cur_option_idx]);
        if(cancel){
            switch (return_menu)
            {
                case "items":
                    Target_To_Items();
                    break;
                case "skills":
                    Target_To_Skills();
                    break;
                case "action":
                    Target_To_Action();
                    break;
            }
            
        }
        if(select){
            is_target_select_menu = false;
            current_targets.Add(target_options.options[cur_option_idx]);
            if(current_targets.Count <= 1 && current_targets[0].special_action != null){
                Target_To_SpecialAction(current_targets[0].special_action); //pass in action
            }
            else{
                target_select_camera.Priority = 0;
                target_options.Hide_Arrow();
                battle_controller.Confirm_Action(current_targets,cur_action_idx);
            }
        }
    }

    public void SpecialAction_Select_Menu(float horiz, float vert, bool select, bool cancel){
        int prev_option_index = cur_option_idx;
         if(!holding_vert){
                if(vert > 0 && cur_option_idx > 0){
                    cur_option_idx--;
                    holding_vert = true;
                }
                if(vert < 0 && cur_option_idx < special_action_options.Get_Option_Count()-1){
                    cur_option_idx++;
                    holding_vert = true;
                }
         }
        special_action_options.Hover_Option(cur_option_idx);
        if(cur_option_idx != prev_option_index)
            special_action_options.UnHover_Option(prev_option_index);
        if(cancel){
            special_action_options.UnHover_Option(cur_option_idx);
            SpecialAction_To_Target();
        }
        if(select){
            special_action_options.UnHover_Option(cur_option_idx);
            target_options.Hide_Arrow();
            target_options.Hide_SpecialSelect();
            target_select_camera.Priority = 0;
            is_specialaction_select_menu = false;
            //use old targets
            battle_controller.Init_CombatAction(special_action_options.options[cur_option_idx].attack); //set a new action up
            battle_controller.Confirm_Action(current_targets,cur_action_idx);
        }
    }

    public void Swap_Select_Menu(float horiz, float vert, bool select, bool cancel){
        if(!holding_horiz){
            if(horiz > 0 && cur_option_idx < swap_options.options.Count-1){
                int last_idx = cur_option_idx;
                cur_option_idx++;
                while(swap_options.options[cur_option_idx].is_being_switched && cur_option_idx < swap_options.options.Count-1){
                    cur_option_idx++;
                }
                if(swap_options.options[cur_option_idx].is_being_switched){
                    cur_option_idx = last_idx;
                            //go back because there are no valid switching targets
                }
                holding_horiz = true;
            }
            if(horiz < 0 && cur_option_idx > 0){
                int last_idx = cur_option_idx;
                cur_option_idx--;
                while(swap_options.options[cur_option_idx].is_being_switched && cur_option_idx > 0){
                    cur_option_idx--;
                }
                if(swap_options.options[cur_option_idx].is_being_switched){
                    cur_option_idx = last_idx;
                            //go back because there are no valid switching targets
                }
                holding_horiz = true;
            }
         }
        swap_options.Set_Target(swap_options.options[cur_option_idx]);
        if(select){
            swap_options.Hide_Arrow();
            
            //Add 4 to offset the active party members
            battle_controller.Switch_Combatant(cur_option_idx+4);
        }
        if(cancel){
            Switch_To_Action();
        }
    }

    public void Start_Selecting(Player_Combatant combatant){
        //Reset all cameras and views
        action_select_camera.Priority = 99;
        skill_select_camera.Priority = 0;
        target_select_camera.Priority = 0;
        swap_select_camera.Priority = 0;
        is_target_select_menu = false;
        is_item_select_menu = false;
        is_selecting_target = false;
        is_switching_party = false;
        is_action_select_menu = true;
        //skill_select_menu.SetActive(false);
        //set reference
        cur_combatant = combatant;
        current_targets.Clear();
        Grey_Out_Options(cur_combatant);
        //Enable menus
        root_menu.SetActive(true);
        action_select_menu.SetActive(true);
        is_action_select_menu = true;
        //get our options
        cur_selection_menu = action_select_menu;
        cur_option_idx = 0;
        //Move menu to correct combatant
        //move camera to correct combatant location
    }

    public void Action_To_Skills(){
        action_select_camera.Priority = 0;
        skill_select_camera.Priority = 99;
        skill_select_menu.SetActive(true);
        cur_selection_menu = skill_select_menu;
        skill_options.attacks = cur_combatant.attacks;
        skill_options.Set_Options(cur_combatant.attacks);
        is_action_select_menu = false;
        is_skill_select_menu = true;
        cur_option_idx = 0;
    }

    public void Skills_To_Action(){
        foreach (var item in skill_options.options)
        {
            item.hovered = false;
        }
        action_select_camera.Priority = 99;
        skill_select_camera.Priority = 0;
        skill_select_menu.SetActive(false);
        cur_selection_menu = action_select_menu;
        is_action_select_menu = true;
        is_skill_select_menu = false;
        cur_option_idx = 2;
        action_options.options[cur_option_idx].hovered = true;
    }

    public void Action_To_Items(){
        action_select_camera.Priority = 0;
        item_select_camera.Priority = 99;
        item_select_menu.SetActive(true);
        cur_selection_menu = item_select_menu;
        item_options.items = Game_Manager.Instance.player_data.items;//pull from item bag in game_manager;
        item_options.Set_Options();
        is_action_select_menu = false;
        is_item_select_menu = true;
        cur_option_idx = 0;
    }

    public void Items_To_Action(){
        foreach (var item in item_options.options)
        {
            item.hovered = false;
        }
        action_select_camera.Priority = 99;
        item_select_camera.Priority = 0;
        item_select_menu.SetActive(false);
        cur_selection_menu = action_select_menu;
        is_action_select_menu = true;
        is_item_select_menu = false;
        cur_option_idx = 3;
        action_options.options[cur_option_idx].hovered = true;
    }

    public void Skills_To_Target(){
        //We'll have a shader effect to hide them later
        action_select_menu.SetActive(false);
        skill_select_menu.SetActive(false);
        battle_controller.Init_CombatAction(skill_options.options[cur_option_idx].attack);
        battle_controller.cur_selected_action.action = skill_options.options[cur_option_idx].attack;
        foreach (var item in skill_options.options)
        {
            item.hovered = false;
        }
        target_select_camera.Priority = 99;
        skill_select_camera.Priority = 0;
        cur_selection_menu = target_select_menu;
        is_target_select_menu = true;
        is_skill_select_menu = false;
        //set valid targets as everyone except the target;
        target_options.Set_Combatants_For_Attack_Skill(cur_combatant);
        target_options.Set_Target(target_options.options[cur_option_idx]);
        cur_action_idx = cur_option_idx;
        cur_option_idx = 0;
        return_menu = "skills";
    }

    public void Action_To_Target(){
        //We'll have a shader effect to hide them later
        action_select_menu.SetActive(false);
        battle_controller.Init_CombatAction(cur_combatant.default_attack);
        battle_controller.cur_selected_action.action = cur_combatant.default_attack;
        target_select_camera.Priority = 99;
        action_select_camera.Priority = 0;
        cur_selection_menu = target_select_menu;
        is_target_select_menu = true;
        is_action_select_menu = false;
        target_options.Set_Combatants_For_Attack_Skill(cur_combatant);
        target_options.Set_Target(target_options.options[cur_option_idx]);
        cur_action_idx = cur_option_idx;
        cur_option_idx = 0;
        return_menu = "action";
    }
    

    public void Target_To_Action(){
        target_options.Hide_Arrow();
        action_select_menu.SetActive(true);
        target_select_camera.Priority = 0;
        action_select_camera.Priority = 99;
        cur_selection_menu = action_select_menu;
        is_action_select_menu = true;
        is_target_select_menu = false;
        cur_option_idx = 0;
    }

    public void SpecialAction_To_Target(){
        current_targets.Clear(); //clear targets so we can select again.
        target_options.Hide_SpecialSelect();
        target_options.Show_Arrow();
        cur_selection_menu = target_select_menu;
        is_specialaction_select_menu = false;
        is_target_select_menu = true;
        cur_option_idx = last_target_idx;
    }
    public void Target_To_SpecialAction(Action special){
        target_options.Show_SpecialSelect(); //setactive
        List<Action> s_actions = new List<Action>();
        s_actions.Add(special);
        s_actions.Add(battle_controller.cur_selected_action.action);
        special_action_options.actions = s_actions;
        special_action_options.Set_Options(s_actions);
        cur_selection_menu = specialaction_select_menu;
        is_specialaction_select_menu = true;
        is_target_select_menu = false;
        last_target_idx = cur_option_idx;
        cur_option_idx = 0; //we need to make sure the option idx is the same
    }

    public void Action_To_Switch(){
        action_select_menu.SetActive(false);
        swap_select_camera.Priority = 99;
        action_select_camera.Priority = 0;
        cur_selection_menu = swap_select_menu;
        is_switching_party = true;
        is_action_select_menu = false;
        swap_options.Set_Reserve_Characters();
        cur_option_idx = 0;
        while(swap_options.options[cur_option_idx].is_being_switched && cur_option_idx < swap_options.options.Count){
            cur_option_idx++;
        }
        if(swap_options.options[cur_option_idx].is_being_switched){
            Switch_To_Action();
            //go back because there are no valid switching targets
        }
        swap_options.Set_Target(swap_options.options[cur_option_idx]);
    }
    

    public void Switch_To_Action(){
        swap_options.Hide_Arrow();
        action_select_menu.SetActive(true);
        swap_select_camera.Priority = 0;
        action_select_camera.Priority = 99;
        cur_selection_menu = action_select_menu;
        is_action_select_menu = true;
        is_switching_party = false;
        cur_option_idx = 4;
    }


    public void Action_To_Defend(){
        action_options.options[cur_option_idx].hovered = false;
        battle_controller.Init_CombatAction(cur_combatant.default_guard);
        battle_controller.cur_selected_action.action = cur_combatant.default_guard;
        List<Combatant> self_target = new List<Combatant>();
        self_target.Add(cur_combatant);
        battle_controller.Confirm_Action(self_target,0);
        cur_option_idx = 0;
    }

    public void Items_To_Target(){
        //We'll have a shader effect to hide them later
        action_select_menu.SetActive(false);
        item_select_menu.SetActive(false);
        battle_controller.Init_CombatAction(item_options.options[cur_option_idx].item);
        battle_controller.cur_selected_action.action = item_options.options[cur_option_idx].item;
        foreach (var item in item_options.options)
        {
            item.hovered = false;
        }
        target_select_camera.Priority = 99;
        item_select_camera.Priority = 0;
        cur_selection_menu = target_select_menu;
        is_target_select_menu = true;
        is_item_select_menu = false;
        target_options.Set_Combatants_For_Support_Skill();
        target_options.Set_Target(target_options.options[cur_option_idx]);
        cur_option_idx = 0;
        return_menu = "items";
    }

    public void Target_To_Skills(){
        target_options.Hide_Arrow();
        action_select_menu.SetActive(true);
        skill_select_menu.SetActive(true);
        target_select_camera.Priority = 0;
        skill_select_camera.Priority = 99;
        skill_select_menu.SetActive(true);
        cur_selection_menu = skill_select_menu;
        skill_options.Set_Options(cur_combatant.attacks);
        is_action_select_menu = false;
        is_skill_select_menu = true;
        cur_option_idx = 0;
    }

    public void Target_To_Items(){
        target_options.Hide_Arrow();
        action_select_menu.SetActive(true);
        item_select_menu.SetActive(true);
        target_select_camera.Priority = 0;
        item_select_camera.Priority = 99;
        item_select_menu.SetActive(true);
        cur_selection_menu = item_select_menu;
        item_options.items = Game_Manager.Instance.player_data.items;
        item_options.Set_Options();
        is_action_select_menu = false;
        is_item_select_menu = true;
        cur_option_idx = 0;
    }

    public void Reset_Menu_Cameras(){
        //set all cameras to priority 0
        target_select_camera.Priority = 0;
        item_select_camera.Priority = 0;
        skill_select_camera.Priority = 0;
        swap_select_camera.Priority = 0;
        action_select_camera.Priority = 0;
    }

    public void Action_To_Run(){
        //Once scene switching is in we'll do a run sequence.
        return;
    }
  

    public void HideMenus(){
        root_menu.SetActive(false);
    }
    public void ShowMenus(){
        root_menu.SetActive(true);
    }

}
