using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackDisplayController : MonoBehaviour
{
    [SerializeField]
    private Image banner;
    [SerializeField]
    private TMPro.TextMeshProUGUI desc;
    public void Display(string text){
        banner.enabled = true;
        desc.text = text;
    }

    public void Hide(){
        banner.enabled = false;
        desc.text = "";
    }
}
