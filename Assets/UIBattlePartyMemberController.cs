using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIBattlePartyMemberController : MonoBehaviour
{
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private TextMeshProUGUI character_name;
    [SerializeField]
    private UIMeterController HP_meter;
    [SerializeField]
    private UIMeterController MP_meter;
    [SerializeField]
    private UIAilmentController ailments;

    public void Fill_Combatant_Info(Combatant c){
        Update_MP(c.stats.cur_mp,c.stats.max_mp);
        Update_HP(c.stats.cur_hp,c.stats.max_hp);
        Update_Character(c.combatant_name,c.portrait);
    }

    public void Update_MP(int cur,int max){
        MP_meter.Set_Value(cur,max);
    }

    public void Update_HP(int cur,int max){
        HP_meter.Set_Value(cur,max);
    }

    public void Update_Character(string name, Sprite image){
        portrait.sprite = image;
        character_name.text = name;
    }

}
