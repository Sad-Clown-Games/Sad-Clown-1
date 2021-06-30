using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Singleton responsible for scene changes, saving data, and pulling data.
Add this to every scene.
*/
public class Game_Manager : MonoBehaviour
{

    //Static reference
    public static Game_Manager Instance;

    [Header("Player Data")]
    public Player_Data player_data;

    [Header("Characters")]
    //we need a way to represent active party members, and also access stats at all times in a reasonable way
    //with no duplicate "player character" classes
    public Player_Combatant placeholder1;
    public Player_Combatant placeholder2;
    public Player_Combatant placeholder3;
    public Player_Combatant placeholder4;
    public Player_Combatant placeholder5;

    void Awake()
    {
        //Let the gameobject persist over the scenes
        DontDestroyOnLoad(gameObject);
        //Check if the Instance instance is null
        if (Instance == null)
        {
            //This instance becomes the single instance available
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
