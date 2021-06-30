using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Option_Controller : OptionController
{
    public List<Battle_Option_Item> options;
    public List<Item> items;
    public GameObject option_prefab;
    public float x_offset;
    public float y_offset;

    public int cur_page = 0;
    public int cur_options = 0;

    public void Set_Options(){
        Reset_Options();
        cur_page = 0;
        for (int i = 0; i < options.Count; i++)
        {
            if(i > items.Count-1)
                break;
            options[i].Activate(items[i]);
            options[i].item = items[i];
            cur_options++;
        }
    }

    //false if we dont change page true if we do
    public bool Goto_Page(int page){
        int first_index = page * options.Count;
        if(options.Count >= items.Count || first_index > items.Count || page < 0){
            return false;
        }
        cur_page = page;
        Reset_Options();
        for(int i = first_index; i-first_index < options.Count; i++){
            if(i > items.Count-1)
                break;
            options[i-first_index].Activate(items[i]);
            options[i-first_index].item = items[i];
            cur_options++;
        }
        return true;

    }

    private void Reset_Options(){
        foreach (var option in options)
        {
            option.Deactivate();
        }
        cur_options = 0;
    }
}
