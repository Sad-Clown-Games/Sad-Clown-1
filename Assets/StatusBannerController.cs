using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBannerController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<UIBattlePartyMemberController> party_members;

    public UIBattlePartyMemberController GetPartyMemberByIdx(int idx){
        if(idx > 4 || idx < 0){
            return party_members[0];
        }
        return party_members[idx];

    }
}
