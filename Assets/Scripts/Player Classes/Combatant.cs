using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

abstract public class Combatant : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera idle_cam;
    public string combatant_name = "Teste";
    public Attack default_attack;
    public Attack default_guard;
    public Vector3 target_arrow_location;
}
