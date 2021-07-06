using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

abstract public class Combatant : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera idle_cam;
    public string combatant_name = "Teste";
    public Attack default_attack;
    public Attack default_guard;
    public Vector3 target_arrow_location;
    public GameObject pawn;

    //List of attacks the combatant has
    public List<Attack> attacks;
    public Vector3 menu_pos;
    public Vector3 offset;
    public Character_Stats stats;
    public Animator animator;
    public Sprite battle_sprite;

    //We need to build the billboarding logic for changing sprites here
    //As well as build the logic for changing the sprites for attacks
    //Basically we call all of the above methods in the specific combatant, and then in the child class we call
    //logic for any character specific sprite ruitines, like special attacks. 
}
