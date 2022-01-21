
using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Game_Manager))]
public class SomeScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
         //to start from scratch, but I use this when I'm just adding a button or
         //some small addition and don't feel like recreating the whole inspector.
         Game_Manager gm = (Game_Manager) target;
         if(GUILayout.Button("SaveGameSave")) {
             //add everthing the button would do.
             gm.Save_Game();
         }
        if(GUILayout.Button("LoadGameSave")) {
            //add everthing the button would do.
            gm.Load_Game();
         }
        if(GUILayout.Button("SaveSession")) {
             //add everthing the button would do.
             gm.Save_Session();
         }
        if(GUILayout.Button("LoadSession")) {
            //add everthing the button would do.
            gm.Load_Session();
         }
    }
 }

 #endif