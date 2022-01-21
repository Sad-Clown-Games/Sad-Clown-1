using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Pause_Controller : MonoBehaviour
{

    public List<Item> items;
    public List<Item> all_equipment;
    public List<Menu_Option> item_options;
    public List<Menu_Option> party_options;
    public List<Menu_Option> select_options;
    public List<Menu_Option> equip_options;
    public List<Menu_Option> equip_select_options;
    public List<Menu_Option> ability_options;
    //party -> select -> (equip, ability)
    public List<Player_Character> party;
    private Player_Character current_member;
    [SerializeField]
    private UnityEngine.UI.Image item_icon;
    [SerializeField]
    private TMPro.TMP_Text item_description;
    [SerializeField]
    private UnityEngine.UI.Image icon;
    [SerializeField]
    private TMPro.TMP_Text description;
    [SerializeField]
    private TMPro.TMP_Text member_name;
    [SerializeField]
    private TMPro.TMP_Text page_num;
    [SerializeField]
    private TMPro.TMP_Text item_page_num;
    [SerializeField]
    private TMPro.TMP_Text weapon_text;
    [SerializeField]
    private TMPro.TMP_Text armor_text;
    [SerializeField]
    private TMPro.TMP_Text trinket_text;
    [SerializeField]
    private TMPro.TMP_Text deep_description_text;
    [SerializeField]
    private GameObject deep_description_view;
    [SerializeField]
    private GameObject deep_description_icon_view;
    [SerializeField]
    private GameObject page_num_view;
    private UnityEngine.UI.Image deep_description_sprite;
    private int party_option_idx;
    private int select_option_idx;
    private int equip_option_idx;
    private int equip_select_idx;
    private int ability_option_idx;
    private int ability_page_idx;
    private int ability_option_num = 0; //total number of party members
    public int item_option_idx;
    public int item_option_num; //number of options total
    public int item_page_idx;
    [SerializeField]
    private GameObject view;
    [SerializeField]
    private GameObject item_view;
    private bool holding_horiz;
    private bool holding_vert;
    private int colsize = 4;
    private int item_colsize = 8;
    [SerializeField]
    PauseMenuController controller;
    private enum PartyMenu_Sub_State
    {
        party,
        select,
        equip,
        ability,
        equipselect,
        equiplist
    }
    private enum PartyMenu_Item_State
    {
        weapon,
        armor,
        trinket,
    }

    [SerializeField]
    private PartyMenu_Sub_State state;
    [SerializeField]
    private PartyMenu_Item_State item_state;

    public void Control(inputs input){
        switch (state)
            {
                case PartyMenu_Sub_State.party:
                    Linear_Control(input,ref party_option_idx,ref party_options,false,true);
                    current_member = party_options[party_option_idx].character;
                    Set_Abilities(); //changes based on movements
                    if(input.select){
                        Party_To_Select();
                    }
                    break;
                case PartyMenu_Sub_State.select:
                    Linear_Control(input,ref select_option_idx,ref select_options);
                    if(input.select){
                        switch (select_options[select_option_idx].option_name)
                        {
                            case "Equip":
                                Select_To_Equip();
                                break;
                            case "Ability":
                                if(ability_options.Count > 0)
                                    Select_To_Ability();
                                break;
                        }
                    }
                    if(input.cancel){
                        Select_To_Party();
                    }
                    break;
                case PartyMenu_Sub_State.equip:
                    Linear_Control(input,ref equip_option_idx,ref equip_options);
                    if(input.select){
                        switch (equip_options[equip_option_idx].option_name)
                        {
                            case "Weapon":
                                items = all_equipment.Get_By_Type(Item.Item_Type.weapon);
                                item_state = PartyMenu_Item_State.weapon;
                                break;
                            case "Armor":
                                items = all_equipment.Get_By_Type(Item.Item_Type.armor);
                                item_state = PartyMenu_Item_State.armor;
                                break;
                            case "Trinket":
                                items = all_equipment.Get_By_Type(Item.Item_Type.trinket);
                                item_state = PartyMenu_Item_State.trinket;
                                break;
                        }
                        Equip_To_EquipList();
                    }
                    if(input.cancel){
                        Equip_To_Select();
                    }
                    break;
                case PartyMenu_Sub_State.equipselect:
                    Linear_Control(input,ref equip_select_idx, ref equip_select_options);
                    if(input.select){
                        switch(equip_select_options[equip_select_idx].option_name){
                            case "Equip":
                                //EquipSelect_To_EquipList();
                                break;
                            case "Unequip":
                                switch (item_state)
                                {
                                case PartyMenu_Item_State.weapon:
                                    Game_Manager.Instance.Unequip_Character(current_member,"Weapon");
                                    break;
                                case PartyMenu_Item_State.armor:
                                    Game_Manager.Instance.Unequip_Character(current_member,"Armor");
                                    break;
                                case PartyMenu_Item_State.trinket:
                                    Game_Manager.Instance.Unequip_Character(current_member,"Trinket");
                                    break;
                                }
                                break;
                        }
                    }
                    break;
                case PartyMenu_Sub_State.ability:
                    Paginated_Control(input);
                    if(input.cancel){
                        Ability_To_Select();
                    }
                    break;
                case PartyMenu_Sub_State.equiplist:
                    if(items.Count > 0){
                        Item_Control(input);
                        if(input.select){
                            switch (item_state)
                            {
                            case PartyMenu_Item_State.weapon:
                                Game_Manager.Instance.Equip_Character(current_member,item_options[item_option_idx].item,"Weapon");
                                break;
                            case PartyMenu_Item_State.armor:
                                Game_Manager.Instance.Equip_Character(current_member,item_options[item_option_idx].item,"Armor");
                                break;
                            case PartyMenu_Item_State.trinket:
                                Game_Manager.Instance.Equip_Character(current_member,item_options[item_option_idx].item,"Trinket");
                                break;
                            
                            }
                            Refresh_Item_List();
                            EquipList_To_Equip();
                        }
                    }
                    if(input.cancel){
                        EquipList_To_Equip();
                    }
                    break;
            }
        Update_View();
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

    private void Paginated_Control(inputs input){
        int prev_option_index = ability_option_idx;
        float vert = input.YAxis;
        float horiz = input.XAxis;
        if(!holding_vert){
            if(vert > 0){
                if(ability_options[ability_option_idx].up_adjacent 
                && ability_options[ability_option_idx].up_adjacent.activeSelf){
                    ability_option_idx -= 2;
                    
                }
                else{
                    if(Goto_Page(ability_page_idx-1)){
                        if(ability_option_idx%2 == 1)
                            ability_option_idx = colsize*2-1;
                        else
                            ability_option_idx = colsize*2-2;
                    }
                }
                holding_vert = true;
            }
            if(vert < 0){
                if(ability_options[ability_option_idx].down_adjacent
                && ability_options[ability_option_idx].down_adjacent.activeSelf){
                    ability_option_idx += 2;
                    
                }
                else{
                    if(Goto_Page(ability_page_idx+1)){
                        if(ability_option_idx%2 == 0)
                            ability_option_idx = 0;
                        else if(ability_option_num < 2)
                            ability_option_idx = 0;
                        else
                            ability_option_idx = 1;
                    }
                }
                holding_vert = true;
            }
            
        }
        if(!holding_horiz){
            if(horiz > 0){
                if(ability_options[ability_option_idx].right_adjacent
                && ability_options[ability_option_idx].right_adjacent.activeSelf){
                    ability_option_idx += 1;
                    holding_horiz = true;
                }
                else{
                    holding_horiz = true;
                    if(Goto_Page(ability_page_idx+1)){
                        if(ability_option_num-1 >= ability_option_idx-1)
                            ability_option_idx -= 1;
                        else if(ability_option_num%2.0 == 1)  
                            ability_option_idx = ability_option_num-1;
                        else
                            ability_option_idx = ability_option_num-2;
                    }
                }
            }
            if(horiz < 0){
                if(ability_options[ability_option_idx].left_adjacent
                && ability_options[ability_option_idx].left_adjacent.activeSelf){
                    ability_option_idx -= 1;
                    
                }
                else{
                    if(Goto_Page(ability_page_idx-1)){
                        ability_option_idx += 1;
                    }
                }
                holding_horiz = true;
            }
            
        }
        ability_options[ability_option_idx].Show_Selector();
        if(ability_option_idx != prev_option_index)
            ability_options[prev_option_index].Hide_Selector();
        Update_Preview(ability_options[ability_option_idx].action);

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }

  private void Item_Control(inputs input){
        int prev_option_index = item_option_idx;
        float vert = input.YAxis;
        float horiz = input.XAxis;
        if(!holding_vert){
            if(vert > 0){
                if(item_options[item_option_idx].up_adjacent 
                && item_options[item_option_idx].up_adjacent.activeSelf){
                    item_option_idx -= 2;
                    
                }
                else{
                    if(Goto_Item_Page(item_page_idx-1)){
                        if(item_option_idx%2 == 1)
                            item_option_idx = item_colsize*2-1;
                        else
                            item_option_idx = item_colsize*2-2;
                    }
                }
                holding_vert = true;
            }
            if(vert < 0){
                if(item_options[item_option_idx].down_adjacent
                && item_options[item_option_idx].down_adjacent.activeSelf){
                    item_option_idx += 2;
                    
                }
                else{
                    if(Goto_Item_Page(item_page_idx+1)){
                        if(item_option_idx%2 == 0)
                            item_option_idx = 0;
                        else if(item_option_num < 2)
                            item_option_idx = 0;
                        else
                            item_option_idx = 1;
                    }
                }
                holding_vert = true;
            }
            
        }
        if(!holding_horiz){
            if(horiz > 0){
                if(item_options[item_option_idx].right_adjacent
                && item_options[item_option_idx].right_adjacent.activeSelf){
                    item_option_idx += 1;
                    holding_horiz = true;
                }
                else{
                    holding_horiz = true;
                    if(Goto_Item_Page(item_page_idx+1)){
                        if(item_option_num-1 >= item_option_idx-1)
                            item_option_idx -= 1;
                        else if(item_option_num%2.0 == 1)  
                            item_option_idx = item_option_num-1;
                        else
                            item_option_idx = item_option_num-2;
                    }
                }
            }
            if(horiz < 0){
                if(item_options[item_option_idx].left_adjacent
                && item_options[item_option_idx].left_adjacent.activeSelf){
                    item_option_idx -= 1;
                    
                }
                else{
                    if(Goto_Item_Page(item_page_idx-1)){
                        item_option_idx += 1;
                    }
                }
                holding_horiz = true;
            }
            
        }
        item_options[item_option_idx].Show_Selector();
        if(item_option_idx != prev_option_index)
            item_options[prev_option_index].Hide_Selector();
        Update_Item_Preview(item_options[item_option_idx].item);

        if(input.select){
            //TakeorSwapItem();
        }

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }



    public void Update_View(){
        armor_text.text = current_member.battle_object?.armor?.action_name ?? "None";
        weapon_text.text = current_member.battle_object?.weapon?.action_name ?? "None";
        trinket_text.text = current_member.battle_object?.trinket?.action_name ?? "None";
        icon.sprite = current_member.battle_object.portrait;
        member_name.text = current_member.battle_object.combatant_name;
        description.text = current_member.battle_object.combatant_title;
    }

    private void Clear_Abilites(){
        foreach (var option in ability_options)
        {
            option.Deactivate();
        }
        ability_option_num = 0;
    }

    public void Set_Abilities(){
        Clear_Abilites();
        ability_page_idx = 0;
        List<Attack> attacks = current_member.Get_All_Attacks();
        for (int i = 0; i < ability_options.Count; i++)
        {
            if(i > attacks.Count-1)
                break;
            ability_options[i].Activate(attacks[i],i,i,colsize,ability_options);
            ability_options[i].action = attacks[i];
            ability_option_num++;
        }
    }
    public void Set_Party(){
        for(int i = 0; i < party_options.Count; i++){
            if(i > party.Count-1)
                break;
            party_options[i].Activate(party[i]);
        }
    }

    //party -> select -> (equip, ability)
    private void Party_To_Select(){
        state = PartyMenu_Sub_State.select;
        controller.is_sub_menu = true;
    }
    private void Select_To_Equip(){
        select_options[select_option_idx].Hide_All();
        deep_description_view.SetActive(true);
        deep_description_icon_view.SetActive(true);
        state = PartyMenu_Sub_State.equip;
    }
    private void Select_To_Ability(){
        ability_page_idx = 0;
        select_options[select_option_idx].Hide_All();
        deep_description_view.SetActive(true);
        page_num_view.SetActive(true);
        state = PartyMenu_Sub_State.ability;
    }
    private void Select_To_Party(){
        select_options[select_option_idx].Hide_All();
        select_option_idx = 0;
        state = PartyMenu_Sub_State.party;
        controller.is_sub_menu = false;
    }
    private void Equip_To_Select(){
        equip_options[equip_option_idx].Hide_All();
        deep_description_view.SetActive(false);
        deep_description_icon_view.SetActive(false);
        equip_option_idx = 0;
        state = PartyMenu_Sub_State.select;
    }
    private void Ability_To_Select(){
        ability_options[ability_option_idx].Hide_All();
        ability_page_idx = 0;
        ability_option_idx = 0;
        deep_description_view.SetActive(false);
        page_num_view.SetActive(false);
        state = PartyMenu_Sub_State.select; 
    }
    private void Equip_To_EquipList(){
        view.SetActive(false);
        item_view.SetActive(true);
        item_option_idx = 0;
        item_page_idx = 0;
        Set_Item_Options();
        state = PartyMenu_Sub_State.equiplist; 
        //list all equipment and swap/add it to the character
    }
    private void EquipList_To_Equip(){
        item_option_idx = 0;
        item_page_idx = 0;
        item_view.SetActive(false);
        view.SetActive(true);
        state = PartyMenu_Sub_State.equip; 
    }

    public bool Goto_Page(int page){
        int first_index = page * ability_options.Count;
        List<Attack> attacks = current_member.Get_All_Attacks();
        if(ability_options.Count >= attacks.Count || first_index > attacks.Count || page < 0){
            return false;
        }
        ability_page_idx = page;
        Clear_Abilites();
        for(int i = first_index; i-first_index < ability_options.Count; i++){
            if(i > attacks.Count-1)
                break;
            ability_options[i-first_index].Activate(attacks[i]);
            ability_options[i-first_index].action = attacks[i];
            ability_option_num++;
        }
        return true;
    }
    public bool Goto_Item_Page(int page){
        int first_index = page * item_options.Count;
        if(item_options.Count >= items.Count || first_index > items.Count || page < 0){
            return false;
        }
        item_page_idx = page;
        Clear_Item_Options();
        for(int i = first_index; i-first_index < item_options.Count; i++){
            if(i > items.Count-1)
                break;
            item_options[i-first_index].Activate(items[i],i,i-first_index,item_colsize,item_options);
            item_options[i-first_index].item = items[i];
            item_option_num++;
        }
        return true;
    }

    public void Update_Preview(Action ability){
        deep_description_text.text = ability.description;
        page_num.text = (ability_page_idx+1).ToString();
    }
    public void Update_Item_Preview(Item item){
        item_icon.sprite = item.icon;
        item_description.text = item.description;
        page_num.text = (item_page_idx+1).ToString();
    }

    public void Update_Preview(Item item){
        deep_description_sprite.sprite = item.icon;
        deep_description_text.text = item.description;
    }
    public void Refresh_Item_List(){
        all_equipment = Game_Manager.Instance.Get_Items();
        Set_Item_Options();
    }
    public void Set_Item_Options(){
        Clear_Item_Options();
        item_page_idx = 0;
        for (int i = 0; i < item_options.Count; i++)
        {
            if(i > items.Count-1)
                break;
            item_options[i].Activate(items[i],i,i,item_colsize,item_options);
            item_options[i].item = items[i];
            item_options[i].item.active_idx = i; //save index for when we need to remove item;
            item_option_num++;
        }
    }
    private void Clear_Item_Options(){
        foreach (var option in item_options)
        {
            option.Deactivate();
        }
        item_option_num = 0;
    }

    public void Open(){
        party = Game_Manager.Instance.party_controller.Get_Active_Party_By_Order();
        all_equipment = Game_Manager.Instance.Get_Items().Get_By_Type(Item.Item_Type.equipment);
        current_member = party[0];
        party_option_idx = 0;
        ability_page_idx = 0;
        state = PartyMenu_Sub_State.party;
        Set_Party();
        Set_Abilities();
        view.SetActive(true);
    }

    public void Close(){
        Clear_Highlights();
        party = null;
        party_option_idx = 0;
        ability_page_idx = 0;
        view.SetActive(false);
        controller.is_sub_menu = false;
    }
    private void Clear_Highlights(){
        for(int i = 0; i < party_options.Count;i++){
            party_options[i].hovered = false;
            party_options[i].Hide_Selector();
        }
        for(int i = 0; i < item_options.Count;i++){
            item_options[i].hovered = false;
            item_options[i].Hide_Selector();
        }
        for(int i = 0; i < ability_options.Count;i++){
            ability_options[i].Hide_All();
        }
        for(int i = 0; i < equip_options.Count;i++){
            equip_options[i].Hide_All();
        }
    }
}
