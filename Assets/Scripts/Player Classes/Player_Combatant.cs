using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combatant : Combatant
{
    // Start is called before the first frame update

    //List of attacks the combatant has
    public List<Attack> attacks;
    public Vector3 menu_pos;

    public Vector3 Get_Menu_Position(){
        return transform.position + menu_pos;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
