using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : Action
{
    
    #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
    #else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
    #endif
    override abstract public void Do_Action();
    abstract public void Do_Overworld_Action();
    [System.Flags]
    public enum Item_Type
    {
        usable = 0,
        single_use = 1,
        reusable = 2,
        equipment = 4,
        weapon = 8,
        armor = 16,
        trinket = 32,
        party_use = 64,
        overworld_use = 128,
        battle_use = 256,
        essential = 512,

    }
    public int original_list_idx;
    public Item_Type types;
    public Sprite icon;
}
