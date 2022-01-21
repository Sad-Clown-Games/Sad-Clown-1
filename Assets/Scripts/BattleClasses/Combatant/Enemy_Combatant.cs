using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Enemy_Combatant : Combatant
{

    //ok so the way this is going to work is that we will use the attacks list for if the player is scanning the character for attacks/beastiary
    //and we just keep track of the index of the attack we want to do
    //enemies will really only have under 10 attacks, and for the most part 2-3.
    [System.Serializable]
    public class Drop{
        public float chance;
        public Item item;
    };

    //
    abstract public CombatAction Run_AI(List<Combatant> player_party, List<Combatant> enemy_party );
    [SerializeField]
    public List<Drop> drop_pool; //we may want to add items or exp for a condition
    [SerializeField] 
    public int exp = 1;
    public List<Item> Calculate_Drop(){
        //you can get more than one drop from an enemy because this is the easiest implimentation.
        List<Item> items = new List<Item>();
        foreach(Drop d in drop_pool){
            float d100 = Random.Range(0f,100f);
            if(d100 <= d.chance){ //90 = 90% chance, 10 = 10% chance, 
                items.Add(d.item);
            }
        }
        return items; //if there's no drops in the pool or the
    }

    public int Get_Exp(){
        return exp;
    }

}
