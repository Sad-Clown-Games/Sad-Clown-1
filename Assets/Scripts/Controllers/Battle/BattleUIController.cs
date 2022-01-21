using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BattleUIController : MonoBehaviour
{
    public AttackDisplayController attackDisplayController;

    public StatusBannerController bannerController;

    public void Raise_Text(string text){
        attackDisplayController.Display(text);
    }
    public void Hide_Text(){
        attackDisplayController.Hide();
    }


}
