using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pause_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Item> items;
    public List<Player_Character> party;
    [SerializeField]
    private GameObject view;
    [SerializeField]
    private UnityEngine.UI.Image item_icon;
    [SerializeField]
    private TMPro.TMP_Text item_description;
    [SerializeField]
    private TMPro.TMP_Text page_num;
    private bool holding_horiz;
    private bool holding_vert;
    public List<Menu_Option> item_options;
    public List<Menu_Option> use_options;
    public List<Menu_Option> char_options;
    [SerializeField]
    PauseMenuController controller;
    public int item_option_idx;
    public int item_option_num; //number of options total
    public int item_page_idx;
    public int use_option_idx;
    public int char_option_idx;
    public int colsize = 8;
    public GameObject item_use_menu;
    public GameObject character_select_menu;
    public Item cur_item;
    public Player_Combatant cur_combatant;

    private enum ItemMenu_Sub_State
    {
        item,
        use,
        character
    }
    [SerializeField]
    private ItemMenu_Sub_State state;
    public void Control(inputs input){
        if(items.Count <= 0){

        }
        else{
        switch (state)
            {
                case ItemMenu_Sub_State.item:
                    Item_Control(input);
                    break;
                case ItemMenu_Sub_State.use:
                    Use_Control(input);
                    break;
                case ItemMenu_Sub_State.character:
                    Char_Control(input);
                    break;
            }
        }
    }

    private void Use_Control(inputs input){
        int prev_option_idx = use_option_idx;
        float horiz = input.XAxis;
        float vert = input.YAxis;
        if(vert < 0 && use_option_idx < use_options.Count-1){
            if(!holding_vert){
                use_option_idx += 1;
                holding_vert = true;
                bool skip_option = false;
                //ok now we make sure that we're not selecting a gray option
                do
                {
                    skip_option = false;
                    switch (use_options[use_option_idx].option_name)
                        {
                            case "Use":
                                break;
                            case "Equip":
                                if(cur_item.types.HasFlag(Item.Item_Type.equipment)){
                                    //yeah ok
                                }
                                else{
                                    use_option_idx +=1;
                                    skip_option = true;
                                }
                                break;
                            case "Discard":
                                if(!cur_item.types.HasFlag(Item.Item_Type.essential)){
                                    //yeah ok
                                }
                                else{
                                    use_option_idx -=1;
                                    skip_option = false; //do not advance
                                }
                                break;
                        }
                } while (skip_option);
            }
        }
        if(vert > 0 && use_option_idx > 0){
            if(!holding_vert){
                use_option_idx -= 1;
                holding_vert = true;
                bool skip_option;
                do{
                    skip_option = false;
                    switch (use_options[use_option_idx].option_name)
                    {
                        case "Use":
                            break;
                        case "Equip":
                            if(cur_item.types.HasFlag(Item.Item_Type.equipment)){
                                //yeah ok
                            }
                            else{
                                use_option_idx -=1;
                                skip_option = true;
                            }
                            break;
                        case "Discard":
                            if(!cur_item.types.HasFlag(Item.Item_Type.essential)){
                                //yeah ok
                            }
                            else{
                                use_option_idx +=1;
                                skip_option = false; //do not advance
                            }
                            break;
                    }
                } while (skip_option);
            }
        }
        use_options[use_option_idx].hovered = true;
        if(use_option_idx != prev_option_idx)
            use_options[prev_option_idx].hovered = false;
        if(input.select){
            switch (use_options[use_option_idx].option_name)
            {
                case "Item":
                    break;
                case "Equip":
                    break;
                case "Discard":
                    Game_Manager.Instance.Remove_Item(cur_item.original_list_idx);
                    if(item_option_idx != 0){
                        item_option_idx--;
                    }
                    Refresh_List();
                    Use_To_Item();
                    break;
            }
            
        }
        else if(input.cancel || input.pause){
            Use_To_Item();
        }
        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }
    private void Char_Control(inputs input){
        int prev_option_idx = char_option_idx;
        float horiz = input.XAxis;
        float vert = input.YAxis;
        if(vert < 0 && char_option_idx < char_options.Count-1){
            if(!holding_vert){
                char_option_idx += 1;
                holding_vert = true;
            }
        }
        if(vert > 0 && char_option_idx > 0){
            if(!holding_vert){
                char_option_idx -= 1;
                holding_vert = true;
            }
        }
        use_options[char_option_idx].hovered = true;
        if(char_option_idx != prev_option_idx)
            char_options[prev_option_idx].hovered = false;
        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;

        if(input.select){

        }

        if(input.cancel || input.pause){
            Char_To_Use();
        }
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
                    if(Goto_Page(item_page_idx-1)){
                        if(item_option_idx%2 == 1)
                            item_option_idx = colsize*2-1;
                        else
                            item_option_idx = colsize*2-2;
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
                    if(Goto_Page(item_page_idx+1)){
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
                    if(Goto_Page(item_page_idx+1)){
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
                    if(Goto_Page(item_page_idx-1)){
                        item_option_idx += 1;
                    }
                }
                holding_horiz = true;
            }
            
        }
        item_options[item_option_idx].Show_Selector();
        if(item_option_idx != prev_option_index)
            item_options[prev_option_index].Hide_Selector();
        Update_Preview(item_options[item_option_idx].item);

        if(input.select){
            Item_To_Use();
        }

        if(vert == 0)
            holding_vert = false;
        if(horiz == 0)
            holding_horiz = false;
    }
    //set options up when we remove and add items while in menu
    public void Set_Options(){
        Clear_Options();
        item_page_idx = 0;
        for (int i = 0; i < item_options.Count; i++)
        {
            if(i > items.Count-1)
                break;
            item_options[i].Activate(items[i],i,i,colsize,item_options);
            item_options[i].item = items[i];
            item_options[i].item.active_idx = i; //save index for when we need to remove item;
            item_option_num++;
        }
    }

    public void Update_Preview(Item item){
        item_icon.sprite = item.icon;
        item_description.text = item.description;
        page_num.text = (item_page_idx+1).ToString();
    }

    //false if we dont change page true if we do
    public bool Goto_Page(int page){
        int first_index = page * item_options.Count;
        if(item_options.Count >= items.Count || first_index > items.Count || page < 0){
            return false;
        }
        item_page_idx = page;
        Clear_Options();
        for(int i = first_index; i-first_index < item_options.Count; i++){
            if(i > items.Count-1)
                break;
            item_options[i-first_index].Activate(items[i],i,i-first_index,colsize,item_options);
            item_options[i-first_index].item = items[i];
            item_option_num++;
        }
        return true;
    }

    private void Clear_Options(){
        foreach (var option in item_options)
        {
            option.Deactivate();
        }
        item_option_num = 0;
    }
    private void Clear_Highlights(){
        for(int i = 0; i < item_options.Count;i++){
            item_options[i].hovered = false;
            item_options[i].Hide_Selector();
        }
        for(int i = 0; i < use_options.Count;i++){
            use_options[i].hovered = false;
            use_options[i].Hide_Selector();
        }
        for(int i = 0; i < char_options.Count;i++){
            char_options[i].hovered = false;
            char_options[i].Hide_Selector();
        }
    }

    private void Item_To_Use(){
        item_use_menu.SetActive(true);
        cur_item = item_options[item_option_idx].item;
        state = ItemMenu_Sub_State.use;
        controller.is_sub_menu = true;
    }

    private void Char_To_Use(){
        char_options[char_option_idx].hovered = false;
        char_option_idx = 0;
        character_select_menu.SetActive(false);
        state = ItemMenu_Sub_State.use;
        controller.is_sub_menu = true;
    }

    private void To_Char(){
        character_select_menu.SetActive(true);
        state = ItemMenu_Sub_State.character;
        controller.is_sub_menu = true;
    }

    private void Use_To_Item(){
        use_options[use_option_idx].hovered = false;
        use_option_idx = 0;
        item_use_menu.SetActive(false);
        state = ItemMenu_Sub_State.item;
        controller.is_sub_menu = false;
    }
    
    public void Refresh_List(){
        items = Game_Manager.Instance.Get_Items();
        Set_Options();
    }

    public void Open(){
        items = Game_Manager.Instance.Get_Items();
        party = Game_Manager.Instance.party_controller.Get_Active_Party_By_Order();
        item_option_idx = 0;
        item_page_idx = 0;
        state = ItemMenu_Sub_State.item;
        view.SetActive(true);
        Set_Options();
    }

    public void Grey_Out(){
        item_option_idx = 0;
        item_page_idx = 0;
        view.SetActive(true);
    }

    public void Close(){
        Clear_Highlights();
        items = null;
        item_option_idx = 0;
        item_page_idx = 0;
        use_option_idx = 0;
        char_option_idx = 0;
        controller.is_sub_menu = false;
        item_use_menu.SetActive(false);
        view.SetActive(false);
    }
}
