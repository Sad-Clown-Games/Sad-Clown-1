using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Option_Controller : OptionController
{
    public List<Battle_Option_Skill> options;
    public List<Attack> attacks;
    public GameObject option_prefab;
    public float x_offset;
    public float y_offset;

    public int cur_page = 0;

    public void Set_Options(){
        Reset_Options();
        for (int i = 0; i < options.Count; i++)
        {
            if(i > attacks.Count-1)
                break;
            options[i].Activate(attacks[i]);
            options[i].attack = attacks[i];
        }
    }

    //false if we dont change page true if we do
    public bool Goto_Page(int page){
        int first_index = page * options.Count;
        if(options.Count >= attacks.Count || first_index > attacks.Count || page < 0){
            return false;
        }
        cur_page = page;
        Reset_Options();
        Debug.Log(first_index);
        Debug.Log(options.Count);
        Debug.Log(attacks.Count);
        for(int i = first_index; i-first_index < options.Count; i++){
            if(i > attacks.Count-1)
                break;
            options[i-first_index].Activate(attacks[i]);
            options[i].attack = attacks[i];
        }
        return true;

    }

    private void Reset_Options(){
        foreach (var option in options)
        {
            option.Deactivate();
        }
    }
}
