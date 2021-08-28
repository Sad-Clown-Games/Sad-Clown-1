using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaManager : MonoBehaviour
{
    public string name;

    public AreaTransfer location;

    public class AreaTransfer
    {
        public string AreaName;
        public float x, y;

        public AreaTransfer(string name)
        {
            AreaName = name;
        }

    }

 
    // Start is called before the first frame update
    void Start()
    {
        location = new AreaTransfer(name);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(location.AreaName);
    }
}

