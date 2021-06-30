using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Select_Controller : OptionController
{
     public List<Combatant> options;
     public Battle_Controller battle_controller;
     public Cinemachine.CinemachineVirtualCamera targeting_camera;
     public GameObject selection_arrow;

     public void Set_Combatants_For_Attack_Skill(){
          options = battle_controller.Get_All_Combatants_Enemy_First();
     }

     public void Set_Combatants_For_Support_Skill(){
          options = battle_controller.Get_All_Combatants_Player_First();
     }

     public void Set_Target(Combatant combatant){
          Show_Arrow();
          targeting_camera.m_LookAt = combatant.transform;
          selection_arrow.transform.position = combatant.transform.position + combatant.target_arrow_location;
     }

     public void Hide_Arrow(){
          selection_arrow.SetActive(false);
     }
     public void Show_Arrow(){
          selection_arrow.SetActive(true);
     }
}
