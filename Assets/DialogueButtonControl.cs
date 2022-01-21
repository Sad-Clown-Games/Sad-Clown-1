using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButtonControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    UnityEngine.UI.Button button;
    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject && Input.GetButtonDown("Select")){
            button.onClick.Invoke();
        }
    }
}
