using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player_Data : MonoBehaviour {
    
    [SerializeField]
    public Game_Data save_data;
    //all playable party member data go below
    public List<Item> items;
}