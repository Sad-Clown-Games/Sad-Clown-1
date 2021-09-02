using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct inputs{
    public bool select;
    public bool cancel;
    public bool pause;
    public float XAxis;
    public float YAxis;
};
public class Input_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    public inputs inputs;
    public bool debug;
    void Update()
    {
        inputs.select = Input.GetButtonDown("Select");
        inputs.cancel = Input.GetButtonDown("Cancel");
        inputs.pause = Input.GetButtonDown("Pause");
        inputs.XAxis  = Input.GetAxis("Horizontal");
        inputs.YAxis  = Input.GetAxis("Vertical");
        if(debug){
            Debug.Log(inputs.select);
            Debug.Log(inputs.cancel);
            Debug.Log(inputs.pause);
        }
    }
}
