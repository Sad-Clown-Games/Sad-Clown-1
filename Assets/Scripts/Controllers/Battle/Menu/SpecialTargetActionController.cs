using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//upon selection, 
public class SpecialTargetActionController : MonoBehaviour
{
    public List<Battle_Option_Skill> options;
    public List<Action> actions;
    public GameObject option_prefab;
    public Vector3 offset;
    public int cur_page = 0;
    public int cur_options = 0;
    private void Update() {

    }

    public void Set_Options(List<Action> combatant_actions){
        Reset_Options();
        actions = combatant_actions;
        for (int i = 0; i < options.Count; i++)
        {
            options[i].Activate(actions[i]);
            options[i].attack = actions[i];
        }

    }

    public void Hover_Option(int idx){
        options[idx].Hover();
    }
    public void UnHover_Option(int idx){
        options[idx].UnHover();
    }
    public int Get_Option_Count(){
        return options.Count;
    }

    private void Reset_Options(){
        foreach (var option in options)
        {
            option.Deactivate();   
        }
    }
}