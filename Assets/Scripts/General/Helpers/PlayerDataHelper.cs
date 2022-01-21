using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public static class PlayerDataHelper
{
    public static List<Item> Get_By_Type(this List<Item> items,Item.Item_Type type){
        return items.Where(a=> a.types.HasFlag(type)).ToList();
    }

    //removes items[idx] and replaces it wih given item
    //returns old item
    public static Item Remove_Or_Swap_Item(this List<Item> items, Item item, int idx){
        Item temp = items[idx];
        if(item != null){
            items[idx] = item; //swap item
        }
        else{
            items.RemoveAt(idx); //remove item
        }
        return temp;
    }
}