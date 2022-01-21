using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransferHelper
{
    
    public static void GoToWorldScene(string scene_name, Vector3 loadLocation){
        Scene_Loc new_scene = new Scene_Loc{location=loadLocation,scene_name=scene_name};
        Game_Manager.Instance.SetState_NewMap();
        Game_Manager.Instance.SetNextScene(new_scene);
        Game_Manager.Instance.Save_Session();
        //
        SceneManager.LoadScene(scene_name);
    }

    public static void GoToWorldSceneFromBattleScene(){
        Scene_Loc last_scene = Game_Manager.Instance.GetLast_Scene();
        Game_Manager.Instance.SetState_FromBattle();
        Game_Manager.Instance.SetNextScene(last_scene);
        //
        Game_Manager.Instance.Save_Session();

        SceneManager.LoadScene(last_scene.scene_name);
    }

    public static void GoToBattleScene(string scene_name){
        Game_Manager.Instance.SetLastScene();
        Game_Manager.Instance.SetState_ToBattle();
        //
        Game_Manager.Instance.Save_Session();
        SceneManager.LoadScene(scene_name);
    }
}