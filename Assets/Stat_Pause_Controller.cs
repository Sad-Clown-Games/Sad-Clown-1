using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stat_Pause_Controller : MonoBehaviour
{

    public List<Menu_Option> party_options;
    public List<Player_Character> party;
    private Player_Character current_member;
    [SerializeField]
    private UnityEngine.UI.Image full_body;
    [SerializeField]
    private TextMeshProUGUI text_name;
    [SerializeField]
    private TextMeshProUGUI text_level;
    [SerializeField]
    private TextMeshProUGUI text_xp;
    [SerializeField]
    private TextMeshProUGUI text_nextxp;
    [SerializeField]
    private TextMeshProUGUI text_hp;
    [SerializeField]
    private TextMeshProUGUI text_mp;
    [SerializeField]
    private TextMeshProUGUI text_atk;
    [SerializeField]
    private TextMeshProUGUI text_def;
    [SerializeField]
    private TextMeshProUGUI text_luc;
    [SerializeField]
    private TextMeshProUGUI text_spd;
    [SerializeField]
    private TextMeshProUGUI text_description;
    [SerializeField]
    private GameObject view;
    private int party_option_idx;
    private bool holding_horiz;
    private bool holding_vert;
    [SerializeField]
    PauseMenuController controller;
    public void Control(inputs input){
        Linear_Control(input,ref party_option_idx,ref party_options,false,true);
        current_member = party_options[party_option_idx].character;
        Set_Character();
    }

    private void Set_Character(){
        text_name.text = current_member.character_name;
        text_description.text = current_member.long_description;
        full_body.sprite = current_member.full_body_sprite;
        text_level.text = string.Format("Level {0}",current_member.Get_Stats().level);
        text_xp.text = string.Format("XP:{0}",current_member.Get_Stats().exp);
        text_hp.text = string.Format("Health: {0}/{1}", current_member.Get_Stats().cur_hp, current_member.Get_Stats().max_hp);
        text_mp.text = string.Format("Mental: {0}/{1}", current_member.Get_Stats().cur_mp, current_member.Get_Stats().max_mp);
        text_atk.text = string.Format("Attack: {0}", current_member.Get_Stats().atk);
        text_def.text = string.Format("Defence: {0}", current_member.Get_Stats().def);
        text_luc.text = string.Format("Luck: {0}", current_member.Get_Stats().luc);
        text_spd.text = string.Format("Speed: {0}", current_member.Get_Stats().spd);

    }
    public void Set_Party(){
        for(int i = 0; i < party_options.Count; i++){
            if(i > party.Count-1)
                break;
            party_options[i].Activate(party[i]);
        }
    }

    public void Linear_Control(inputs input, ref int idx, ref List<Menu_Option> options, bool isvert = true,bool reverse = false){
        int prev_option_idx = idx;
        float cursor = isvert ? input.YAxis : input.XAxis;
        if(reverse){
            cursor *= -1;
        }
        bool holding_cursor = isvert ? holding_vert : holding_horiz;
        if(cursor < 0 && idx < options.Count-1){
            if(!holding_cursor){
                idx += 1;
                holding_vert = true;
                holding_horiz = true;
            }
        }
        if(cursor > 0 && idx > 0){
            if(!holding_cursor){
                idx -= 1;
                holding_vert = true;
                holding_horiz = true;
            }
        }
        options[idx].hovered = true;
        options[idx].Show_Selector();
        if(idx != prev_option_idx){
            options[prev_option_idx].Hide_All();
        }
            
        if(cursor == 0){
            holding_vert = false;
            holding_horiz = false;
        }
    }

    public void Open(){
        party = Game_Manager.Instance.party_controller.Get_Active_Party_By_Order();
        current_member = party[0];
        party_option_idx = 0;
        Set_Party();
        view.SetActive(true);
    }

    public void Close(){
        Clear_Highlights();
        party = null;
        party_option_idx = 0;
        view.SetActive(false);
        controller.is_sub_menu = false;
    }

    private void Clear_Highlights(){
        for(int i = 0; i < party_options.Count;i++){
            party_options[i].hovered = false;
            party_options[i].Hide_Selector();
        }
    }
}
