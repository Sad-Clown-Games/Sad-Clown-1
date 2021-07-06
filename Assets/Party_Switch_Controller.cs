using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party_Switch_Controller : OptionController
{
     public List<Player_Combatant> options;
     public Battle_Controller battle_controller;
     public Cinemachine.CinemachineVirtualCamera targeting_camera;
     public GameObject selection_arrow;

     public void Set_Reserve_Characters(){
          options.Clear();
          var reserve_list = battle_controller.reserve_player_combatants;
          //We do it this way because we want to easily preserve the full indexes instead of getting an incomplete list
          //With the alternate, only adding to the list reserve that aren't being switched.
          foreach(Player_Combatant c in reserve_list){
                    options.Add(c);
          }
     }

     public void Set_Target(Combatant combatant){
          Show_Arrow();
          targeting_camera.m_LookAt = combatant.pawn.transform;
          targeting_camera.m_Follow = combatant.pawn.transform;
          selection_arrow.transform.position = combatant.pawn.transform.position + combatant.target_arrow_location;
     }

     public void Hide_Arrow(){
          selection_arrow.SetActive(false);
     }
     public void Show_Arrow(){
          selection_arrow.SetActive(true);
     }
}
