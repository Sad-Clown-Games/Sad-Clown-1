
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Game_Manager))]
public class SomeScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
         //to start from scratch, but I use this when I'm just adding a button or
         //some small addition and don't feel like recreating the whole inspector.
         Game_Manager gm = (Game_Manager) target;
         if(GUILayout.Button("Save")) {
             //add everthing the button would do.
             gm.Save_Game();
         }
        if(GUILayout.Button("Load")) {
            //add everthing the button would do.
            gm.Load_Game();
         }
    }
 }