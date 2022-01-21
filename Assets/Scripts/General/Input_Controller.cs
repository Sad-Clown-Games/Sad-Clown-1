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
    private bool disabled;
    void Update()
    {
        if(!disabled){
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
    public inputs Get_Inputs(){
        return inputs;
    }
    public void Disable(){
        inputs.select = false;
        inputs.cancel = false;
        inputs.pause = false;
        inputs.XAxis = 0;
        inputs.YAxis = 0;
        disabled = true;
    }
    public void Enable(){
        disabled = false;
    }
}
