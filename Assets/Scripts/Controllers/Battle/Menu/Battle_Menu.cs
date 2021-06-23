using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Battle Menu
User chooses options, each option is tagged with it's respective action
User should be able to go back to previous character
If there are remaining characters that havn't acted then we go to the next one in the user's
preferred turn order.
*/
public class Battle_Menu : MonoBehaviour
{
    public OptionController cur_options;
    public int cur_option_idx;
    public bool is_action_select_menu;
    public bool is_skill_select_menu;
    public bool is_item_select_menu;
    public bool is_selecting_target;

    public GameObject root_menu;
    public GameObject action_select_menu;
    public GameObject skill_select_menu;
    public GameObject item_select_menu;
    public GameObject cur_selection_menu;

    private bool holding_horiz;
    private bool holding_vert;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        if(is_action_select_menu){
            Action_Select_Menu(horiz,vert);
        }
        else if(is_item_select_menu){

        }
        else if(is_skill_select_menu){
            
        }

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }

    public void Action_Select_Menu(float horiz, float vert){
        int prev_option_index = cur_option_idx;
        if(vert < 0 && cur_option_idx < cur_options.options.Count-1){
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
            cur_options.options[cur_option_idx].hovered = true;
            if(cur_option_idx != prev_option_index)
            cur_options.options[prev_option_index].hovered = false;
    }

    public void Start_Selecting(Player_Combatant combatant){
        //Enable menus
        root_menu.SetActive(true);
        action_select_menu.SetActive(true);
        is_action_select_menu = true;
        //get our options
        cur_selection_menu = action_select_menu;
        cur_options = cur_selection_menu.GetComponent<OptionController>();
        cur_option_idx = 0;
        //Move menu to correct combatant
        //move camera to correct combatant location
    }
}
