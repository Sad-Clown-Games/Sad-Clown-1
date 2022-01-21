using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Pause_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject view;
    private bool holding_horiz;
    private bool holding_vert;
    public List<Menu_Option> options;
    [SerializeField]
    PauseMenuController controller;
    public int cur_option_idx;
    public int cur_page_idx;

    public void Control(inputs input){
        int prev_option_idx = cur_option_idx;
        float horiz = input.XAxis;
        float vert = input.YAxis;
        if(vert < 0 && cur_option_idx < options.Count-1){
            if(!holding_vert){
                cur_option_idx += 1;
                holding_vert = true;
                bool skip_option = false;
                //ok now we make sure that we're not selecting a gray option
                do
                {
                    skip_option = false;
                    switch (options[cur_option_idx].option_name)
                        {
                            case "Item":
                                //maybe we disallow it under a status effect
                                break;
                            case "Fish":
                                //no skills
                                break;
                            case "Stats":
                                //no items
                                break;
                            case "Party":
                                //gray out if we have nobody to switch to
                                break;
                            case "Options":
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
                    switch (options[cur_option_idx].option_name)
                    {
                        case "Item":
                            //maybe we disallow it under a status effect
                            break;
                        case "Fish":
                            //no skills
                            break;
                        case "Stats":
                            //no items
                            break;
                        case "Party":
                            //gray out if we have nobody to switch to
                            break;
                        case "Options":
                            //maybe we gray out run at some point
                            break;
                        default:
                            break;
                    }
                } while (skip_option);
            }
        }
        options[cur_option_idx].hovered = true;
        if(cur_option_idx != prev_option_idx)
            options[prev_option_idx].hovered = false;
        if(input.select){
            switch (options[cur_option_idx].option_name)
            {
                case "Item":
                    controller.Select_Sub_Menu(PauseMenuController.Pause_State.item);
                    break;
                case "Fish":
                    controller.Select_Sub_Menu(PauseMenuController.Pause_State.fish);
                    break;
                case "Stat":
                    controller.Select_Sub_Menu(PauseMenuController.Pause_State.stat);
                    break;
                case "Party":
                    controller.Select_Sub_Menu(PauseMenuController.Pause_State.party);
                    break;
                case "Option":
                    controller.Select_Sub_Menu(PauseMenuController.Pause_State.option);
                    break;
                default:
                    break;
            }
            
        }
        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
        
    }

    public void Open(){
        options[cur_option_idx].hovered = false;
        cur_option_idx = 0;
        cur_page_idx = 0;
        view.SetActive(true);
        
    }

    public void Grey_Out(){
        options[cur_option_idx].hovered = false;
        view.SetActive(true);
    }

    public void Close(){
        options[cur_option_idx].hovered = false;
        view.SetActive(false);
    }
}
