using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextHelper{
    public static string GenerateBattleString(string target, string actor, string base_string){
        string battle_string = base_string.Replace("%a",actor);
        battle_string = battle_string.Replace("%t",target);
        return battle_string;
    }
}