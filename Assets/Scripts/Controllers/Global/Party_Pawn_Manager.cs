using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Party_Pawn_Manager : MonoBehaviour {
    
    [SerializeField]
    public Game_Data save_data;
    //all playable party member data go below
    [SerializeField]
    public List<Item> items;
}