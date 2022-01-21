using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMeterController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI value;
    [SerializeField]
    private UnityEngine.UI.Image meter;

    public void Set_Value(int val, int maxVal){
        value.text = val.ToString();
        meter.fillAmount = (val*1.0f)/(maxVal*1.8f);
    }
}
