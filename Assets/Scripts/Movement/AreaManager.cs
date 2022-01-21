using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class AreaTransfer
{
    public string scene_name;
    public Vector3 scene_location;

}


 [System.Serializable]

public class AreaManager : MonoBehaviour
{
    public string scene_name;

    public AreaTransfer location;

 

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(location.scene_name);
    }
}
