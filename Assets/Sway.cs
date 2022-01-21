using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ySway;
    [Range(0.1f,10f)]
    public float yAmp;
    [Range(0.1f,10f)]
    public float ySpeed;
    public bool xSway;
    [Range(0.1f,10f)]
    public float xAmp;
    [Range(0.1f,10f)]
    public float xSpeed;
    public bool zSway;
    [Range(0.1f,10f)]
    public float zAmp;
    [Range(0.1f,10f)]
    public float zSpeed;
    void Update()
    {
        float xMov = 0;
        float timer = Time.timeSinceLevelLoad;
        if(xSway){
            xMov = .001f*xAmp * Mathf.Sin(timer*xSpeed);
        }
        float yMov = 0;
        if(ySway){
            yMov = .001f*yAmp * Mathf.Sin(timer*ySpeed);
        }
        float zMov = 0;
        if(zSway){
            zMov = .001f*zAmp*  Mathf.Sin(timer*zSpeed);
        }
        transform.position += new Vector3(xMov,yMov,zMov);
    }
}
