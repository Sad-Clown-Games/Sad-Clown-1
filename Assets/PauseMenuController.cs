using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    We could have a class relationship here, but the actions that each menus takes will likely be different to each one, so it's not that
    big of a code efficiency here.


*/
public class PauseMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Main_Pause_Controller main_menu;
    [SerializeField]
    private Item_Pause_Controller item_menu;
    [SerializeField]
    private Fish_Pause_Controller fish_menu;
    [SerializeField]
    private Stat_Pause_Controller stat_menu;
    [SerializeField]
    private Party_Pause_Controller party_menu;
    [SerializeField]
    private Option_Pause_Controller option_menu;
    [SerializeField]
    public enum Pause_State
    {
        unpaused,
        main,
        item,
        fish,
        stat,
        party,
        option,
    }

    [SerializeField]
    public Pause_State pause_state;
    public bool is_sub_menu = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputs input = Game_Manager.Instance.Get_Inputs();
        if(input.pause || input.cancel && !is_sub_menu && pause_state != Pause_State.unpaused){
            switch (pause_state)
            {
                case Pause_State.unpaused:
                    Enter_Pause_State();
                    break;
                case Pause_State.main:
                    Exit_Pause_State();
                    break;
                case Pause_State.item:
                    Item_To_Main();
                    break;
                case Pause_State.fish:
                    Fish_To_Main();
                    break;
                case Pause_State.stat:
                    Stat_To_Main();
                    break;
                case Pause_State.party:
                    Party_To_Main();
                    break;
                case Pause_State.option:
                    Option_To_Main();
                    break;
                default:
                    break;
            }
        }
        else{ //ignore same frame simultanious button presses
            switch (pause_state)
            {
                case Pause_State.unpaused:
                    break;
                case Pause_State.main:
                    main_menu.Control(input);
                    break;
                case Pause_State.item:
                    item_menu.Control(input);
                    break;
                case Pause_State.fish:
                    fish_menu.Control(input);
                    break;
                case Pause_State.stat:
                    stat_menu.Control(input);
                    break;
                case Pause_State.party:
                    party_menu.Control(input);
                    break;
                case Pause_State.option:
                    option_menu.Control(input);
                    break;
                default:
                    break;
            }
        }
    }

    void Enter_Pause_State(){
        main_menu.Open();
        Game_Manager.Instance.Disable_Player_Control();
        Game_Manager.Instance.Global_Disable_AI();
        //Game_Manager.Instance.Freeze_Time();
        pause_state = Pause_State.main;
    }
    void Exit_Pause_State(){
        main_menu.Close();
        Game_Manager.Instance.Enable_Player_Control();
        Game_Manager.Instance.Global_Enable_AI();
        //Game_Manager.Instance.Resume_Time();
        pause_state = Pause_State.unpaused;
    }

    public void Select_Sub_Menu(Pause_State state){
        switch (state)
        {
            case Pause_State.item:
                To_Item();
                break;
            case Pause_State.fish:
                To_Fish();
                break;
            case Pause_State.stat:
                To_Stat();
                break;
            case Pause_State.party:
                To_Party();
                break;
            case Pause_State.option:
                To_Option();
                break;
            default:
                Exit_Pause_State();
                break;
        }
    }

    void To_Item(){
        main_menu.Grey_Out();
        pause_state = Pause_State.item;
        item_menu.Open();
    }
    void To_Fish(){
        main_menu.Grey_Out();
        pause_state = Pause_State.fish;
        fish_menu.Open();
    }
    void To_Stat(){
        main_menu.Grey_Out();
        pause_state = Pause_State.stat;
        stat_menu.Open();
    }
    void To_Party(){
        main_menu.Grey_Out();
        pause_state = Pause_State.party;
        party_menu.Open();
    }
    void To_Option(){
        main_menu.Grey_Out();
        pause_state = Pause_State.option;
        option_menu.Open();
    }

    void Item_To_Main(){
        item_menu.Close();
        pause_state = Pause_State.main;
        main_menu.Open();
    }
    void Fish_To_Main(){
        fish_menu.Close();
        pause_state = Pause_State.main;
        main_menu.Open();
    }
    void Stat_To_Main(){
        stat_menu.Close();
        pause_state = Pause_State.main;
        main_menu.Open();
    }
    void Party_To_Main(){
        party_menu.Close();
        pause_state = Pause_State.main;
        main_menu.Open();
    }
    void Option_To_Main(){
        option_menu.Close();
        pause_state = Pause_State.main;
        main_menu.Open();
    }
}
