using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class camera_set : MonoBehaviour
{
    // Start is called before the first frame update
    public List<CinemachineVirtualCamera> cameras;


    public void Flip_Set(){
        foreach(CinemachineVirtualCamera c in cameras){
            c.transform.Rotate(new Vector3(0,180,0)); //rotate 180
            Vector3 newscale = new Vector3(1,1,-1); //flip entire set
            transform.localScale = newscale;
        }
    }
}
