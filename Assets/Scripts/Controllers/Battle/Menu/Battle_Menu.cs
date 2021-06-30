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
    public Item_Option_Controller item_options;
    public int cur_option_idx;
    public bool is_action_select_menu;
    public bool is_skill_select_menu;
    public bool is_target_select_menu;
    public bool is_item_select_menu;
    public bool is_selecting_target;

    public GameObject root_menu;
    public GameObject action_select_menu;
    public GameObject skill_select_menu;
    public GameObject target_select_menu;
    public GameObject item_select_menu;
    public GameObject cur_selection_menu;

    public Battle_Controller battle_controller;

    public Cinemachine.CinemachineVirtualCamera action_select_camera;
    public Cinemachine.CinemachineVirtualCamera skill_select_camera;
    public Cinemachine.CinemachineVirtualCamera item_select_camera;
    public Cinemachine.CinemachineVirtualCamera target_select_camera;

    public Player_Combatant cur_combatant;

    private bool holding_horiz;
    private bool holding_vert;
    private string return_menu; //keep track of the menu we're returning to when we target


    // Start is called before the first frame update
    void Start()
    {
        target_options.Hide_Arrow();
    }

    // Update is called once per frame
    void Update()
    {
        //if we do it during update then it'll be easier to position for chars
        transform.position = cur_combatant.Get_Menu_Position();
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        bool input_select = Input.GetKeyDown("x");
        bool input_cancel = Input.GetKeyDown("c");
        if(is_action_select_menu){
            Action_Select_Menu(horiz,vert,input_select,input_cancel);
        }
        else if(is_item_select_menu){
            Item_Select_Menu(horiz,vert,input_select,input_cancel);
        }
        else if(is_skill_select_menu){
            Skill_Select_Menu(horiz,vert,input_select,input_cancel);
        }
        else if(is_target_select_menu){
            Target_Select_Menu(horiz,vert,input_select,input_cancel);
        }

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }

    public void Action_Select_Menu(float horiz, float vert, bool select, bool cancel){
        int prev_option_index = cur_option_idx;
        if(vert < 0 && cur_option_idx < action_options.options.Count-1){
                if(!holding_vert){
                    cur_option_idx += 1;
                    holding_vert = true;
                }
            }
        if(vert > 0 && cur_option_idx > 0){
            if(!holding_vert){
                cur_option_idx -= 1;
                holding_vert = true;
            }
        }
        action_options.options[cur_option_idx].hovered = true;
        if(cur_option_idx != prev_option_index)
        action_options.options[prev_option_index].hovered = false;
        if(select){
            switch (action_options.options[cur_option_idx].option_name)
            {
                case "Attack":
                    //base attack, straight to targets
                    break;
                case "Defend":
                    //straight to creating a combat action
                    break;
                case "Mental":
                    Action_To_Skills();
                    break;
                case "Items":
                    Action_To_Items();
                    break;
                case "Switch":
                    //go to switching menus
                    break;
                case "Run":
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
            }
            
        }
        if(select){
            target_options.Hide_Arrow();
            List<Combatant> targets = new List<Combatant>();
            targets.Add(target_options.options[cur_option_idx]);
            battle_controller.Confirm_Action(targets);
        }
    }

    public void Start_Selecting(Player_Combatant combatant){
        //Reset all cameras and views
        action_select_camera.Priority = 99;
        skill_select_camera.Priority = 0;
        target_select_camera.Priority = 0;
        //is_target_select_menu = false;
        //is_item_select_menu = false;
        //is_selecting_target = false;
        //is_action_select_menu = true;
        //skill_select_menu.SetActive(false);
        //set reference
        cur_combatant = combatant;
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
        target_options.Set_Combatants_For_Attack_Skill();
        target_options.Set_Target(target_options.options[cur_option_idx]);
        cur_option_idx = 0;
        return_menu = "skills";
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


    public void HideMenus(){
        root_menu.SetActive(false);
    }
    public void ShowMenus(){
        root_menu.SetActive(true);
    }

}
