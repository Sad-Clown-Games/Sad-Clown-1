using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManagement : MonoBehaviour
{
    public class CutScene
    {
        public string SceneName;


        public CutScene(string name)
        {
            SceneName = name;
        }

    }

    public class ManageScenes
    {
        public void changescene(CutScene Scene)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scene.SceneName));
        }
        public void loadscene(CutScene Scene)
        {
            SceneManager.LoadSceneAsync(Scene.SceneName);
        }
        public void unloadscene(CutScene Scene)
        {
            SceneManager.UnloadSceneAsync(Scene.SceneName);
        }
        public void loadflags()
        {
        }
    }

}
